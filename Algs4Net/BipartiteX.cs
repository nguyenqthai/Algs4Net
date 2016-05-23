/******************************************************************************
 *  File name :    BipartiteX.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Given a graph, find either (i) a bipartition or (ii) an odd-length cycle.
 *  Runs in O(E + V) time.
 *
 *
 ******************************************************************************/
 
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>BipartiteX</c> class represents a data type for
  /// determining whether an undirected graph is bipartite or whether
  /// it has an odd-length cycle.
  /// The <c>IsBipartite</c> operation determines whether the graph is
  /// bipartite. If so, the <c>Color</c> operation determines a
  /// bipartition; if not, the <c>OddCycle</c> operation determines a
  /// cycle with an odd number of edges.
  /// </para><para>
  /// This implementation uses breadth-first search and is nonrecursive.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c>
  /// (in the worst case),
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>IsBipartite</c> and <c>Color</c> operations
  /// take constant time; the <c>OddCycle</c> operation takes time proportional
  /// to the length of the cycle.
  /// See <seealso cref="Bipartite"/> for a recursive version that uses depth-first search.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a>   
  ///  of <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BipartiteX.java.html">BipartiteX</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class BipartiteX
  {
    private const bool WHITE = false;
    private const bool BLACK = true;

    private bool isBipartite;       // is the graph bipartite?
    private bool[] color;           // color[v] gives vertices on one side of bipartition
    private bool[] marked;          // marked[v] = true if v has been visited in DFS
    private int[] edgeTo;           // edgeTo[v] = last edge on path to v
    private LinkedQueue<int> cycle; // odd-length cycle
    private Graph g;

    /// <summary>Determines whether an undirected graph is bipartite and finds either a
    /// bipartition or an odd-length cycle.</summary>
    /// <param name="G">the graph</param>
    ///
    public BipartiteX(Graph G)
    {
      isBipartite = true;
      color = new bool[G.V];
      marked = new bool[G.V];
      edgeTo = new int[G.V];

      for (int v = 0; v < G.V && isBipartite; v++)
      {
        if (!marked[v])
        {
          bfs(G, v);
        }
      }
      g = G;
      Debug.Assert(check(G));
    }

    /// <summary>
    /// Returns the graph for which the bipartite is computed
    /// </summary>
    public Graph G
    {
      get { return g; }
    }

    private void bfs(Graph G, int s)
    {
      LinkedQueue<int> q = new LinkedQueue<int>();
      color[s] = WHITE;
      marked[s] = true;
      q.Enqueue(s);

      while (!q.IsEmpty)
      {
        int v = q.Dequeue();
        foreach (int w in G.Adj(v))
        {
          if (!marked[w])
          {
            marked[w] = true;
            edgeTo[w] = v;
            color[w] = !color[v];
            q.Enqueue(w);
          }
          else if (color[w] == color[v])
          {
            isBipartite = false;

            // to form odd cycle, consider s-v path and s-w path
            // and let x be closest node to v and w common to two paths
            // then (w-x path) + (x-v path) + (edge v-w) is an odd-length cycle
            // Note: distTo[v] == distTo[w];
            cycle = new LinkedQueue<int>();
            LinkedStack<int> stack = new LinkedStack<int>();
            int x = v, y = w;
            while (x != y)
            {
              stack.Push(x);
              cycle.Enqueue(y);
              x = edgeTo[x];
              y = edgeTo[y];
            }
            stack.Push(x);
            while (!stack.IsEmpty)
              cycle.Enqueue(stack.Pop());
            cycle.Enqueue(w);
            return;
          }
        }
      }
    }

    /// <summary>
    /// Returns true if the graph is bipartite.</summary>
    /// <returns><c>true</c> if the graph is bipartite; <c>false</c> otherwise</returns>
    ///
    public bool IsBipartite
    {
      get { return isBipartite; }
    }

    /// <summary>
    /// Returns the side of the bipartite that vertex <c>v</c> is on.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the side of the bipartition that vertex <c>v</c> is on; two 
    /// vertices are in the same side of the bipartition if and only if they 
    /// have the same color</returns>
    /// <exception cref="IndexOutOfRangeException">unless <c>0 &lt;= v &lt; V</c> </exception>
    /// <exception cref="InvalidOperationException">if this method is called when the graph
    /// is not bipartite</exception>
    ///
    public bool Color(int v)
    {
      if (!isBipartite)
        throw new InvalidOperationException("Graph is not bipartite");
      return color[v]; // IndexOutOfRangeException may be thown from here
    }

    /// <summary>
    /// Returns an odd-length cycle if the graph is not bipartite, and
    /// <c>null</c> otherwise.</summary>
    /// <returns>an odd-length cycle if the graph is not bipartite
    /// (and hence has an odd-length cycle), and <c>null</c> otherwise</returns>
    ///
    public IEnumerable<int> OddCycle()
    {
      return cycle;
    }

    private bool check(Graph G)
    {
      // graph is bipartite
      if (isBipartite)
      {
        for (int v = 0; v < G.V; v++)
        {
          foreach (int w in G.Adj(v))
          {
            if (color[v] == color[w])
            {
              Console.Error.WriteLine("edge {0}-{1} with {2} and {3} in same side of bipartition", v, w, v, w);
              return false;
            }
          }
        }
      }

      // graph has an odd-length cycle
      else
      {
        // verify cycle
        int first = -1, last = -1;
        foreach (int v in OddCycle())
        {
          if (first == -1) first = v;
          last = v;
        }
        if (first != last)
        {
          Console.Error.WriteLine("cycle begins with {0} and ends with {1}", first, last);
          return false;
        }
      }
      return true;
    }

    private static void BipartiteXReport(BipartiteX b)
    {
      if (b.IsBipartite)
      {
        Graph G = b.G;
        Console.WriteLine("Graph is bipartite");
        for (int v = 0; v < G.V; v++)
        {
          Console.WriteLine(v + ": " + b.Color(v));
        }
      }
      else
      {
        Console.Write("Graph has an odd-length cycle: ");
        foreach (int x in b.OddCycle())
        {
          Console.Write(x + " ");
        }
      }
      Console.WriteLine();
    }

    /// <summary>
    /// Demo test the <c>BipartiteX</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BipartiteX V1 V2 E F", "V1, V2-vertices on either side, E-number of edges, F-number of random edges")]
    public static void MainTest(string[] args)
    {
      int V1 = int.Parse(args[0]);
      int V2 = int.Parse(args[1]);
      int E = int.Parse(args[2]);
      int F = int.Parse(args[3]);

      BipartiteX b;
      // create random bipartite graph with V1 vertices on left side,
      // V2 vertices on right side, and E edges; then add F random edges
      Graph G = GraphGenerator.Bipartite(V1, V2, E);
      Console.WriteLine("Graph is {0}\n", G);

      b = new BipartiteX(G);
      BipartiteXReport(b);

      for (int i = 0; i < F; i++)
      {
        int v = StdRandom.Uniform(V1 + V2);
        int w = StdRandom.Uniform(V1 + V2);
        G.AddEdge(v, w);
      }
      Console.WriteLine("After adding {0} random edges", F);
      Console.WriteLine("Graph is {0}\n", G);

      b = new BipartiteX(G);
      BipartiteXReport(b);
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
