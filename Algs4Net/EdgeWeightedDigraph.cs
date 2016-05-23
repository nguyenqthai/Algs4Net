/******************************************************************************
 *  File name :    EdgeWeightedDigraph.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  An edge-weighted digraph, implemented using adjacency lists.
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>EdgeWeightedDigraph</c> class represents a edge-weighted
  /// digraph of vertices named 0 through <c>V</c> - 1, where each
  /// directed edge is of type <seealso cref="DirectedEdge"/> and has a real-valued weight.
  /// It supports the following two primary operations: add a directed edge
  /// to the digraph and iterate over all of edges incident from a given vertex.
  /// It also provides
  /// methods for returning the number of vertices <c>V</c> and the number
  /// of edges <c>E</c>. Parallel edges and self-loops are permitted.
  /// </para><para>
  /// This implementation uses an adjacency-lists representation, which 
  /// is a vertex-indexed array of <see cref="Bag{Item}"/> objects.
  /// All operations take constant time (in the worst case) except
  /// iterating over the edges incident from a given vertex, which takes
  /// time proportional to the number of such edges.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/44sp">Section 4.4</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/EdgeWeightedDigraph.java.html">EdgeWeightedDigraph</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class EdgeWeightedDigraph
  {
    private readonly int numVertices;          // number of vertices in this digraph
    private int numEdges;                      // number of edges in this digraph
    private Bag<DirectedEdge>[] adj;    // adj[v] = adjacency list for vertex v
    private int[] indegree;             // indegree[v] = indegree of vertex v

    /// <summary>
    /// Initializes an empty edge-weighted digraph with <c>V</c> vertices and 0 edges.</summary>
    /// <param name="V">the number of vertices</param>
    /// <exception cref="ArgumentException">if <c>V</c> &lt; 0</exception>
    ///
    public EdgeWeightedDigraph(int V)
    {
      if (V < 0) throw new ArgumentException("Number of vertices in a Digraph must be nonnegative");
      numVertices = V;
      numEdges = 0;
      indegree = new int[V];
      adj = new Bag<DirectedEdge>[V];
      for (int v = 0; v < V; v++)
        adj[v] = new Bag<DirectedEdge>();
    }

    /// <summary>
    /// Initializes a random edge-weighted digraph with <c>V</c> vertices and <c>E</c> edges.</summary>
    /// <param name="V">the number of vertices</param>
    /// <param name="E">the number of edges</param>
    /// <exception cref="ArgumentException">if <c>V</c> &lt; 0</exception>
    /// <exception cref="ArgumentException">if <c>E</c> &lt; 0</exception>
    ///
    public EdgeWeightedDigraph(int V, int E) : this(V)
    {
      if (E < 0) throw new ArgumentException("Number of edges in a Digraph must be nonnegative");
      for (int i = 0; i < E; i++)
      {
        int v = StdRandom.Uniform(V);
        int w = StdRandom.Uniform(V);
        double weight = .01 * StdRandom.Uniform(100);
        DirectedEdge e = new DirectedEdge(v, w, weight);
        AddEdge(e);
      }
    }

    /// <summary>
    /// Initializes an edge-weighted digraph from a text input stream.
    /// The format is the number of vertices <c>V</c>,
    /// followed by the number of edges <c>E</c>,
    /// followed by <c>E</c> pairs of vertices and edge weights,
    /// with each entry separated by whitespace.</summary>
    /// <param name="input">the input stream</param>
    /// <exception cref="IndexOutOfRangeException">if the endpoints of any edge are not in prescribed range</exception>
    /// <exception cref="ArgumentException">if the number of vertices or edges is negative</exception>
    ///
    public EdgeWeightedDigraph(TextInput input) : this(input.ReadInt())
    {
      int E = input.ReadInt();
      if (E < 0) throw new ArgumentException("Number of edges must be nonnegative");
      for (int i = 0; i < E; i++)
      {
        int v = input.ReadInt();
        int w = input.ReadInt();
        // validation wil be done from within AddEdge
        double weight = input.ReadDouble();
        AddEdge(new DirectedEdge(v, w, weight));
      }
    }

    /// <summary>
    /// Initializes a new edge-weighted digraph that is a deep copy of <c>G</c>.</summary>
    /// <param name="G">the edge-weighted digraph to copy</param>
    ///
    public EdgeWeightedDigraph(EdgeWeightedDigraph G) : this(G.V)
    {
      numEdges = G.E;
      for (int v = 0; v < G.V; v++)
        indegree[v] = G.Indegree(v);
      for (int v = 0; v < G.V; v++)
      {
        // reverse so that adjacency list is in same order as original
        Stack<DirectedEdge> reverse = new Stack<DirectedEdge>();
        foreach (DirectedEdge e in G.adj[v])
        {
          reverse.Push(e);
        }
        foreach (DirectedEdge e in reverse)
        {
          adj[v].Add(e);
        }
      }
    }

    /// <summary>
    /// Returns the number of vertices in this edge-weighted digraph.</summary>
    /// <returns>the number of vertices in this edge-weighted digraph</returns>
    ///
    public int V
    {
      get { return numVertices; }
    }

    /// <summary>
    /// Returns the number of edges in this edge-weighted digraph.</summary>
    /// <returns>the number of edges in this edge-weighted digraph</returns>
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
    /// Adds the directed edge <c>e</c> to this edge-weighted digraph.</summary>
    /// <param name="e">the edge</param>
    /// <exception cref="IndexOutOfRangeException">unless endpoints of edge are between 0 and V-1</exception>
    ///
    public void AddEdge(DirectedEdge e)
    {
      int v = e.From;
      int w = e.To;
      validateVertex(v);
      validateVertex(w);
      adj[v].Add(e);
      indegree[w]++;
      numEdges++;
    }

    /// <summary>
    /// Returns the directed edges incident from vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the directed edges incident from vertex <c>v</c> as an Iterable</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public IEnumerable<DirectedEdge> Adj(int v)
    {
      validateVertex(v);
      return adj[v];
    }

    /// <summary>
    /// Returns the number of directed edges incident from vertex <c>v</c>.
    /// This is known as the <c>Outdegree</c> of vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the outdegree of vertex <c>v</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public int Outdegree(int v)
    {
      validateVertex(v);
      return adj[v].Count;
    }

    /// <summary>
    /// Returns the number of directed edges incident to vertex <c>v</c>.
    /// This is known as the <c>Indegree</c> of vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the indegree of vertex <c>v</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public int Indegree(int v)
    {
      validateVertex(v);
      return indegree[v];
    }

    /// <summary>
    /// Returns all directed edges in this edge-weighted digraph.
    /// To iterate over the edges in this edge-weighted digraph, use foreach notation:
    /// <c>foreach (DirectedEdge e in G.Edges())</c>.</summary>
    /// <returns>all edges in this edge-weighted digraph, as an iterable</returns>
    ///
    public IEnumerable<DirectedEdge> Edges()
    {
      Bag<DirectedEdge> list = new Bag<DirectedEdge>();
      for (int v = 0; v < V; v++)
      {
        foreach (DirectedEdge e in Adj(v))
        {
          list.Add(e);
        }
      }
      return list;
    }

    /// <summary>
    /// Returns a string representation of this edge-weighted digraph.</summary>
    /// <returns>the number of vertices <c>V</c>, followed by the number of edges <c>E</c>,
    ///        followed by the <c>V</c> adjacency lists of edges</returns>
    ///
    public override string ToString()
    {
      const string NEWLINE = "\n";
      StringBuilder s = new StringBuilder();
      s.Append(V + " " + E + NEWLINE);
      for (int v = 0; v < V; v++)
      {
        s.Append(v + ": ");
        foreach (DirectedEdge e in adj[v])
        {
          s.Append(e + "  ");
        }
        s.Append(NEWLINE);
      }
      return s.ToString();
    }

    /// <summary>
    /// Demo test the <c>EdgeWeightedDigraph</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd EdgeWeightedDigraph tinyEWG.txt", "File with the pre-defined format for directed, weighted graph")]
    public static void MainTest(string[] args)
    {
      EdgeWeightedDigraph G = new EdgeWeightedDigraph(new TextInput(args[0]));
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
