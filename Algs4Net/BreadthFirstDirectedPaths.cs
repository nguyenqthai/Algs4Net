/******************************************************************************
 *  File name :    BreadthFirstDirectedPaths.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Run breadth first search on a digraph.
 *  Runs in O(E + V) time.
 *
 *  C:\> algscmd BreadthFirstDirectedPaths tinyDG.txt 3
 *  3 to 0 (2):  3->2->0
 *  3 to 1 (3):  3->2->0->1
 *  3 to 2 (1):  3->2
 *  3 to 3 (0):  3
 *  3 to 4 (2):  3->5->4
 *  3 to 5 (1):  3->5
 *  3 to 6 (-):  not connected
 *  3 to 7 (-):  not connected
 *  3 to 8 (-):  not connected
 *  3 to 9 (-):  not connected
 *  3 to 10 (-):  not connected
 *  3 to 11 (-):  not connected
 *  3 to 12 (-):  not connected
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>BreadthDirectedFirstPaths</c> class represents a data type for finding
  /// shortest paths (number of edges) from a source vertex <c>S</c>
  /// (or set of source vertices) to every other vertex in the digraph.
  /// </para><para>This implementation uses breadth-first search.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// It uses extra space (not including the digraph) proportional to <c>V</c>.</para></summary>
  /// <remarks><para>For additional documentation, 
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of 
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BreadthFirstDirectedPaths.java.html">BreadthFirstDirectedPaths</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class BreadthFirstDirectedPaths
  {
    private static readonly int INFINITY = int.MaxValue;
    private bool[] marked;  // marked[v] = is there an s->v path?
    private int[] edgeTo;   // edgeTo[v] = last edge on shortest s->v path
    private int[] distTo;   // distTo[v] = length of shortest s->v path

    /// <summary>
    /// Computes the shortest path from <c>s</c> and every other vertex in graph <c>G</c>.</summary>
    /// <param name="G">the digraph</param>
    /// <param name="s">the source vertex</param>
    ///
    public BreadthFirstDirectedPaths(Digraph G, int s)
    {
      marked = new bool[G.V];
      distTo = new int[G.V];
      edgeTo = new int[G.V];
      for (int v = 0; v < G.V; v++)
        distTo[v] = INFINITY;
      bfs(G, s);
    }

    /// <summary>
    /// Computes the shortest path from any one of the source vertices in <c>sources</c>
    /// to every other vertex in graph <c>G</c>.</summary>
    /// <param name="G">the digraph</param>
    /// <param name="sources">the source vertices</param>
    ///
    public BreadthFirstDirectedPaths(Digraph G, IEnumerable<int> sources)
    {
      marked = new bool[G.V];
      distTo = new int[G.V];
      edgeTo = new int[G.V];
      for (int v = 0; v < G.V; v++)
        distTo[v] = INFINITY;
      bfs(G, sources);
    }

    // BFS from single source
    private void bfs(Digraph G, int s)
    {
      LinkedQueue<int> q = new LinkedQueue<int>();
      marked[s] = true;
      distTo[s] = 0;
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

    // BFS from multiple sources
    private void bfs(Digraph G, IEnumerable<int> sources)
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
    /// Is there a directed path from the source <c>s</c> (or sources) to vertex <c>v</c>?</summary>
    /// <param name="v">the vertex</param>
    /// <returns><c>true</c> if there is a directed path, <c>false</c> otherwise</returns>
    ///
    public bool HasPathTo(int v)
    {
      return marked[v];
    }

    /// <summary>
    /// Returns the number of edges in a shortest path from the source <c>s</c>
    /// (or sources) to vertex <c>v</c>?</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the number of edges in a shortest path</returns>
    ///
    public int DistTo(int v)
    {
      return distTo[v];
    }

    /// <summary>
    /// Returns a shortest path from <c>s</c> (or sources) to <c>v</c>, or
    /// <c>null</c> if no such path.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the sequence of vertices on a shortest path, as an IEnumerable</returns>
    ///
    public IEnumerable<int> PathTo(int v)
    {
      if (!HasPathTo(v)) return null;
      Stack<int> path = new Stack<int>();
      int x;
      for (x = v; distTo[x] != 0; x = edgeTo[x])
        path.Push(x);
      path.Push(x);
      return path;
    }

    /// <summary>
    /// Demo test the <c>BreadthFirstDirectedPaths</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BreadthFirstDirectedPaths tinyDG.txt s", "File in the format for digraph and a source vertex")]
    public static void MainTest(string[] args)
    {
      // read in digraph from command-line argument
      //if (args.Length < 3) throw new ArgumentException("Expecting input file and source vertex");
      TextInput input = new TextInput(args[0]);
      Digraph G = new Digraph(input);
      int s = int.Parse(args[1]);

      BreadthFirstDirectedPaths bfs = new BreadthFirstDirectedPaths(G, s);

      for (int v = 0; v < G.V; v++)
      {
        if (bfs.HasPathTo(v))
        {
          Console.Write("{0} to {1} ({2}):  ", s, v, bfs.DistTo(v));
          foreach (int x in bfs.PathTo(v))
          {
            if (x == s) Console.Write(x);
            else Console.Write("->" + x);
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
