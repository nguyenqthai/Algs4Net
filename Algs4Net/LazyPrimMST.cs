/******************************************************************************
 *  File name :    LazyPrimMST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *                MinPQ.java UF.java In.java Console.java
 *  Data files:   http://algs4.cs.princeton.edu/43mst/tinyEWG.txt
 *                http://algs4.cs.princeton.edu/43mst/mediumEWG.txt
 *                http://algs4.cs.princeton.edu/43mst/largeEWG.txt
 *
 *  Compute a minimum spanning forest using a lazy version of Prim's 
 *  algorithm.
 *
 *  C:\> algscmd LazyPrimMST tinyEWG.txt 
 *  0-7 0.16000
 *  1-7 0.19000
 *  0-2 0.26000
 *  2-3 0.17000
 *  5-7 0.28000
 *  4-5 0.35000
 *  6-2 0.40000
 *  1.81000
 *
 *  C:\> algscmd LazyPrimMST mediumEWG.txt
 *  0-225   0.02383
 *  49-225  0.03314
 *  44-49   0.02107
 *  44-204  0.01774
 *  49-97   0.03121
 *  202-204 0.04207
 *  176-202 0.04299
 *  176-191 0.02089
 *  68-176  0.04396
 *  58-68   0.04795
 *  10.46351
 *
 *  C:\> algscmd LazyPrimMST largeEWG.txt
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
  /// The <c>LazyPrimMST</c> class represents a data type for computing a
  /// <c>Minimum spanning tree</c> in an edge-weighted graph.
  /// The edge weights can be positive, zero, or negative and need not
  /// be distinct. If the graph is not connected, it computes a <c>Minimum
  /// spanning forest</c>, which is the union of minimum spanning trees
  /// in each connected component. The <c>weight()</c> method returns the 
  /// weight of a minimum spanning tree and the <c>edges()</c> method
  /// returns its edges.</para><para>
  /// This implementation uses a lazy version of <c>Prim's algorithm</c>
  /// with a binary heap of edges.
  /// The constructor takes time proportional to <c>E</c> log <c>E</c>
  /// and extra space (not including the graph) proportional to <c>E</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>weight()</c> method takes constant time
  /// and the <c>edges()</c> method takes time proportional to <c>V</c>.
  /// </para><para>
  /// For alternate implementations, see <seealso cref="PrimMST"/>, <seealso cref="KruskalMST"/>,
  /// and <seealso cref="BoruvkaMST"/>.</para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/43mst">Section 4.3</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/LazyPrimMST.java.html">LazyPrimMST</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class LazyPrimMST
  {
    private static readonly double FLOATING_POINT_EPSILON = 1E-12;

    private double weight;       // total weight of MST
    private LinkedQueue<Edge> mst;     // edges in the MST
    private bool[] marked;    // marked[v] = true if v on tree
    private MinPQ<Edge> pq;      // edges with one endpoint in tree

    /// <summary>
    /// Compute a minimum spanning tree (or forest) of an edge-weighted graph.</summary>
    /// <param name="G">the edge-weighted graph</param>
    ///
    public LazyPrimMST(EdgeWeightedGraph G)
    {
      mst = new LinkedQueue<Edge>();
      pq = new MinPQ<Edge>();
      marked = new bool[G.V];
      for (int v = 0; v < G.V; v++)     // run Prim from all vertices to
        if (!marked[v]) prim(G, v);     // get a minimum spanning forest

      // check optimality conditions
      Debug.Assert(check(G));
    }

    // run Prim's algorithm
    private void prim(EdgeWeightedGraph G, int s)
    {
      scan(G, s);
      while (!pq.IsEmpty)
      {                        // better to stop when mst has V-1 edges
        Edge e = pq.DelMin();                   // smallest edge on pq
        int v = e.Either, w = e.Other(v);       // two endpoints
        Debug.Assert(marked[v] || marked[w]);
        if (marked[v] && marked[w]) continue;   // lazy, both v and w already scanned
        mst.Enqueue(e);                         // add e to MST
        weight += e.Weight;
        if (!marked[v]) scan(G, v);             // v becomes part of tree
        if (!marked[w]) scan(G, w);             // w becomes part of tree
      }
    }

    // add all edges e incident to v onto pq if the other endpoint has not yet been scanned
    private void scan(EdgeWeightedGraph G, int v)
    {
      Debug.Assert(!marked[v]);
      marked[v] = true;
      foreach (Edge e in G.Adj(v))
        if (!marked[e.Other(v)]) pq.Insert(e);
    }

    /// <summary>
    /// Returns the edges in a minimum spanning tree (or forest).</summary>
    /// <returns>the edges in a minimum spanning tree (or forest) as
    ///   an iterable of edges</returns>
    ///
    public IEnumerable<Edge> Edges()
    {
      return mst;
    }

    /// <summary>
    /// Returns the sum of the edge weights in a minimum spanning tree (or forest).</summary>
    /// <returns>the sum of the edge weights in a minimum spanning tree (or forest)</returns>
    ///
    public double Weight
    {
      get { return weight; }
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
        Console.Error.Write("Weight of edges does not equal weight(): {0} vs. {1}\n", totalWeight, Weight);
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
      foreach (Edge e in G.Edges())
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
        foreach (Edge f in mst)
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
    /// Demo test the <c>LazyPrimMST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd LazyPrimMST tinyEWG.txt", "File with the pre-defined format for directed, weighted graph")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      EdgeWeightedGraph G = new EdgeWeightedGraph(input);
      LazyPrimMST mst = new LazyPrimMST(G);
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

