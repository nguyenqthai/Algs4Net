/******************************************************************************
 *  File name :    EdgeWeightedDirectedCycle.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Finds a directed cycle in an edge-weighted digraph.
 *  Runs in O(E + V) time.
 *
 *
 ******************************************************************************/

 using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>EdgeWeightedDirectedCycle</c> class represents a data type for
  /// determining whether an edge-weighted digraph has a directed cycle.
  /// The <c>HasCycle</c> operation determines whether the edge-weighted
  /// digraph has a directed cycle and, if so, the <c>Cycle</c> operation
  /// returns one.</para>
  /// <para>This implementation uses depth-first search.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c> (in the worst case),
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>HasCycle</c> operation takes constant time;
  /// the <c>Cycle</c> operation takes time proportional
  /// to the length of the cycle.</para>
  /// <para>See <seealso cref="Topological"/> to compute a topological order if 
  /// the edge-weighted digraph is acyclic.</para></summary>
  /// <remarks><para>For additional documentation,   
  /// see <a href="http://algs4.cs.princeton.edu/44sp">Section 4.4</a> of   
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/EdgeWeightedDirectedCycle.java.html">EdgeWeightedDirectedCycle</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class EdgeWeightedDirectedCycle
  {
    private bool[] marked;                    // marked[v] = has vertex v been marked?
    private DirectedEdge[] edgeTo;            // edgeTo[v] = previous edge on path to v
    private bool[] onStack;                   // onStack[v] = is vertex on the stack?
    private LinkedStack<DirectedEdge> cycle;  // directed cycle (or null if no such cycle)

    /// <summary>
    /// Determines whether the edge-weighted digraph <c>G</c> has a directed cycle and,
    /// if so, finds such a cycle.</summary>
    /// <param name="G">the edge-weighted digraph</param>
    ///
    public EdgeWeightedDirectedCycle(EdgeWeightedDigraph G)
    {
      marked = new bool[G.V];
      onStack = new bool[G.V];
      edgeTo = new DirectedEdge[G.V];
      for (int v = 0; v < G.V; v++)
        if (!marked[v]) dfs(G, v);

      // check that digraph has a cycle
      Debug.Assert(check(G));
    }

    // check that algorithm computes either the topological order or finds a directed cycle
    private void dfs(EdgeWeightedDigraph G, int v)
    {
      onStack[v] = true;
      marked[v] = true;
      foreach (DirectedEdge e in G.Adj(v))
      {
        int w = e.To;

        // short circuit if directed cycle found
        if (cycle != null) return;
        else if (!marked[w])
        {
          //found new vertex, so recur
          edgeTo[w] = e;
          dfs(G, w);
        }
        else if (onStack[w])
        {
          // trace back directed cycle
          cycle = new LinkedStack<DirectedEdge>();
          DirectedEdge eTemp = e;
          while (eTemp.From != w)
          {
            cycle.Push(eTemp);
            eTemp = edgeTo[eTemp.From];
          }
          cycle.Push(eTemp);
          return;
        }
      }

      onStack[v] = false;
    }

    /// <summary>
    /// Does the edge-weighted digraph have a directed cycle?</summary>
    /// <returns><c>true</c> if the edge-weighted digraph has a directed cycle,
    /// <c>false</c> otherwise</returns>
    ///
    public bool HasCycle
    {
      get { return cycle != null; }
    }

    /// <summary>
    /// Returns a directed cycle if the edge-weighted digraph has a directed cycle,
    /// and <c>null</c> otherwise.</summary>
    /// <returns>a directed cycle (as an iterable) if the edge-weighted digraph
    /// has a directed cycle, and <c>null</c> otherwise</returns>
    /// <remarks>A property in place of a method would be better; however since the class <see cref="Cycle"/>
    /// has defined <c>GetCycle()</c>, the convention follows.</remarks>  
    ///
    public IEnumerable<DirectedEdge> GetCycle()
    {
      return cycle;
    }

    // certify that digraph is either acyclic or has a directed cycle
    private bool check(EdgeWeightedDigraph G)
    {

      // edge-weighted digraph is cyclic
      if (HasCycle)
      {
        // verify cycle
        DirectedEdge first = null, last = null;
        foreach (DirectedEdge e in GetCycle())
        {
          if (first == null) first = e;
          if (last != null)
          {
            if (last.To != e.From)
            {
              Console.Error.Write("cycle edges {0} and {1} not incident\n", last, e);
              return false;
            }
          }
          last = e;
        }

        if (last.To != first.From)
        {
          Console.Error.Write("cycle edges {0} and {1} not incident\n", last, first);
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Demo test the <c>EdgeWeightedDirectedCycle</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd EdgeWeightedDirectedCycle V E F", "V vertices, E edges and F randome edges")]
    public static void MainTest(string[] args)
    {
      // create random DAG with V vertices and E edges; then add F random edges
      int V = int.Parse(args[0]);
      int E = int.Parse(args[1]);
      int F = int.Parse(args[2]);
      EdgeWeightedDigraph G = new EdgeWeightedDigraph(V);
      int[] vertices = new int[V];
      for (int i = 0; i < V; i++)
        vertices[i] = i;
      StdRandom.Shuffle(vertices);
      for (int i = 0; i < E; i++)
      {
        int v, w;
        do
        {
          v = StdRandom.Uniform(V);
          w = StdRandom.Uniform(V);
        } while (v >= w);
        double weight = StdRandom.Uniform();
        G.AddEdge(new DirectedEdge(v, w, weight));
      }

      // add F extra edges
      for (int i = 0; i < F; i++)
      {
        int v = StdRandom.Uniform(V);
        int w = StdRandom.Uniform(V);
        double weight = StdRandom.Uniform(0.0, 1.0);
        G.AddEdge(new DirectedEdge(v, w, weight));
      }

      Console.WriteLine(G);

      // find a directed cycle
      EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(G);
      if (finder.HasCycle)
      {
        Console.Write("Cycle: ");
        foreach (DirectedEdge e in finder.GetCycle())
        {
          Console.Write(e + " ");
        }
        Console.WriteLine();
      }

      // or give topologial sort
      else
      {
        Console.WriteLine("No directed cycle");
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
