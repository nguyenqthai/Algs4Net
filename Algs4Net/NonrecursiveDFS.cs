/******************************************************************************
 *  File name :    NonrecursiveDFS.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/41graph/tinyCG.txt
 *
 *  Run nonrecurisve depth-first search on an undirected graph.
 *  Runs in O(E + V) time.
 *
 *  Explores the vertices in exactly the same order as DepthFirstSearch.java.
 *
 *  C:\> algscmd NonrecursiveDFS tinyCG.txt 0
 *  0 1 2 3 4 5
 *  Total 6 marked vertices out of 6 vertices
 *  
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
  /// The <c>NonrecursiveDFS</c> class represents a data type for finding
  /// the vertices connected to a source vertex <c>S</c> in the undirected
  /// graph.</para><para>
  /// This implementation uses a nonrecursive version of depth-first search
  /// with an explicit stack and the path tracing feature found in <see cref="DepthFirstPaths"/>.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// It uses extra space (not including the graph) proportional to <c>V</c>.
  /// </para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a>   
  /// of <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/NonrecursiveDFS.java.html">NonrecursiveDFS</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class NonrecursiveDFS
  {
    private bool[] marked;    // marked[v] = is there an s-v path?
    private int[] edgeTo;     // edgeTo[v] = last edge on s-v path
    private readonly int s;   // source vertex

    /// <summary>
    /// Computes the vertices connected to the source vertex <c>s</c> in the graph <c>G</c>.</summary>
    /// <param name="G">the graph</param>
    /// <param name="s">the source vertex</param>
    ///
    public NonrecursiveDFS(Graph G, int s)
    {
      this.s = s;
      marked = new bool[G.V];
      edgeTo = new int[G.V];

      // to be able to iterate over each adjacency list, keeping track of which
      // vertex in each adjacency list needs to be explored next
      IEnumerator<int>[] adj = new IEnumerator<int>[G.V];
      for (int v = 0; v < G.V; v++)
        adj[v] = G.Adj(v).GetEnumerator();

      // depth-first search using an explicit stack
      LinkedStack <int> stack = new LinkedStack<int>();
      marked[s] = true;
      stack.Push(s);
      //Console.Write("Visiting ({0})\n", s);
      while (!stack.IsEmpty)
      {
        int v = stack.Peek();
        if (adj[v].MoveNext())
        {
          int w = adj[v].Current;
          //Console.Write("Approaching {0}\n", w);
          if (!marked[w])
          {
            // discovered vertex w for the first time
            marked[w] = true;
            edgeTo[w] = v;
            stack.Push(w);
            //Console.Write("Visiting ({0})\n", w);
          }
          //else
          //  Console.Write("Visited {0}\n", w);
        }
        else
        {
          //Console.Write("Returning from {0}\n", stack.Peek());
          stack.Pop();
        }
      }
    }
    /// <summary>
    /// Is vertex <c>v</c> connected to the source vertex <c>s</c>?</summary>
    /// <param name="v">the vertex</param>
    /// <returns><c>true</c> if vertex <c>v</c> is connected to the source vertex <c>s</c>,
    ///   and <c>false</c> otherwise</returns>
    ///
    public bool Marked(int v)
    {
      return marked[v];
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
    /// Demo test the <c>NonrecursiveDFS</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd NonrecursiveDFS tinyG.txt s", "File with the pre-defined format for undirected graph and a source vertex")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      Graph G = new Graph(input);
      int s = int.Parse(args[1]);
      NonrecursiveDFS dfs = new NonrecursiveDFS(G, s);

      int markedCount = 0;
      for (int v = 0; v < G.V; v++)
      {
        if (dfs.Marked(v))
        {
          Console.Write(v + " ");
          markedCount++;
        }
      }
      Console.WriteLine("\nTotal {0} marked vertices out of {1} vertices\n", markedCount, G.V);

      for (int v = 0; v < G.V; v++)
      {
        if (dfs.HasPathTo(v))
        {
          Console.Write("{0,2} to {1,2}:  ", s, v);
          foreach (int x in dfs.PathTo(v))
          {
            if (x == s) Console.Write(x);
            else Console.Write("-" + x);
          }
          Console.WriteLine();
        }

        else
        {
          Console.Write("{0,2} to {1,2}:  not connected\n", s, v);
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
