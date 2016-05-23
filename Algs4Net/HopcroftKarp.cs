/******************************************************************************
 *  File name :    HopcroftKarp.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Find a maximum cardinality matching (and minimum cardinality vertex cover)
 *  in a bipartite graph using Hopcroft-Karp algorithm.
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>HopcroftKarp</c> class represents a data type for computing a
  /// <c>Maximum (cardinality) matching</c> and a
  /// <c>Minimum (cardinality) vertex cover</c> in a bipartite graph.
  /// A <c>Bipartite graph</c> in a graph whose vertices can be partitioned
  /// into two disjoint sets such that every edge has one endpoint in either set.
  /// A <c>Matching</c> in a graph is a subset of its edges with no common
  /// vertices. A <c>Maximum matching</c> is a matching with the maximum number
  /// of edges.</para><para>
  /// A <c>Perfect matching</c> is a matching which matches all vertices in the graph.
  /// A <c>Vertex cover</c> in a graph is a subset of its vertices such that
  /// every edge is incident to at least one vertex. A <c>Minimum vertex cover</c>
  /// is a vertex cover with the minimum number of vertices.
  /// By Konig's theorem, in any biparite
  /// graph, the maximum number of edges in matching equals the minimum number
  /// of vertices in a vertex cover.
  /// The maximum matching problem in <c>Nonbipartite</c> graphs is
  /// also important, but all known algorithms for this more general problem
  /// are substantially more complicated.
  /// </para><para>
  /// This implementation uses the <c>Hopcroft-Karp algorithm</c>.
  /// The order of growth of the running time in the worst case is
  /// (<c>E</c> + <c>V</c>) sqrt(<c>V</c>),
  /// where <c>E</c> is the number of edges and <c>V</c> is the number
  /// of vertices in the graph. It uses extra space (not including the graph)
  /// proportional to <c>V</c>.
  /// </para><para>
  /// See also <seealso cref="BipartiteMatching"/>, which solves the problem in
  /// O(<c>E V</c>) time using the <c>Alternating path algorithm</c>
  /// and <a href = "http://algs4.cs.princeton.edu/65reductions/BipartiteMatchingToMaxflow.java.html">BipartiteMatchingToMaxflow</a>,
  /// which solves the problem in O(<c>E V</c>) time via a reduction
  /// to the maxflow problem.
  /// </para></summary>
  /// <remarks><para>For additional documentation, see
  /// <a href="http://algs4.cs.princeton.edu/65reductions">Section 6.5</a>
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/HopcroftKarp.java.html">HopcroftKarp</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class HopcroftKarp
  {
    private const int UNMATCHED = -1;

    private readonly int V;             // number of vertices in the graph
    private BipartiteX bipartition;     // the bipartition
    private int cardinality;            // cardinality of current matching
    private int[] mate;                 // mate[v] =  w if v-w is an edge in current matching
                                        //         = -1 if v is not in current matching
    private bool[] inMinVertexCover;    // inMinVertexCover[v] = true iff v is in min vertex cover
    private bool[] marked;              // marked[v] = true iff v is reachable via alternating path
    private int[] distTo;               // distTo[v] = number of edges on shortest path to v

    /// <summary>
    /// Determines a maximum matching (and a minimum vertex cover)
    /// in a bipartite graph.</summary>
    /// <param name="G">the bipartite graph</param>
    /// <exception cref="ArgumentException">if <c>G</c> is not bipartite</exception>
    ///
    public HopcroftKarp(Graph G)
    {
      bipartition = new BipartiteX(G);
      if (!bipartition.IsBipartite)
      {
        throw new ArgumentException("graph is not bipartite");
      }

      // initialize empty matching
      this.V = G.V;
      mate = new int[V];
      for (int v = 0; v < V; v++)
        mate[v] = UNMATCHED;

      // the call to hasAugmentingPath() provides enough info to reconstruct level graph
      while (hasAugmentingPath(G))
      {

        // to be able to iterate over each adjacency list, keeping track of which
        // vertex in each adjacency list needs to be explored next
        IEnumerator<int>[] adj = new IEnumerator<int>[G.V];
        for (int v = 0; v < G.V; v++)
          adj[v] = G.Adj(v).GetEnumerator();

        // for each unmatched vertex s on one side of bipartition
        for (int s = 0; s < V; s++)
        {
          if (IsMatched(s) || !bipartition.Color(s)) continue;   // or use distTo[s] == 0

          // find augmenting path from s using nonrecursive DFS
          LinkedStack<int> path = new LinkedStack<int>();
          path.Push(s);
          while (!path.IsEmpty)
          {
            int v = path.Peek();

            // retreat, no more edges in level graph leaving v
            if (!adj[v].MoveNext())
              path.Pop();

            // advance
            else
            {
              // process edge v-w only if it is an edge in level graph
              int w = adj[v].Current;
              if (!isLevelGraphEdge(v, w)) continue;

              // add w to augmenting path
              path.Push(w);

              // augmenting path found: update the matching
              if (!IsMatched(w))
              {
                // Console.WriteLine("augmenting path: " + toString(path));

                while (!path.IsEmpty)
                {
                  int x = path.Pop();
                  int y = path.Pop();
                  mate[x] = y;
                  mate[y] = x;
                }
                cardinality++;
              }
            }
          }
        }
      }

      // also find a min vertex cover
      inMinVertexCover = new bool[V];
      for (int v = 0; v < V; v++)
      {
        if (bipartition.Color(v) && !marked[v]) inMinVertexCover[v] = true;
        if (!bipartition.Color(v) && marked[v]) inMinVertexCover[v] = true;
      }

      Debug.Assert(certifySolution(G));
    }

    // string representation of augmenting path (chop off last vertex)
    private static string toString(IEnumerable<int> path)
    {
      StringBuilder sb = new StringBuilder();
      foreach (int v in path)
        sb.Append(v + "-");
      string s = sb.ToString();
      s = s.Substring(0, s.LastIndexOf('-'));
      return s;
    }

    // is the edge v-w in the level graph?
    private bool isLevelGraphEdge(int v, int w)
    {
      return (distTo[w] == distTo[v] + 1) && isResidualGraphEdge(v, w);
    }

    // is the edge v-w a forward edge not in the matching or a reverse edge in the matching?
    private bool isResidualGraphEdge(int v, int w)
    {
      if ((mate[v] != w) && bipartition.Color(v)) return true;
      if ((mate[v] == w) && !bipartition.Color(v)) return true;
      return false;
    }


    // is there an augmenting path?
    // an alternating path is a path whose edges belong alternately to the matching and not to the matchign
    // an augmenting path is an alternating path that starts and ends at unmatched vertices
    //
    // if so, upon termination adj[] contains the level graph;
    // if not, upon termination marked[] specifies those vertices reachable via an alternating path
    // from one side of the bipartition
    private bool hasAugmentingPath(Graph G)
    {

      // shortest path distances
      marked = new bool[V];
      distTo = new int[V];
      for (int v = 0; v < V; v++)
        distTo[v] = int.MaxValue;

      // breadth-first search (starting from all unmatched vertices on one side of bipartition)
      LinkedQueue<int> queue = new LinkedQueue<int>();
      for (int v = 0; v < V; v++)
      {
        if (bipartition.Color(v) && !IsMatched(v))
        {
          queue.Enqueue(v);
          marked[v] = true;
          distTo[v] = 0;
        }
      }

      // run BFS until an augmenting path is found
      // (and keep going until all vertices at that distance are explored)
      bool hasAugmentingPath = false;
      while (!queue.IsEmpty)
      {
        int v = queue.Dequeue();
        foreach (int w in G.Adj(v))
        {

          // forward edge not in matching or backwards edge in matching
          if (isResidualGraphEdge(v, w))
          {
            if (!marked[w])
            {
              distTo[w] = distTo[v] + 1;
              marked[w] = true;
              if (!IsMatched(w))
                hasAugmentingPath = true;

              // stop enqueuing vertices once an alternating path has been discovered
              // (no vertex on same side will be marked if its shortest path distance longer)
              if (!hasAugmentingPath) queue.Enqueue(w);
            }
          }
        }
      }

      return hasAugmentingPath;
    }

    /// <summary>
    /// Returns the vertex to which the specified vertex is matched in
    /// the maximum matching computed by the algorithm.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the vertex to which vertex <c>v</c> is matched in the
    ///        maximum matching; <c>-1</c> if the vertex is not matched</returns>
    /// <exception cref="ArgumentException">unless <c>0 &lt;= v &lt; V</c></exception>
    ///
    ///
    public int Mate(int v)
    {
      validate(v);
      return mate[v];
    }

    /// <summary>
    /// Returns true if the specified vertex is matched in the maximum matching
    /// computed by the algorithm.</summary>
    /// <param name="v">the vertex</param>
    /// <returns><c>true</c> if vertex <c>v</c> is matched in maximum matching;
    ///        <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentException">unless <c>0 &lt;= v &lt; V</c></exception>
    ///
    public bool IsMatched(int v)
    {
      validate(v);
      return mate[v] != UNMATCHED;
    }

    /// <summary>
    /// Returns the number of edges in any maximum matching.</summary>
    /// <returns>the number of edges in any maximum matching</returns>
    ///
    public int Count
    {
      get { return cardinality; }
    }

    /// <summary>
    /// Returns true if the graph contains a perfect matching.
    /// That is, the number of edges in a maximum matching is equal to one half
    /// of the number of vertices in the graph (so that every vertex is matched).</summary>
    /// <returns><c>true</c> if the graph contains a perfect matching;
    /// <c>false</c> otherwise</returns>
    ///
    public bool IsPerfect
    {
      get { return cardinality * 2 == V; }
    }

    /// <summary>
    /// Returns true if the specified vertex is in the minimum vertex cover
    /// computed by the algorithm.</summary>
    /// <param name="v">the vertex</param>
    /// <returns><c>true</c> if vertex <c>v</c> is in the minimum vertex cover;
    ///        <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentException">unless <c>0 &lt;= v &lt; V</c></exception>
    ///
    public bool InMinVertexCover(int v)
    {
      validate(v);
      return inMinVertexCover[v];
    }

    // throw an exception if vertex is invalid
    private void validate(int v)
    {
      if (v < 0 || v >= V)
        throw new IndexOutOfRangeException("vertex " + v + " is not between 0 and " + (V - 1));
    }

    /**************************************************************************
     *
     *  The code below is solely for testing correctness of the data type.
     *
     **************************************************************************/

    // check that mate[] and inVertexCover[] define a max matching and min vertex cover, respectively
    private bool certifySolution(Graph G)
    {

      // check that Mate(v) = w iff Mate(w) = v
      for (int v = 0; v < V; v++)
      {
        if (Mate(v) == -1) continue;
        if (Mate(Mate(v)) != v) return false;
      }

      // check that Count is consistent with Mate()
      int matchedVertices = 0;
      for (int v = 0; v < V; v++)
      {
        if (Mate(v) != -1) matchedVertices++;
      }
      if (2 * Count != matchedVertices) return false;

      // check that Count is consistent with minVertexCover()
      int sizeOfMinVertexCover = 0;
      for (int v = 0; v < V; v++)
        if (InMinVertexCover(v)) sizeOfMinVertexCover++;
      if (Count != sizeOfMinVertexCover) return false;

      // check that Mate() uses each vertex at most once
      bool[] isMatched = new bool[V];
      for (int v = 0; v < V; v++)
      {
        int w = mate[v];
        if (w == -1) continue;
        if (v == w) return false;
        if (v >= w) continue;
        if (isMatched[v] || isMatched[w]) return false;
        isMatched[v] = true;
        isMatched[w] = true;
      }

      // check that Mate() uses only edges that appear in the graph
      for (int v = 0; v < V; v++)
      {
        if (Mate(v) == -1) continue;
        bool isEdge = false;
        foreach (int w in G.Adj(v))
        {
          if (Mate(v) == w) isEdge = true;
        }
        if (!isEdge) return false;
      }

      // check that inMinVertexCover() is a vertex cover
      for (int v = 0; v < V; v++)
        foreach (int w in G.Adj(v))
          if (!InMinVertexCover(v) && !InMinVertexCover(w)) return false;

      return true;
    }

    /// <summary>
    /// Demo test the <c>HopcroftKarp</c> data type.
    /// Takes three command-line arguments <c>V1</c>, <c>V2</c>, and <c>E</c>;
    /// creates a random bipartite graph with <c>V1</c> + <c>V2</c> vertices
    /// and <c>E</c> edges; computes a maximum matching and minimum vertex cover;
    /// and prints the results.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd HopcroftKarp V1 V2 E", "V1, V2-vertices on either side, E-number of edges")]
    public static void MainTest(string[] args)
    {

      int V1 = int.Parse(args[0]);
      int V2 = int.Parse(args[1]);
      int E = int.Parse(args[2]);
      Graph G = GraphGenerator.Bipartite(V1, V2, E);
      if (G.V < 1000) Console.WriteLine(G);

      HopcroftKarp matching = new HopcroftKarp(G);

      // print maximum matching
      Console.Write("Number of edges in max matching        = {0}\n", matching.Count);
      Console.Write("Number of vertices in min vertex cover = {0}\n", matching.Count);
      Console.Write("Graph has a perfect matching           = {0}\n", matching.IsPerfect);
      Console.WriteLine();

      if (G.V >= 1000) return;

      Console.Write("Max matching: ");
      for (int v = 0; v < G.V; v++)
      {
        int w = matching.Mate(v);
        if (matching.IsMatched(v) && v < w)  // print each edge only once
          Console.Write(v + "-" + w + " ");
      }
      Console.WriteLine();

      // print minimum vertex cover
      Console.Write("Min vertex cover: ");
      for (int v = 0; v < G.V; v++)
        if (matching.InMinVertexCover(v))
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
