/******************************************************************************
 *  File name :    DirectedEulerianPath.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Find an Eulerian path in a digraph, if one exists.
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DirectedEulerianPath</c> class represents a data type
  /// for finding an Eulerian path in a digraph.
  /// An <c>Eulerian path</c> is a path (not necessarily simple) that
  /// uses every edge in the digraph exactly once.
  /// </para><para>
  /// This implementation uses a nonrecursive depth-first search.
  /// The constructor runs in O(E + V) time, and uses O(V) extra space,
  /// where E is the number of edges and V the number of vertices
  /// All other methods take O(1) time.
  /// </para><para>
  /// To compute Eulerian cycles in digraphs, see <seealso cref="DirectedEulerianCycle"/>.
  /// To compute Eulerian cycles and paths in undirected graphs, see
  /// <seealso cref="EulerianCycle"/> and <seealso cref="EulerianPath"/>.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DirectedEulerianPath.java.html">DirectedEulerianPath</a>
  /// implementation by the respective authors.</para></remarks>
  /// 
  public class DirectedEulerianPath
  {
    private LinkedStack<int> path = null;   // Eulerian path; null if no suh path

    /// <summary>
    /// Computes an Eulerian path in the specified digraph, if one exists.</summary>
    /// <param name="G">the digraph</param>
    ///
    public DirectedEulerianPath(Digraph G)
    {

      // find vertex from which to start potential Eulerian path:
      // a vertex v with outdegree(v) > indegree(v) if it exits;
      // otherwise a vertex with outdegree(v) > 0
      int deficit = 0;
      int s = nonIsolatedVertex(G);
      for (int v = 0; v < G.V; v++)
      {
        if (G.Outdegree(v) > G.Indegree(v))
        {
          deficit += (G.Outdegree(v) - G.Indegree(v));
          s = v;
        }
      }

      // digraph can't have an Eulerian path
      // (this condition is needed)
      if (deficit > 1) return;

      // special case for digraph with zero edges (has a degenerate Eulerian path)
      if (s == -1) s = 0;

      // create local view of adjacency lists, to iterate one vertex at a time
      IEnumerator<int>[] adj = new IEnumerator<int>[G.V];
      for (int v = 0; v < G.V; v++)
        adj[v] = G.Adj(v).GetEnumerator();

      // greedily add to cycle, depth-first search style
      LinkedStack<int> stack = new LinkedStack<int>();
      stack.Push(s);
      path = new LinkedStack<int>();
      while (!stack.IsEmpty)
      {
        int v = stack.Pop();
        while (adj[v].MoveNext())
        {
          stack.Push(v);
          v = adj[v].Current;
        }
        // push vertex with no more available edges to path
        path.Push(v);
      }

      // check if all edges have been used
      if (path.Count != G.E + 1)
        path = null;

      Debug.Assert(check(G));
    }

    /// <summary>
    /// Returns the sequence of vertices on an Eulerian path.</summary>
    /// <returns>the sequence of vertices on an Eulerian path;
    ///        <c>null</c> if no such path</returns>
    ///
    public IEnumerable<int> Path()
    {
      return path;
    }

    /// <summary>
    /// Returns true if the digraph has an Eulerian path.</summary>
    /// <returns><c>true</c> if the digraph has an Eulerian path;
    ///        <c>false</c> otherwise</returns>
    ///
    public bool HasEulerianPath
    {
      get { return path != null; }
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

    // Determines whether a digraph has an Eulerian path using necessary
    // and sufficient conditions (without computing the path itself):
    //    - indegree(v) = outdegree(v) for every vertex,
    //      except one vertex v may have outdegree(v) = indegree(v) + 1
    //      (and one vertex v may have indegree(v) = outdegree(v) + 1)
    //    - the graph is connected, when viewed as an undirected graph
    //      (ignoring isolated vertices)
    // This method is solely for unit testing.
    private static bool hasEulerianPath(Digraph G)
    {
      if (G.E == 0) return true;

      // Condition 1: indegree(v) == outdegree(v) for every vertex,
      // except one vertex may have outdegree(v) = indegree(v) + 1
      int deficit = 0;
      for (int v = 0; v < G.V; v++)
        if (G.Outdegree(v) > G.Indegree(v))
          deficit += (G.Outdegree(v) - G.Indegree(v));
      if (deficit > 1) return false;

      // Condition 2: graph is connected, ignoring isolated vertices
      Graph H = new Graph(G.V);
      for (int v = 0; v < G.V; v++)
        foreach (int w in G.Adj(v))
          H.AddEdge(v, w);

      // check that all non-isolated vertices are connected
      int s = nonIsolatedVertex(G);
      BreadthFirstPaths bfs = new BreadthFirstPaths(H, s);
      for (int v = 0; v < G.V; v++)
        if (H.Degree(v) > 0 && !bfs.HasPathTo(v))
          return false;

      return true;
    }


    private bool check(Digraph G)
    {

      // internal consistency check
      if (HasEulerianPath == (Path() == null)) return false;

      // HashEulerianPath returns correct value
      if (HasEulerianPath != hasEulerianPath(G)) return false;

      // nothing else to check if no Eulerian path
      if (path == null) return true;

      // check that path() uses correct number of edges
      if (path.Count != G.E + 1) return false;

      // TODO: check that Path() is a directed path in G

      return true;
    }


    internal static void UnitTest(Digraph G, String description)
    {
      Console.WriteLine(description);
      Console.WriteLine("-------------------------------------");
      Console.Write(G);

      DirectedEulerianPath euler = new DirectedEulerianPath(G);

      Console.Write("Eulerian path:  ");
      if (euler.HasEulerianPath)
      {
        foreach (int v in euler.Path())
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
    /// Demo test the <c>DirectedEulerianPath</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DirectedEulerianPath V E", "V vertices and E edges")]
    public static void MainTest(string[] args)
    {
      int V = int.Parse(args[0]);
      int E = int.Parse(args[1]);

      // Eulerian cycle
      Digraph G1 = DigraphGenerator.EulerianCycle(V, E);
      DirectedEulerianPath.UnitTest(G1, "Eulerian cycle");

      // Eulerian path
      Digraph G2 = DigraphGenerator.EulerianPath(V, E);
      DirectedEulerianPath.UnitTest(G2, "Eulerian path");

      // add one random edge
      Digraph G3 = new Digraph(G2);
      G3.AddEdge(StdRandom.Uniform(V), StdRandom.Uniform(V));
      DirectedEulerianPath.UnitTest(G3, "one random edge added to Eulerian path");

      // self loop
      Digraph G4 = new Digraph(V);
      int v4 = StdRandom.Uniform(V);
      G4.AddEdge(v4, v4);
      DirectedEulerianPath.UnitTest(G4, "single self loop");

      // single edge
      Digraph G5 = new Digraph(V);
      G5.AddEdge(StdRandom.Uniform(V), StdRandom.Uniform(V));
      DirectedEulerianPath.UnitTest(G5, "single edge");

      // empty digraph
      Digraph G6 = new Digraph(V);
      DirectedEulerianPath.UnitTest(G6, "empty digraph");

      // random digraph
      Digraph G7 = DigraphGenerator.Simple(V, E);
      DirectedEulerianPath.UnitTest(G7, "simple digraph");

      // 4-vertex digraph
      //Digraph G8 = new Digraph(new TextInput("eulerianD.txt"));
      //DirectedEulerianPath.UnitTest(G8, "4-vertex Eulerian digraph");
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
