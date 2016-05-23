/******************************************************************************
 *  File name :    Topological.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *                EdgeWeightedDigraph.java EdgeWeightedDirectedCycle.java
 *                SymbolDigraph.java
 *  Data files:   http://algs4.cs.princeton.edu/42digraph/jobs.txt
 *
 *  Compute topological ordering of a DAG or edge-weighted DAG.
 *  Runs in O(E + V) time.
 *
 *  C:\> algscmd Topological jobs.txt "/"
 *  Calculus
 *  Linear Algebra
 *  Introduction to CS
 *  Programming Systems
 *  Algorithms
 *  Theoretical CS
 *  Artificial Intelligence
 *  Machine Learning
 *  Neural Networks
 *  Robotics
 *  Scientific Computing
 *  Computational Biology
 *  Databases
 *
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>Topological</c> class represents a data type for
  /// determining a topological order of a directed acyclic graph (DAG).
  /// Recall, a digraph has a topological order if and only if it is a DAG.
  /// The <c>HasOrder</c> operation determines whether the digraph has
  /// a topological order, and if so, the <c>Order</c> operation
  /// returns one.</para>
  /// <para>This implementation uses depth-first search.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c> (in the worst case),
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>HasOrder</c> and <c>Rank</c> operations takes constant time;
  /// the <c>Order</c> operation takes time proportional to <c>V</c>.</para><para>
  /// See <seealso cref="DirectedCycle"/>, <seealso cref="DirectedCycleX"/>, and
  /// <seealso cref="EdgeWeightedDirectedCycle"/> to compute a
  /// directed cycle if the digraph is not a DAG. Also, see 
  /// <seealso cref="TopologicalX"/> for a nonrecursive queue-based algorithm
  /// to compute a topological order of a DAG.</para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Topological.java.html">Topological</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Topological
  {
    private IEnumerable<int> order; // topological order
    private int[] rank;             // rank[v] = position of vertex v in topological order

    /// <summary>
    /// Determines whether the digraph <c>G</c> has a topological order and, if so,
    /// finds such a topological order.</summary>
    /// <param name="G">the digraph</param>
    ///
    public Topological(Digraph G)
    {
      DirectedCycle finder = new DirectedCycle(G);
      if (!finder.HasCycle)
      {
        DepthFirstOrder dfs = new DepthFirstOrder(G);
        order = dfs.ReversePost();
        rank = new int[G.V];
        int i = 0;
        foreach (int v in order)
          rank[v] = i++;
      }
    }

    /// <summary>
    /// Determines whether the edge-weighted digraph <c>G</c> has a topological
    /// order and, if so, finds such an order.</summary>
    /// <param name="G">the edge-weighted digraph</param>
    ///
    public Topological(EdgeWeightedDigraph G)
    {
      EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(G);
      if (!finder.HasCycle)
      {
        DepthFirstOrder dfs = new DepthFirstOrder(G);
        order = dfs.ReversePost();
      }
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

        Topological topological = new Topological(g);
        foreach (int v in topological.Order())
        {
          Console.WriteLine(sg.Name(v));
        }
      }
      else
      {
        g = new Digraph(new TextInput(filename));

        Topological topological = new Topological(g);
        foreach (int v in topological.Order())
        {
          Console.WriteLine(v);
        }
      }

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
