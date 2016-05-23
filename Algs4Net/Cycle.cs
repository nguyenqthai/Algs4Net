/******************************************************************************
 *  File name :    Cycle.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Identifies a cycle.
 *  Runs in O(E + V) time.
 *
 *  C:\> algscmd Cycle tinyG.txt
 *  3 4 5 3 
 * 
 *  C:\> algscmd Cycle mediumG.txt 
 *  15 0 225 15 
 * 
 *  C:\> algscmd Cycle largeG.txt 
 *  996673 762 840164 4619 785187 194717 996673 
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary><para>The <c>Cycle</c> class represents a data type for
  /// determining whether an undirected graph has a cycle.
  /// The <c>HasCycle</c> operation determines whether the graph has
  /// a cycle and, if so, the <c>GetCycle</c> operation returns one.</para>
  /// <para>This implementation uses depth-first search. The constructor 
  /// takes time proportional to <c>V</c> + <c>E</c> (in the worst case),
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>HasCycle</c> operation takes constant time; the <c>Cycle</c>
  /// operation takes time proportional to the length of the cycle.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a>   
  /// of <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Cycle.java.html">Cycle</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Cycle
  {
    private bool[] marked;
    private int[] edgeTo;
    private LinkedStack<int> cycle;

    /// <summary>Determines whether the undirected graph <c>G</c> has a cycle and,
    /// if so, finds such a cycle.</summary>
    /// <param name="G">the undirected graph</param>
    ///
    public Cycle(Graph G)
    {
      if (hasSelfLoop(G)) return;
      if (hasParallelEdges(G)) return;
      marked = new bool[G.V];
      edgeTo = new int[G.V];
      for (int v = 0; v < G.V; v++)
        if (!marked[v])
          dfs(G, -1, v);
    }

    // does this graph have a self loop?
    // side effect: initialize cycle to be self loop
    private bool hasSelfLoop(Graph G)
    {
      for (int v = 0; v < G.V; v++)
      {
        foreach (int w in G.Adj(v))
        {
          if (v == w)
          {
            cycle = new LinkedStack<int>();
            cycle.Push(v);
            cycle.Push(v);
            return true;
          }
        }
      }
      return false;
    }

    // does this graph have two parallel edges?
    // side effect: initialize cycle to be two parallel edges
    private bool hasParallelEdges(Graph G)
    {
      marked = new bool[G.V];

      for (int v = 0; v < G.V; v++)
      {

        // check for parallel edges incident to v
        foreach (int w in G.Adj(v))
        {
          if (marked[w])
          {
            cycle = new LinkedStack<int>();
            cycle.Push(v);
            cycle.Push(w);
            cycle.Push(v);
            return true;
          }
          marked[w] = true;
        }

        // reset so marked[v] = false for all v
        foreach (int w in G.Adj(v))
        {
          marked[w] = false;
        }
      }
      return false;
    }

    /// <summary>
    /// Returns true if the graph <c>G</c> has a cycle.</summary>
    /// <returns><c>true</c> if the graph has a cycle; <c>false</c> otherwise</returns>
    ///
    public bool HasCycle
    {
      get { return cycle != null; }
    }

    /// <summary>
    /// Returns a cycle in the graph <c>G</c>. A property in place of a method would be better;
    /// however the compiler will not allow a property having the same name as the defining class.
    /// </summary>
    /// <returns>a cycle if the graph <c>G</c> has a cycle,
    ///        and <c>null</c> otherwise</returns>
    ///
    public IEnumerable<int> GetCycle()
    {
      return cycle;
    }

    private void dfs(Graph G, int u, int v)
    {
      marked[v] = true;
      foreach (int w in G.Adj(v))
      {
        // short circuit if cycle already found
        if (cycle != null) return;

        if (!marked[w])
        {
          edgeTo[w] = v;
          dfs(G, v, w);
        }
        // check for cycle (but disregard reverse of edge leading to v)
        else if (w != u)
        {
          cycle = new LinkedStack<int>();
          for (int x = v; x != w; x = edgeTo[x])
          {
            cycle.Push(x);
          }
          cycle.Push(w);
          cycle.Push(v);
        }
      }
    }

    /// <summary>
    /// Demo test the <c>Cycle</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Cycle mediumG.txt", "File with the pre - defined format for undirected graph")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      Graph G = new Graph(input);

      Cycle finder = new Cycle(G);
      if (finder.HasCycle)
      {
        foreach (int v in finder.GetCycle())
        {
          Console.Write(v + " ");
        }
        Console.WriteLine();
      }
      else
      {
        Console.WriteLine("Graph is acyclic");
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
