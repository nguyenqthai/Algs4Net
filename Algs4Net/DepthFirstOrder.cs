/******************************************************************************
 *  File name :    DepthFirstOrder.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *                EdgeWeightedDigraph.java DirectedEdge.java
 *  Data files:   http://algs4.cs.princeton.edu/42digraph/tinyDAG.txt
 *                http://algs4.cs.princeton.edu/42digraph/tinyDG.txt
 *
 *  Compute preorder and postorder for a digraph or edge-weighted digraph.
 *  Runs in O(E + V) time.
 *
 *  C:\> algscmd DepthFirstOrder tinyDAG.txt
 *     v  pre post
 *  --------------
 *     v  pre post
 *  --------------
 *     0    0    8
 *     1    3    2
 *     2    9   10
 *     3   10    9
 *     4    2    0
 *     5    1    1
 *     6    4    7
 *     7   11   11
 *     8   12   12
 *     9    5    6
 *    10    8    5
 *    11    6    4
 *    12    7    3
 *  Preorder:  0 5 4 1 6 9 11 12 10 2 3 7 8
 *  Postorder: 4 5 1 12 11 10 9 6 0 3 2 7 8
 *  Reverse postorder: 8 7 2 3 0 6 9 10 11 12 1 5 4
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DepthFirstOrder</c> class represents a data type for
  /// determining depth-first search ordering of the vertices in a digraph
  /// or edge-weighted digraph, including preorder, postorder, and reverse postorder.
  /// </para><para>This implementation uses depth-first search.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c> (in the worst case),
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>Preorder</c>, <c>Postorder</c>, and <c>Reverse postorder</c>
  /// operation takes take time proportional to <c>V</c>.</para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DepthFirstOrder.java.html">DepthFirstOrder</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DepthFirstOrder
  {
    private bool[] marked;              // marked[v] = has v been marked in dfs?
    private int[] pre;                  // pre[v]    = preorder  number of v
    private int[] post;                 // post[v]   = postorder number of v
    private LinkedQueue<int> preorder;  // vertices in preorder
    private LinkedQueue<int> postorder; // vertices in postorder
    private int preCounter;             // counter or preorder numbering
    private int postCounter;            // counter for postorder numbering

    /// <summary>
    /// Determines a depth-first order for the digraph <c>G</c>.</summary>
    /// <param name="G">the digraph</param>
    ///
    public DepthFirstOrder(Digraph G)
    {
      pre = new int[G.V];
      post = new int[G.V];
      postorder = new LinkedQueue<int>();
      preorder = new LinkedQueue<int>();
      marked = new bool[G.V];
      for (int v = 0; v < G.V; v++)
        if (!marked[v]) dfs(G, v);
    }

    /// <summary>
    /// Determines a depth-first order for the edge-weighted digraph <c>G</c>.</summary>
    /// <param name="G">the edge-weighted digraph</param>
    ///
    public DepthFirstOrder(EdgeWeightedDigraph G)
    {
      pre = new int[G.V];
      post = new int[G.V];
      postorder = new LinkedQueue<int>();
      preorder = new LinkedQueue<int>();
      marked = new bool[G.V];
      for (int v = 0; v < G.V; v++)
        if (!marked[v]) dfs(G, v);
    }

    // run DFS in digraph G from vertex v and compute preorder/postorder
    private void dfs(Digraph G, int v)
    {
      marked[v] = true;
      pre[v] = preCounter++;
      preorder.Enqueue(v);
      foreach (int w in G.Adj(v))
      {
        if (!marked[w])
        {
          dfs(G, w);
        }
      }
      postorder.Enqueue(v);
      post[v] = postCounter++;
    }

    // run DFS in edge-weighted digraph G from vertex v and compute preorder/postorder
    private void dfs(EdgeWeightedDigraph G, int v)
    {
      marked[v] = true;
      pre[v] = preCounter++;
      preorder.Enqueue(v);
      foreach (DirectedEdge e in G.Adj(v))
      {
        int w = e.To;
        if (!marked[w])
        {
          dfs(G, w);
        }
      }
      postorder.Enqueue(v);
      post[v] = postCounter++;
    }

    /// <summary>
    /// Returns the preorder number of vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the preorder number of vertex <c>v</c></returns>
    ///
    public int Pre(int v)
    {
      return pre[v];
    }

    /// <summary>
    /// Returns the postorder number of vertex <c>v</c>.</summary>
    /// <param name="v">the vertex</param>
    /// <returns>the postorder number of vertex <c>v</c></returns>
    ///
    public int Post(int v)
    {
      return post[v];
    }

    /// <summary>
    /// Returns the vertices in postorder.</summary>
    /// <returns>the vertices in postorder, as an iterable of vertices</returns>
    ///
    public IEnumerable<int> Post()
    {
      return postorder;
    }

    /// <summary>
    /// Returns the vertices in preorder.</summary>
    /// <returns>the vertices in preorder, as an iterable of vertices</returns>
    ///
    public IEnumerable<int> Pre()
    {
      return preorder;
    }

    /// <summary>
    /// Returns the vertices in reverse postorder.</summary>
    /// <returns>the vertices in reverse postorder, as an iterable of vertices</returns>
    ///
    public IEnumerable<int> ReversePost()
    {
      LinkedStack<int> reverse = new LinkedStack<int>();
      foreach (int v in postorder)
        reverse.Push(v);
      return reverse;
    }

    // check that pre() and post() are consistent with pre(v) and post(v)
    private bool check(Digraph G)
    {

      // check that post(v) is consistent with post()
      int r = 0;
      foreach (int v in Post())
      {
        if (Post(v) != r)
        {
          Console.Error.WriteLine("Post(v) and Post() inconsistent");
          return false;
        }
        r++;
      }

      // check that pre(v) is consistent with pre()
      r = 0;
      foreach (int v in Pre())
      {
        if (Pre(v) != r)
        {
          Console.Error.WriteLine("Pre(v) and Pre() inconsistent");
          return false;
        }
        r++;
      }
      return true;
    }
    /// <summary>
    /// Demo test the <c>DepthFirstOrder</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DepthFirstOrder tinyDAG.txt", "File with the pre-defined format for digraph")]
    public static void MainTest(string[] args)
    {
      // read in digraph from command-line argument
      TextInput input = new TextInput(args[0]);
      Digraph G = new Digraph(input);

      DepthFirstOrder dfs = new DepthFirstOrder(G);
      Console.WriteLine("   v  pre post");
      Console.WriteLine("--------------");
      for (int v = 0; v < G.V; v++)
      {
        Console.Write("{0,4} {1,4:} {2,4}\n", v, dfs.Pre(v), dfs.Post(v));
      }

      Console.Write("Preorder:  ");
      foreach (int v in dfs.Pre())
      {
        Console.Write(v + " ");
      }
      Console.WriteLine();

      Console.Write("Postorder: ");
      foreach (int v in dfs.Post())
      {
        Console.Write(v + " ");
      }
      Console.WriteLine();

      Console.Write("Reverse postorder: ");
      foreach (int v in dfs.ReversePost())
      {
        Console.Write(v + " ");
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
