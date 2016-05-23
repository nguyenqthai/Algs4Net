/******************************************************************************
 *  File name :    CC.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/41graph/tinyG.txt
 *
 *  Compute connected components using depth first search.
 *  Runs in O(E + V) time.
 *
 *  C:\> algscmd CC tinyG.txt
 *  3 components
 *  0 1 2 3 4 5 6
 *  7 8 
 *  9 10 11 12
 *
 *  C:\> algscmd CC mediumG.txt 
 *  1 components
 *  0 1 2 3 4 5 6 7 8 9 10 ...
 *
 *  C:\> algscmd -Xss50m CC largeG.txt 
 *  1 components
 *  0 1 2 3 4 5 6 7 8 9 10 ...
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>CC</c> class represents a data type for
  /// determining the connected components in an undirected graph.
  /// The <c>Id</c> operation determines in which connected component
  /// a given vertex lies; the <c>Connected</c> operation
  /// determines whether two vertices are in the same connected component;
  /// the <c>Count</c> operation determines the number of connected
  /// components; and the <c>Count</c> operation determines the number
  /// of vertices in the connect component containing a given vertex.
  /// The <c>Component identifier</c> of a connected component is one of the
  /// vertices in the connected component: two vertices have the same component
  /// identifier if and only if they are in the same connected component.
  /// </para><para>
  /// This implementation uses depth-first search. The constructor takes time 
  /// proportional to <c>V + E</c> (in the worst case), <c>V</c> is the number 
  /// of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>Id</c>, <c>Count</c>, <c>Connected</c>,
  /// and <c>Count</c> operations take constant time.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a>   
  ///  of <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/CC.java.html">CC</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class CC
  {
    private bool[] marked;   // marked[v] = has vertex v been marked?
    private int[] id;        // id[v] = id of connected component containing v
    private int[] size;      // size[id] = number of vertices in given component
    private int count;       // number of connected components

    /// <summary>Computes the connected components of the undirected graph <c>G</c>.</summary>
    /// <param name="G">the undirected graph</param>
    ///
    public CC(Graph G)
    {
      marked = new bool[G.V];
      id = new int[G.V];
      size = new int[G.V];
      for (int v = 0; v < G.V; v++)
      {
        if (!marked[v])
        {
          dfs(G, v);
          count++;
        }
      }
    }

    // depth-first search
    private void dfs(Graph G, int v)
    {
      marked[v] = true;
      id[v] = count;
      size[count]++;
      foreach (int w in G.Adj(v))
      {
        if (!marked[w])
        {
          dfs(G, w);
        }
      }
    }

    /// <summary>
    /// Returns the component id of the connected component containing vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the component id of the connected component containing vertex <c>v</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    /// 
    public int Id(int v)
    {
      validateVertex(v);
      return id[v];
    }

    /// <summary>
    /// Returns the number of vertices in the connected component containing vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the number of vertices in the connected component containing vertex <c>v</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public int Size(int v)
    {
      validateVertex(v);
      return size[id[v]];
    }

    /// <summary>
    /// Returns the number of connected components in the graph <c>G</c>.</summary>
    /// <returns>the number of connected components in the graph <c>G</c></returns>
    ///
    public int Count
    {
      get { return count; }
    }

    /// <summary>
    /// Returns true if vertices <c>v</c> and <c>w</c> are in the same
    /// connected component.</summary>
    /// <param name="v">one vertex</param>
    /// <param name="w">the other vertex</param>
    /// <returns><c>true</c> if vertices <c>v</c> and <c>w</c> are in the same
    ///        connected component; <c>false</c> otherwise</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v, w &lt; V</exception>
    ///
    public bool Connected(int v, int w)
    {
      return Id(v) == Id(w);
    }

    private void validateVertex(int v)
    {
      if (v < 0 || v >= id.Length)
        throw new IndexOutOfRangeException("vertex " + v + " is not between 0 and " + id.Length);
    }

    /// <summary>
    /// Demo test the <c>CC</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd CC tinyG.txt", "File with the pre-defined format for undirected graph")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      Graph G = new Graph(input);

      CC cc = new CC(G);

      // number of connected components
      int M = cc.Count;
      Console.WriteLine(M + " components");

      // compute list of vertices in each connected component
      LinkedQueue<int>[] components = new LinkedQueue<int>[M];
      for (int i = 0; i < M; i++)
      {
        components[i] = new LinkedQueue<int>();
      }
      for (int v = 0; v < G.V; v++)
      {
        components[cc.Id(v)].Enqueue(v);
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
