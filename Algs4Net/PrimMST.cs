/******************************************************************************
 *  File name :    PrimMST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *                IndexMinPQ.java UF.java In.java Console.java
 *  Data files:   http://algs4.cs.princeton.edu/43mst/tinyEWG.txt
 *                http://algs4.cs.princeton.edu/43mst/mediumEWG.txt
 *                http://algs4.cs.princeton.edu/43mst/largeEWG.txt
 *
 *  Compute a minimum spanning forest using Prim's algorithm.
 *
 * C:\> algscmd PrimMST tinyEWG.txt 
 *  1-7 0.19000
 *  0-2 0.26000
 *  2-3 0.17000
 *  4-5 0.35000
 *  5-7 0.28000
 *  6-2 0.40000
 *  0-7 0.16000
 *  1.81000
 *
 *  C:\> algscmd PrimMST mediumEWG.txt
 *  1-72   0.06506
 *  2-86   0.05980
 *  3-67   0.09725
 *  4-55   0.06425
 *  5-102  0.03834
 *  6-129  0.05363
 *  7-157  0.00516
 *  ...
 *  10.46351
 *
 *  C:\> algscmd PrimMST largeEWG.txt
 *  ...
 *  647.66307
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>PrimMST</c> class represents a data type for computing a
  /// <c>Minimum spanning tree</c> in an edge-weighted graph.
  /// The edge weights can be positive, zero, or negative and need not
  /// be distinct. If the graph is not connected, it computes a <c>Minimum
  /// spanning forest</c>, which is the union of minimum spanning trees
  /// in each connected component. The <c>Weight</c> property returns the 
  /// weight of a minimum spanning tree and the <c>Edges</c> property
  /// returns its edges.</para><para>
  /// This implementation uses <c>Prim's algorithm</c> with an indexed
  /// binary heap.
  /// The constructor takes time proportional to <c>E</c> log <c>V</c>
  /// and extra space (not including the graph) proportional to <c>V</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>Weight</c> property takes constant time
  /// and the <c>Edges</c> method takes time proportional to <c>V</c>.
  /// </para><para>For alternate implementations, see <seealso cref="LazyPrimMST"/>, 
  /// <seealso cref="KruskalMST"/>, and <seealso cref="BoruvkaMST"/>.</para>
  /// </summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/43mst">Section 4.3</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/PrimMST.java.html">PrimMST</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class PrimMST
  {
    private static readonly double FLOATING_POINT_EPSILON = 1E-12;

    private Edge[] edgeTo;        // edgeTo[v] = shortest edge from tree vertex to non-tree vertex
    private double[] distTo;      // distTo[v] = weight of shortest such edge
    private bool[] marked;        // marked[v] = true if v on tree, false otherwise
    private IndexMinPQ<double> pq;

    /// <summary>
    /// Compute a minimum spanning tree (or forest) of an edge-weighted graph.</summary>
    /// <param name="G">the edge-weighted graph</param>
    ///
    public PrimMST(EdgeWeightedGraph G)
    {
      edgeTo = new Edge[G.V];
      distTo = new double[G.V];
      marked = new bool[G.V];
      pq = new IndexMinPQ<double>(G.V);
      for (int v = 0; v < G.V; v++)
        distTo[v] = double.PositiveInfinity;

      for (int v = 0; v < G.V; v++)      // run from each vertex to find
        if (!marked[v]) prim(G, v);      // minimum spanning forest

      // check optimality conditions
      Debug.Assert(check(G));
    }

    // run Prim's algorithm in graph G, starting from vertex s
    private void prim(EdgeWeightedGraph G, int s)
    {
      distTo[s] = 0.0;
      pq.Insert(s, distTo[s]);
      while (!pq.IsEmpty)
      {
        int v = pq.DelMin();
        scan(G, v);
      }
    }

    // scan vertex v
    private void scan(EdgeWeightedGraph G, int v)
    {
      marked[v] = true;
      foreach (Edge e in G.Adj(v))
      {
        int w = e.Other(v);
        if (marked[w]) continue;         // v-w is obsolete edge
        if (e.Weight < distTo[w])
        {
          distTo[w] = e.Weight;
          edgeTo[w] = e;
          if (pq.Contains(w)) pq.DecreaseKey(w, distTo[w]);
          else pq.Insert(w, distTo[w]);
        }
      }
    }

    /// <summary>
    /// Returns the edges in a minimum spanning tree (or forest).</summary>
    /// <returns>the edges in a minimum spanning tree (or forest) as
    /// an iterable of edges</returns>
    ///
    public IEnumerable<Edge> Edges()
    {
      LinkedQueue<Edge> mst = new LinkedQueue<Edge>();
      for (int v = 0; v < edgeTo.Length; v++)
      {
        Edge e = edgeTo[v];
        if (e != null)
        {
          mst.Enqueue(e);
        }
      }
      return mst;
    }

    /// <summary>
    /// Returns the sum of the edge weights in a minimum spanning tree (or forest).</summary>
    /// <returns>the sum of the edge weights in a minimum spanning tree (or forest)</returns>
    ///
    public double Weight
    {
      get {
        double weight = 0.0;
        foreach (Edge e in Edges())
          weight += e.Weight;
        return weight;
      }
    }


    // check optimality conditions (takes time proportional to E V lg* V)
    private bool check(EdgeWeightedGraph G)
    {
      // check weight
      double totalWeight = 0.0;
      foreach (Edge e in Edges())
      {
        totalWeight += e.Weight;
      }
      if (Math.Abs(totalWeight - Weight) > FLOATING_POINT_EPSILON)
      {
        Console.Error.Write("Weight of edges does not equal Weight: {0} vs. {1}\n", totalWeight, Weight);
        return false;
      }

      // check that it is acyclic
      UF uf = new UF(G.V);
      foreach (Edge e in Edges())
      {
        int v = e.Either, w = e.Other(v);
        if (uf.Connected(v, w))
        {
          Console.Error.WriteLine("Not a forest");
          return false;
        }
        uf.Union(v, w);
      }

      // check that it is a spanning forest
      foreach (Edge e in Edges())
      {
        int v = e.Either, w = e.Other(v);
        if (!uf.Connected(v, w))
        {
          Console.Error.WriteLine("Not a spanning forest");
          return false;
        }
      }

      // check that it is a minimal spanning forest (cut optimality conditions)
      foreach (Edge e in Edges())
      {

        // all edges in MST except e
        uf = new UF(G.V);
        foreach (Edge f in Edges())
        {
          int x = f.Either, y = f.Other(x);
          if (f != e) uf.Union(x, y);
        }

        // check that e is min weight edge in crossing cut
        foreach (Edge f in G.Edges())
        {
          int x = f.Either, y = f.Other(x);
          if (!uf.Connected(x, y))
          {
            if (f.Weight < e.Weight)
            {
              Console.Error.WriteLine("Edge " + f + " violates cut optimality conditions");
              return false;
            }
          }
        }
      }
      return true;
    }

    /// <summary>
    /// Demo test the <c>PrimMST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd PrimMST tinyEWG.txt", "File with the pre-defined format for directed, weighted graph")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      EdgeWeightedGraph G = new EdgeWeightedGraph(input);

      PrimMST mst = new PrimMST(G);
      foreach (Edge e in mst.Edges())
      {
        Console.WriteLine(e);
      }
      Console.WriteLine("{0:F5}", mst.Weight);
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

