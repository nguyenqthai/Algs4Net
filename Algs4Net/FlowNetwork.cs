/******************************************************************************
 *  File name :    FlowNetwork.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A capacitated flow network, implemented using adjacency lists.
 *  
 *  C:\> algscmd FlowNetwork tinyFN.txt
 *  6 8
 *  0:  0->2 0.00/3.00  0->1 0.00/2.00
 *  1:  1->4 0.00/1.00  1->3 0.00/3.00
 *  2:  2->4 0.00/1.00  2->3 0.00/1.00
 *  3:  3->5 0.00/2.00
 *  4:  4->5 0.00/3.00
 *  5:
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>FlowNetwork</c> class represents a capacitated network
  /// with vertices named 0 through <c>V</c> - 1, where each directed
  /// edge is of type <seealso cref="FlowEdge"/> and has a real-valued capacity
  /// and flow.</para><para>
  /// It supports the following two primary operations: add an edge to the network,
  /// iterate over all of the edges incident to or from a vertex. It also provides
  /// methods for returning the number of vertices <c>V</c> and the number
  /// of edges <c>E</c>. Parallel edges and self-loops are permitted.</para>
  /// <para>This implementation uses an adjacency-lists representation, which 
  /// is a vertex-indexed array of <see cref="Bag{Item}"/> objects.
  /// All operations take constant time (in the worst case) except
  /// iterating over the edges incident to a given vertex, which takes
  /// time proportional to the number of such edges.</para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/64maxflow">Section 6.4</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/FlowNetwork.java.html">FlowNetwork</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class FlowNetwork
  {
    private readonly int numVertices;
    private int numEdges;
    private Bag<FlowEdge>[] adj;

    /// <summary>Initializes an empty flow network with <c>V</c> vertices and 0 edges.</summary>
    /// <param name="V">the number of vertices</param>
    /// <exception cref="ArgumentException">if <c>V</c> &lt; 0</exception>
    ///
    public FlowNetwork(int V)
    {
      if (V < 0) throw new ArgumentException("Number of vertices in a Graph must be nonnegative");
      numVertices = V;
      numEdges = 0;
      adj = new Bag<FlowEdge>[V];

      for (int v = 0; v < V; v++)
        adj[v] = new Bag<FlowEdge>();
    }

    /// <summary>
    /// Initializes a random flow network with <c>V</c> vertices and <c>E</c> edges.
    /// The capacities are integers between 0 and 99 and the flow values are zero.</summary>
    /// <param name="V">the number of vertices</param>
    /// <param name="E">the number of edges</param>
    /// <exception cref="ArgumentException">if <c>V</c> &lt; 0</exception>
    /// <exception cref="ArgumentException">if <c>E</c> &lt; 0</exception>
    ///
    public FlowNetwork(int V, int E) : this(V)
    {
      if (E < 0) throw new ArgumentException("Number of edges must be nonnegative");
      for (int i = 0; i < E; i++)
      {
        int v = StdRandom.Uniform(V);
        int w = StdRandom.Uniform(V);
        double capacity = StdRandom.Uniform(100);
        AddEdge(new FlowEdge(v, w, capacity));
      }
    }

    /// <summary>
    /// Initializes a flow network from an input stream.
    /// The format is the number of vertices <c>V</c>,
    /// followed by the number of edges <c>E</c>,
    /// followed by <c>E</c> pairs of vertices and edge capacities,
    /// with each entry separated by whitespace.</summary>
    /// <param name="input">in the input stream</param>
    /// <exception cref="IndexOutOfRangeException">if the endpoints of any edge are not in prescribed range</exception>
    /// <exception cref="ArgumentException">if the number of vertices or edges is negative</exception>
    ///
    public FlowNetwork(TextInput input) : this(input.ReadInt())
    {
      int E = input.ReadInt();
      //if (E < 0) throw new ArgumentException("Number of edges must be nonnegative");
      for (int i = 0; i < E; i++)
      {
        int v = input.ReadInt();
        int w = input.ReadInt();
        validateVertex(v);
        validateVertex(w);
        double capacity = input.ReadDouble();
        AddEdge(new FlowEdge(v, w, capacity));
      }
    }

    /// <summary>
    /// Returns the number of vertices in the edge-weighted graph.</summary>
    /// <returns>the number of vertices in the edge-weighted graph</returns>
    ///
    public int V
    {
      get { return numVertices; }
    }

    /// <summary>
    /// Returns the number of edges in the edge-weighted graph.</summary>
    /// <returns>the number of edges in the edge-weighted graph</returns>
    ///
    public int E
    {
      get { return numEdges; }
    }

    // throw an IndexOutOfRangeException unless 0 <= v < V
    private void validateVertex(int v)
    {
      if (v < 0 || v >= V)
        throw new IndexOutOfRangeException("vertex " + v + " is not between 0 and " + (V - 1));
    }

    /// <summary>
    /// Adds the edge <c>e</c> to the network.</summary>
    /// <param name="e">the edge</param>
    /// <exception cref="IndexOutOfRangeException">unless endpoints of edge are between 0 and V-1</exception>
    ///
    public void AddEdge(FlowEdge e)
    {
      int v = e.From;
      int w = e.To;
      validateVertex(v);
      validateVertex(w);
      adj[v].Add(e);
      adj[w].Add(e);
      numEdges++;
    }

    /// <summary>
    /// Returns the edges incident on vertex <c>v</c> (includes both edges pointing to
    /// and from <c>v</c>).</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the edges incident on vertex <c>v</c> as an Iterable</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public IEnumerable<FlowEdge> Adj(int v)
    {
      validateVertex(v);
      return adj[v];
    }

    /// <summary>
    /// Returns all edges in this edge-weighted graph, excludeing self loops.
    /// To iterate over the edges in this edge-weighted graph, use foreach notation:
    /// <c>foreach (Edge e in G.Edges())</c>.</summary>
    /// <returns>all edges in this edge-weighted graph, as an iterable</returns>
    ///
    public IEnumerable<FlowEdge> Edges()
    {
      Bag<FlowEdge> list = new Bag<FlowEdge>();
      for (int v = 0; v < V; v++)
        foreach (FlowEdge e in Adj(v))
        {
          if (e.To != v)
            list.Add(e);
        }
      return list;
    }

    /// <summary>
    /// Returns a string representation of the flow network.
    /// This method takes time proportional to <c>E</c> + <c>V</c>.</summary>
    /// <returns>the number of vertices <c>V</c>, followed by the number of edges <c>E</c>,  
    /// followed by the <c>V</c> adjacency lists</returns>
    ///
    public override string ToString()
    {
      string NEWLINE = Environment.NewLine;
      StringBuilder s = new StringBuilder();
      s.Append(V + " " + E + NEWLINE);
      for (int v = 0; v < V; v++)
      {
        s.Append(v + ":  ");
        foreach (FlowEdge e in adj[v])
        {
          if (e.To != v) s.Append(e + "  ");
        }
        s.Append(NEWLINE);
      }
      return s.ToString();
    }

    /// <summary>
    /// Demo test the <c>FlowNetwork</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd FlowNetwork tinyFN.txt", "Input file with the flow network format")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      FlowNetwork G = new FlowNetwork(input);
      Console.WriteLine(G);
    }
  }

}

/******************************************************************************
 *  Copyright 2016, Thai Nguyen.
 *  Copyright 2002-2015, Robert Sedgewick and Kevin Wayne.
 *
 *  This file is part of Algs4Net.dll, a .NET library that ports algs4.jar,
 *  which accompanies the textbook
 *
 *      Algorithms, 4th edition by Robert Sedgewick and Kevin Wayne,
 *      Addison-Wesley Professional, 2011, ISBN 0-321-57351-X.
 *      http://algs4.cs.princeton.edu
 *
 *
 *  Algs4Net.dll is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Algs4Net.dll is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Algs4Net.dll.  If not, see http://www.gnu.org/licenses.
 ******************************************************************************/
