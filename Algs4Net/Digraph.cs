/******************************************************************************
 *  File name :    Digraph.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/42digraph/tinyDG.txt
 *
 *  A graph, implemented using an array of lists.
 *  Parallel edges and self-loops are permitted.
 *
 *  C:\> algscmd Digraph tinyDG.txt
 *  13 vertices, 22 edges
 *  0: 5 1 
 *  1: 
 *  2: 0 3 
 *  3: 5 2 
 *  4: 3 2 
 *  5: 4 
 *  6: 9 4 8 0 
 *  7: 6 9
 *  8: 6 
 *  9: 11 10 
 *  10: 12 
 *  11: 4 12 
 *  12: 9 
 *  
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>Digraph</c> class represents a directed graph of vertices
  /// named 0 through <c>V</c> - 1.
  /// It supports the following two primary operations: add an edge to the digraph,
  /// iterate over all of the vertices adjacent from a given vertex.
  /// Parallel edges and self-loops are permitted.</para><para>
  /// This implementation uses an adjacency-lists representation, which 
  /// is a vertex-indexed array of <seealso cref="Bag{Item}"/> objects.
  /// All operations take constant time (in the worst case) except
  /// iterating over the vertices adjacent from a given vertex, which takes
  /// time proportional to the number of such vertices.</para></summary>
  /// <remarks><para>
  /// For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Digraph.java.html">Digraph</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Digraph
  {
    private static readonly string NEWLINE = Environment.NewLine;

    private readonly int numVertices;   // number of vertices in this digraph
    private int numEdges;               // number of edges in this digraph
    private Bag<int>[] adj;             // adj[v] = adjacency list for vertex v
    private int[] indegree;             // indegree[v] = indegree of vertex v

    /// <summary>Initializes an empty digraph with <c>V</c> vertices.</summary>
    /// <param name="V">the number of vertices</param>
    /// <exception cref="ArgumentException">if V &lt; 0</exception>
    ///
    public Digraph(int V)
    {
      if (V < 0) throw new ArgumentException("Number of vertices in a Digraph must be nonnegative");
      numVertices = V;
      numEdges = 0;
      indegree = new int[V];
      adj = new Bag<int>[V];
      for (int v = 0; v < V; v++)
      {
        adj[v] = new Bag<int>();
      }
    }

    /// <summary>
    /// Initializes a digraph from the text input stream.
    /// The format is the number of vertices <c>V</c>,
    /// followed by the number of edges <c>E</c>,
    /// followed by <c>E</c> pairs of vertices, with each entry separated by whitespace.</summary>
    /// <param name="input">the text input stream</param>
    /// <exception cref="IndexOutOfRangeException">if the endpoints of any edge are not in prescribed range</exception>
    /// <exception cref="ArgumentException">if the number of vertices or edges is negative</exception>
    /// <exception cref="FormatException">if the edges in the file are in an invalid input format</exception>
    ///
    public Digraph(TextInput input) : this(input.ReadInt())
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
    /// Initializes a new digraph that is a deep copy of the specified digraph.</summary>
    /// <param name="G">the digraph to copy</param>
    ///
    public Digraph(Digraph G) : this(G.V)
    {
      numEdges = G.E;
      for (int v = 0; v < V; v++)
        this.indegree[v] = G.Indegree(v);
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
    /// Returns the number of vertices in this digraph.</summary>
    /// <returns>the number of vertices in this digraph</returns>
    ///
    public int V
    {
      get { return numVertices; }
    }

    /// <summary>
    /// Returns the number of edges in this digraph.</summary>
    /// <returns>the number of edges in this digraph</returns>
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
    /// Adds the directed edge v->w to this digraph.</summary>
    /// <param name="v">the tail vertex</param>
    /// <param name="w">the head vertex</param>
    /// <exception cref="IndexOutOfRangeException">unless both 0 &lt;= v &lt; V and 0 &lt;= w &lt; V</exception>
    ///
    public void AddEdge(int v, int w)
    {
      validateVertex(v);
      validateVertex(w);
      adj[v].Add(w);
      indegree[w]++;
      numEdges++;
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
    /// Returns the vertices adjacent from vertex <c>v</c> in this digraph.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the vertices adjacent from vertex <c>v</c> in this digraph, as an iterable</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public IEnumerable<int> Adj(int v)
    {
      validateVertex(v);
      return adj[v];
    }

    /// <summary>
    /// Returns the number of directed edges incident from vertex <c>v</c>.
    /// This is known as the <c>Outdegree</c> of vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the outdegree of vertex <c>v</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public int Outdegree(int v)
    {
      validateVertex(v);
      return adj[v].Count;
    }

    /// <summary>
    /// Returns the number of directed edges incident to vertex <c>v</c>.
    /// This is known as the <c>Indegree</c> of vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the indegree of vertex <c>v</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public int Indegree(int v)
    {
      validateVertex(v);
      return indegree[v];
    }

    /// <summary>
    /// Returns the reverse of the digraph.</summary>
    /// <returns>the reverse of the digraph</returns>
    ///
    public Digraph Reverse()
    {
      Digraph R = new Digraph(V);
      for (int v = 0; v < V; v++)
      {
        foreach (int w in Adj(v))
        {
          R.AddEdge(w, v);
        }
      }
      return R;
    }

    /// <summary>
    /// Returns a string representation of the graph.</summary>
    /// <returns>the number of vertices <c>V</c>, followed by the number of edges <c>E</c>,
    ///        followed by the <c>V</c> adjacency lists</returns>
    ///
    public override string ToString()
    {
      StringBuilder s = new StringBuilder();
      s.Append(V + " vertices, " + E + " edges " + NEWLINE);
      for (int v = 0; v < V; v++)
      {
        s.Append(string.Format("{0}: ", v));
        foreach (int w in adj[v])
        {
          s.Append(string.Format("{0} ", w));
        }
        s.Append(NEWLINE);
      }
      return s.ToString();
    }

    /// <summary>
    /// Demo test the <c>Digraph</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Digraph tinyDG.txt", "File with the pre-defined format for directed graph")]
    public static void MainTest(string[] args)
    {
      if (args.Length < 1) throw new ArgumentException("Expecting input file");
      TextInput input = new TextInput(args[0]);
      Digraph G = new Digraph(input);
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
