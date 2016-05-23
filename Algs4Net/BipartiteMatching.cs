/******************************************************************************
 *  File name :    BipartiteMatching.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Find a maximum cardinality matching (and minimum cardinality vertex cover)
 *  in a bipartite graph using the alternating path algorithm.
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>BipartiteMatching</c> class represents a data type for computing a
  /// <c>Maximum (cardinality) matching</c> and a <c>Minimum (cardinality) vertex cover</c>
  /// in a bipartite graph.</para><para>
  /// A <c>Bipartite graph</c> in a graph whose vertices can be partitioned
  /// into two disjoint sets such that every edge has one endpoint in either set.
  /// A <c>Matching</c> in a graph is a subset of its edges with no common
  /// vertices. A <c>Maximum matching</c> is a matching with the maximum number
  /// of edges.
  /// A <c>Perfect matching</c> is a matching which matches all vertices in the graph.
  /// A <c>Vertex cover</c> in a graph is a subset of its vertices such that
  /// every edge is incident to at least one vertex. A <c>Minimum vertex cover</c>
  /// is a vertex cover with the minimum number of vertices.
  /// By Konig's theorem, in any biparite graph, the maximum number of 
  /// edges in matching equals the minimum number of vertices in a vertex cover.</para>
  /// <para>
  /// The maximum matching problem in <c>Nonbipartite</c> graphs is
  /// also important, but all known algorithms for this more general problem
  /// are substantially more complicated.
  /// </para><para>
  /// This implementation uses the <c>Alternating path algorithm</c>.
  /// It is equivalent to reducing to the maximum flow problem and running
  /// the augmenting path algorithm on the resulting flow network, but it
  /// does so with less overhead.
  /// The order of growth of the running time in the worst case is
  /// (<c>E</c> + <c>V</c>) <c>V</c>,
  /// where <c>E</c> is the number of edges and <c>V</c> is the number
  /// of vertices in the graph. It uses extra space (not including the graph)
  /// proportional to <c>V</c>.
  /// </para><para>
  /// See also <seealso cref="HopcroftKarp"/>, which solves the problem in  O(<c>E</c> sqrt(<c>V</c>))
  /// using the Hopcroft-Karp algorithm and
  /// <a href = "http://algs4.cs.princeton.edu/65reductions/BipartiteMatchingToMaxflow.java.html">BipartiteMatchingToMaxflow</a>, which solves the problem in
  /// O(<c>E V</c>) time via a reduction to maxflow.
  /// </para></summary>
  /// <remarks><para>For additional documentation, see
  /// <a href="http://algs4.cs.princeton.edu/65reductions">Section 6.5</a>
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BipartiteMatching.java.html">BipartiteMatching</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class BipartiteMatching
  {
    private const int UNMATCHED = -1;

    private readonly int numVertices;   // number of vertices in the graph
    private BipartiteX bipartition;     // the bipartition
    private int cardinality;            // cardinality of current matching
    private int[] mate;                 // mate[v] =  w if v-w is an edge in current matching
                                        //         = -1 if v is not in current matching
    private bool[] inMinVertexCover;    // inMinVertexCover[v] = true iff v is in min vertex cover
    private bool[] marked;              // marked[v] = true iff v is reachable via alternating path
    private int[] edgeTo;               // edgeTo[v] = w if v-w is last edge on path to w

    /// <summary>
    /// Determines a maximum matching (and a minimum vertex cover)
    /// in a bipartite graph.</summary>
    /// <param name="G">the bipartite graph</param>
    /// <exception cref="ArgumentException">if <c>G</c> is not bipartite</exception>
    ///
    public BipartiteMatching(Graph G)
    {
      bipartition = new BipartiteX(G);
      if (!bipartition.IsBipartite)
      {
        throw new ArgumentException("graph is not bipartite");
      }

      numVertices = G.V;

      // initialize empty matching
      mate = new int[numVertices];
      for (int v = 0; v < numVertices; v++)
        mate[v] = UNMATCHED;

      // alternating path algorithm
      while (hasAugmentingPath(G))
      {
        // find one endpoint t in alternating path
        int t = -1;
        for (int v = 0; v < numVertices; v++)
        {
          if (!IsMatched(v) && edgeTo[v] != -1)
          {
            t = v;
            break;
          }
        }

        // update the matching according to alternating path in edgeTo[] array
        for (int v = t; v != -1; v = edgeTo[edgeTo[v]])
        {
          int w = edgeTo[v];
          mate[v] = w;
          mate[w] = v;
        }
        cardinality++;
      }

      // find min vertex cover from marked[] array
      inMinVertexCover = new bool[numVertices];
      for (int v = 0; v < numVertices; v++)
      {
        if (bipartition.Color(v) && !marked[v]) inMinVertexCover[v] = true;
        if (!bipartition.Color(v) && marked[v]) inMinVertexCover[v] = true;
      }
      Debug.Assert(certifySolution(G));
    }
    
    // is there an augmenting path?
    // an alternating path is a path whose edges belong alternately to the matching and not to the matching
    // an augmenting path is an alternating path that starts and ends at unmatched vertices
    //
    // if so, upon termination edgeTo[] contains a parent-link representation of such a path
    // if not, upon terminatation marked[] specifies the subset of vertices reachable via an alternating
    // path from one side of the bipartition
    //
    // this implementation finds a shortest augmenting path (fewest number of edges), though there
    // is no particular advantage to do so here
    private bool hasAugmentingPath(Graph G)
    {
      marked = new bool[numVertices];

      edgeTo = new int[numVertices];
      for (int v = 0; v < numVertices; v++)
        edgeTo[v] = -1;

      // breadth-first search (starting from all unmatched vertices on one side of bipartition)
      LinkedQueue<int> queue = new LinkedQueue<int>();
      for (int v = 0; v < numVertices; v++)
      {
        if (bipartition.Color(v) && !IsMatched(v))
        {
          queue.Enqueue(v);
          marked[v] = true;
        }
      }

      // run BFS, stopping as soon as an alternating path is found
      while (!queue.IsEmpty)
      {
        int v = queue.Dequeue();
        foreach (int w in G.Adj(v))
        {

          // either (1) forward edge not in matching or (2) backward edge in matching
          if (isResidualGraphEdge(v, w))
          {
            if (!marked[w])
            {
              edgeTo[w] = v;
              marked[w] = true;
              if (!IsMatched(w)) return true;
              queue.Enqueue(w);
            }
          }
        }
      }

      return false;
    }

    // is the edge v-w a forward edge not in the matching or a reverse edge in the matching?
    private bool isResidualGraphEdge(int v, int w)
    {
      if ((mate[v] != w) && bipartition.Color(v)) return true;
      if ((mate[v] == w) && !bipartition.Color(v)) return true;
      return false;
    }

    /// <summary>
    /// Returns the vertex to which the specified vertex is matched in
    /// the maximum matching computed by the algorithm.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the vertex to which vertex <c>v</c> is matched in the
    /// maximum matching; <c>-1</c> if the vertex is not matched</returns>
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
    /// <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentException">unless <c>0 &lt;= v &lt; V</c></exception>
    ///
    public bool IsMatched(int v)
    {
      validate(v);
      return mate[v] != UNMATCHED;
    }

    /// <summary>
    /// Returns the number of edges in a maximum matching.</summary>
    /// <returns>the number of edges in a maximum matching</returns>
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
      get { return cardinality * 2 == numVertices; }
    }

    /// <summary>
    /// Returns true if the specified vertex is in the minimum vertex cover
    /// computed by the algorithm.</summary>
    /// <param name="v">the vertex</param>
    /// <returns><c>true</c> if vertex <c>v</c> is in the minimum vertex cover;
    /// <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentException">unless <c>0 &lt;= v &lt; V</c></exception>
    ///
    public bool InMinVertexCover(int v)
    {
      validate(v);
      return inMinVertexCover[v];
    }

    private void validate(int v)
    {
      if (v < 0 || v >= numVertices)
        throw new IndexOutOfRangeException("vertex " + v + " is not between 0 and " + (numVertices - 1));
    }

    /**************************************************************************
     *
     *  The code below is solely for testing correctness of the data type.
     *
     **************************************************************************/

    // check that mate[] and inVertexCover[] define a max matching and min vertex cover, respectively
    private bool certifySolution(Graph G)
    {
      // check that mate(v) = w iff mate(w) = v
      for (int v = 0; v < numVertices; v++)
      {
        if (Mate(v) == -1) continue;
        if (Mate(Mate(v)) != v) return false;
      }

      // check that size() is consistent with mate()
      int matchedVertices = 0;
      for (int v = 0; v < numVertices; v++)
      {
        if (Mate(v) != -1) matchedVertices++;
      }
      if (2 * Count != matchedVertices) return false;

      // check that size() is consistent with minVertexCover()
      int sizeOfMinVertexCover = 0;
      for (int v = 0; v < numVertices; v++)
        if (InMinVertexCover(v)) sizeOfMinVertexCover++;
      if (Count != sizeOfMinVertexCover) return false;

      // check that mate() uses each vertex at most once
      bool[] isMatched = new bool[numVertices];
      for (int v = 0; v < numVertices; v++)
      {
        int w = mate[v];
        if (w == -1) continue;
        if (v == w) return false;
        if (v >= w) continue;
        if (isMatched[v] || isMatched[w]) return false;
        isMatched[v] = true;
        isMatched[w] = true;
      }

      // check that mate() uses only edges that appear in the graph
      for (int v = 0; v < numVertices; v++)
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
      for (int v = 0; v < numVertices; v++)
        foreach (int w in G.Adj(v))
          if (!InMinVertexCover(v) && !InMinVertexCover(w)) return false;

      return true;
    }

    /// <summary>Demo test the <c>HopcroftKarp</c> data type.
    /// Takes three command-line arguments <c>V1</c>, <c>V2</c>, and <c>E</c>;
    /// creates a random bipartite graph with <c>V1</c> + <c>V2</c> vertices
    /// and <c>E</c> edges; computes a maximum matching and minimum vertex cover;
    /// and prints the results.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BipartiteMatching V1 V2 E", "V1, V2-vertices on either side, E-number of edges")]
    public static void MainTest(string[] args)
    {
      int V1 = int.Parse(args[0]);
      int V2 = int.Parse(args[1]);
      int E = int.Parse(args[2]);
      Graph G = GraphGenerator.Bipartite(V1, V2, E);

      if (G.V < 1000) Console.WriteLine(G);

      BipartiteMatching matching = new BipartiteMatching(G);

      // print maximum matching
      Console.WriteLine("Number of edges in max matching        = {0}", matching.Count);
      Console.WriteLine("Number of vertices in min vertex cover = {0}", matching.Count);
      Console.WriteLine("Graph has a perfect matching           = {0}", matching.IsPerfect);
      Console.WriteLine();

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
