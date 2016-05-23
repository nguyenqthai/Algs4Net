/******************************************************************************
 *  File name :    DijkstraSP.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/44sp/tinyEWD.txt
 *                http://algs4.cs.princeton.edu/44sp/mediumEWD.txt
 *                http://algs4.cs.princeton.edu/44sp/largeEWD.txt
 *
 *  Dijkstra's algorithm. Computes the shortest path tree.
 *  Assumes all weights are nonnegative.
 *
 *  C:\> algscmd DijkstraSP tinyEWG.txt 0
 *  0 to 0 (0.00)
 *  0 to 1 no path
 *  0 to 2 (0.26)  0->2 0.26
 *  0 to 3 (0.43)  0->2 0.26   2->3 0.17
 *  0 to 4 (0.38)  0->4 0.38
 *  0 to 5 (0.73)  0->4 0.38   4->5 0.35
 *  0 to 6 (0.95)  0->2 0.26   2->3 0.17   3->6 0.52
 *  0 to 7 (0.16)  0->7 0.16
 *
 *  C:\> algscmd DijkstraSP tinyEWG.txt 3
 *  ...
 *  3 to 247 no path
 *  3 to 248 (0.30)  3->45 0.12   45->104 0.11   104->248 0.07
 *  3 to 249 no path
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DijkstraSP</c> class represents a data type for solving the
  /// single-source shortest paths problem in edge-weighted digraphs
  /// where the edge weights are nonnegative.</para><para>
  /// This implementation uses Dijkstra's algorithm with a binary heap.
  /// The constructor takes time proportional to <c>E</c> log <c>V</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>DistTo()</c> and <c>HasPathTo()</c> methods take
  /// constant time and the <c>PathTo()</c> method takes time proportional to the
  /// number of edges in the shortest path returned.</para></summary>
  /// <remarks><para>For additional documentation,    
  /// see <a href="http://algs4.cs.princeton.edu/44sp">Section 4.4</a> of    
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DijkstraSP.java.html">DijkstraSP</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DijkstraSP
  {
    private double[] distTo;          // distTo[v] = distance  of shortest s->v path
    private DirectedEdge[] edgeTo;    // edgeTo[v] = last edge on shortest s->v path
    private IndexMinPQ<double> pq;    // priority queue of vertices

    /// <summary>Computes a shortest-paths tree from the source vertex <c>s</c> to every other
    /// vertex in the edge-weighted digraph <c>G</c>.</summary>
    /// <param name="G">the edge-weighted digraph</param>
    /// <param name="s">the source vertex</param>
    /// <exception cref="ArgumentException">if an edge weight is negative</exception>
    /// <exception cref="ArgumentException">unless 0 &lt;= <c>s</c> &lt;=e <c>V</c> - 1</exception>
    ///
    public DijkstraSP(EdgeWeightedDigraph G, int s)
    {
      foreach (DirectedEdge e in G.Edges())
      {
        if (e.Weight < 0)
          throw new ArgumentException("edge " + e + " has negative weight");
      }

      distTo = new double[G.V];
      edgeTo = new DirectedEdge[G.V];
      for (int v = 0; v < G.V; v++)
        distTo[v] = double.PositiveInfinity;
      distTo[s] = 0.0;

      // relax vertices in order of distance from s
      pq = new IndexMinPQ<double>(G.V);
      pq.Insert(s, distTo[s]);
      while (!pq.IsEmpty)
      {
        int v = pq.DelMin();
        foreach (DirectedEdge e in G.Adj(v))
          relax(e);
      }

      // check optimality conditions
      Debug.Assert(check(G, s));
    }

    // relax edge e and update pq if changed
    private void relax(DirectedEdge e)
    {
      int v = e.From, w = e.To;
      if (distTo[w] > distTo[v] + e.Weight)
      {
        distTo[w] = distTo[v] + e.Weight;
        edgeTo[w] = e;
        if (pq.Contains(w)) pq.DecreaseKey(w, distTo[w]);
        else pq.Insert(w, distTo[w]);
      }
    }

    /// <summary>
    /// Returns the length of a shortest path from the source vertex <c>s</c> to vertex <c>v</c>.</summary>
    /// <param name="v">the destination vertex</param>
    /// <returns>the length of a shortest path from the source vertex <c>s</c> to vertex <c>v</c>;
    ///        <c>double.POSITIVE_INFINITY</c> if no such path</returns>
    ///
    public double DistTo(int v)
    {
      return distTo[v];
    }

    /// <summary>
    /// Returns true if there is a path from the source vertex <c>s</c> to vertex <c>v</c>.</summary>
    /// <param name="v">the destination vertex</param>
    /// <returns><c>true</c> if there is a path from the source vertex
    ///        <c>s</c> to vertex <c>v</c>; <c>false</c> otherwise</returns>
    ///
    public bool HasPathTo(int v)
    {
      return distTo[v] < double.PositiveInfinity;
    }

    /// <summary>
    /// Returns a shortest path from the source vertex <c>s</c> to vertex <c>v</c>.</summary>
    /// <param name="v">the destination vertex</param>
    /// <returns>a shortest path from the source vertex <c>s</c> to vertex <c>v</c>
    ///        as an iterable of edges, and <c>null</c> if no such path</returns>
    ///
    public IEnumerable<DirectedEdge> PathTo(int v)
    {
      if (!HasPathTo(v)) return null;
      LinkedStack<DirectedEdge> path = new LinkedStack<DirectedEdge>();
      for (DirectedEdge e = edgeTo[v]; e != null; e = edgeTo[e.From])
      {
        path.Push(e);
      }
      return path;
    }


    // check optimality conditions:
    // (i) for all edges e:            distTo[e.To] <= distTo[e.From] + e.Weight
    // (ii) for all edge e on the SPT: distTo[e.To] == distTo[e.From] + e.Weight
    private bool check(EdgeWeightedDigraph G, int s)
    {

      // check that edge weights are nonnegative
      foreach (DirectedEdge e in G.Edges())
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

      // check that all edges e = v->w satisfy distTo[w] <= distTo[v] + e.weight()
      for (int v = 0; v < G.V; v++)
      {
        foreach (DirectedEdge e in G.Adj(v))
        {
          int w = e.To;
          if (distTo[v] + e.Weight < distTo[w])
          {
            Console.Error.WriteLine("edge " + e + " not relaxed");
            return false;
          }
        }
      }

      // check that all edges e = v->w on SPT satisfy distTo[w] == distTo[v] + e.weight()
      for (int w = 0; w < G.V; w++)
      {
        if (edgeTo[w] == null) continue;
        DirectedEdge e = edgeTo[w];
        int v = e.From;
        if (w != e.To) return false;
        if (distTo[v] + e.Weight != distTo[w])
        {
          Console.Error.WriteLine("edge " + e + " on shortest path not tight");
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Demo test the <c>DijkstraSP</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DijkstraSP tinyEWD.txt s", "File with the format for directed, weighted graph and a source")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      EdgeWeightedDigraph G = new EdgeWeightedDigraph(input);
      int s = int.Parse(args[1]);

      // compute shortest paths
      DijkstraSP sp = new DijkstraSP(G, s);

      // print shortest path
      for (int t = 0; t < G.V; t++)
      {
        if (sp.HasPathTo(t))
        {
          Console.Write("{0} to {1} ({2:F2})  ", s, t, sp.DistTo(t));
          foreach (DirectedEdge e in sp.PathTo(t))
          {
            Console.Write(e + "   ");
          }
          Console.WriteLine();
        }
        else
        {
          Console.Write("{0} to {1} no path\n", s, t);
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

