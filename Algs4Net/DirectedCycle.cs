/******************************************************************************
 *  File name :    DirectedCycle.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/42digraph/tinyDG.txt
 *                http://algs4.cs.princeton.edu/42digraph/tinyDAG.txt
 *
 *  Finds a directed cycle in a digraph.
 *  Runs in O(E + V) time.
 *
 *  C:\> algscmd DirectedCycle tinyDG.txt 
 *  Directed cycle: 3 5 4 3 
 *
 *  C:\> algscmd DirectedCycle tinyDAG.txt 
 *  No directed cycle
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DirectedCycle</c> class represents a data type for
  /// determining whether a digraph has a directed cycle.
  /// The <c>HasCycle</c> operation determines whether the digraph has
  /// a directed cycle and, and of so, the <c>Cycle</c> operation
  /// returns one.</para>
  /// <para>This implementation uses depth-first search.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c>
  /// (in the worst case),
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>HasCycle</c> operation takes constant time;
  /// the <c>Cycle</c> operation takes time proportional
  /// to the length of the cycle.</para><para>
  /// See <seealso cref="Topological"/> to compute a topological order if the
  /// digraph is acyclic.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DirectedCycle.java.html">DirectedCycle</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DirectedCycle
  {
    private bool[] marked;            // marked[v] = has vertex v been marked?
    private int[] edgeTo;             // edgeTo[v] = previous vertex on path to v
    private bool[] onStack;           // onStack[v] = is vertex on the stack?
    private LinkedStack<int> cycle;   // directed cycle (or null if no such cycle)

    /// <summary>
    /// Determines whether the digraph <c>G</c> has a directed cycle and, if so,
    /// finds such a cycle.</summary>
    /// <param name="G">the digraph</param>
    ///
    public DirectedCycle(Digraph G)
    {
      marked = new bool[G.V];
      onStack = new bool[G.V];
      edgeTo = new int[G.V];
      for (int v = 0; v < G.V; v++)
        if (!marked[v] && cycle == null) dfs(G, v);
    }

    // check that algorithm computes either the topological order or finds a directed cycle
    private void dfs(Digraph G, int v)
    {
      onStack[v] = true;
      marked[v] = true;
      foreach (int w in G.Adj(v))
      {
        // short circuit if directed cycle found
        if (cycle != null) return;
        else if (!marked[w]) 
        {
          // found new vertex, so recur
          edgeTo[w] = v;
          dfs(G, w);
        }
        else if (onStack[w])
        {
          // trace back directed cycle
          cycle = new LinkedStack<int>();
          for (int x = v; x != w; x = edgeTo[x])
          {
            cycle.Push(x);
          }
          cycle.Push(w);
          cycle.Push(v);
          Debug.Assert(check());
        }
      }
      onStack[v] = false;
    }

    /// <summary>
    /// Does the digraph have a directed cycle?</summary>
    /// <returns><c>true</c> if the digraph has a directed cycle, <c>false</c> otherwise</returns>
    ///
    public bool HasCycle
    {
      get { return cycle != null; }
    }

    /// <summary>
    /// Returns a directed cycle if the digraph has a directed cycle, and <c>null</c> otherwise.</summary>
    /// <returns>a directed cycle (as an iterable) if the digraph has a directed cycle,
    /// and <c>null</c> otherwise</returns>
    /// <remarks>A property in place of a method would be better; however since the class <see cref="Cycle"/>
    /// has defined <c>GetCycle()</c>, the convention follows.</remarks>  
    ///
    public IEnumerable<int> GetCycle()
    {
      return cycle;
    }

    // certify that digraph has a directed cycle if it reports one
    private bool check()
    {
      if (HasCycle)
      {
        // verify cycle
        int first = -1, last = -1;
        foreach (int v in GetCycle())
        {
          if (first == -1) first = v;
          last = v;
        }
        if (first != last)
        {
          Console.Error.WriteLine("cycle begins with {0} and ends with {0}", first, last);
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Demo test the <c>DirectedCycle</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DirectedCycle tinyDG.txt", "File with the format for directed graph")]
    public static void MainTest(string[] args)
    {
      // read in digraph from command-line argument
      if (args.Length < 1) throw new ArgumentException("Expecting input file");
      TextInput input = new TextInput(args[0]);
      Digraph G = new Digraph(input);

      DirectedCycle finder = new DirectedCycle(G);

      if (finder.HasCycle)
      {
        Console.Write("Directed cycle: ");
        foreach (int v in finder.GetCycle())
        {
          Console.Write(v + " ");
        }
        Console.WriteLine();
      }
      else
      {
        Console.WriteLine("No directed cycle");
      }
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
