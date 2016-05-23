/******************************************************************************
 *  File name :    BoruvkaMST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *            
 *  Data files:   http://algs4.cs.princeton.edu/43mst/tinyEWG.txt
 *                http://algs4.cs.princeton.edu/43mst/mediumEWG.txt
 *                http://algs4.cs.princeton.edu/43mst/largeEWG.txt
 *
 *  Compute a minimum spanning forest using Boruvka's algorithm.
 *
 *  C:\> algscmd BoruvkaMST tinyEWG.txt 
 *  0-2 0.26000
 *  6-2 0.40000
 *  5-7 0.28000
 *  4-5 0.35000
 *  2-3 0.17000
 *  1-7 0.19000
 *  0-7 0.16000
 *  1.81000
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>BoruvkaMST</c> class represents a data type for computing a
  /// <c>Minimum spanning tree</c> in an edge-weighted graph.
  /// The edge weights can be positive, zero, or negative and need not
  /// be distinct. If the graph is not connected, it computes a <em>Minimum
  /// spanning forest</em>, which is the union of minimum spanning trees
  /// in each connected component. The <c>Weight</c> method returns the 
  /// weight of a minimum spanning tree and the <c>Edges()</c> method
  /// returns its edges.
  /// </para><para>
  /// This implementation uses <c>Boruvka's algorithm</c> and the union-find
  /// data type.</para><para>
  /// The constructor takes time proportional to <c>E</c> log <c>V</c>
  /// and extra space (not including the graph) proportional to <c>V</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>weight()</c> method takes constant time
  /// and the <c>edges()</c> method takes time proportional to <c>V</c>.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/43mst">Section 4.3</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BoruvkaMST.java.html">BoruvkaMST</a>
  /// implementation by the respective authors.</para></remarks>
  /// For alternate implementations, see <seealso cref="LazyPrimMST"/>, <seealso cref="PrimMST"/>,
  /// and <seealso cref="KruskalMST"/>.
  ///
  public class BoruvkaMST
  {
    private static readonly double FLOATING_POINT_EPSILON = 1E-12;

    private Bag<Edge> mst = new Bag<Edge>();    // edges in MST
    private double weight;                      // weight of MST

    /// <summary>
    /// Compute a minimum spanning tree (or forest) of an edge-weighted graph.</summary>
    /// <param name="G">the edge-weighted graph</param>
    ///
    public BoruvkaMST(EdgeWeightedGraph G)
    {
      UF uf = new UF(G.V);

      // repeat at most log V times or until we have V-1 edges
      for (int t = 1; t < G.V && mst.Count < G.V - 1; t = t + t)
      {

        // foreach tree in forest, find closest edge
        // if edge weights are equal, ties are broken in favor of first edge in G.edges()
        Edge[] closest = new Edge[G.V];
        foreach (Edge e in G.Edges())
        {
          int v = e.Either, w = e.Other(v);
          int i = uf.Find(v), j = uf.Find(w);
          if (i == j) continue;   // same tree
          if (closest[i] == null || less(e, closest[i])) closest[i] = e;
          if (closest[j] == null || less(e, closest[j])) closest[j] = e;
        }

        // add newly discovered edges to MST
        for (int i = 0; i < G.V; i++)
        {
          Edge e = closest[i];
          if (e != null)
          {
            int v = e.Either, w = e.Other(v);
            // don't add the same edge twice
            if (!uf.Connected(v, w))
            {
              mst.Add(e);
              weight += e.Weight;
              uf.Union(v, w);
            }
          }
        }
      }

      // check optimality conditions
      Debug.Assert(check(G));
    }

    /// <summary>
    /// Returns the edges in a minimum spanning tree (or forest).</summary>
    /// <returns>the edges in a minimum spanning tree (or forest) as
    /// an iterable of edges</returns>
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

    // is the weight of edge e strictly less than that of edge f?
    private static bool less(Edge e, Edge f)
    {
      return e.Weight < f.Weight;
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
    /// Demo test the <c>BoruvkaMST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BoruvkaMST tinyEWG.txt", "File with the pre-defined format for directed, weighted graph")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      EdgeWeightedGraph G = new EdgeWeightedGraph(input);
      BoruvkaMST mst = new BoruvkaMST(G);
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
