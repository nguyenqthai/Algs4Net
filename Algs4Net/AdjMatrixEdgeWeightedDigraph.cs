/******************************************************************************
 *  File name :    AdjMatrixEdgeWeightedDigraph.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  An edge-weighted digraph, implemented using an adjacency matrix.
 *  Parallel edges are disallowed; self-loops are allowed.
 *  
 *  C:\> algscmd AdjMatrixEdgeWeightedDigraph 5 18
 *  5 18
 *  0: 0->1 0.20  0->2 0.91  0->3 0.00
 *  1: 1->2 0.86  1->3 0.55
 *  2: 2->0 0.24  2->3 0.18  2->4 0.61
 *  3: 3->0 0.29  3->1 0.38  3->2 0.28  3->3 0.70  3->4 0.17
 *  4: 4->0 0.50  4->1 0.10  4->2 0.96  4->3 0.64  4->4 0.01
 *  
 ******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>AdjMatrixEdgeWeightedDigraph</c> class represents a edge-weighted
  /// digraph of vertices named 0 through <c>V</c> - 1, where each
  /// directed edge is of type <seealso cref="DirectedEdge"/> and has a real-valued weight.
  /// It supports the following two primary operations: add a directed edge
  /// to the digraph and iterate over all of edges incident from a given vertex.
  /// It also provides methods for returning the number of vertices <c>V</c> and the number
  /// of edges <c>E</c>. Parallel edges are disallowed; self-loops are permitted.
  /// </para><para>
  /// This implementation uses an adjacency-matrix representation.
  /// All operations take constant time (in the worst case) except
  /// iterating over the edges incident from a given vertex, which takes
  /// time proportional to <c>V</c>.</para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/44sp">Section 4.4</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/AdjMatrixEdgeWeightedDigraph.java.html">AdjMatrixEdgeWeightedDigraph</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class AdjMatrixEdgeWeightedDigraph
  {
    private static readonly string NEWLINE = Environment.NewLine;

    private int numVertices;
    private int numEdges;
    private DirectedEdge[,] adj;

    /// <summary>
    /// Initializes an empty edge-weighted digraph with <c>V</c> vertices and 0 edges.</summary>
    /// <param name="V">the number of vertices</param>
    /// <exception cref="ArgumentException">if <c>V</c> &lt; 0</exception>
    ///
    public AdjMatrixEdgeWeightedDigraph(int V)
    {
      if (V < 0) throw new ArgumentException("Number of vertices must be nonnegative");
      numVertices = V;
      numEdges = 0;
      adj = new DirectedEdge[V,V];
    }

    /// <summary>
    /// Initializes a random edge-weighted digraph with <c>V</c> vertices and <c>E</c> edges.</summary>
    /// <param name="V">the number of vertices</param>
    /// <param name="E">the number of edges</param>
    /// <exception cref="ArgumentException">if <c>V</c> &lt; 0</exception>
    /// <exception cref="ArgumentException">if <c>E</c> &lt; 0</exception>
    ///
    public AdjMatrixEdgeWeightedDigraph(int V, int E) : this(V)
    {
      if (E < 0) throw new ArgumentException("Number of edges must be nonnegative");
      if (E > V * V) throw new ArgumentException("Too many edges");

      // can be inefficient
      while (numEdges != E)
      {
        int v = StdRandom.Uniform(V);
        int w = StdRandom.Uniform(V);
        double weight = Math.Round(100 * StdRandom.Uniform()) / 100.0;
        AddEdge(new DirectedEdge(v, w, weight));
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
    public AdjMatrixEdgeWeightedDigraph(TextInput input) : this(input.ReadInt())
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
    /// Returns the number of vertices in the edge-weighted digraph.</summary>
    /// <returns>the number of vertices in the edge-weighted digraph</returns>
    ///
    public int V
    {
      get { return numVertices; }
    }

    /// <summary>
    /// Returns the number of edges in the edge-weighted digraph.</summary>
    /// <returns>the number of edges in the edge-weighted digraph</returns>
    ///
    public int E
    {
      get { return numEdges; }
    }

    /// <summary>
    /// Adds the directed edge <c>e</c> to the edge-weighted digraph (if there
    /// is not already an edge with the same endpoints).</summary>
    /// <param name="e">the edge</param>
    ///
    public void AddEdge(DirectedEdge e)
    {
      int v = e.From;
      int w = e.To;
      if (adj[v,w] == null)
      {
        numEdges++;
        adj[v,w] = e;
      }
    }

    /// <summary>
    /// Returns the directed edges incident from vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the directed edges incident from vertex <c>v</c> as an Iterable</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public IEnumerable<DirectedEdge> Adj(int v)
    {
      return new AdjIEnumerator(adj, v);
    }

    // support iteration over graph vertices
    private class AdjIEnumerator : IEnumerable<DirectedEdge>
    {
      //List<DirectedEdge> adjacents;
      DirectedEdge[,] adj;
      private int v;    // the vertex under consideration

      public AdjIEnumerator(DirectedEdge[,] adj, int v)
      {
        //adjacents = new List<DirectedEdge>();
        //int V = adj.GetLength(0);
        //for (int e=0; i<V; i++)

        this.adj = adj;
        this.v = v;
      }

      public IEnumerator<DirectedEdge> GetEnumerator()
      {
        int V = adj.GetLength(0);
        int e = 0;
        while (e < V && adj[v, e] == null) e++;
        if (e < V)
        {
          DirectedEdge current = adj[v, e];
          while (e < V)
          {
            yield return current;
            e++;
            while (e < V && adj[v, e] == null) e++;
            if (e == V) break;
            current = adj[v, e];
          }
        }
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return GetEnumerator();
      }

    }

    /// <summary>
    /// Returns a string representation of the edge-weighted digraph. This method takes
    /// time proportional to <c>V</c><sup>2</sup>.</summary>
    /// <returns>the number of vertices <c>V</c>, followed by the number of edges <c>E</c>,
    /// followed by the <c>V</c> adjacency lists of edges</returns>
    ///
    public override string ToString()
    {
      StringBuilder s = new StringBuilder();
      s.Append(V + " " + E + NEWLINE);
      for (int v = 0; v < V; v++)
      {
        s.Append(v + ": ");
        IEnumerable<DirectedEdge> adjacents = Adj(v);
        foreach (DirectedEdge e in adjacents)
        {
          s.Append(e + "  ");
        }
        s.Append(NEWLINE);
      }
      return s.ToString();
    }

    /// <summary>
    /// Demo test the <c>AdjMatrixEdgeWeightedDigraph</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd AdjMatrixEdgeWeightedDigraph V E", "V, E-number of vertices and number of edges")]
    public static void MainTest(string[] args)
    {
      int V = int.Parse(args[0]);
      int E = int.Parse(args[1]);
      AdjMatrixEdgeWeightedDigraph G = new AdjMatrixEdgeWeightedDigraph(V, E);
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
