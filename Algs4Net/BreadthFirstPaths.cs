/******************************************************************************
 *  File name :    BreadthFirstPaths.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/41graph/tinyCG.txt
 *
 *  Run breadth first search on an undirected graph.
 *  Runs in O(E + V) time.
 *
 *  C:\> algscmd Graph tinyCG.txt
 *  6 8
 *  0: 2 1 5 
 *  1: 0 2 
 *  2: 0 1 3 4 
 *  3: 5 4 2 
 *  4: 3 2 
 *  5: 3 0 
 *
 *  C:\> algscmd BreadthFirstPaths tinyCG.txt 0
 *  0 to 0 (0):  0
 *  0 to 1 (1):  0-1
 *  0 to 2 (1):  0-2
 *  0 to 3 (2):  0-2-3
 *  0 to 4 (2):  0-2-4
 *  0 to 5 (1):  0-5
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>BreadthFirstPaths</c> class represents a data type for finding
  /// shortest paths (number of edges) from a source vertex <c>S</c>
  /// (or a set of source vertices)
  /// to every other vertex in an undirected graph.
  /// </para><para>This implementation uses breadth-first search.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// It uses extra space (not including the graph) proportional to <c>V</c>.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a>   
  ///  of <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BreadthFirstPaths.java.html">BreadthFirstPaths</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class BreadthFirstPaths
  {
    private static readonly int INFINITY = int.MaxValue;
    private bool[] marked;  // marked[v] = is there an s-v path
    private int[] edgeTo;   // edgeTo[v] = previous edge on shortest s-v path
    private int[] distTo;   // distTo[v] = number of edges shortest s-v path

    /// <summary>
    /// Computes the shortest path between the source vertex <c>s</c></summary>
    /// and every other vertex in the graph <c>G</c>.
    /// <param name="G">the graph</param>
    /// <param name="s">the source vertex</param>
    ///
    public BreadthFirstPaths(Graph G, int s)
    {
      marked = new bool[G.V];
      distTo = new int[G.V];
      edgeTo = new int[G.V];
      bfs(G, s);

      Debug.Assert(check(G, s));
    }

    /// <summary>
    /// Computes the shortest path between any one of the source vertices in <c>sources</c>
    /// and every other vertex in graph <c>G</c>.</summary>
    /// <param name="G">the graph</param>
    /// <param name="sources">the source vertices</param>
    ///
    public BreadthFirstPaths(Graph G, IEnumerable<int> sources)
    {
      marked = new bool[G.V];
      distTo = new int[G.V];
      edgeTo = new int[G.V];
      for (int v = 0; v < G.V; v++)
        distTo[v] = INFINITY;
      bfs(G, sources);
    }

    // breadth-first search from a single source
    private void bfs(Graph G, int s)
    {
      LinkedQueue<int> q = new LinkedQueue<int>();
      for (int v = 0; v < G.V; v++)
        distTo[v] = INFINITY;
      distTo[s] = 0;
      marked[s] = true;
      q.Enqueue(s);

      while (!q.IsEmpty)
      {
        int v = q.Dequeue();
        foreach (int w in G.Adj(v))
        {
          if (!marked[w])
          {
            edgeTo[w] = v;
            distTo[w] = distTo[v] + 1;
            marked[w] = true;
            q.Enqueue(w);
          }
        }
      }
    }

    // breadth-first search from multiple sources
    private void bfs(Graph G, IEnumerable<int> sources)
    {
      LinkedQueue<int> q = new LinkedQueue<int>();
      foreach (int s in sources)
      {
        marked[s] = true;
        distTo[s] = 0;
        q.Enqueue(s);
      }
      while (!q.IsEmpty)
      {
        int v = q.Dequeue();
        foreach (int w in G.Adj(v))
        {
          if (!marked[w])
          {
            edgeTo[w] = v;
            distTo[w] = distTo[v] + 1;
            marked[w] = true;
            q.Enqueue(w);
          }
        }
      }
    }

    /// <summary>
    /// Is there a path between the source vertex <c>s</c> (or sources) and vertex <c>v</c>?</summary>
    /// <param name="v">the vertex</param>
    /// <returns><c>true</c> if there is a path, and <c>false</c> otherwise</returns>
    ///
    public bool HasPathTo(int v)
    {
      return marked[v];
    }

    /// <summary>
    /// Returns the number of edges in a shortest path between the source vertex <c>s</c>
    /// (or sources) and vertex <c>v</c>?</summary>
    /// <param name="v">v the vertex</param>
    /// <returns>the number of edges in a shortest path. If s is not connected to v, return
    /// int.MaxValue</returns>
    ///
    public int DistTo(int v)
    {
      return distTo[v];
    }

    /// <summary>
    /// Returns a shortest path between the source vertex <c>s</c> (or sources)
    /// and <c>v</c>, or <c>null</c> if no such path.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the sequence of vertices on a shortest path, as an Iterable</returns>
    ///
    public IEnumerable<int> PathTo(int v)
    {
      if (!HasPathTo(v)) return null;
      LinkedStack<int> path = new LinkedStack<int>();
      int x;
      for (x = v; distTo[x] != 0; x = edgeTo[x])
        path.Push(x);
      path.Push(x);
      return path;
    }

    // check optimality conditions for single source
    private bool check(Graph G, int s)
    {

      // check that the distance of s = 0
      if (distTo[s] != 0)
      {
        Console.WriteLine("distance of source " + s + " to itself = " + distTo[s]);
        return false;
      }

      // check that for each edge v-w dist[w] <= dist[v] + 1
      // provided v is reachable from s
      for (int v = 0; v < G.V; v++)
      {
        foreach (int w in G.Adj(v))
        {
          if (HasPathTo(v) != HasPathTo(w))
          {
            Console.WriteLine("edge " + v + "-" + w);
            Console.WriteLine("HasPathTo(" + v + ") = " + HasPathTo(v));
            Console.WriteLine("HasPathTo(" + w + ") = " + HasPathTo(w));
            return false;
          }
          if (HasPathTo(v) && (distTo[w] > distTo[v] + 1))
          {
            Console.WriteLine("edge " + v + "-" + w);
            Console.WriteLine("distTo[" + v + "] = " + distTo[v]);
            Console.WriteLine("distTo[" + w + "] = " + distTo[w]);
            return false;
          }
        }
      }

      // check that v = edgeTo[w] satisfies distTo[w] + distTo[v] + 1
      // provided v is reachable from s
      for (int w = 0; w < G.V; w++)
      {
        if (!HasPathTo(w) || w == s) continue;
        int v = edgeTo[w];
        if (distTo[w] != distTo[v] + 1)
        {
          Console.WriteLine("shortest path edge " + v + "-" + w);
          Console.WriteLine("distTo[" + v + "] = " + distTo[v]);
          Console.WriteLine("distTo[" + w + "] = " + distTo[w]);
          return false;
        }
      }

      return true;
    }

    /// <summary>
    /// Demo test the <c>BreadthFirstPaths</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BreadthFirstPaths tinyCG.txt s", "File with the pre-defined format for undirected graph and a source vertex")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      Graph G = new Graph(input);
      int s = int.Parse(args[1]);

      BreadthFirstPaths bfs = new BreadthFirstPaths(G, s);

      for (int v = 0; v < G.V; v++)
      {
        if (bfs.HasPathTo(v))
        {
          Console.Write("{0} to {1} ({2}):  ", s, v, bfs.DistTo(v));
          foreach (int x in bfs.PathTo(v))
          {
            if (x == s) Console.Write(x);
            else Console.Write("-" + x);
          }
          Console.WriteLine();
        }

        else
        {
          Console.Write("{0} to {1} (-):  not connected\n", s, v);
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
