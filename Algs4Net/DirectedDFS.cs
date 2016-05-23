/******************************************************************************
 *  File name :    DirectedDFS.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/42digraph/tinyDG.txt
 *
 *  Determine single-source or multiple-source reachability in a digraph
 *  using depth first search.
 *  Runs in O(E + V) time.
 *
 *  C:\> algscmd DirectedDFS tinyDG.txt 1
 *  1
 *
 *  C:\> algscmd DirectedDFS tinyDG.txt 2
 *  0 1 2 3 4 5
 *
 *  C:\> algscmd DirectedDFS tinyDG.txt 1 2 6
 *  0 1 2 3 4 5 6 8 9 10 11 12 
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DirectedDFS</c> class represents a data type for
  /// determining the vertices reachable from a given source vertex <c>S</c>
  /// (or set of source vertices) in a digraph. For versions that find the paths,
  /// see <seealso cref="DepthFirstDirectedPaths"/> and <seealso cref="BreadthFirstDirectedPaths"/>.
  /// </para><para>This implementation uses depth-first search.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c> (in the worst case),
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.</para></summary>
  /// <remarks><para>
  /// For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DirectedDFS.java.html">DirectedDFS</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DirectedDFS
  {
    private bool[] marked;  // marked[v] = true if v is reachable
                            // from source (or sources)
    private int count;      // number of vertices reachable from s

    /// <summary>
    /// Computes the vertices in digraph <c>G</c> that are
    /// reachable from the source vertex <c>s</c>.</summary>
    /// <param name="G">the digraph</param>
    /// <param name="s">the source vertex</param>
    ///
    public DirectedDFS(Digraph G, int s)
    {
      marked = new bool[G.V];
      dfs(G, s);
    }

    /// <summary>
    /// Computes the vertices in digraph <c>G</c> that are
    /// connected to any of the source vertices <c>sources</c>.</summary>
    /// <param name="G">the graph</param>
    /// <param name="sources">sources the source vertices</param>
    ///
    public DirectedDFS(Digraph G, IEnumerable<int> sources)
    {
      marked = new bool[G.V];
      foreach (int v in sources)
      {
        if (!marked[v]) dfs(G, v);
      }
    }

    private void dfs(Digraph G, int v)
    {
      count++;
      marked[v] = true;
      foreach (int w in G.Adj(v))
      {
        if (!marked[w]) dfs(G, w);
      }
    }

    /// <summary>
    /// Is there a directed path from the source vertex (or any
    /// of the source vertices) and vertex <c>v</c>?</summary>
    /// <param name="v">the vertex</param>
    /// <returns><c>true</c> if there is a directed path, <c>false</c> otherwise</returns>
    ///
    public bool Marked(int v)
    {
      return marked[v];
    }

    /// <summary>
    /// Returns the number of vertices reachable from the source vertex
    /// (or source vertices).</summary>
    /// <returns>the number of vertices reachable from the source vertex
    ///  (or source vertices)</returns>
    ///
    public int Count
    {
      get { return count; }
    }

    /// <summary>
    /// Demo test the <c>DirectedDFS</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DirectedDFS tinyDG.txt 1 2 6", "File in the format for digraph and a list of source vertices")]
    public static void MainTest(string[] args)
    {
      // read in digraph from command-line argument
      //if (args.Length < 2) throw new ArgumentException("Expecting input file");
      TextInput input = new TextInput(args[0]);
      Digraph G = new Digraph(input);

      // read in sources from command-line arguments
      Bag<int> sources = new Bag<int>();
      for (int i = 1; i < args.Length; i++)
      {
        int s = int.Parse(args[i]);
        sources.Add(s);
      }

      // multiple-source reachability
      DirectedDFS dfs = new DirectedDFS(G, sources);

      // print out vertices reachable from sources
      for (int v = 0; v < G.V; v++)
      {
        if (dfs.Marked(v)) Console.Write(v + " ");
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

