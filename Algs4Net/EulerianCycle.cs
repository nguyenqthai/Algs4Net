/******************************************************************************
 *  File name :    EulerianCycle.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Find an Eulerian cycle in a graph, if one exists.
 *
 *  Runs in O(E + V) time.
 *
 *  This implementation is tricker than the one for digraphs because
 *  when we use edge v-w from v's adjacency list, we must be careful
 *  not to use the second copy of the edge from w's adjaceny list.
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>EulerianCycle</c> class represents a data type
  /// for finding an Eulerian cycle or path in a graph.
  /// An <c>Eulerian cycle</c> is a cycle (not necessarily simple) that
  /// uses every edge in the graph exactly once.
  /// </para><para>
  /// This implementation uses a nonrecursive depth-first search.
  /// The constructor runs in O(<c>E</c> + <c>V</c>) time,
  /// and uses O(<c>E</c> + <c>V</c>) extra space, where <c>E</c> is the
  /// number of edges and <c>V</c> the number of vertices
  /// All other methods take O(1) time.
  /// </para><para>
  /// To compute Eulerian paths in graphs, see <seealso cref="EulerianPath"/>.
  /// To compute Eulerian cycles and paths in digraphs, see
  /// <seealso cref="DirectedEulerianCycle"/> and <seealso cref="DirectedEulerianPath"/>.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/EulerianCycle.java.html">EulerianCycle</a>
  /// implementation by the respective authors.</para></remarks>
  /// 
  public class EulerianCycle
  {
    private LinkedStack<int> cycle = new LinkedStack<int>();  // Eulerian cycle; null if no such cycle

    // an undirected edge, with a field to indicate whether the edge has already been used
    private class Edge
    {
      private readonly int v;
      private readonly int w;

      public bool IsUsed { get; set; }

      public Edge(int v, int w)
      {
        this.v = v;
        this.w = w;
        IsUsed = false;
      }

      // returns the other vertex of the edge
      public int Other(int vertex)
      {
        if (vertex == v) return w;
        else if (vertex == w) return v;
        else throw new ArgumentException("Illegal endpoint");
      }
    }

    /// <summary>
    /// Computes an Eulerian cycle in the specified graph, if one exists.</summary>
    /// <param name="G">the graph</param>
    ///
    public EulerianCycle(Graph G)
    {

      // must have at least one edge
      if (G.E == 0) return;

      // necessary condition: all vertices have even degree
      // (this test is needed or it might find an Eulerian path instead of cycle)
      for (int v = 0; v < G.V; v++)
        if (G.Degree(v) % 2 != 0)
          return;

      // create local view of adjacency lists, to iterate one vertex at a time
      // the helper Edge data type is used to avoid exploring both copies of an edge v-w
      LinkedQueue<Edge>[] adj = new LinkedQueue<Edge>[G.V];
      for (int v = 0; v < G.V; v++)
        adj[v] = new LinkedQueue<Edge>();

      for (int v = 0; v < G.V; v++)
      {
        int selfLoops = 0;
        foreach (int w in G.Adj(v))
        {
          // careful with self loops
          if (v == w)
          {
            if (selfLoops % 2 == 0)
            {
              Edge e = new Edge(v, w);
              adj[v].Enqueue(e);
              adj[w].Enqueue(e);
            }
            selfLoops++;
          }
          else if (v < w)
          {
            Edge e = new Edge(v, w);
            adj[v].Enqueue(e);
            adj[w].Enqueue(e);
          }
        }
      }

      // initialize stack with any non-isolated vertex
      int s = nonIsolatedVertex(G);
      LinkedStack<int> stack = new LinkedStack<int>();
      stack.Push(s);

      // greedily search through edges in iterative DFS style
      cycle = new LinkedStack<int>();
      while (!stack.IsEmpty)
      {
        int v = stack.Pop();
        while (!adj[v].IsEmpty)
        {
          Edge edge = adj[v].Dequeue();
          if (edge.IsUsed) continue;
          edge.IsUsed = true;
          stack.Push(v);
          v = edge.Other(v);
        }
        // push vertex with no more leaving edges to cycle
        cycle.Push(v);
      }

      // check if all edges are used
      if (cycle.Count != G.E + 1)
        cycle = null;

      Debug.Assert(certifySolution(G));
    }

    /// <summary>
    /// Returns the sequence of vertices on an Eulerian cycle.</summary>
    /// <returns>the sequence of vertices on an Eulerian cycle;
    ///        <c>null</c> if no such cycle</returns>
    ///
    public IEnumerable<int> Cycle()
    {
      return cycle;
    }

    /// <summary>
    /// Returns true if the graph has an Eulerian cycle.</summary>
    /// <returns><c>true</c> if the graph has an Eulerian cycle;
    ///        <c>false</c> otherwise</returns>
    ///
    public bool HasEulerianCycle
    {
      get { return cycle != null; }
    }

    // returns any non-isolated vertex; -1 if no such vertex
    private static int nonIsolatedVertex(Graph G)
    {
      for (int v = 0; v < G.V; v++)
        if (G.Degree(v) > 0)
          return v;
      return -1;
    }

    // TODO: Move these code to a statcic class EulerianCycleTestHelper

    /**************************************************************************
     *
     *  The code below is solely for testing correctness of the data type.
     *
     **************************************************************************/

    // Determines whether a graph has an Eulerian cycle using necessary
    // and sufficient conditions (without computing the cycle itself):
    //    - at least one edge
    //    - degree(v) is even for every vertex v
    //    - the graph is connected (ignoring isolated vertices)
    private static bool hasEulerianCycle(Graph G)
    {

      // Condition 0: at least 1 edge
      if (G.E == 0) return false;

      // Condition 1: degree(v) is even for every vertex
      for (int v = 0; v < G.V; v++)
        if (G.Degree(v) % 2 != 0)
          return false;

      // Condition 2: graph is connected, ignoring isolated vertices
      int s = nonIsolatedVertex(G);
      BreadthFirstPaths bfs = new BreadthFirstPaths(G, s);
      for (int v = 0; v < G.V; v++)
        if (G.Degree(v) > 0 && !bfs.HasPathTo(v))
          return false;

      return true;
    }

    // check that solution is correct
    private bool certifySolution(Graph G)
    {

      // internal consistency check
      if (HasEulerianCycle == (Cycle() == null)) return false;

      // hashEulerianCycle() returns correct value
      if (HasEulerianCycle != hasEulerianCycle(G)) return false;

      // nothing else to check if no Eulerian cycle
      if (cycle == null) return true;

      // check that cycle() uses correct number of edges
      if (cycle.Count != G.E + 1) return false;

      // TODO: check that cycle() is a cycle of G

      // check that first and last vertices in cycle() are the same
      int first = -1, last = -1;
      foreach (int v in Cycle())
      {
        if (first == -1) first = v;
        last = v;
      }
      if (first != last) return false;

      return true;
    }

    internal static void UnitTest(Graph G, string description)
    {
      Console.WriteLine(description);
      Console.WriteLine("-------------------------------------");
      Console.Write(G);

      EulerianCycle euler = new EulerianCycle(G);

      Console.Write("Eulerian cycle: ");
      if (euler.HasEulerianCycle)
      {
        foreach (int v in euler.Cycle())
        {
          Console.Write(v + " ");
        }
        Console.WriteLine();
      }
      else
      {
        Console.WriteLine("none");
      }
      Console.WriteLine();
    }

    /// <summary>
    /// Demo test the <c>EulerianCycle</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd EulerianCycle V E", "V, E-number of vertices and number of edges")]
    public static void MainTest(string[] args)
    {
      int V = int.Parse(args[0]);
      int E = int.Parse(args[1]);

      // Eulerian cycle
      Graph G1 = GraphGenerator.EulerianCycle(V, E);
      EulerianCycle.UnitTest(G1, "Eulerian cycle");

      // Eulerian path
      Graph G2 = GraphGenerator.EulerianPath(V, E);
      EulerianCycle.UnitTest(G2, "Eulerian path");

      // empty graph
      Graph G3 = new Graph(V);
      EulerianCycle.UnitTest(G3, "empty graph");

      // self loop
      Graph G4 = new Graph(V);
      int v4 = StdRandom.Uniform(V);
      G4.AddEdge(v4, v4);
      EulerianCycle.UnitTest(G4, "single self loop");

      // union of two disjoint cycles
      Graph H1 = GraphGenerator.EulerianCycle(V / 2, E / 2);
      Graph H2 = GraphGenerator.EulerianCycle(V - V / 2, E - E / 2);
      int[] perm = new int[V];
      for (int i = 0; i < V; i++)
        perm[i] = i;
      StdRandom.Shuffle(perm);
      Graph G5 = new Graph(V);
      for (int v = 0; v < H1.V; v++)
        foreach (int w in H1.Adj(v))
          G5.AddEdge(perm[v], perm[w]);
      for (int v = 0; v < H2.V; v++)
        foreach (int w in H2.Adj(v))
          G5.AddEdge(perm[V / 2 + v], perm[V / 2 + w]);
      EulerianCycle.UnitTest(G5, "Union of two disjoint cycles");

      // random digraph
      Graph G6 = GraphGenerator.Simple(V, E);
      EulerianCycle.UnitTest(G6, "simple graph");
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
