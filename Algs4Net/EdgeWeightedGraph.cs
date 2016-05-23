/******************************************************************************
 *  File name :    EdgeWeightedGraph.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/43mst/tinyEWG.txt
 *
 *  An edge-weighted undirected graph, implemented using adjacency lists.
 *  Parallel edges and self-loops are permitted.
 *
 *  C:\> algscmd EdgeWeightedGraph tinyEWG.txt
 *  8 16
 *  0: 6-0 0.58000  0-2 0.26000  0-4 0.38000  0-7 0.16000
 *  1: 1-3 0.29000  1-2 0.36000  1-7 0.19000  1-5 0.32000
 *  2: 6-2 0.40000  2-7 0.34000  1-2 0.36000  0-2 0.26000  2-3 0.17000
 *  3: 3-6 0.52000  1-3 0.29000  2-3 0.17000
 *  4: 6-4 0.93000  0-4 0.38000  4-7 0.37000  4-5 0.35000
 *  5: 1-5 0.32000  5-7 0.28000  4-5 0.35000
 *  6: 6-4 0.93000  6-0 0.58000  3-6 0.52000  6-2 0.40000
 *  7: 2-7 0.34000  1-7 0.19000  0-7 0.16000  5-7 0.28000  4-7 0.37000
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>EdgeWeightedGraph</c> class represents an edge-weighted
  /// graph of vertices named 0 through <c>V</c> - 1, where each
  /// undirected edge is of type <seealso cref="Edge"/> and has a real-valued weight.
  /// It supports the following two primary operations: add an edge to the graph,
  /// iterate over all of the edges incident to a vertex. It also provides
  /// methods for returning the number of vertices <c>V</c> and the number
  /// of edges <c>E</c>. Parallel edges and self-loops are permitted.
  /// </para><para>
  /// This implementation uses an adjacency-lists representation, which
  /// is a vertex-indexed array of @link{Bag} objects.
  /// All operations take constant time (in the worst case) except
  /// iterating over the edges incident to a given vertex, which takes
  /// time proportional to the number of such edges.
  /// </para></summary>
  /// <remarks>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/43mst">Section 4.3</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/EdgeWeightedGraph.java.html">EdgeWeightedGraph</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.
  /// </para></remarks> 
  ///
  public class EdgeWeightedGraph
  {
    private static readonly string NEWLINE = Environment.NewLine;

    private readonly int numVertices;
    private int numEdges;
    private Bag<Edge>[] adj;

    /// <summary>
    /// Initializes an empty edge-weighted graph with <c>V</c> vertices and 0 edges.</summary>
    /// <param name="V">V the number of vertices</param>
    /// <exception cref="ArgumentException">if <c>V</c> &lt; 0</exception>
    ///
    public EdgeWeightedGraph(int V)
    {
      if (V < 0) throw new ArgumentException("Number of vertices must be nonnegative");
      numVertices = V;
      numEdges = 0;
      adj = new Bag<Edge>[V];
      for (int v = 0; v < V; v++)
      {
        adj[v] = new Bag<Edge>();
      }
    }

    /// <summary>
    /// Initializes a random edge-weighted graph with <c>V</c> vertices and <c>E</c> edges.</summary>
    /// <param name="V"> V the number of vertices</param>
    /// <param name="E"> E the number of edges</param>
    /// <exception cref="ArgumentException">if <c>V</c> &lt; 0</exception>
    /// <exception cref="ArgumentException">if <c>E</c> &lt; 0</exception>
    ///
    public EdgeWeightedGraph(int V, int E) : this(V)
    {
      if (E < 0) throw new ArgumentException("Number of edges must be nonnegative");
      for (int i = 0; i < E; i++)
      {
        int v = StdRandom.Uniform(V);
        int w = StdRandom.Uniform(V);
        double weight = Math.Round(100 * StdRandom.Uniform()) / 100.0;
        Edge e = new Edge(v, w, weight);
        AddEdge(e);
      }
    }

    /// <summary>
    /// Initializes an edge-weighted graph from an input stream.
    /// The format is the number of vertices <c>V</c>,
    /// followed by the number of edges <c>E</c>,
    /// followed by <c>E</c> pairs of vertices and edge weights,
    /// with each entry separated by whitespace.</summary>
    /// <param name="input"> in the input stream</param>
    /// <exception cref="IndexOutOfRangeException">if the endpoints of any edge are not in prescribed range</exception>
    /// <exception cref="ArgumentException">if the number of vertices or edges is negative</exception>
    ///
    public EdgeWeightedGraph(TextInput input) : this(input.ReadInt())
    {
      int E = input.ReadInt();
      if (E < 0) throw new ArgumentException("Number of edges must be nonnegative");
      for (int i = 0; i < E; i++)
      {
        int v = input.ReadInt();
        int w = input.ReadInt();
        double weight = input.ReadDouble();
        Edge e = new Edge(v, w, weight);
        AddEdge(e);
      }
    }

    /// <summary>
    /// Returns the number of vertices in this edge-weighted graph.</summary>
    /// <returns>the number of vertices in this edge-weighted graph</returns>
    ///
    public int V
    {
      get { return numVertices; }
    }

    /// <summary>
    /// Returns the number of edges in this edge-weighted graph.</summary>
    /// <returns>the number of edges in this edge-weighted graph</returns>
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
    /// Adds the undirected edge <c>e</c> to this edge-weighted graph.</summary>
    /// <param name="e"> e the edge</param>
    /// <exception cref="IndexOutOfRangeException">unless both endpoints are between 0 and V-1</exception>
    ///
    public void AddEdge(Edge e)
    {
      int v = e.Either;
      int w = e.Other(v);
      validateVertex(v);
      validateVertex(w);
      adj[v].Add(e);
      adj[w].Add(e);
      numEdges++;
    }

    /// <summary>
    /// Returns the edges incident on vertex <c>v</c>.</summary>
    /// <param name="v">v the vertex</param>
    /// <returns>the edges incident on vertex <c>v</c> as an Iterable</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public IEnumerable<Edge> Adj(int v)
    {
      validateVertex(v);
      return adj[v];
    }

    /// <summary>
    /// Returns the degree of vertex <c>v</c>.</summary>
    /// <param name="v">v the vertex</param>
    /// <returns>the degree of vertex <c>v</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public int Degree(int v)
    {
      validateVertex(v);
      return adj[v].Count;
    }

    /// <summary>
    /// Returns all edges in this edge-weighted graph.
    /// To iterate over the edges in this edge-weighted graph, use foreach notation:
    /// <c>foreach (Edge e in G.Edges())</c>.</summary>
    /// <returns>all edges in this edge-weighted graph, as an iterable</returns>
    ///
    public IEnumerable<Edge> Edges()
    {
      Bag<Edge> list = new Bag<Edge>();
      for (int v = 0; v < V; v++)
      {
        int selfLoops = 0;
        foreach (Edge e in Adj(v))
        {
          if (e.Other(v) > v)
          {
            list.Add(e);
          }
          // only add one copy of each self loop (self loops will be consecutive)
          else if (e.Other(v) == v)
          {
            if (selfLoops % 2 == 0) list.Add(e);
            selfLoops++;
          }
        }
      }
      return list;
    }

    /// <summary>
    /// Returns a string representation of the edge-weighted graph.
    /// This method takes time proportional to <c>E</c> + <c>V</c>.</summary>
    /// <returns>the number of vertices <c>V</c>, followed by the number of edges <c>E</c>,
    ///        followed by the <c>V</c> adjacency lists of edges</returns>
    ///
    public override string ToString()
    {
      StringBuilder s = new StringBuilder();
      s.Append(V + " " + E + NEWLINE);
      for (int v = 0; v < V; v++)
      {
        s.Append(v + ": ");
        foreach (Edge e in Adj(v))
        {
          s.Append(e + "  ");
        }
        s.Append(NEWLINE);
      }
      return s.ToString();
    }

    /// <summary>Demo test the <c>EdgeWeightedGraph</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    ///
    [HelpText("algscmd EdgeWeightedGraph tinyEWG.txt", "File with the pre-defined format for directed graph")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      EdgeWeightedGraph G = new EdgeWeightedGraph(input);
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
