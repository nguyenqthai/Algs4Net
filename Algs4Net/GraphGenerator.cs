/******************************************************************************
 *  File name :    GraphGenerator.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A graph generator.
 *
 *  For many more graph generators, see
 *  http://networkx.github.io/documentation/latest/reference/generators.html
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>GraphGenerator</c> class provides static methods for creating
  /// various graphs, including Erdos-Renyi random graphs, random bipartite
  /// graphs, random k-regular graphs, and random rooted trees.
  /// </summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/GraphGenerator.java.html">GraphGenerator</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class GraphGenerator
  {
    private sealed class Edge : IComparable<Edge> {
      private int v;
      private int w;

      public Edge(int v, int w)
      {
        if (v < w)
        {
          this.v = v;
          this.w = w;
        }
        else
        {
          this.v = w;
          this.w = v;
        }
      }

      public int CompareTo(Edge that)
      {
        if (this.v < that.v) return -1;
        if (this.v > that.v) return +1;
        if (this.w < that.w) return -1;
        if (this.w > that.w) return +1;
        return 0;
      }
    }

    // this class cannot be instantiated
    private GraphGenerator() { }

    /// <summary>Returns a random simple graph containing <c>V</c> vertices and <c>E</c> edges.
    /// </summary>
    /// <param name="V">the number of vertices</param>
    /// <param name="E">the number of vertices</param>
    /// <returns>a random simple graph on <c>V</c> vertices, containing a total
    /// of <c>E</c> edges</returns>
    /// <exception cref="ArgumentException">if no such simple graph exists</exception>
    ///
    public static Graph Simple(int V, int E)
    {
      if (E > (long)V * (V - 1) / 2) throw new ArgumentException("Too many edges for simple graphs");
      if (E < 0) throw new ArgumentException("Too few edges for simple graphs");
      Graph G = new Graph(V);
      SET<Edge> set = new SET<Edge>();
      while (G.E < E)
      {
        int v = StdRandom.Uniform(V);
        int w = StdRandom.Uniform(V);
        Edge e = new Edge(v, w);
        if ((v != w) && !set.Contains(e))
        {
          set.Add(e);
          G.AddEdge(v, w);
        }
      }
      return G;
    }

    /// <summary>
    /// Returns a random simple graph on <c>V</c> vertices, with an
    /// edge between any two vertices with probability <c>p</c>. This is sometimes
    /// referred to as the Erdos-Renyi random graph model.</summary>
    /// <param name="V">the number of vertices</param>
    /// <param name="p">the probability of choosing an edge</param>
    /// <returns>a random simple graph on <c>V</c> vertices, with an edge between
    ///    any two vertices with probability <c>p</c></returns>
    /// <exception cref="ArgumentException">if probability is not between 0 and 1</exception>
    ///
    public static Graph Simple(int V, double p)
    {
      if (p < 0.0 || p > 1.0)
        throw new ArgumentException("Probability must be between 0 and 1");
      Graph G = new Graph(V);
      for (int v = 0; v < V; v++)
        for (int w = v + 1; w < V; w++)
          if (StdRandom.Bernoulli(p))
            G.AddEdge(v, w);
      return G;
    }

    /// <summary>
    /// Returns the complete graph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices</param>
    /// <returns>the complete graph on <c>V</c> vertices</returns>
    ///
    public static Graph Complete(int V)
    {
      return Simple(V, 1.0);
    }

    /// <summary>
    /// Returns a complete bipartite graph on <c>V1</c> and <c>V2</c> vertices.</summary>
    /// <param name="V1">the number of vertices in one partition</param>
    /// <param name="V2">the number of vertices in the other partition</param>
    /// <returns>a complete bipartite graph on <c>V1</c> and <c>V2</c> vertices</returns>
    /// <exception cref="ArgumentException">if probability is not between 0 and 1</exception>
    ///
    public static Graph CompleteBipartite(int V1, int V2)
    {
      return Bipartite(V1, V2, V1 * V2);
    }

    /// <summary>
    /// Returns a random simple bipartite graph on <c>V1</c> and <c>V2</c> vertices
    /// with <c>E</c> edges.</summary>
    /// <param name="V1">the number of vertices in one partition</param>
    /// <param name="V2">the number of vertices in the other partition</param>
    /// <param name="E">the number of edges</param>
    /// <returns>a random simple bipartite graph on <c>V1</c> and <c>V2</c> vertices,
    ///   containing a total of <c>E</c> edges</returns>
    /// <exception cref="ArgumentException">if no such simple bipartite graph exists</exception>
    ///
    public static Graph Bipartite(int V1, int V2, int E)
    {
      if (E > (long)V1 * V2) throw new ArgumentException("Too many edges for bipartite graphs");
      if (E < 0) throw new ArgumentException("Too few edges for bipartite graphs");
      Graph G = new Graph(V1 + V2);

      int[] vertices = new int[V1 + V2];
      for (int i = 0; i < V1 + V2; i++)
        vertices[i] = i;
      StdRandom.Shuffle(vertices);

      SET<Edge> set = new SET<Edge>();
      while (G.E < E)
      {
        int i = StdRandom.Uniform(V1);
        int j = V1 + StdRandom.Uniform(V2);
        Edge e = new Edge(vertices[i], vertices[j]);
        if (!set.Contains(e))
        {
          set.Add(e);
          G.AddEdge(vertices[i], vertices[j]);
        }
      }
      return G;
    }

    /// <summary>
    /// Returns a random simple bipartite graph on <c>V1</c> and <c>V2</c> vertices,
    /// containing each possible edge with probability <c>p</c>.</summary>
    /// <param name="V1">the number of vertices in one partition</param>
    /// <param name="V2">the number of vertices in the other partition</param>
    /// <param name="p">the probability that the graph contains an edge with one endpoint in either side</param>
    /// <returns>a random simple bipartite graph on <c>V1</c> and <c>V2</c> vertices,
    ///   containing each possible edge with probability <c>p</c></returns>
    /// <exception cref="ArgumentException">if probability is not between 0 and 1</exception>
    ///
    public static Graph Bipartite(int V1, int V2, double p)
    {
      if (p < 0.0 || p > 1.0)
        throw new ArgumentException("Probability must be between 0 and 1");
      int[] vertices = new int[V1 + V2];
      for (int i = 0; i < V1 + V2; i++)
        vertices[i] = i;
      StdRandom.Shuffle(vertices);
      Graph G = new Graph(V1 + V2);
      for (int i = 0; i < V1; i++)
        for (int j = 0; j < V2; j++)
          if (StdRandom.Bernoulli(p))
            G.AddEdge(vertices[i], vertices[V1 + j]);
      return G;
    }

    /// <summary>
    /// Returns a path graph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices in the path</param>
    /// <returns>a path graph on <c>V</c> vertices</returns>
    ///
    public static Graph Path(int V)
    {
      Graph G = new Graph(V);
      int[] vertices = new int[V];
      for (int i = 0; i < V; i++)
        vertices[i] = i;
      StdRandom.Shuffle(vertices);
      for (int i = 0; i < V - 1; i++)
      {
        G.AddEdge(vertices[i], vertices[i + 1]);
      }
      return G;
    }

    /// <summary>
    /// Returns a complete binary tree graph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices in the binary tree</param>
    /// <returns>a complete binary tree graph on <c>V</c> vertices</returns>
    ///
    public static Graph BinaryTree(int V)
    {
      Graph G = new Graph(V);
      int[] vertices = new int[V];
      for (int i = 0; i < V; i++)
        vertices[i] = i;
      StdRandom.Shuffle(vertices);
      for (int i = 1; i < V; i++)
      {
        G.AddEdge(vertices[i], vertices[(i - 1) / 2]);
      }
      return G;
    }

    /// <summary>
    /// Returns a cycle graph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices in the cycle</param>
    /// <returns>a cycle graph on <c>V</c> vertices</returns>
    ///
    public static Graph Cycle(int V)
    {
      Graph G = new Graph(V);
      int[] vertices = new int[V];
      for (int i = 0; i < V; i++)
        vertices[i] = i;
      StdRandom.Shuffle(vertices);
      for (int i = 0; i < V - 1; i++)
      {
        G.AddEdge(vertices[i], vertices[i + 1]);
      }
      G.AddEdge(vertices[V - 1], vertices[0]);
      return G;
    }

    /// <summary>
    /// Returns an Eulerian cycle graph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices in the cycle</param>
    /// <param name="E">the number of edges in the cycle</param>
    /// <returns>a graph that is an Eulerian cycle on <c>V</c> vertices
    ///        and <c>E</c> edges</returns>
    /// <exception cref="ArgumentException">if either V &lt;= 0 or E &lt;= 0</exception>
    ///
    public static Graph EulerianCycle(int V, int E)
    {
      if (E <= 0)
        throw new ArgumentException("An Eulerian cycle must have at least one edge");
      if (V <= 0)
        throw new ArgumentException("An Eulerian cycle must have at least one vertex");
      Graph G = new Graph(V);
      int[] vertices = new int[E];
      for (int i = 0; i < E; i++)
        vertices[i] = StdRandom.Uniform(V);
      for (int i = 0; i < E - 1; i++)
      {
        G.AddEdge(vertices[i], vertices[i + 1]);
      }
      G.AddEdge(vertices[E - 1], vertices[0]);
      return G;
    }

    /// <summary>
    /// Returns an Eulerian path graph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices in the path</param>
    /// <param name="E">the number of edges in the path</param>
    /// <returns>a graph that is an Eulerian path on <c>V</c> vertices
    ///        and <c>E</c> edges</returns>
    /// <exception cref="ArgumentException">if either V &lt;= 0 or E &lt; 0</exception>
    ///
    public static Graph EulerianPath(int V, int E)
    {
      if (E < 0)
        throw new ArgumentException("negative number of edges");
      if (V <= 0)
        throw new ArgumentException("An Eulerian path must have at least one vertex");
      Graph G = new Graph(V);
      int[] vertices = new int[E + 1];
      for (int i = 0; i < E + 1; i++)
        vertices[i] = StdRandom.Uniform(V);
      for (int i = 0; i < E; i++)
      {
        G.AddEdge(vertices[i], vertices[i + 1]);
      }
      return G;
    }

    /// <summary>
    /// Returns a wheel graph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices in the wheel</param>
    /// <returns>a wheel graph on <c>V</c> vertices: a single vertex connected to
    ///    every vertex in a cycle on <c>V-1</c> vertices</returns>
    ///
    public static Graph Wheel(int V)
    {
      if (V <= 1) throw new ArgumentException("Number of vertices must be at least 2");
      Graph G = new Graph(V);
      int[] vertices = new int[V];
      for (int i = 0; i < V; i++)
        vertices[i] = i;
      StdRandom.Shuffle(vertices);

      // simple cycle on V-1 vertices
      for (int i = 1; i < V - 1; i++)
      {
        G.AddEdge(vertices[i], vertices[i + 1]);
      }
      G.AddEdge(vertices[V - 1], vertices[1]);

      // connect vertices[0] to every vertex on cycle
      for (int i = 1; i < V; i++)
      {
        G.AddEdge(vertices[0], vertices[i]);
      }

      return G;
    }

    /// <summary>
    /// Returns a star graph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices in the star</param>
    /// <returns>a star graph on <c>V</c> vertices: a single vertex connected to
    ///    every other vertex</returns>
    ///
    public static Graph Star(int V)
    {
      if (V <= 0) throw new ArgumentException("Number of vertices must be at least 1");
      Graph G = new Graph(V);
      int[] vertices = new int[V];
      for (int i = 0; i < V; i++)
        vertices[i] = i;
      StdRandom.Shuffle(vertices);

      // connect vertices[0] to every other vertex
      for (int i = 1; i < V; i++)
      {
        G.AddEdge(vertices[0], vertices[i]);
      }

      return G;
    }

    /// <summary>
    /// Returns a uniformly random <c>k</c>-regular graph on <c>V</c> vertices
    /// (not necessarily simple). The graph is simple with probability only about e^(-k^2/4),
    /// which is tiny when k = 14.</summary>
    /// <param name="V">the number of vertices in the graph</param>
    /// <param name="k">the k-regular value</param>
    /// <returns>a uniformly random <c>k</c>-regular graph on <c>V</c> vertices.</returns>
    ///
    public static Graph Regular(int V, int k)
    {
      if (V * k % 2 != 0) throw new ArgumentException("Number of vertices * k must be even");
      Graph G = new Graph(V);

      // create k copies of each vertex
      int[] vertices = new int[V * k];
      for (int v = 0; v < V; v++)
      {
        for (int j = 0; j < k; j++)
        {
          vertices[v + V * j] = v;
        }
      }

      // pick a random perfect matching
      StdRandom.Shuffle(vertices);
      for (int i = 0; i < V * k / 2; i++)
      {
        G.AddEdge(vertices[2 * i], vertices[2 * i + 1]);
      }
      return G;
    }

    // For a complete description, see
    // http://www.proofwiki.org/wiki/Labeled_Tree_from_Prüfer_Sequence
    // http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.36.6484&rep=rep1&type=pdf
    /// <summary>
    /// Returns a uniformly random tree on <c>V</c> vertices.
    /// This algorithm uses a Prufer sequence and takes time proportional to <c>V log V</c>.</summary>
    /// <param name="V">the number of vertices in the tree</param>
    /// <returns>a uniformly random tree on <c>V</c> vertices</returns>
    ///
    public static Graph Tree(int V)
    {
      Graph G = new Graph(V);

      // special case
      if (V == 1) return G;

      // Cayley's theorem: there are V^(V-2) labeled trees on V vertices
      // Prufer sequence: sequence of V-2 values between 0 and V-1
      // Prufer's proof of Cayley's theorem: Prufer sequences are in 1-1
      // with labeled trees on V vertices
      int[] prufer = new int[V - 2];
      for (int i = 0; i < V - 2; i++)
        prufer[i] = StdRandom.Uniform(V);

      // degree of vertex v = 1 + number of times it appers in Prufer sequence
      int[] degree = new int[V];
      for (int v = 0; v < V; v++)
        degree[v] = 1;
      for (int i = 0; i < V - 2; i++)
        degree[prufer[i]]++;

      // pq contains all vertices of degree 1
      MinPQ<int> pq = new MinPQ<int>();
      for (int v = 0; v < V; v++)
        if (degree[v] == 1) pq.Insert(v);

      // repeatedly delMin() degree 1 vertex that has the minimum index
      for (int i = 0; i < V - 2; i++)
      {
        int v = pq.DelMin();
        G.AddEdge(v, prufer[i]);
        degree[v]--;
        degree[prufer[i]]--;
        if (degree[prufer[i]] == 1) pq.Insert(prufer[i]);
      }
      G.AddEdge(pq.DelMin(), pq.DelMin());
      return G;
    }

    /// <summary>
    /// Demo test the <c>GraphGenerator</c> library.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd GraphGenerator V E", "Number of vertices and number of edges")]
    public static void MainTest(string[] args)
    {
      int V = int.Parse(args[0]);
      int E = int.Parse(args[1]);
      int V1 = V / 2;
      int V2 = V - V1;

      Console.WriteLine("complete graph");
      Console.WriteLine(GraphGenerator.Complete(V));
      Console.WriteLine();

      Console.WriteLine("simple");
      Console.WriteLine(GraphGenerator.Simple(V, E));
      Console.WriteLine();

      Console.WriteLine("Erdos-Renyi");
      double p = E / (V * (V - 1) / 2.0);
      Console.WriteLine(GraphGenerator.Simple(V, p));
      Console.WriteLine();

      Console.WriteLine("complete bipartite");
      Console.WriteLine(GraphGenerator.CompleteBipartite(V1, V2));
      Console.WriteLine();

      Console.WriteLine("bipartite");
      Console.WriteLine(GraphGenerator.Bipartite(V1, V2, E));
      Console.WriteLine();

      Console.WriteLine("Erdos Renyi bipartite");
      double q = (double)E / (V1 * V2);
      Console.WriteLine(GraphGenerator.Bipartite(V1, V2, q));
      Console.WriteLine();

      Console.WriteLine("path");
      Console.WriteLine(GraphGenerator.Path(V));
      Console.WriteLine();

      Console.WriteLine("cycle");
      Console.WriteLine(GraphGenerator.Cycle(V));
      Console.WriteLine();

      Console.WriteLine("binary tree");
      Console.WriteLine(GraphGenerator.BinaryTree(V));
      Console.WriteLine();

      Console.WriteLine("tree");
      Console.WriteLine(GraphGenerator.Tree(V));
      Console.WriteLine();

      Console.WriteLine("4-regular");
      Console.WriteLine(GraphGenerator.Regular(V, 4));
      Console.WriteLine();

      Console.WriteLine("star");
      Console.WriteLine(GraphGenerator.Star(V));
      Console.WriteLine();

      Console.WriteLine("wheel");
      Console.WriteLine(GraphGenerator.Wheel(V));
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
