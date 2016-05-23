/******************************************************************************
 *  File name :    KruskalMST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *                UF.java In.java Console.java
 *  Data files:   http://algs4.cs.princeton.edu/43mst/tinyEWG.txt
 *                http://algs4.cs.princeton.edu/43mst/mediumEWG.txt
 *                http://algs4.cs.princeton.edu/43mst/largeEWG.txt
 *
 *  Compute a minimum spanning forest using Kruskal's algorithm.
 *
 *  C:\> algscmd KruskalMST tinyEWG.txt 
 *  0-7 0.16000
 *  2-3 0.17000
 *  1-7 0.19000
 *  0-2 0.26000
 *  5-7 0.28000
 *  4-5 0.35000
 *  6-2 0.40000
 *  1.81000
 *
 *  C:\> algscmd KruskalMST mediumEWG.txt
 *  168-231 0.00268
 *  151-208 0.00391
 *  7-157   0.00516
 *  122-205 0.00647
 *  8-152   0.00702
 *  156-219 0.00745
 *  28-198  0.00775
 *  38-126  0.00845
 *  10-123  0.00886
 *  ...
 *  10.46351
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>KruskalMST</c> class represents a data type for computing a
  /// <c>Minimum spanning tree</c> in an edge-weighted graph.
  /// The edge weights can be positive, zero, or negative and need not
  /// be distinct. If the graph is not connected, it computes a <c>Minimum
  /// spanning forest</c>, which is the union of minimum spanning trees
  /// in each connected component. The <c>weight()</c> property returns the 
  /// weight of a minimum spanning tree and the <c>Edge()</c> property
  /// returns its edges.</para><para>
  /// This implementation uses <c>Krusal's algorithm</c> and the
  /// union-find data type.
  /// The constructor takes time proportional to <c>E</c> log <c>E</c>
  /// and extra space (not including the graph) proportional to <c>V</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>Weight</c> property takes constant time
  /// and the <c>Edges</c> method takes time proportional to <c>V</c>.
  /// </para><para>
  /// For alternate implementations, see <seealso cref="LazyPrimMST"/>, <seealso cref="PrimMST"/>,
  /// and <seealso cref="BoruvkaMST"/>.</para>
  /// </summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/43mst">Section 4.3</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/KruskalMST.java.html">KruskalMST</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class KruskalMST
  {
    private static readonly double FLOATING_POINT_EPSILON = 1E-12;

    private double weight;                                    // weight of MST
    private LinkedQueue<Edge> mst = new LinkedQueue<Edge>();  // edges in MST

    /// <summary>
    /// Compute a minimum spanning tree (or forest) of an edge-weighted graph.</summary>
    /// <param name="G">the edge-weighted graph</param>
    ///
    public KruskalMST(EdgeWeightedGraph G)
    {
      // more efficient to build heap by passing array of edges
      MinPQ<Edge> pq = new MinPQ<Edge>();
      foreach (Edge e in G.Edges())
      {
        pq.Insert(e);
      }

      // run greedy algorithm
      UF uf = new UF(G.V);
      while (!pq.IsEmpty && mst.Count < G.V - 1)
      {
        Edge e = pq.DelMin();
        int v = e.Either;
        int w = e.Other(v);
        if (!uf.Connected(v, w))
        { // v-w does not create a cycle
          uf.Union(v, w);  // merge v and w components
          mst.Enqueue(e);  // add edge e to mst
          weight += e.Weight;
        }
      }

      // check optimality conditions
      Debug.Assert(check(G));
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
      // check total weight
      double total = 0.0;
      foreach (Edge e in Edges())
      {
        total += e.Weight;
      }
      if (Math.Abs(total - Weight) > FLOATING_POINT_EPSILON)
      {
        Console.Error.Write("Weight of edges does not equal Weight: {0} vs. {0}\n", total, Weight);
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
    /// Demo test the <c>KruskalMST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd KruskalMST tinyEWG.txt", "File with the pre-defined format for directed, weighted graph")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      EdgeWeightedGraph G = new EdgeWeightedGraph(input);

      KruskalMST mst = new KruskalMST(G);
      foreach (Edge e in mst.Edges())
      {
        Console.WriteLine(e);
      }
      Console.Write("{0:F5}\n", mst.Weight);
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
