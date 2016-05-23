/******************************************************************************
 *  File name :    DirectedEulerianCycle.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Find an Eulerian cycle in a digraph, if one exists.
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DirectedEulerianCycle</c> class represents a data type
  /// for finding an Eulerian cycle or path in a digraph.
  /// An <c>Eulerian cycle</c> is a cycle (not necessarily simple) that
  /// uses every edge in the digraph exactly once.
  /// </para><para>
  /// This implementation uses a nonrecursive depth-first search.
  /// The constructor runs in O(<c>E</c> + <c>V</c>) time,
  /// and uses O(<c>V</c>) extra space, where <c>E</c> is the
  /// number of edges and <c>V</c> the number of vertices
  /// All other methods take O(1) time.</para><para>
  /// To compute Eulerian paths in digraphs, see <seealso cref="DirectedEulerianPath"/>.
  /// To compute Eulerian cycles and paths in undirected graphs, see
  /// <seealso cref="EulerianCycle"/> and <seealso cref="EulerianPath"/>.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DirectedEulerianCycle.java.html">DirectedEulerianCycle</a>
  /// implementation by the respective authors.</para></remarks>
  /// 
  public class DirectedEulerianCycle
  {
    private LinkedStack<int> cycle = null;  // Eulerian cycle; null if no such cylce

    /// <summary>
    /// Computes an Eulerian cycle in the specified digraph, if one exists.
    /// </summary>
    /// <param name="G">the digraph</param>
    ///
    public DirectedEulerianCycle(Digraph G)
    {
      // must have at least one edge
      if (G.E == 0) return;

      // necessary condition: indegree(v) = outdegree(v) for each vertex v
      // (without this check, DFS might return a path instead of a cycle)
      for (int v = 0; v < G.V; v++)
        if (G.Outdegree(v) != G.Indegree(v))
          return;

      // create local view of adjacency lists, to iterate one vertex at a time
      IEnumerator<int>[] adj = new IEnumerator<int>[G.V];
      for (int v = 0; v < G.V; v++)
        adj[v] = G.Adj(v).GetEnumerator();

      // initialize stack with any non-isolated vertex
      int s = nonIsolatedVertex(G);
      LinkedStack<int> stack = new LinkedStack<int>();
      stack.Push(s);

      // greedily add to putative cycle, depth-first search style
      cycle = new LinkedStack<int>();
      while (!stack.IsEmpty)
      {
        int v = stack.Pop();
        while (adj[v].MoveNext())
        {
          stack.Push(v);
          v = adj[v].Current;
        }
        // add vertex with no more leaving edges to cycle
        cycle.Push(v);
      }

      // check if all edges have been used
      // (in case there are two or more vertex-disjoint Eulerian cycles)
      if (cycle.Count != G.E + 1)
        cycle = null;

      Debug.Assert(certifySolution(G));
    }

    /// <summary>
    /// Returns the sequence of vertices on an Eulerian cycle.</summary>
    /// <returns>the sequence of vertices on an Eulerian cycle;
    ///        <c>null</c> if no such cycle</returns>
    ///
    public IEnumerable<int> GetCycle()
    {
      return cycle;
    }

    /// <summary>
    /// Returns true if the digraph has an Eulerian cycle.</summary>
    /// 
    /// <returns><c>true</c> if the digraph has an Eulerian cycle;</returns>
    ///        <c>false</c> otherwise
    ///
    public bool HasEulerianCycle
    {
      get { return cycle != null; }
    }

    // returns any non-isolated vertex; -1 if no such vertex
    private static int nonIsolatedVertex(Digraph G)
    {
      for (int v = 0; v < G.V; v++)
        if (G.Outdegree(v) > 0)
          return v;
      return -1;
    }

    /**************************************************************************
     *
     *  The code below is solely for testing correctness of the data type.
     *
     **************************************************************************/

    // Determines whether a digraph has an Eulerian cycle using necessary
    // and sufficient conditions (without computing the cycle itself):
    //    - at least one edge
    //    - indegree(v) = outdegree(v) for every vertex
    //    - the graph is connected, when viewed as an undirected graph
    //      (ignoring isolated vertices)
    private static bool hasEulerianCycle(Digraph G)
    {

      // Condition 0: at least 1 edge
      if (G.E == 0) return false;

      // Condition 1: indegree(v) == outdegree(v) for every vertex
      for (int v = 0; v < G.V; v++)
        if (G.Outdegree(v) != G.Indegree(v))
          return false;

      // Condition 2: graph is connected, ignoring isolated vertices
      Graph H = new Graph(G.V);
      for (int v = 0; v < G.V; v++)
        foreach (int w in G.Adj(v))
          H.AddEdge(v, w);

      // check that all non-isolated vertices are conneted
      int s = nonIsolatedVertex(G);
      BreadthFirstPaths bfs = new BreadthFirstPaths(H, s);
      for (int v = 0; v < G.V; v++)
        if (H.Degree(v) > 0 && !bfs.HasPathTo(v))
          return false;

      return true;
    }

    // check that solution is correct
    private bool certifySolution(Digraph G)
    {

      // internal consistency check
      if (HasEulerianCycle == (GetCycle() == null)) return false;

      // hashEulerianCycle() returns correct value
      if (HasEulerianCycle != hasEulerianCycle(G)) return false;

      // nothing else to check if no Eulerian cycle
      if (cycle == null) return true;

      // check that cycle() uses correct number of edges
      if (cycle.Count != G.E + 1) return false;

      // TODO: check that cycle() is a directed cycle of G

      return true;
    }


    internal static void UnitTest(Digraph G, String description)
    {
      Console.WriteLine(description);
      Console.WriteLine("-------------------------------------");
      Console.Write(G);

      DirectedEulerianCycle euler = new DirectedEulerianCycle(G);

      Console.Write("Eulerian cycle: ");
      if (euler.HasEulerianCycle)
      {
        foreach (int v in euler.GetCycle())
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
    /// Demo test the <c>DirectedEulerianCycle</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DirectedEulerianCycle V E", "V vertices and E edges")]
    public static void MainTest(string[] args)
    {
      int V = int.Parse(args[0]);
      int E = int.Parse(args[1]);

      // Eulerian cycle
      Digraph G1 = DigraphGenerator.EulerianCycle(V, E);
      DirectedEulerianCycle.UnitTest(G1, "Eulerian cycle");

      // Eulerian path
      Digraph G2 = DigraphGenerator.EulerianPath(V, E);
      DirectedEulerianCycle.UnitTest(G2, "Eulerian path");

      // empty digraph
      Digraph G3 = new Digraph(V);
      DirectedEulerianCycle.UnitTest(G3, "empty digraph");

      // self loop
      Digraph G4 = new Digraph(V);
      int v4 = StdRandom.Uniform(V);
      G4.AddEdge(v4, v4);
      DirectedEulerianCycle.UnitTest(G4, "single self loop");

      // union of two disjoint cycles
      Digraph H1 = DigraphGenerator.EulerianCycle(V / 2, E / 2);
      Digraph H2 = DigraphGenerator.EulerianCycle(V - V / 2, E - E / 2);
      int[] perm = new int[V];
      for (int i = 0; i < V; i++)
        perm[i] = i;
      StdRandom.Shuffle(perm);
      Digraph G5 = new Digraph(V);
      for (int v = 0; v < H1.V; v++)
        foreach (int w in H1.Adj(v))
          G5.AddEdge(perm[v], perm[w]);
      for (int v = 0; v < H2.V; v++)
        foreach (int w in H2.Adj(v))
          G5.AddEdge(perm[V / 2 + v], perm[V / 2 + w]);
      DirectedEulerianCycle.UnitTest(G5, "Union of two disjoint cycles");

      // random digraph
      Digraph G6 = DigraphGenerator.Simple(V, E);
      DirectedEulerianCycle.UnitTest(G6, "simple digraph");

      // 4-vertex digraph - no data file
      //Digraph G7 = new Digraph(new TextInput("eulerianD.txt"));
      //DirectedEulerianCycle.UnitTest(G7, "4-vertex Eulerian digraph");
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
