/******************************************************************************
 *  File name :    DepthFirstSearch.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/41graph/tinyG.txt
 *
 *  Run depth first search on an undirected graph.
 *  Runs in O(E + V) time.
 *
 *  C:\> algscmd DepthFirstSearch tinyG.txt 0
 *  0 1 2 3 4 5 6 
 *  NOT connected
 *
 *  C:\> algscmd DepthFirstSearch tinyG.txt 9
 *  9 10 11 12 
 *  NOT connected
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DepthFirstSearch</c> class represents a data type for
  /// determining the vertices connected to a given source vertex <c>S</c>
  /// in an undirected graph. For versions that find the paths, see
  /// <seealso cref="DepthFirstPaths"/> and <seealso cref="BreadthFirstPaths"/>.
  /// This implementation uses depth-first search.</para><para>
  /// The constructor takes time proportional to <c>V</c> + <c>E</c> (in the worst case),
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// It uses extra space (not including the graph) proportional to <c>V</c>.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a>   
  /// of <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DepthFirstSearch.java.html">DepthFirstSearch</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DepthFirstSearch
  {
    private bool[] marked;    // marked[v] = is there an s-v path?
    private int count;        // number of vertices connected to s

    /// <summary>Computes the vertices in graph <c>G</c> that are
    /// connected to the source vertex <c>s</c>.</summary>
    /// <param name="G">the graph</param>
    /// <param name="s">the source vertex</param>
    ///
    public DepthFirstSearch(Graph G, int s)
    {
      marked = new bool[G.V];
      dfs(G, s);
    }

    // depth first search from v
    private void dfs(Graph G, int v)
    {
      count++;
      marked[v] = true;
      foreach (int w in G.Adj(v))
      {
        if (!marked[w])
        {
          dfs(G, w);
        }
      }
    }

    /// <summary>
    /// Is there a path between the source vertex <c>s</c> and vertex <c>v</c>?</summary>
    /// <param name="v">the vertex</param>
    /// <returns><c>true</c> if there is a path, <c>false</c> otherwise</returns>
    ///
    public bool Marked(int v)
    {
      return marked[v];
    }

    /// <summary>
    /// Returns the number of vertices connected to the source vertex <c>s</c>.</summary>
    /// <returns>the number of vertices connected to the source vertex <c>s</c></returns>
    ///
    public int Count
    {
      get { return count; }
    }

    /// <summary>
    /// Demo test the <c>DepthFirstSearch</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DepthFirstSearch tinyG.txt s", "File with the pre-defined format for undirected graph and a source vertex")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      Graph G = new Graph(input);

      int s = int.Parse(args[1]);
      DepthFirstSearch search = new DepthFirstSearch(G, s);

      for (int v = 0; v < G.V; v++)
      {
        if (search.Marked(v))
          Console.Write(v + " ");
      }
      Console.WriteLine();
      if (search.Count != G.V) Console.WriteLine("NOT connected");
      else Console.WriteLine("connected");
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
