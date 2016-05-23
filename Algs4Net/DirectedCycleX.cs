/******************************************************************************
 *  File name :    DirectedCycleX.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Find a directed cycle in a digraph, using a nonrecursive, queue-based
 *  algorithm. Runs in O(E + V) time.
 *  
 *  C:\> algscmd DirectedCycleX 5 10 5
 *  5 vertices, 15 edges
 *  0: 3 2 1 3
 *  1: 3 3
 *  2: 3 1
 *  3: 2 1 2
 *  4: 0 3 1 2
 *  
 *  Directed cycle: 3 2 3
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DirectedCycleX</c> class represents a data type for
  /// determining whether a digraph has a directed cycle.
  /// The <c>HasCycle</c> operation determines whether the digraph has
  /// a directed cycle and, and of so, the <c>Cycle</c> operation
  /// returns one.
  /// </para><para>
  /// This implementation uses a nonrecursive, queue-based algorithm.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c>
  /// (in the worst case),
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>HasCycle</c> operation takes constant time;
  /// the <c>Cycle</c> operation takes time proportional
  /// to the length of the cycle.
  /// </para><para>
  /// See <seealso cref="DirectedCycle"/> for a recursive version that uses depth-first search.
  /// See <seealso cref="Topological"/> or <seealso cref="TopologicalX"/> to compute a topological order
  /// when the digraph is acyclic.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DirectedCycleX.java.html">DirectedCycleX</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DirectedCycleX
  {
    private LinkedStack<int> cycle;     // the directed cycle; null if digraph is acyclic

    /// <summary>
    /// Determines whether the digraph <c>G</c> has a directed cycle and, if so,
    /// finds such a cycle.</summary>
    /// <param name="G">the digraph</param>
    /// 
    public DirectedCycleX(Digraph G)
    {
      // indegrees of remaining vertices
      int[] indegree = new int[G.V];
      for (int v = 0; v < G.V; v++)
      {
        indegree[v] = G.Indegree(v);
      }

      // initialize queue to contain all vertices with indegree = 0
      LinkedQueue<int> queue = new LinkedQueue<int>();
      for (int v = 0; v < G.V; v++)
        if (indegree[v] == 0) queue.Enqueue(v);

      for (int j = 0; !queue.IsEmpty; j++)
      {
        int v = queue.Dequeue();
        foreach (int w in G.Adj(v))
        {
          indegree[w]--;
          if (indegree[w] == 0) queue.Enqueue(w);
        }
      }

      // there is a directed cycle in subgraph of vertices with indegree >= 1.
      int[] edgeTo = new int[G.V];
      int root = -1;  // any vertex with indegree >= -1
      for (int v = 0; v < G.V; v++)
      {
        if (indegree[v] == 0) continue;
        else root = v;
        foreach (int w in G.Adj(v))
        {
          if (indegree[w] > 0)
          {
            edgeTo[w] = v;
          }
        }
      }

      if (root != -1)
      {
        // find any vertex on cycle
        bool[] visited = new bool[G.V];
        while (!visited[root])
        {
          visited[root] = true;
          root = edgeTo[root];
        }

        // extract cycle
        cycle = new LinkedStack<int>();
        int v = root;
        do
        {
          cycle.Push(v);
          v = edgeTo[v];
        } while (v != root);
        cycle.Push(root);
      }

      Debug.Assert(check());
    }

    /// <summary>
    /// Returns a directed cycle if the digraph has a directed cycle, and <c>null</c> otherwise.</summary>
    /// <returns>a directed cycle (as an iterable) if the digraph has a directed cycle,
    /// and <c>null</c> otherwise</returns>
    ///
    public IEnumerable<int> GetCycle()
    {
      return cycle;
    }

    /// <summary>
    /// Does the digraph have a directed cycle?</summary>
    /// <returns><c>true</c> if the digraph has a directed cycle, <c>false</c> otherwise</returns>
    ///
    public bool HasCycle
    {
      get { return cycle != null; }
    }

    // certify that digraph has a directed cycle if it reports one
    private bool check()
    {

      if (HasCycle)
      {
        // verify cycle
        int first = -1, last = -1;
        foreach (int v in GetCycle())
        {
          if (first == -1) first = v;
          last = v;
        }
        if (first != last)
        {
          Console.Error.Write("cycle begins with {0} and ends with {1}\n", first, last);
          return false;
        }
      }

      return true;
    }

    /// <summary>Demo test for the <c>DirectedCycle</c> class</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DirectedCycleX V E F", "V vertices, E edges, followed by F random edges")]
    public static void MainTest(string[] args)
    {
      // create random DAG with V vertices and E edges; then add F random edges
      int V = int.Parse(args[0]);
      int E = int.Parse(args[1]);
      int F = int.Parse(args[2]);
      Digraph G = DigraphGenerator.Dag(V, E);

      // add F extra edges
      for (int i = 0; i < F; i++)
      {
        int v = StdRandom.Uniform(V);
        int w = StdRandom.Uniform(V);
        G.AddEdge(v, w);
      }

      Console.WriteLine(G);

      DirectedCycleX finder = new DirectedCycleX(G);
      if (finder.HasCycle)
      {
        Console.Write("Directed cycle: ");
        foreach (int v in finder.GetCycle())
        {
          Console.Write(v + " ");
        }
        Console.WriteLine();
      }
      else
      {
        Console.WriteLine("No directed cycle");
      }
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
