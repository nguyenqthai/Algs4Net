/******************************************************************************
 *  File name :    FloydWarshall.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Floyd-Warshall all-pairs shortest path algorithm.
 *
 *  C:\> algscmd FloydWarshall 100 500
 *
 *  Should check for negative cycles during triple loop; otherwise
 *  intermediate numbers can get exponentially large.
 *  Reference: "The Floyd-Warshall algorithm on graphs with negative cycles"
 *  by Stefan Hougardy
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>FloydWarshall</c> class represents a data type for solving the
  /// all-pairs shortest paths problem in edge-weighted digraphs with
  /// no negative cycles.
  /// The edge weights can be positive, negative, or zero.
  /// This class finds either a shortest path between every pair of vertices
  /// or a negative cycle.
  /// </para><para>
  /// This implementation uses the Floyd-Warshall algorithm.
  /// The constructor takes time proportional to <c>V</c><sup>3</sup> in the
  /// worst case, where <c>V</c> is the number of vertices.
  /// Afterwards, the <c>Dist()</c>, <c>HasPath()</c>, and <c>HasNegativeCycle()</c>
  /// methods take constant time; the <c>Path()</c> and <c>GetNegativeCycle()</c>
  /// method takes time proportional to the number of edges returned.
  /// </para></summary>
  /// <remarks><para>For additional documentation,    
  /// see <a href="http://algs4.cs.princeton.edu/44sp">Section 4.4</a> of    
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/FloydWarshall.java.html">FloydWarshall</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class FloydWarshall
  {
    private bool hasNegativeCycle;  // is there a negative cycle?
    private double[,] distTo;       // distTo[v,w] = length of shortest v->w path
    private DirectedEdge[,] edgeTo; // edgeTo[v,w] = last edge on shortest v->w path

    /// <summary>Computes a shortest paths tree from each vertex to to every 
    /// other vertex in the edge-weighted digraph <c>G</c>. If no such shortest
    /// path exists for some pair of vertices, it computes a negative cycle.
    /// </summary>
    /// <param name="G">the edge-weighted digraph</param>
    ///
    public FloydWarshall(AdjMatrixEdgeWeightedDigraph G)
    {
      int V = G.V;
      distTo = new double[V, V];
      edgeTo = new DirectedEdge[V, V];

      // initialize distances to infinity
      for (int v = 0; v < V; v++)
      {
        for (int w = 0; w < V; w++)
        {
          distTo[v, w] = double.PositiveInfinity;
        }
      }

      // initialize distances using edge-weighted digraph's
      for (int v = 0; v < G.V; v++)
      {
        foreach (DirectedEdge e in G.Adj(v))
        {
          distTo[e.From, e.To] = e.Weight;
          edgeTo[e.From, e.To] = e;
        }
        // in case of self-loops
        if (distTo[v, v] >= 0.0)
        {
          distTo[v, v] = 0.0;
          edgeTo[v, v] = null;
        }
      }

      // Floyd-Warshall updates
      for (int i = 0; i < V; i++)
      {
        // compute shortest paths using only 0, 1, ..., i as intermediate vertices
        for (int v = 0; v < V; v++)
        {
          if (edgeTo[v,i] == null) continue;  // optimization
          for (int w = 0; w < V; w++)
          {
            if (distTo[v, w] > distTo[v, i] + distTo[i, w])
            {
              distTo[v, w] = distTo[v, i] + distTo[i, w];
              edgeTo[v, w] = edgeTo[i, w];
            }
          }
          // check for negative cycle
          if (distTo[v, v] < 0.0)
          {
            hasNegativeCycle = true;
            return;
          }
        }
      }
    }

    /// <summary>
    /// Is there a negative cycle?</summary>
    /// <returns><c>true</c> if there is a negative cycle, and <c>false</c> otherwise</returns>
    ///
    public bool HasNegativeCycle
    {
      get { return hasNegativeCycle; }
    }

    /// <summary>
    /// Returns a negative cycle, or <c>null</c> if there is no such cycle.</summary>
    /// <returns>a negative cycle as an iterable of edges,
    /// or <c>null</c> if there is no such cycle</returns>
    ///
    public IEnumerable<DirectedEdge> NegativeCycle()
    {
      for (int v = 0; v < distTo.Length; v++)
      {
        // negative cycle in v's predecessor graph
        if (distTo[v,v] < 0.0)
        {
          int V = edgeTo.Length;
          EdgeWeightedDigraph spt = new EdgeWeightedDigraph(V);
          for (int w = 0; w < V; w++)
            if (edgeTo[v,w] != null)
              spt.AddEdge(edgeTo[v,w]);
          EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(spt);
          Debug.Assert(finder.HasCycle);
          return finder.GetCycle();
        }
      }
      return null;
    }

    /// <summary>
    /// Is there a path from the vertex <c>s</c> to vertex <c>t</c>?</summary>
    /// <param name="s">the source vertex</param>
    /// <param name="t">the destination vertex</param>
    /// <returns><c>true</c> if there is a path from vertex <c>s</c>
    /// to vertex <c>t</c>, and <c>false</c> otherwise</returns>
    ///
    public bool HasPath(int s, int t)
    {
      return distTo[s,t] < double.PositiveInfinity;
    }

    /// <summary>
    /// Returns the length of a shortest path from vertex <c>s</c> to vertex <c>t</c>.</summary>
    /// <param name="s">the source vertex</param>
    /// <param name="t">the destination vertex</param>
    /// <returns>the length of a shortest path from vertex <c>s</c> to vertex <c>t</c>;
    /// <c>double.PositiveInfinity</c> if no such path</returns>
    /// <exception cref="InvalidOperationException">if there is a negative cost cycle</exception>
    ///
    public double Dist(int s, int t)
    {
      if (HasNegativeCycle)
        throw new InvalidOperationException("Negative cost cycle exists");
      return distTo[s,t];
    }

    /// <summary>
    /// Returns a shortest path from vertex <c>s</c> to vertex <c>t</c>.</summary>
    /// <param name="s">the source vertex</param>
    /// <param name="t">the destination vertex</param>
    /// <returns>a shortest path from vertex <c>s</c> to vertex <c>t</c>
    /// as an iterable of edges, and <c>null</c> if no such path</returns>
    /// <exception cref="InvalidOperationException">if there is a negative cost cycle</exception>
    ///
    public IEnumerable<DirectedEdge> Path(int s, int t)
    {
      if (HasNegativeCycle)
        throw new InvalidOperationException("Negative cost cycle exists");
      if (!HasPath(s, t)) return null;
      LinkedStack<DirectedEdge> path = new LinkedStack<DirectedEdge>();
      for (DirectedEdge e = edgeTo[s,t]; e != null; e = edgeTo[s,e.From])
      {
        path.Push(e);
      }
      return path;
    }

    // check optimality conditions
    private bool check(EdgeWeightedDigraph G, int s)
    {

      // no negative cycle
      if (!HasNegativeCycle)
      {
        for (int v = 0; v < G.V; v++)
        {
          foreach (DirectedEdge e in G.Adj(v))
          {
            int w = e.To;
            for (int i = 0; i < G.V; i++)
            {
              if (distTo[i, w] > distTo[i, v] + e.Weight)
              {
                Console.Error.WriteLine("edge " + e + " is eligible");
                return false;
              }
            }
          }
        }
      }
      return true;
    }

    /// <summary>
    /// Demo test the <c>FloydWarshall</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd FloydWarshall V E",
      "V vertices and E edges. Alternatively, enter use a file such as\n" +
      "   algscmd FloydWarshall tinyEWD.txt")]
    public static void MainTest(string[] args)
    {
      AdjMatrixEdgeWeightedDigraph G;
      if (args.Length == 1)
      {
        G = new AdjMatrixEdgeWeightedDigraph(new TextInput(args[0]));
      }
      else
      {
        // random graph with V vertices and E edges, parallel edges allowed
        int V = int.Parse(args[0]);
        int E = int.Parse(args[1]);
        G = new AdjMatrixEdgeWeightedDigraph(V);
        for (int i = 0; i < E; i++)
        {
          int v = StdRandom.Uniform(V);
          int w = StdRandom.Uniform(V);
          double weight = Math.Round(100 * (StdRandom.Uniform() - 0.15)) / 100.0;
          try
          {
            if (v == w) G.AddEdge(new DirectedEdge(v, w, Math.Abs(weight)));
            else G.AddEdge(new DirectedEdge(v, w, weight));
          }
          catch (Exception)
          {
            Console.Error.WriteLine("What could be wrong?");
          }
        }
      }
      Console.WriteLine(G);
      // run Floyd-Warshall algorithm
      FloydWarshall spt = new FloydWarshall(G);

      // print all-pairs shortest path distances
      Console.Write("  ");
      for (int v = 0; v < G.V; v++)
      {
        Console.Write("{0,6} ", v);
      }
      Console.WriteLine();
      for (int v = 0; v < G.V; v++)
      {
        Console.Write("{0,3}: ", v);
        for (int w = 0; w < G.V; w++)
        {
          if (spt.HasPath(v, w)) Console.Write("{0,6:F2} ", spt.Dist(v, w));
          else Console.Write("  Inf ");
        }
        Console.WriteLine();
      }

      // print negative cycle
      if (spt.HasNegativeCycle)
      {
        Console.WriteLine("Negative cost cycle:");
        foreach (DirectedEdge e in spt.NegativeCycle())
          Console.WriteLine(e);
        Console.WriteLine();
      }

      // print all-pairs shortest paths
      else
      {
        for (int v = 0; v < G.V; v++)
        {
          for (int w = 0; w < G.V; w++)
          {
            if (spt.HasPath(v, w))
            {
              Console.Write("{0} to {1} ({2,5:F2})  ", v, w, spt.Dist(v, w));
              foreach (DirectedEdge e in spt.Path(v, w))
                Console.Write(e + "  ");
              Console.WriteLine();
            }
            else
            {
              Console.Write("{0} to {1} no path\n", v, w);
            }
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
