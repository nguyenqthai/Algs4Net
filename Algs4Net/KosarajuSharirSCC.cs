/******************************************************************************
 *  File name :    KosarajuSharirSCC.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/42digraph/tinyDG.txt
 *
 *  Compute the strongly-connected components of a digraph using the
 *  Kosaraju-Sharir algorithm.
 *
 *  Runs in O(E + V) time.
 *
 *  C:\> algscmd KosarajuSCC tinyDG.txt
 *  5 components
 *  1 
 *  0 2 3 4 5 
 *  9 10 11 12 
 *  6 8 
 *  7
 *
 *  C:\> algscmd KosarajuSharirSCC mediumDG.txt 
 *  10 components
 *  21 
 *  2 5 6 8 9 11 12 13 15 16 18 19 22 23 25 26 28 29 30 31 32 33 34 35 37 38 39 40 42 43 44 46 47 48 49 
 *  14 
 *  3 4 17 20 24 27 36 
 *  41 
 *  7 
 *  45 
 *  1 
 *  0 
 *  10 
 *
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>KosarajuSharirSCC</c> class represents a data type for
  /// determining the strong components in a digraph.
  /// The <c>Id</c> operation determines in which strong component
  /// a given vertex lies; the <c>AreStronglyConnected</c> operation
  /// determines whether two vertices are in the same strong component;
  /// and the <c>Count</c> operation determines the number of strong
  /// components.
  /// The <c>Component identifier</c> of a component is one of the
  /// vertices in the strong component: two vertices have the same component
  /// identifier if and only if they are in the same strong component.
  /// </para><para>
  /// This implementation uses the Kosaraju-Sharir algorithm.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c>
  /// (in the worst case),
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>Id</c>, <c>Count</c>, and <c>AreStronglyConnected</c>
  /// operations take constant time.
  /// For alternate implementations of the same API, see
  /// <seealso cref="TarjanSCC"/> and <seealso cref="GabowSCC"/>.
  /// </para></summary>
  /// <remarks><para>
  /// For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/KosarajuSharirSCC.java.html">KosarajuSharirSCC</a>
  /// implementation by the respective authors.</para></remarks>
  ///

  public class KosarajuSharirSCC
  {
    private bool[] marked;    // marked[v] = has vertex v been visited?
    private int[] id;         // id[v] = id of strong component containing v
    private int count;        // number of strongly-connected components

    /// <summary>Computes the strong components of the digraph <c>G</c>.</summary>
    /// <param name="G">the digraph</param>
    ///
    public KosarajuSharirSCC(Digraph G)
    {
      // compute reverse postorder of reverse graph
      DepthFirstOrder dfs = new DepthFirstOrder(G.Reverse());

      // run DFS on G, using reverse postorder to guide calculation
      marked = new bool[G.V];
      id = new int[G.V];
      foreach (int v in dfs.ReversePost())
      {
        if (!marked[v])
        {
          this.dfs(G, v);
          count++;
        }
      }
      // check that id[] gives strong components
      Debug.Assert(check(G));
    }

    // DFS on graph G
    private void dfs(Digraph G, int v)
    {
      marked[v] = true;
      id[v] = count;
      foreach (int w in G.Adj(v))
      {
        if (!marked[w]) dfs(G, w);
      }
    }

    /// <summary>
    /// Returns the number of strong components.</summary>
    /// <returns>the number of strong components</returns>
    ///
    public int Count
    {
      get { return count; }
    }

    /// <summary>
    /// Are vertices <c>v</c> and <c>w</c> in the same strong component?</summary>
    /// <param name="v">one vertex</param>
    /// <param name="w">the other vertex</param>
    /// <returns><c>true</c> if vertices <c>v</c> and <c>w</c> are in the same
    ///    strong component, and <c>false</c> otherwise</returns>
    ///
    public bool StronglyConnected(int v, int w)
    {
      return id[v] == id[w];
    }

    /// <summary>
    /// Returns the component id of the strong component containing vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the component id of the strong component containing vertex <c>v</c></returns>
    ///
    public int Id(int v)
    {
      return id[v];
    }

    // does the id[] array contain the strongly connected components?
    private bool check(Digraph G)
    {
      TransitiveClosure tc = new TransitiveClosure(G);
      for (int v = 0; v < G.V; v++)
      {
        for (int w = 0; w < G.V; w++)
        {
          if (StronglyConnected(v, w) != (tc.Reachable(v, w) && tc.Reachable(w, v)))
            return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Demo test the <c>KosarajuSharirSCC</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd KosarajuSharirSCC tinyDG.txt", "File with the pre-defined format for directed graph")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      Digraph G = new Digraph(input);

      KosarajuSharirSCC scc = new KosarajuSharirSCC(G);

      // number of connected components
      int M = scc.Count;
      Console.WriteLine(M + " components");

      // compute list of vertices in each strong component
      LinkedQueue<int>[] components = new LinkedQueue<int>[M];
      for (int i = 0; i < M; i++)
      {
        components[i] = new LinkedQueue<int>();
      }
      for (int v = 0; v < G.V; v++)
      {
        components[scc.Id(v)].Enqueue(v);
      }

      // print results
      for (int i = 0; i < M; i++)
      {
        foreach (int v in components[i])
        {
          Console.Write(v + " ");
        }
        Console.WriteLine();
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
