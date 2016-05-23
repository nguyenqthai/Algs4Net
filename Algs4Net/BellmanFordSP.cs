/******************************************************************************
 *  File name :    BellmanFordSP.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *                EdgeWeightedDirectedCycle.java
 *  Data files:   http://algs4.cs.princeton.edu/44sp/tinyEWDn.txt
 *                http://algs4.cs.princeton.edu/44sp/mediumEWDnc.txt
 *
 *  Bellman-Ford shortest path algorithm. Computes the shortest path tree in
 *  edge-weighted digraph G from vertex s, or finds a negative cost cycle
 *  reachable from s.
 *
 *  C:\> algscmd BellmanFordSP tinyEWDn.txt 0
 *  0 to 0 ( 0.00)  
 *  0 to 1 ( 0.93)  0->2  0.26   2->7  0.34   7->3  0.39   3->6  0.52   6->4 -1.25   4->5  0.35   5->1  0.32
 *  0 to 2 ( 0.26)  0->2  0.26   
 *  0 to 3 ( 0.99)  0->2  0.26   2->7  0.34   7->3  0.39   
 *  0 to 4 ( 0.26)  0->2  0.26   2->7  0.34   7->3  0.39   3->6  0.52   6->4 -1.25   
 *  0 to 5 ( 0.61)  0->2  0.26   2->7  0.34   7->3  0.39   3->6  0.52   6->4 -1.25   4->5  0.35
 *  0 to 6 ( 1.51)  0->2  0.26   2->7  0.34   7->3  0.39   3->6  0.52   
 *  0 to 7 ( 0.60)  0->2  0.26   2->7  0.34   
 *
 *  C:\> algscmd BellmanFordSP tinyEWDnc.txt 0
 *  4->5  0.35
 *  5->4 -0.66
 *
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>BellmanFordSP</c> class represents a data type for solving the
  /// single-source shortest paths problem in edge-weighted digraphs with
  /// no negative cycles. The edge weights can be positive, negative, or zero.
  /// This class finds either a shortest path from the source vertex <c>S</c>
  /// to every other vertex or a negative cycle reachable from the source vertex.
  /// </para><para>This implementation uses the Bellman-Ford-Moore algorithm.
  /// The constructor takes time proportional to <c>V</c> (<c>V</c> + <c>E</c>)
  /// in the worst case, where <c>V</c> is the number of vertices and <c>E</c>
  /// is the number of edges.
  /// Afterwards, the <c>distTo()</c>, <c>hasPathTo()</c>, and <c>hasNegativeCycle()</c>
  /// methods take constant time; the <c>pathTo()</c> and <c>negativeCycle()</c>
  /// method takes time proportional to the number of edges returned.
  /// </para></summary>
  /// <remarks><para>For additional documentation,    
  /// see <a href="http://algs4.cs.princeton.edu/44sp">Section 4.4</a> of    
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BellmanFordSP.java.html">BellmanFordSP</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class BellmanFordSP
  {
    private double[] distTo;                  // distTo[v] = distance  of shortest s->v path
    private DirectedEdge[] edgeTo;            // edgeTo[v] = last edge on shortest s->v path
    private bool[] onQueue;                   // onQueue[v] = is v currently on the queue?
    private LinkedQueue<int> queue;           // queue of vertices to relax
    private int cost;                         // number of calls to relax()
    private IEnumerable<DirectedEdge> cycle;  // negative cycle (or null if no such cycle)

    /// <summary>Computes a shortest paths tree from <c>s</c> to every other vertex in
    /// the edge-weighted digraph <c>G</c>.</summary>
    /// <param name="G">the acyclic digraph</param>
    /// <param name="s">the source vertex</param>
    /// <exception cref="ArgumentException">unless 0 &lt;= <c>s</c> &lt;= <c>V</c> - 1</exception>
    ///
    public BellmanFordSP(EdgeWeightedDigraph G, int s)
    {
      distTo = new double[G.V];
      edgeTo = new DirectedEdge[G.V];
      onQueue = new bool[G.V];
      for (int v = 0; v < G.V; v++)
        distTo[v] = double.PositiveInfinity;
      distTo[s] = 0.0;

      // Bellman-Ford algorithm
      queue = new LinkedQueue<int>();
      queue.Enqueue(s);
      onQueue[s] = true;
      while (!queue.IsEmpty && !HasNegativeCycle)
      {
        int v = queue.Dequeue();
        onQueue[v] = false;
        relax(G, v);
      }

      Debug.Assert(check(G, s));
    }

    // relax vertex v and put other endpoints on queue if changed
    private void relax(EdgeWeightedDigraph G, int v)
    {
      foreach (DirectedEdge e in G.Adj(v))
      {
        int w = e.To;
        if (distTo[w] > distTo[v] + e.Weight)
        {
          distTo[w] = distTo[v] + e.Weight;
          edgeTo[w] = e;
          if (!onQueue[w])
          {
            queue.Enqueue(w);
            onQueue[w] = true;
          }
        }
        if (cost++ % G.V == 0)
        {
          findNegativeCycle();
          if (HasNegativeCycle) return;  // found a negative cycle
        }
      }
    }

    /// <summary>
    /// Is there a negative cycle reachable from the source vertex <c>s</c>?</summary>
    /// <returns><c>true</c> if there is a negative cycle reachable from the
    /// source vertex <c>s</c>, and <c>false</c> otherwise</returns>
    ///
    public bool HasNegativeCycle
    {
      get { return cycle != null; }
    }

    /// <summary>
    /// Returns a negative cycle reachable from the source vertex <c>s</c>, or <c>null</c>
    /// if there is no such cycle.</summary>
    /// <returns>a negative cycle reachable from the soruce vertex <c>s</c> 
    /// as an iterable of edges, and <c>null</c> if there is no such cycle</returns>
    ///
    public IEnumerable<DirectedEdge> GetNegativeCycle()
    {
      return cycle;
    }

    // by finding a cycle in predecessor graph
    private void findNegativeCycle()
    {
      int V = edgeTo.Length;
      EdgeWeightedDigraph spt = new EdgeWeightedDigraph(V);
      for (int v = 0; v < V; v++)
        if (edgeTo[v] != null)
          spt.AddEdge(edgeTo[v]);

      EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(spt);
      cycle = finder.GetCycle();
    }

    /// <summary>
    /// Returns the length of a shortest path from the source vertex <c>s</c> to vertex <c>v</c>.</summary>
    /// <param name="v">he destination vertex</param>
    /// <returns>the length of a shortest path from the source vertex <c>s</c> to vertex <c>v</c>;
    /// <c>double.PositiveInfinity</c> if no such path</returns>
    /// <exception cref="InvalidOperationException">if there is a negative cost cycle reachable</exception>
    /// from the source vertex <c>s</c>
    ///
    public double DistTo(int v)
    {
      if (HasNegativeCycle)
        throw new InvalidOperationException("Negative cost cycle exists");
      return distTo[v];
    }

    /// <summary>
    /// Is there a path from the source <c>s</c> to vertex <c>v</c>?</summary>
    /// <param name="v">the destination vertex</param>
    /// <returns><c>true</c> if there is a path from the source vertex</returns>
    ///   <c>s</c> to vertex <c>v</c>, and <c>false</c> otherwise
    ///
    public bool HasPathTo(int v)
    {
      return distTo[v] < double.PositiveInfinity;
    }

    /// <summary>
    /// Returns a shortest path from the source <c>s</c> to vertex <c>v</c>.</summary>
    /// <param name="v">the destination vertex</param>
    /// <returns>a shortest path from the source <c>s</c> to vertex <c>v</c>
    /// as an iterable of edges, and <c>null</c> if no such path</returns>
    /// <exception cref="InvalidOperationException">if there is a negative cost cycle reachable</exception>
    /// from the source vertex <c>s</c>
    ///
    public IEnumerable<DirectedEdge> PathTo(int v)
    {
      if (HasNegativeCycle)
        throw new InvalidOperationException("Negative cost cycle exists");
      if (!HasPathTo(v)) return null;
      Stack<DirectedEdge> path = new Stack<DirectedEdge>();
      for (DirectedEdge e = edgeTo[v]; e != null; e = edgeTo[e.From])
      {
        path.Push(e);
      }
      return path;
    }

    // check optimality conditions: either
    // (i) there exists a negative cycle reacheable from s
    //     or
    // (ii)  for all edges e = v->w:            distTo[w] <= distTo[v] + e.Weight
    // (ii') for all edges e = v->w on the SPT: distTo[w] == distTo[v] + e.Weight
    private bool check(EdgeWeightedDigraph G, int s)
    {

      // has a negative cycle
      if (HasNegativeCycle)
      {
        double weight = 0.0;
        foreach (DirectedEdge e in GetNegativeCycle())
        {
          weight += e.Weight;
        }
        if (weight >= 0.0)
        {
          Console.Error.WriteLine("error: weight of negative cycle = " + weight);
          return false;
        }
      }

      // no negative cycle reachable from source
      else
      {
        // check that distTo[v] and edgeTo[v] are consistent
        if (distTo[s] != 0.0 || edgeTo[s] != null)
        {
          Console.Error.WriteLine("distanceTo[s] and edgeTo[s] inconsistent");
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

        // check that all edges e = v->w satisfy distTo[w] <= distTo[v] + e.Weight
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

        // check that all edges e = v->w on SPT satisfy distTo[w] == distTo[v] + e.Weight
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
      }

      //Console.WriteLine("Satisfies optimality conditions");
      return true;
    }

    /// <summary>
    /// Demo test the <c>BellmanFordSP</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BellmanFordSP tinyEWDn.txt 5", "File with the format for directed, weighted graph and a source")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      EdgeWeightedDigraph G = new EdgeWeightedDigraph(input);
      int s = int.Parse(args[1]);

      BellmanFordSP sp = new BellmanFordSP(G, s);

      // print negative cycle
      if (sp.HasNegativeCycle)
      {
        foreach (DirectedEdge e in sp.GetNegativeCycle())
          Console.WriteLine(e);
      }
      else // print shortest paths
      {
        for (int v = 0; v < G.V; v++)
        {
          if (sp.HasPathTo(v))
          {
            Console.Write("{0} to {1} ({2:F2})  ", s, v, sp.DistTo(v));
            foreach (DirectedEdge e in sp.PathTo(v))
            {
              Console.Write(e + "   ");
            }
            Console.WriteLine();
          }
          else
          {
            Console.Write("{0} to {1}: no path\n", s, v);
          }
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
