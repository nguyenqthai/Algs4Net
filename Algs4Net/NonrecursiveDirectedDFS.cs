/******************************************************************************
 *  File name :    NonrecursiveDirectedDFS.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/42digraph/tinyDG.txt
 *
 *  Run nonrecurisve depth-first search on an directed graph.
 *  Runs in O(E + V) time.
 *
 *  Explores the vertices in exactly the same order as DirectedDFS.cs
 *
 *  C:\> algscmd NonrecursiveDirectedDFS tinyDG.txt 1
 *  1
 *
 *  C:\> algscmd NonrecursiveDirectedDFS tinyDG.txt 2
 *  0 1 2 3 4 5
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>NonrecursiveDirectedDFS</c> class represents a data type for finding
  /// the vertices reachable from a source vertex <c>S</c> in the digraph. This
  /// implementation uses a nonrecursive version of depth-first search
  /// with an explicit stack.</para><para>
  /// The constructor takes time proportional to <c>V</c> + <c>E</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// It uses extra space (not including the digraph) proportional to <c>V</c>.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/NonrecursiveDirectedDFS.java.html">NonrecursiveDirectedDFS</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class NonrecursiveDirectedDFS
  {
    private bool[] marked;  // marked[v] = is there an s->v path?

    /// <summary>
    /// Computes the vertices reachable from the source vertex <c>s</c> in the 
    /// digraph <c>G</c>.</summary>
    /// <param name="G">the digraph</param>
    /// <param name="s">the source vertex</param>
    ///
    public NonrecursiveDirectedDFS(Digraph G, int s)
    {
      marked = new bool[G.V];

      // to be able to iterate over each adjacency list, keeping track of which
      // vertex in each adjacency list needs to be explored next
      IEnumerator<int>[] adj = new IEnumerator<int>[G.V];
      for (int v = 0; v < G.V; v++)
        adj[v] = G.Adj(v).GetEnumerator();

      // depth-first search using an explicit stack
      LinkedStack<int> stack = new LinkedStack<int>();
      marked[s] = true;
      stack.Push(s);
      while (!stack.IsEmpty)
      {
        int v = stack.Peek();
        if (adj[v].MoveNext())
        {
          int w = adj[v].Current;
          // Console.Write("check {0}\n", w);
          if (!marked[w])
          {
            // discovered vertex w for the first time
            marked[w] = true;
            // edgeTo[w] = v;
            stack.Push(w);
            // Console.Write("dfs({0})\n", w);
          }
        }
        else
        {
          // Console.Write("{0} done\n", v);
          stack.Pop();
        }
      }
    }

    /// <summary>
    /// Is vertex <c>v</c> reachable from the source vertex <c>s</c>?</summary>
    /// <param name="v">the vertex</param>
    /// <returns><c>true</c> if vertex <c>v</c> is reachable from the source vertex <c>s</c>,
    ///   and <c>false</c> otherwise</returns>
    ///
    public bool Marked(int v)
    {
      return marked[v];
    }

    /// <summary>
    /// Demo test the <c>NonrecursiveDirectedDFS</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd NonrecursiveDirectedDFS tinyDG.txt s", "File in the format for digraph and a source vertex")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      Digraph G = new Digraph(input);
      int s = int.Parse(args[1]);
      NonrecursiveDirectedDFS dfs = new NonrecursiveDirectedDFS(G, s);
      for (int v = 0; v < G.V; v++)
        if (dfs.Marked(v))
          Console.Write(v + " ");
      Console.WriteLine();
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
