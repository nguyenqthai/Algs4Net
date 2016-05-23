/******************************************************************************
 *  File name :    DepthFirstDirectedPaths.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Determine reachability in a digraph from a given vertex using
 *  depth first search.
 *  Runs in O(E + V) time.
 *
 *  C:\> algscmd DepthFirstDirectedPaths tinyDG.txt 3
 *  3 to 0:  3-5-4-2-0
 *  3 to 1:  3-5-4-2-0-1
 *  3 to 2:  3-5-4-2
 *  3 to 3:  3
 *  3 to 4:  3-5-4
 *  3 to 5:  3-5
 *  3 to 6:  not connected
 *  3 to 7:  not connected
 *  3 to 8:  not connected
 *  3 to 9:  not connected
 *  3 to 10:  not connected
 *  3 to 11:  not connected
 *  3 to 12:  not connected
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DepthFirstDirectedPaths</c> class represents a data type for finding
  /// directed paths from a source vertex <c>S</c> to every other vertex in the digraph.
  /// </para><para>This implementation uses depth-first search.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// It uses extra space (not including the graph) proportional to <c>V</c>.
  /// </para></summary>
  /// <remarks><para>For additional documentation,  
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of  
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DepthFirstDirectedPaths.java.html">DepthFirstDirectedPaths</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DepthFirstDirectedPaths
  {
    private bool[] marked;    // marked[v] = true if v is reachable from s
    private int[] edgeTo;     // edgeTo[v] = last edge on path from s to v
    private readonly int s;   // source vertex

    /// <summary>
    /// Computes a directed path from <c>s</c> to every other vertex in digraph <c>G</c>.</summary>
    /// <param name="G">the digraph</param>
    /// <param name="s">the source vertex</param>
    ///
    public DepthFirstDirectedPaths(Digraph G, int s)
    {
      marked = new bool[G.V];
      edgeTo = new int[G.V];
      this.s = s;
      dfs(G, s);
    }

    private void dfs(Digraph G, int v)
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
    /// Is there a directed path from the source vertex <c>s</c> to vertex <c>v</c>?</summary>
    /// <param name="v">the vertex</param>
    /// <returns><c>true</c> if there is a directed path from the source
    ///  vertex <c>s</c> to vertex <c>v</c>, <c>false</c> otherwise</returns>
    ///
    public bool HasPathTo(int v)
    {
      return marked[v];
    }

    /// <summary>
    /// Returns a directed path from the source vertex <c>s</c> to vertex <c>v</c>, or
    /// <c>null</c> if no such path.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the sequence of vertices on a directed path from the source vertex
    ///  <c>s</c> to vertex <c>v</c>, as an Iterable</returns>
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
    /// Demo test the <c>DepthFirstDirectedPaths</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DepthFirstDirectedPaths tinyDG.txt s", "File in the format for digraph and a source vertex")]
    public static void MainTest(string[] args)
    {
      // read in digraph from command-line argument
      TextInput input = new TextInput(args[0]);
      Digraph G = new Digraph(input);
      int s = int.Parse(args[1]);
      DepthFirstDirectedPaths dfs = new DepthFirstDirectedPaths(G, s);

      for (int v = 0; v < G.V; v++)
      {
        if (dfs.HasPathTo(v))
        {
          Console.Write("{0} to {1}: ", s, v);
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
