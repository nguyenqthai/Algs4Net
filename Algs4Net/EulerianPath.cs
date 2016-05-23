/******************************************************************************
 *  File name :    EulerianPath.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Find an Eulerian path in a graph, if one exists.
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>EulerianPath</c> class represents a data type
  /// for finding an Eulerian path in a graph.
  /// An <c>Eulerian path</c> is a path (not necessarily simple) that
  /// uses every edge in the graph exactly once.
  /// </para><para>
  /// This implementation uses a nonrecursive depth-first search.
  /// The constructor runs in O(<c>E</c> + <c>V</c>) time,
  /// and uses O(<c>E</c> + <c>V</c>) extra space,
  /// where <c>E</c> is the number of edges and <c>V</c> the number of vertices
  /// All other methods take O(1) time.
  /// </para><para>
  /// To compute Eulerian cycles in graphs, see <seealso cref="EulerianCycle"/>.
  /// To compute Eulerian cycles and paths in digraphs, see
  /// <seealso cref="DirectedEulerianCycle"/> and <seealso cref="DirectedEulerianPath"/>.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/EulerianPath.java.html">EulerianPath</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class EulerianPath
  {
    private LinkedStack<int> path = null;   // Eulerian path; null if no suh path

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
    /// Computes an Eulerian path in the specified graph, if one exists.</summary>
    /// <param name="G">the graph</param>
    ///
    public EulerianPath(Graph G)
    {

      // find vertex from which to start potential Eulerian path:
      // a vertex v with odd degree(v) if it exits;
      // otherwise a vertex with degree(v) > 0
      int oddDegreeVertices = 0;
      int s = nonIsolatedVertex(G);
      for (int v = 0; v < G.V; v++)
      {
        if (G.Degree(v) % 2 != 0)
        {
          oddDegreeVertices++;
          s = v;
        }
      }

      // graph can't have an Eulerian path
      // (this condition is needed for correctness)
      if (oddDegreeVertices > 2) return;

      // special case for graph with zero edges (has a degenerate Eulerian path)
      if (s == -1) s = 0;

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
      LinkedStack<int> stack = new LinkedStack<int>();
      stack.Push(s);

      // greedily search through edges in iterative DFS style
      path = new LinkedStack<int>();
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
        // push vertex with no more leaving edges to path
        path.Push(v);
      }

      // check if all edges are used
      if (path.Count != G.E + 1)
        path = null;

      Debug.Assert(certifySolution(G));
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
    /// Returns true if the graph has an Eulerian path.</summary>
    /// 
    /// <returns><c>true</c> if the graph has an Eulerian path;</returns>
    ///        <c>false</c> otherwise
    ///
    public bool HasEulerianPath
    {
      get { return path != null; }
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

    // Determines whether a graph has an Eulerian path using necessary
    // and sufficient conditions (without computing the path itself):
    //    - degree(v) is even for every vertex, except for possibly two
    //    - the graph is connected (ignoring isolated vertices)
    // This method is solely for unit testing.
    private static bool hasEulerianPath(Graph G)
    {
      if (G.E == 0) return true;

      // Condition 1: degree(v) is even except for possibly two
      int oddDegreeVertices = 0;
      for (int v = 0; v < G.V; v++)
        if (G.Degree(v) % 2 != 0)
          oddDegreeVertices++;
      if (oddDegreeVertices > 2) return false;

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
      if (HasEulerianPath == (Path() == null)) return false;

      // hashEulerianPath() returns correct value
      if (HasEulerianPath != hasEulerianPath(G)) return false;

      // nothing else to check if no Eulerian path
      if (path == null) return true;

      // check that path() uses correct number of edges
      if (path.Count != G.E + 1) return false;

      // TODO: check that path() is a path in G

      return true;
    }


    internal static void UnitTest(Graph G, String description)
    {
      Console.WriteLine(description);
      Console.WriteLine("-------------------------------------");
      Console.Write(G);

      EulerianPath euler = new EulerianPath(G);

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
    /// Demo test the <c>EulerianPath</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd EulerianPath V E", "V, E-number of vertices and number of edges")]
    public static void MainTest(string[] args)
    {
      int V = int.Parse(args[0]);
      int E = int.Parse(args[1]);

      // Eulerian cycle
      Graph G1 = GraphGenerator.EulerianCycle(V, E);
      EulerianPath.UnitTest(G1, "Eulerian cycle");

      // Eulerian path
      Graph G2 = GraphGenerator.EulerianPath(V, E);
      EulerianPath.UnitTest(G2, "Eulerian path");

      // add one random edge
      Graph G3 = new Graph(G2);
      G3.AddEdge(StdRandom.Uniform(V), StdRandom.Uniform(V));
      EulerianPath.UnitTest(G3, "one random edge added to Eulerian path");

      // self loop
      Graph G4 = new Graph(V);
      int v4 = StdRandom.Uniform(V);
      G4.AddEdge(v4, v4);
      EulerianPath.UnitTest(G4, "single self loop");

      // single edge
      Graph G5 = new Graph(V);
      G5.AddEdge(StdRandom.Uniform(V), StdRandom.Uniform(V));
      EulerianPath.UnitTest(G5, "single edge");

      // empty graph
      Graph G6 = new Graph(V);
      EulerianPath.UnitTest(G6, "empty graph");

      // random graph
      Graph G7 = GraphGenerator.Simple(V, E);
      EulerianPath.UnitTest(G7, "simple graph");
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
