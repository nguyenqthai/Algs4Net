/******************************************************************************
 *  File name :    Graph.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/41graph/tinyG.txt
 *
 *  A graph, implemented using an array of sets.
 *  Parallel edges and self-loops allowed.
 *
 *  C:\> algscmd Graph tinyG.txt
 *  13 vertices, 13 edges 
 *  0: 6 2 1 5 
 *  1: 0 
 *  2: 0 
 *  3: 5 4 
 *  4: 5 6 3 
 *  5: 3 4 0 
 *  6: 0 4 
 *  7: 8 
 *  8: 7 
 *  9: 11 10 12 
 *  10: 9 
 *  11: 9 12 
 *  12: 11 9 
 *
 *  C:\> algscmd Graph mediumG.txt
 *  250 vertices, 1273 edges 
 *  0: 225 222 211 209 204 202 191 176 163 160 149 114 97 80 68 59 58 49 44 24 15 
 *  1: 220 203 200 194 189 164 150 130 107 72 
 *  2: 141 110 108 86 79 51 42 18 14 
 *  ...
 *  
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>Graph</c> class represents an undirected graph of vertices
  /// named 0 through <c>V - 1</c>.
  /// It supports the following two primary operations: add an edge to the graph,
  /// iterate over all of the vertices adjacent to a vertex. It also provides
  /// methods for returning the number of vertices <c>V</c> and the number
  /// of edges <c>E</c>. Parallel edges and self-loops are permitted.
  /// </para><para>
  /// This implementation uses an adjacency-lists representation, which 
  /// is a vertex-indexed array of <seealso cref="Bag{Item}"/> objects.
  /// All operations take constant time (in the worst case) except
  /// iterating over the vertices adjacent to a given vertex, which takes
  /// time proportional to the number of such vertices.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a>
  /// of <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Graph.java.html">Graph</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Graph
  {
    private static readonly string NEWLINE = Environment.NewLine;

    private readonly int numVertices;
    private int numEdges;
    private Bag<int>[] adj;

    /// <summary>
    /// Initializes an empty graph with <c>V</c> vertices and 0 edges.
    /// param V the number of vertices</summary>
    /// <param name="V"> V number of vertices</param>
    /// <exception cref="ArgumentException">if <c>V</c> &lt; 0</exception>
    ///
    public Graph(int V)
    {
      if (V < 0) throw new ArgumentException("Number of vertices must be nonnegative");
      numVertices = V;
      numEdges = 0;
      adj = new Bag<int>[V];
      for (int v = 0; v < V; v++)
      {
        adj[v] = new Bag<int>();
      }
    }

    /// <summary>Initializes a graph from an initialized <see cref="TextInput"/> stream
    /// The format is the number of vertices <c>V</c>,
    /// followed by the number of edges <c>E</c>,
    /// followed by <c>E</c> pairs of vertices, with each entry separated by whitespace.</summary>
    /// <param name="input"> in the input stream</param>
    /// <exception cref="IndexOutOfRangeException">if the endpoints of any edge are not in prescribed range</exception>
    /// <exception cref="ArgumentException">if the number of vertices or edges is negative</exception>
	  /// <exception cref="FormatException">if the edges in the file are in an invalid input format</exception>
    ///
    public Graph(TextInput input) : this(input.ReadInt())
    {
      int E = input.ReadInt();
      if (E < 0) throw new ArgumentException("Number of edges must be nonnegative");
      for (int i = 0; i < E; i++)
      {
        int v = input.ReadInt();
        int w = input.ReadInt();
        AddEdge(v, w);
      }
    }

    /// <summary>
    /// Initializes a new graph that is a deep copy of <c>G</c>.</summary>
    /// <param name="G">the graph to copy</param>
    ///
    public Graph(Graph G) : this(G.V)
    {
      numEdges = G.E;
      for (int v = 0; v < G.V; v++)
      {
        // reverse so that adjacency list is in same order as original
        LinkedStack<int> reverse = new LinkedStack<int>();
        foreach (int w in G.adj[v])
        {
          reverse.Push(w);
        }
        foreach (int w in reverse)
        {
          adj[v].Add(w);
        }
      }
    }

    /// <summary>
    /// Returns the number of vertices in this graph.</summary>
    /// <returns>the number of vertices in this graph</returns>
    ///
    public int V
    {
      get { return numVertices; }
    }

    /// <summary>
    /// Returns the number of edges in this graph.</summary>
    /// <returns>the number of edges in this graph</returns>
    ///
    public int E
    {
      get { return numEdges; }
    }

    // throw an IndexOutOfRangeException unless 0 <= v < V
    private void validateVertex(int v)
    {
      if (v < 0 || v >= V)
        throw new IndexOutOfRangeException("vertex " + v + " is not between 0 and " + (V - 1));
    }

    /// <summary>
    /// Adds the undirected edge v-w to this graph.</summary>
    /// <param name="v">one vertex in the edge</param>
    /// <param name="w">the other vertex in the edge</param>
    /// <exception cref="IndexOutOfRangeException">unless both 0 &lt;= v &lt; V and 0 &lt;= w &lt; V</exception>
    ///
    public void AddEdge(int v, int w)
    {
      validateVertex(v);
      validateVertex(w);
      numEdges++;
      adj[v].Add(w);
      adj[w].Add(v);
    }

    /// <summary>
    /// Adds the undirected edge to this graph.</summary>
    /// <param name="e">the edge to add, ignoring its weight</param>
    /// <exception cref="IndexOutOfRangeException">unless both 0 &lt;= v &lt; V and 0 &lt;= w &lt; V</exception>
    ///
    public void AddEdge(Edge e)
    {
      int v = e.Either;
      int w = e.Other(v);
      AddEdge(v, w);
    }

    /// <summary>
    /// Returns the vertices adjacent to vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the vertices adjacent to vertex <c>v</c>, as an iterable</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public IEnumerable<int> Adj(int v)
    {
      validateVertex(v);
      return adj[v];
    }

    /// <summary>
    /// Returns the degree of vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the degree of vertex <c>v</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public int Degree(int v)
    {
      validateVertex(v);
      return adj[v].Count;
    }


    /// <summary>
    /// Returns a string representation of this graph.</summary>
    /// <returns>the number of vertices <c>V</c>, followed by the number of edges <c>E</c>,
    ///        followed by the <c>V</c> adjacency lists</returns>
    ///
    public override string ToString()
    {
      StringBuilder s = new StringBuilder();
      s.Append(V + " vertices, " + E + " edges " + NEWLINE);
      for (int v = 0; v < V; v++)
      {
        s.Append(v + ": ");
        foreach (int w in adj[v])
        {
          s.Append(w + " ");
        }
        s.Append(NEWLINE);
      }
      return s.ToString();
    }

    /// <summary>
    /// Demo test the <c>Graph</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Graph tinyG.txt", "File with the pre-defined format for undirected graph")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      Graph G = new Graph(input);
      Console.WriteLine(G);
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
