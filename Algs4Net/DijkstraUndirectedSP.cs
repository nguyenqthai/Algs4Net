/******************************************************************************
 *  File name :    DijkstraUndirectedSP.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/43mst/tinyEWG.txt
 *                http://algs4.cs.princeton.edu/43mst/mediumEWG.txt
 *                http://algs4.cs.princeton.edu/43mst/largeEWG.txt
 *
 *  Dijkstra's algorithm. Computes the shortest path tree.
 *  Assumes all weights are nonnegative.
 *
 *  C:\> algscmd DijkstraUndirectedSP tinyEWG.txt 4
 *  4 to  0 (0.38)  0-4 0.38000
 *  4 to  1 (0.56)  4-7 0.37000   1-7 0.19000
 *  4 to  2 (0.64)  0-4 0.38000   0-2 0.26000
 *  4 to  3 (0.81)  0-4 0.38000   0-2 0.26000   2-3 0.17000
 *  4 to  4 (0.00)
 *  4 to  5 (0.35)  4-5 0.35000
 *  4 to  6 (0.93)  6-4 0.93000
 *  4 to  7 (0.37)  4-7 0.37000
 *
 *  C:\> algscmd DijkstraUndirectedSP mediumEWG.txt 0
 *  0 to 0 (0.00)
 *  0 to 1 (0.71)  0-44 0.06471   44-93  0.06793  ...   1-107 0.07484
 *  0 to 2 (0.65)  0-44 0.06471   44-231 0.10384  ...   2-42  0.11456
 *  0 to 3 (0.46)  0-97 0.07705   97-248 0.08598  ...   3-45  0.11902
 *  ...
 *
 *  C:\> algscmd DijkstraUndirectedSP largeEWG.txt 0
 *  0 to 0 (0.00)  
 *  0 to 1 (0.78)  0-460790 0.00190  460790-696678 0.00173   ...   1-826350 0.00191
 *  0 to 2 (0.61)  0-15786  0.00130  15786-53370   0.00113   ...   2-793420 0.00040
 *  0 to 3 (0.31)  0-460790 0.00190  460790-752483 0.00194   ...   3-698373 0.00172
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DijkstraUndirectedSP</c> class represents a data type for solving
  /// the single-source shortest paths problem in edge-weighted graphs
  /// where the edge weights are nonnegative.
  /// </para><para>
  /// This implementation uses Dijkstra's algorithm with a binary heap.
  /// The constructor takes time proportional to <c>E</c> log <c>V</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>DistTo()</c> and <c>HasPathTo()</c> methods take
  /// constant time and the <c>PathTo()</c> method takes time proportional to the
  /// number of edges in the shortest path returned.</para>
  /// <para>See <seealso cref="DijkstraSP"/> for a version on edge-weighted digraphs.
  /// </para></summary>
  /// <remarks><para>For additional documentation,    
  /// see <a href="http://algs4.cs.princeton.edu/44sp">Section 4.4</a> of    
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DijkstraUndirectedSP.java.html">DijkstraUndirectedSP</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DijkstraUndirectedSP
  {
    private double[] distTo;          // distTo[v] = distance  of shortest s->v path
    private Edge[] edgeTo;            // edgeTo[v] = last edge on shortest s->v path
    private IndexMinPQ<double> pq;    // priority queue of vertices

    /// <summary>
    /// Computes a shortest-paths tree from the source vertex <c>s</c> to every
    /// other vertex in the edge-weighted graph <c>G</c>.</summary>
    /// <param name="G">the edge-weighted digraph</param>
    /// <param name="s">the source vertex</param>
    /// <exception cref="ArgumentException">if an edge weight is negative</exception>
    /// <exception cref="ArgumentException">unless 0 &lt;= <c>s</c> &lt;= <c>V</c> - 1</exception>
    ///
    public DijkstraUndirectedSP(EdgeWeightedGraph G, int s)
    {
      foreach (Edge e in G.Edges())
      {
        if (e.Weight < 0)
          throw new ArgumentException("edge " + e + " has negative weight");
      }

      distTo = new double[G.V];
      edgeTo = new Edge[G.V];
      for (int v = 0; v < G.V; v++)
        distTo[v] = double.PositiveInfinity;
      distTo[s] = 0.0;

      // relax vertices in order of distance from s
      pq = new IndexMinPQ<double>(G.V);
      pq.Insert(s, distTo[s]);
      while (!pq.IsEmpty)
      {
        int v = pq.DelMin();
        foreach (Edge e in G.Adj(v))
          relax(e, v);
      }

      // check optimality conditions
      Debug.Assert(check(G, s));
    }

    // relax edge e and update pq if changed
    private void relax(Edge e, int v)
    {
      int w = e.Other(v);
      if (distTo[w] > distTo[v] + e.Weight)
      {
        distTo[w] = distTo[v] + e.Weight;
        edgeTo[w] = e;
        if (pq.Contains(w)) pq.DecreaseKey(w, distTo[w]);
        else pq.Insert(w, distTo[w]);
      }
    }

    /// <summary>
    /// Returns the length of a shortest path between the source vertex <c>s</c> and
    /// vertex <c>v</c>.</summary>
    /// <param name="v">the destination vertex</param>
    /// <returns>the length of a shortest path between the source vertex <c>s</c> and
    /// the vertex <c>v</c>; <c>Double.POSITIVE_INFINITY</c> if no such path</returns>
    ///
    public double DistTo(int v)
    {
      return distTo[v];
    }

    /// <summary>
    /// Returns true if there is a path between the source vertex <c>s</c> and
    /// vertex <c>v</c>.</summary>
    /// <param name="v">the destination vertex</param>
    /// <returns><c>true</c> if there is a path between the source vertex
    /// <c>s</c> to vertex <c>v</c>; <c>false</c> otherwise</returns>
    ///
    public bool HasPathTo(int v)
    {
      return distTo[v] < double.PositiveInfinity;
    }

    /// <summary>
    /// Returns a shortest path between the source vertex <c>s</c> and vertex <c>v</c>.</summary>
    /// <param name="v">the destination vertex</param>
    /// <returns>a shortest path between the source vertex <c>s</c> and vertex <c>v</c>;
    /// <c>null</c> if no such path</returns>
    ///
    public IEnumerable<Edge> PathTo(int v)
    {
      if (!HasPathTo(v)) return null;
      LinkedStack<Edge> path = new LinkedStack<Edge>();
      int x = v;
      for (Edge e = edgeTo[v]; e != null; e = edgeTo[x])
      {
        path.Push(e);
        x = e.Other(x);
      }
      return path;
    }


    // check optimality conditions:
    // (i) for all edges e = v-w:            distTo[w] <= distTo[v] + e.Weight
    // (ii) for all edge e = v-w on the SPT: distTo[w] == distTo[v] + e.Weight
    private bool check(EdgeWeightedGraph G, int s)
    {

      // check that edge weights are nonnegative
      foreach (Edge e in G.Edges())
      {
        if (e.Weight < 0)
        {
          Console.Error.WriteLine("negative edge weight detected");
          return false;
        }
      }

      // check that distTo[v] and edgeTo[v] are consistent
      if (distTo[s] != 0.0 || edgeTo[s] != null)
      {
        Console.Error.WriteLine("distTo[s] and edgeTo[s] inconsistent");
        return false;
      }
      for (int v = 0; v < G.V; v++)
      {
        if (v == s) continue;
        if (edgeTo[v] == null && distTo[v] != double.PositiveInfinity)
        {
          Console.Error.WriteLine("distTo[] and edgeTo[] inconsistent");
          return false;
        }
      }

      // check that all edges e = v-w satisfy distTo[w] <= distTo[v] + e.Weight
      for (int v = 0; v < G.V; v++)
      {
        foreach (Edge e in G.Adj(v))
        {
          int w = e.Other(v);
          if (distTo[v] + e.Weight < distTo[w])
          {
            Console.Error.WriteLine("edge " + e + " not relaxed");
            return false;
          }
        }
      }

      // check that all edges e = v-w on SPT satisfy distTo[w] == distTo[v] + e.Weight
      for (int w = 0; w < G.V; w++)
      {
        if (edgeTo[w] == null) continue;
        Edge e = edgeTo[w];
        if (w != e.Either && w != e.Other(e.Either)) return false;
        int v = e.Other(w);
        if (distTo[v] + e.Weight != distTo[w])
        {
          Console.Error.WriteLine("edge " + e + " on shortest path not tight");
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Demo test the <c>DijkstraUndirectedSP</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DijkstraUndirectedSP tinyEWG.txt s", "File with the format for undirected, weighted graph and a source")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      EdgeWeightedGraph G = new EdgeWeightedGraph(input);
      int s = int.Parse(args[1]);

      // compute shortest paths
      DijkstraUndirectedSP sp = new DijkstraUndirectedSP(G, s);

      // print shortest path
      for (int t = 0; t < G.V; t++)
      {
        if (sp.HasPathTo(t))
        {
          Console.Write("{0,2} to {1,2} ({2:F2})  ", s, t, sp.DistTo(t));
          foreach (Edge e in sp.PathTo(t))
          {
            Console.Write(e + "   ");
          }
          Console.WriteLine();
        }
        else
        {
          Console.Write("{0} to {0}         no path\n", s, t);
        }
      }
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

