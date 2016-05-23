/******************************************************************************
 *  File name :    DepthFirstPaths.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/41graph/tinyCG.txt
 *
 *  Run depth first search on an undirected graph.
 *  Runs in O(E + V) time.
 *
 *  C:\> algscmd DepthFirstPaths tinyCG.txt 0
 *  0 to 0:  0
 *  0 to 1:  0-2-1
 *  0 to 2:  0-2
 *  0 to 3:  0-2-3
 *  0 to 4:  0-2-3-4
 *  0 to 5:  0-2-3-5
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DepthFirstPaths</c> class represents a data type for finding
  /// paths from a source vertex <c>S</c> to every other vertex
  /// in an undirected graph.
  /// </para><para>This implementation uses depth-first search.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// It uses extra space (not including the graph) proportional to <c>V</c>.
  /// </para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a>   
  /// of <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DepthFirstPaths.java.html">DepthFirstPaths</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DepthFirstPaths
  {
    private bool[] marked;    // marked[v] = is there an s-v path?
    private int[] edgeTo;     // edgeTo[v] = last edge on s-v path
    private readonly int s;   // source vertex

    /// <summary>Computes a path between <c>s</c> and every other vertex in graph <c>G</c>.</summary>
    /// <param name="G">G the graph</param>
    /// <param name="s">s the source vertex</param>
    ///
    public DepthFirstPaths(Graph G, int s)
    {
      this.s = s;
      edgeTo = new int[G.V];
      marked = new bool[G.V];
      dfs(G, s);
    }

    // depth first search from v
    private void dfs(Graph G, int v)
    {
      marked[v] = true;
      foreach (int w in G.Adj(v))
      {
        if (!marked[w])
        {
          edgeTo[w] = v;
          dfs(G, w);
        }
      }
    }

    /// <summary>
    /// Is there a path between the source vertex <c>s</c> and vertex <c>v</c>?</summary>
    /// <param name="v">the vertex</param>
    /// <returns><c>true</c> if there is a path, <c>false</c> otherwise</returns>
    ///
    public bool HasPathTo(int v)
    {
      return marked[v];
    }

    /// <summary>
    /// Returns a path between the source vertex <c>s</c> and vertex <c>v</c>, or
    /// <c>null</c> if no such path.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the sequence of vertices on a path between the source vertex
    /// <c>s</c> and vertex <c>v</c>, as an IEnumerable</returns>
    ///
    public IEnumerable<int> PathTo(int v)
    {
      if (!HasPathTo(v)) return null;
      LinkedStack<int> path = new LinkedStack<int>();
      for (int x = v; x != s; x = edgeTo[x])
        path.Push(x);
      path.Push(s);
      return path;
    }

    /// <summary>
    /// Demo test the <c>DepthFirstPaths</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DepthFirstPaths tinyG.txt s", "File with the pre-defined format for undirected graph and a source vertex")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      Graph G = new Graph(input);

      int s = int.Parse(args[1]);

      DepthFirstPaths dfs = new DepthFirstPaths(G, s);

      for (int v = 0; v < G.V; v++)
      {
        if (dfs.HasPathTo(v))
        {
          Console.Write("{0} to {1}:  ", s, v);
          foreach (int x in dfs.PathTo(v))
          {
            if (x == s) Console.Write(x);
            else Console.Write("-" + x);
          }
          Console.WriteLine();
        }

        else
        {
          Console.Write("{0} to {1}:  not connected\n", s, v);
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
