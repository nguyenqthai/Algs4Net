/******************************************************************************
 *  File name :    DigraphGenerator.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A digraph generator.
 *  
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algs4Net
{
  /// <summary>
  /// The <c>DigraphGenerator</c> class provides static methods for creating
  /// various digraphs, including Erdos-Renyi random digraphs, random DAGs,
  /// random rooted trees, random rooted DAGs, random tournaments, path digraphs,
  /// cycle digraphs, and the complete digraph.
  /// </summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DigraphGenerator.java.html">DigraphGenerator</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DigraphGenerator
  {
    private sealed class Edge : IComparable<Edge>
    {
      private int v;
      private int w;

      public Edge(int v, int w)
      {
        this.v = v;
        this.w = w;
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
    private DigraphGenerator() { }

    /// <summary>Returns a random simple digraph containing <c>V</c> vertices and <c>E</c> edges.</summary>
    /// <param name="V">the number of vertices</param>
    /// <param name="E">the number of vertices</param>
    /// <returns>a random simple digraph on <c>V</c> vertices, containing a total
    ///    of <c>E</c> edges</returns>
    /// <exception cref="ArgumentException">if no such simple digraph exists</exception>
    ///
    public static Digraph Simple(int V, int E)
    {
      if (E > (long)V * (V - 1)) throw new ArgumentException("Too many edges");
      if (E < 0) throw new ArgumentException("Too few edges");
      Digraph G = new Digraph(V);
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
    /// Returns a random simple digraph on <c>V</c> vertices, with an
    /// edge between any two vertices with probability <c>p</c>. This is sometimes
    /// referred to as the Erdos-Renyi random digraph model.
    /// This implementations takes time propotional to V^2 (even if <c>p</c> is small).</summary>
    /// <param name="V">the number of vertices</param>
    /// <param name="p">the probability of choosing an edge</param>
    /// <returns>a random simple digraph on <c>V</c> vertices, with an edge between
    ///    any two vertices with probability <c>p</c></returns>
    /// <exception cref="ArgumentException">if probability is not between 0 and 1</exception>
    ///
    public static Digraph Simple(int V, double p)
    {
      if (p < 0.0 || p > 1.0)
        throw new ArgumentException("Probability must be between 0 and 1");
      Digraph G = new Digraph(V);
      for (int v = 0; v < V; v++)
        for (int w = 0; w < V; w++)
          if (v != w)
            if (StdRandom.Bernoulli(p))
              G.AddEdge(v, w);
      return G;
    }

    /// <summary>
    /// Returns the complete digraph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices</param>
    /// <returns>the complete digraph on <c>V</c> vertices</returns>
    ///
    public static Digraph Complete(int V)
    {
      return Simple(V, V * (V - 1));
    }

    /// <summary>
    /// Returns a random simple DAG containing <c>V</c> vertices and <c>E</c> edges.
    /// Note: it is not uniformly selected at random among all such DAGs.</summary>
    /// <param name="V">the number of vertices</param>
    /// <param name="E">the number of vertices</param>
    /// <returns>a random simple DAG on <c>V</c> vertices, containing a total
    ///    of <c>E</c> edges</returns>
    /// <exception cref="ArgumentException">if no such simple DAG exists</exception>
    ///
    public static Digraph Dag(int V, int E)
    {
      if (E > (long)V * (V - 1) / 2) throw new ArgumentException("Too many edges");
      if (E < 0) throw new ArgumentException("Too few edges");
      Digraph G = new Digraph(V);
      SET<Edge> set = new SET<Edge>();
      int[] vertices = new int[V];
      for (int i = 0; i < V; i++)
        vertices[i] = i;
      StdRandom.Shuffle(vertices);
      while (G.E < E)
      {
        int v = StdRandom.Uniform(V);
        int w = StdRandom.Uniform(V);
        Edge e = new Edge(v, w);
        if ((v < w) && !set.Contains(e))
        {
          set.Add(e);
          G.AddEdge(vertices[v], vertices[w]);
        }
      }
      return G;
    }

    // tournament
    /// <summary>
    /// Returns a random tournament digraph on <c>V</c> vertices. A tournament digraph
    /// is a DAG in which for every two vertices, there is one directed edge.
    /// A tournament is an oriented complete graph.</summary>
    /// <param name="V">the number of vertices</param>
    /// <returns>a random tournament digraph on <c>V</c> vertices</returns>
    ///
    public static Digraph Tournament(int V)
    {
      Digraph G = new Digraph(V);
      for (int v = 0; v < G.V; v++)
      {
        for (int w = v + 1; w < G.V; w++)
        {
          if (StdRandom.Bernoulli(0.5)) G.AddEdge(v, w);
          else G.AddEdge(w, v);
        }
      }
      return G;
    }

    /// <summary>
    /// Returns a random rooted-in DAG on <c>V</c> vertices and <c>E</c> edges.
    /// A rooted in-tree is a DAG in which there is a single vertex
    /// reachable from every other vertex.
    /// The DAG returned is not chosen uniformly at random among all such DAGs.</summary>
    /// <param name="V">the number of vertices</param>
    /// <param name="E">the number of edges</param>
    /// <returns>a random rooted-in DAG on <c>V</c> vertices and <c>E</c> edges</returns>
    ///
    public static Digraph RootedInDAG(int V, int E)
    {
      if (E > (long)V * (V - 1) / 2) throw new ArgumentException("Too many edges");
      if (E < V - 1) throw new ArgumentException("Too few edges");
      Digraph G = new Digraph(V);
      SET<Edge> set = new SET<Edge>();

      // fix a topological order
      int[] vertices = new int[V];
      for (int i = 0; i < V; i++)
        vertices[i] = i;
      StdRandom.Shuffle(vertices);

      // one edge pointing from each vertex, other than the root = vertices[V-1]
      for (int v = 0; v < V - 1; v++)
      {
        int w = StdRandom.Uniform(v + 1, V);
        Edge e = new Edge(v, w);
        set.Add(e);
        G.AddEdge(vertices[v], vertices[w]);
      }

      while (G.E < E)
      {
        int v = StdRandom.Uniform(V);
        int w = StdRandom.Uniform(V);
        Edge e = new Edge(v, w);
        if ((v < w) && !set.Contains(e))
        {
          set.Add(e);
          G.AddEdge(vertices[v], vertices[w]);
        }
      }
      return G;
    }

    /// <summary>
    /// Returns a random rooted-out DAG on <c>V</c> vertices and <c>E</c> edges.
    /// A rooted out-tree is a DAG in which every vertex is reachable from a
    /// single vertex.
    /// The DAG returned is not chosen uniformly at random among all such DAGs.</summary>
    /// <param name="V">the number of vertices</param>
    /// <param name="E">the number of edges</param>
    /// <returns>a random rooted-out DAG on <c>V</c> vertices and <c>E</c> edges</returns>
    ///
    public static Digraph RootedOutDAG(int V, int E)
    {
      if (E > (long)V * (V - 1) / 2) throw new ArgumentException("Too many edges");
      if (E < V - 1) throw new ArgumentException("Too few edges");
      Digraph G = new Digraph(V);
      SET<Edge> set = new SET<Edge>();

      // fix a topological order
      int[] vertices = new int[V];
      for (int i = 0; i < V; i++)
        vertices[i] = i;
      StdRandom.Shuffle(vertices);

      // one edge pointing from each vertex, other than the root = vertices[V-1]
      for (int v = 0; v < V - 1; v++)
      {
        int w = StdRandom.Uniform(v + 1, V);
        Edge e = new Edge(w, v);
        set.Add(e);
        G.AddEdge(vertices[w], vertices[v]);
      }

      while (G.E < E)
      {
        int v = StdRandom.Uniform(V);
        int w = StdRandom.Uniform(V);
        Edge e = new Edge(w, v);
        if ((v < w) && !set.Contains(e))
        {
          set.Add(e);
          G.AddEdge(vertices[w], vertices[v]);
        }
      }
      return G;
    }

    /// <summary>
    /// Returns a random rooted-in tree on <c>V</c> vertices.
    /// A rooted in-tree is an oriented tree in which there is a single vertex
    /// reachable from every other vertex.
    /// The tree returned is not chosen uniformly at random among all such trees.</summary>
    /// <param name="V">the number of vertices</param>
    /// <returns>a random rooted-in tree on <c>V</c> vertices</returns>
    ///
    public static Digraph RootedInTree(int V)
    {
      return RootedInDAG(V, V - 1);
    }

    /// <summary>
    /// Returns a random rooted-out tree on <c>V</c> vertices. A rooted out-tree
    /// is an oriented tree in which each vertex is reachable from a single vertex.
    /// It is also known as a <c>Arborescence</c> or <c>Branching</c>.
    /// The tree returned is not chosen uniformly at random among all such trees.</summary>
    /// <param name="V">the number of vertices</param>
    /// <returns>a random rooted-out tree on <c>V</c> vertices</returns>
    ///
    public static Digraph RootedOutTree(int V)
    {
      return RootedOutDAG(V, V - 1);
    }

    /// <summary>
    /// Returns a path digraph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices in the path</param>
    /// <returns>a digraph that is a directed path on <c>V</c> vertices</returns>
    ///
    public static Digraph Path(int V)
    {
      Digraph G = new Digraph(V);
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
    /// Returns a complete binary tree digraph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices in the binary tree</param>
    /// <returns>a digraph that is a complete binary tree on <c>V</c> vertices</returns>
    ///
    public static Digraph BinaryTree(int V)
    {
      Digraph G = new Digraph(V);
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
    /// Returns a cycle digraph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices in the cycle</param>
    /// <returns>a digraph that is a directed cycle on <c>V</c> vertices</returns>
    ///
    public static Digraph Cycle(int V)
    {
      Digraph G = new Digraph(V);
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
    /// Returns an Eulerian cycle digraph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices in the cycle</param>
    /// <param name="E">the number of edges in the cycle</param>
    /// <returns>a digraph that is a directed Eulerian cycle on <c>V</c> vertices
    ///        and <c>E</c> edges</returns>
    /// <exception cref="ArgumentException">if either V &lt;= 0 or E &lt;= 0</exception>
    ///
    public static Digraph EulerianCycle(int V, int E)
    {
      if (E <= 0)
        throw new ArgumentException("An Eulerian cycle must have at least one edge");
      if (V <= 0)
        throw new ArgumentException("An Eulerian cycle must have at least one vertex");
      Digraph G = new Digraph(V);
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
    /// Returns an Eulerian path digraph on <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices in the path</param>
    /// <param name="E">the number of edges in the path</param>
    /// <returns>a digraph that is a directed Eulerian path on <c>V</c> vertices</returns>
    ///        and <c>E</c> edges
    /// <exception cref="ArgumentException">if either V &lt;= 0 or E &lt; 0</exception>
    ///
    public static Digraph EulerianPath(int V, int E)
    {
      if (E < 0)
        throw new ArgumentException("negative number of edges");
      if (V <= 0)
        throw new ArgumentException("An Eulerian path must have at least one vertex");
      Digraph G = new Digraph(V);
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
    /// Returns a random simple digraph on <c>V</c> vertices, <c>E</c>
    /// edges and (at least) <c>c</c> strong components. The vertices are randomly
    /// assigned integer labels between <c>0</c> and <c>c-1</c> (corresponding to 
    /// strong components). Then, a strong component is creates among the vertices
    /// with the same label. Next, random edges (either between two vertices with
    /// the same labels or from a vetex with a smaller label to a vertex with a 
    /// larger label). The number of components will be equal to the number of
    /// distinct labels that are assigned to vertices.</summary>
    /// <param name="V">the number of vertices</param>
    /// <param name="E">the number of edges</param>
    /// <param name="c">the (maximum) number of strong components</param>
    /// <returns>a random simple digraph on <c>V</c> vertices and
    /// <c>E</c> edges, with (at most) <c>c</c> strong components</returns>
    /// <exception cref="ArgumentException">if <c>c</c> is larger than <c>V</c></exception>
    ///
    public static Digraph Strong(int V, int E, int c)
    {
      if (c >= V || c <= 0)
        throw new ArgumentException("Number of components must be between 1 and V");
      if (E <= 2 * (V - c))
        throw new ArgumentException("Number of edges must be at least 2(V-c)");
      if (E > (long)V * (V - 1) / 2)
        throw new ArgumentException("Too many edges");

      // the digraph
      Digraph G = new Digraph(V);

      // edges added to G (to avoid duplicate edges)
      SET<Edge> set = new SET<Edge>();

      int[] label = new int[V];
      for (int v = 0; v < V; v++)
        label[v] = StdRandom.Uniform(c);

      // make all vertices with label c a strong component by
      // combining a rooted in-tree and a rooted out-tree
      for (int i = 0; i < c; i++)
      {
        // how many vertices in component c
        int count = 0;
        for (int v = 0; v < G.V; v++)
        {
          if (label[v] == i) count++;
        }

        // if (count == 0) System.err.println("less than desired number of strong components");

        int[] vertices = new int[count];
        int j = 0;
        for (int v = 0; v < V; v++)
        {
          if (label[v] == i) vertices[j++] = v;
        }
        StdRandom.Shuffle(vertices);

        // rooted-in tree with root = vertices[count-1]
        for (int v = 0; v < count - 1; v++)
        {
          int w = StdRandom.Uniform(v + 1, count);
          Edge e = new Edge(w, v);
          set.Add(e);
          G.AddEdge(vertices[w], vertices[v]);
        }

        // rooted-out tree with root = vertices[count-1]
        for (int v = 0; v < count - 1; v++)
        {
          int w = StdRandom.Uniform(v + 1, count);
          Edge e = new Edge(v, w);
          set.Add(e);
          G.AddEdge(vertices[v], vertices[w]);
        }
      }

      while (G.E < E)
      {
        int v = StdRandom.Uniform(V);
        int w = StdRandom.Uniform(V);
        Edge e = new Edge(v, w);
        if (!set.Contains(e) && v != w && label[v] <= label[w])
        {
          set.Add(e);
          G.AddEdge(v, w);
        }
      }

      return G;
    }

    /// <summary>
    /// Demo test the <c>DigraphGenerator</c> library.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DigraphGenerator V E", "Number of vertices and number of edges")]
    public static void MainTest(string[] args)
    {
      int V = int.Parse(args[0]);
      int E = int.Parse(args[1]);
      Console.WriteLine("Complete graph");
      Console.WriteLine(DigraphGenerator.Complete(V));
      Console.WriteLine();

      Console.WriteLine("Simple");
      Console.WriteLine(DigraphGenerator.Simple(V, E));
      Console.WriteLine();

      Console.WriteLine("Path");
      Console.WriteLine(DigraphGenerator.Path(V));
      Console.WriteLine();

      Console.WriteLine("Cycle");
      Console.WriteLine(DigraphGenerator.Cycle(V));
      Console.WriteLine();

      Console.WriteLine("Eulierian path");
      Console.WriteLine(DigraphGenerator.EulerianPath(V, E));
      Console.WriteLine();

      Console.WriteLine("Eulierian cycle");
      Console.WriteLine(DigraphGenerator.EulerianCycle(V, E));
      Console.WriteLine();

      Console.WriteLine("Binary tree");
      Console.WriteLine(DigraphGenerator.BinaryTree(V));
      Console.WriteLine();

      Console.WriteLine("Tournament");
      Console.WriteLine(DigraphGenerator.Tournament(V));
      Console.WriteLine();

      Console.WriteLine("DAG");
      Console.WriteLine(DigraphGenerator.Dag(V, E));
      Console.WriteLine();

      Console.WriteLine("Rooted-in DAG");
      Console.WriteLine(DigraphGenerator.RootedInDAG(V, E));
      Console.WriteLine();

      Console.WriteLine("Rooted-out DAG");
      Console.WriteLine(DigraphGenerator.RootedOutDAG(V, E));
      Console.WriteLine();

      Console.WriteLine("Rooted-in tree");
      Console.WriteLine(DigraphGenerator.RootedInTree(V));
      Console.WriteLine();

      Console.WriteLine("Rooted-out DAG");
      Console.WriteLine(DigraphGenerator.RootedOutTree(V));
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
