/******************************************************************************
 *  File name :    TopologicalX.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Compute topological ordering of a DAG using queue-based algorithm.
 *  Runs in O(E + V) time.
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>TopologicalX</c> class represents a data type for
  /// determining a topological order of a directed acyclic graph (DAG).
  /// Recall, a digraph has a topological order if and only if it is a DAG.
  /// The <c>HasOrder</c> operation determines whether the digraph has
  /// a topological order, and if so, the <c>Order</c> operation
  /// returns one.
  /// </para><para>
  /// This implementation uses a nonrecursive, queue-based algorithm.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c>
  /// (in the worst case),
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>HasOrder</c> and <c>Rank</c> operations takes constant time;
  /// the <c>Order</c> operation takes time proportional to <c>V</c>.
  /// </para><para>
  /// See <seealso cref="DirectedCycle"/>, <seealso cref="DirectedCycleX"/>, and
  /// <seealso cref="EdgeWeightedDirectedCycle"/> to compute a
  /// directed cycle if the digraph is not a DAG.
  /// See <seealso cref="Topological"/> for a recursive version that uses depth-first search.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/TopologicalX.java.html">TopologicalX</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class TopologicalX
  {
    private LinkedQueue<int> order;   // vertices in topological order
    private int[] rank;               // rank[v] = order where vertex v appers in order

    /// <summary>
    /// Determines whether the digraph <c>G</c> has a topological order and, if so,
    /// finds such a topological order.
    /// </summary>
    /// <param name="G">the digraph</param>
    ///
    public TopologicalX(Digraph G)
    {

      // indegrees of remaining vertices
      int[] indegree = new int[G.V];
      for (int v = 0; v < G.V; v++)
      {
        indegree[v] = G.Indegree(v);
      }

      // initialize
      rank = new int[G.V];
      order = new LinkedQueue<int>();
      int count = 0;

      // initialize queue to contain all vertices with indegree = 0
      LinkedQueue<int> queue = new LinkedQueue<int>();
      for (int v = 0; v < G.V; v++)
        if (indegree[v] == 0) queue.Enqueue(v);

      for (int j = 0; !queue.IsEmpty; j++)
      {
        int v = queue.Dequeue();
        order.Enqueue(v);
        rank[v] = count++;
        foreach (int w in G.Adj(v))
        {
          indegree[w]--;
          if (indegree[w] == 0) queue.Enqueue(w);
        }
      }

      // there is a directed cycle in subgraph of vertices with indegree >= 1.
      if (count != G.V)
      {
        order = null;
      }

      Debug.Assert(check(G));
    }

    /// <summary>
    /// Determines whether the edge-weighted digraph <c>G</c> has a
    /// topological order and, if so, finds such a topological order.</summary>
    /// <param name="G">the digraph</param>
    ///
    public TopologicalX(EdgeWeightedDigraph G)
    {

      // indegrees of remaining vertices
      int[] indegree = new int[G.V];
      for (int v = 0; v < G.V; v++)
      {
        indegree[v] = G.Indegree(v);
      }

      // initialize
      rank = new int[G.V];
      order = new LinkedQueue<int>();
      int count = 0;

      // initialize queue to contain all vertices with indegree = 0
      LinkedQueue<int> queue = new LinkedQueue<int>();
      for (int v = 0; v < G.V; v++)
        if (indegree[v] == 0) queue.Enqueue(v);

      for (int j = 0; !queue.IsEmpty; j++)
      {
        int v = queue.Dequeue();
        order.Enqueue(v);
        rank[v] = count++;
        foreach (DirectedEdge e in G.Adj(v))
        {
          int w = e.To;
          indegree[w]--;
          if (indegree[w] == 0) queue.Enqueue(w);
        }
      }

      // there is a directed cycle in subgraph of vertices with indegree >= 1.
      if (count != G.V)
      {
        order = null;
      }

      Debug.Assert(check(G));
    }

    /// <summary>
    /// Returns a topological order if the digraph has a topologial order,
    /// and <c>null</c> otherwise.</summary>
    /// <returns>a topological order of the vertices (as an interable) if the
    ///   digraph has a topological order (or equivalently, if the digraph is a DAG),
    ///   and <c>null</c> otherwise</returns>
    ///
    public IEnumerable<int> Order()
    {
      return order;
    }

    /// <summary>
    /// Does the digraph have a topological order?</summary>
    /// <returns><c>true</c> if the digraph has a topological order (or equivalently,
    ///   if the digraph is a DAG), and <c>false</c> otherwise</returns>
    ///
    public bool HasOrder
    {
      get { return order != null; }
    }

    /// <summary>
    /// The the rank of vertex <c>v</c> in the topological order;
    /// -1 if the digraph is not a DAG</summary>
    /// <param name="v">the vertex under consideration</param>
    /// <returns>the position of vertex <c>v</c> in a topological order
    ///   of the digraph; -1 if the digraph is not a DAG</returns>
    /// <exception cref="IndexOutOfRangeException">unless <c>v</c> is between 0 and
    ///   <c>V</c> - 1</exception>
    ///
    public int Rank(int v)
    {
      validateVertex(v);
      if (HasOrder) return rank[v];
      else return -1;
    }

    // certify that digraph is acyclic
    private bool check(Digraph G)
    {

      // digraph is acyclic
      if (HasOrder)
      {
        // check that ranks are a permutation of 0 to V-1
        bool[] found = new bool[G.V];
        for (int i = 0; i < G.V; i++)
        {
          found[Rank(i)] = true;
        }
        for (int i = 0; i < G.V; i++)
        {
          if (!found[i])
          {
            Console.Error.WriteLine("No vertex with rank " + i);
            return false;
          }
        }

        // check that ranks provide a valid topological order
        for (int v = 0; v < G.V; v++)
        {
          foreach (int w in G.Adj(v))
          {
            if (Rank(v) > Rank(w))
            {
              Console.Error.WriteLine("{0}-{1}: rank({2}) = {3}, rank({4}) = {5}\n",
                                v, w, v, Rank(v), w, Rank(w));
              return false;
            }
          }
        }

        // check that order() is consistent with rank()
        int r = 0;
        foreach (int v in Order())
        {
          if (Rank(v) != r)
          {
            Console.Error.WriteLine("Order() and Rank() inconsistent");
            return false;
          }
          r++;
        }
      }


      return true;
    }

    // certify that digraph is acyclic
    private bool check(EdgeWeightedDigraph G)
    {

      // digraph is acyclic
      if (HasOrder)
      {
        // check that ranks are a permutation of 0 to V-1
        bool[] found = new bool[G.V];
        for (int i = 0; i < G.V; i++)
        {
          found[Rank(i)] = true;
        }
        for (int i = 0; i < G.V; i++)
        {
          if (!found[i])
          {
            Console.Error.WriteLine("No vertex with rank " + i);
            return false;
          }
        }

        // check that ranks provide a valid topological order
        for (int v = 0; v < G.V; v++)
        {
          foreach (DirectedEdge e in G.Adj(v))
          {
            int w = e.To;
            if (Rank(v) > Rank(w))
            {
              Console.Error.WriteLine("{0}-{1}: rank({2}) = {3}, rank({4}) = {5}\n",
                                v, w, v, Rank(v), w, Rank(w));
              return false;
            }
          }
        }

        // check that order() is consistent with rank()
        int r = 0;
        foreach (int v in Order())
        {
          if (Rank(v) != r)
          {
            Console.Error.WriteLine("Order() and Rank() inconsistent");
            return false;
          }
          r++;
        }
      }
      return true;
    }

    // throw an IndexOutOfRangeException unless 0 <= v < V
    private void validateVertex(int v)
    {
      int V = rank.Length;
      if (v < 0 || v >= V)
        throw new IndexOutOfRangeException("vertex " + v + " is not between 0 and " + (V - 1));
    }

    /// <summary>
    /// Demo test the <c>Topological</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Topological jobs.txt \"/\"",
      "File with the order constraints and the separator for the file. Alternatively,\n" +
      "   use algscmd Topological tinyDAG.txt to indicate a a file with the digraph file format.")]
    public static void MainTest(string[] args)
    {
      string filename = args[0];
      Digraph g;
      if (args.Length >= 2)
      {
        string delimiter = args[1];
        SymbolDigraph sg = new SymbolDigraph(filename, delimiter);
        g = sg.G;

        TopologicalX topological = new TopologicalX(g);
        foreach (int v in topological.Order())
        {
          Console.WriteLine(sg.Name(v));
        }
      }
      else
      {
        g = new Digraph(new TextInput(filename));

        TopologicalX topological = new TopologicalX(g);
        foreach (int v in topological.Order())
        {
          Console.WriteLine(v);
        }
      }

    }

    // TODO: support it
    /*
    /// <summary>
    /// Demo test the <c>TopologicalX</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    public static void TopologicalXTest01(string[] args)
    {

      // create random DAG with V vertices and E edges; then add F random edges
      int V = int.Parse(args[0]);
      int E = int.Parse(args[1]);
      int F = int.Parse(args[2]);

      Digraph G1 = DigraphGenerator.Dag(V, E);

      // corresponding edge-weighted digraph
      EdgeWeightedDigraph G2 = new EdgeWeightedDigraph(V);
      for (int v = 0; v < G1.V; v++)
        foreach (int w in G1.Adj(v))
          G2.AddEdge(new DirectedEdge(v, w, 0.0));

      // add F extra edges
      for (int i = 0; i < F; i++)
      {
        int v = StdRandom.Uniform(V);
        int w = StdRandom.Uniform(V);
        G1.AddEdge(v, w);
        G2.AddEdge(new DirectedEdge(v, w, 0.0));
      }

      Console.WriteLine(G1);
      Console.WriteLine();
      Console.WriteLine(G2);

      // find a directed cycle
      TopologicalX topological1 = new TopologicalX(G1);
      if (!topological1.HasOrder)
      {
        Console.WriteLine("Not a DAG");
      }

      // or give topologial sort
      else
      {
        Console.Write("Topological order: ");
        foreach (int v in topological1.Order())
        {
          Console.Write(v + " ");
        }
        Console.WriteLine();
      }

      // find a directed cycle
      TopologicalX topological2 = new TopologicalX(G2);
      if (!topological2.HasOrder)
      {
        Console.WriteLine("Not a DAG");
      }

      // or give topologial sort
      else
      {
        Console.Write("Topological order: ");
        foreach (int v in topological2.Order())
        {
          Console.Write(v + " ");
        }
        Console.WriteLine();
      }
    } */

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
