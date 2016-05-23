/******************************************************************************
 *  File name :    TransitiveClosure.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/42digraph/tinyDG.txt
 *
 *  Compute transitive closure of a digraph and support
 *  reachability queries.
 *
 *  Preprocessing time: O(V(E + V)) time.
 *  Query time: O(1).
 *  Space: O(V^2).
 *
 *  C:\> algscmd TransitiveClosure tinyDG.txt
 *         0  1  2  3  4  5  6  7  8  9 10 11 12
--------------------------------------------
 *    0:   T  T  T  T  T  T
 *    1:      T
 *    2:   T  T  T  T  T  T
 *    3:   T  T  T  T  T  T
 *    4:   T  T  T  T  T  T
 *    5:   T  T  T  T  T  T
 *    6:   T  T  T  T  T  T  T     T  T  T  T  T
 *    7:   T  T  T  T  T  T  T  T  T  T  T  T  T
 *    8:   T  T  T  T  T  T  T     T  T  T  T  T
 *    9:   T  T  T  T  T  T           T  T  T  T
 *   10:   T  T  T  T  T  T           T  T  T  T
 *   11:   T  T  T  T  T  T           T  T  T  T
 *   12:   T  T  T  T  T  T           T  T  T  T
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>TransitiveClosure</c> class represents a data type for
  /// computing the transitive closure of a digraph.
  /// </para><para>This implementation runs depth-first search from each vertex.
  /// The constructor takes time proportional to <c>V</c>(<c>V</c> + <c>E</c>)
  /// (in the worst case) and uses space proportional to <c>V</c><sup>2</sup>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// </para><para>For large digraphs, you may want to consider a more sophisticated algorithm.
  /// <a href = "http://www.cs.hut.fi/~enu/thesis.html">Nuutila</a> proposes two
  /// algorithm for the problem (based on strong components and an interval representation)
  /// that runs in <c>E</c> + <c>V</c> time on typical digraphs.</para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/42digraph">Section 4.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/TransitiveClosure.java.html">TransitiveClosure</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class TransitiveClosure
  {
    private DirectedDFS[] tc;  // tc[v] = reachable from v

    /// <summary>Computes the transitive closure of the digraph <c>G</c>.</summary>
    /// <param name="G">the digraph</param>
    ///
    public TransitiveClosure(Digraph G)
    {
      tc = new DirectedDFS[G.V];
      for (int v = 0; v < G.V; v++)
        tc[v] = new DirectedDFS(G, v);
    }

    /// <summary>
    /// Is there a directed path from vertex <c>v</c> to vertex <c>w</c> in the digraph?</summary>
    /// <param name="v">the source vertex</param>
    /// <param name="w">the target vertex</param>
    /// <returns><c>true</c> if there is a directed path from <c>v</c> to <c>w</c>,
    ///   <c>false</c> otherwise</returns>
    ///
    public bool Reachable(int v, int w)
    {
      return tc[v].Marked(w);
    }

    /// <summary>
    /// Demo test the <c>TransitiveClosure</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd TransitiveClosure tinyDG.txt", "File with the pre-defined format for directed graph")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      Digraph G = new Digraph(input);

      TransitiveClosure tc = new TransitiveClosure(G);

      // print header
      Console.Write("     ");
      for (int v = 0; v < G.V; v++)
        Console.Write(" {0,2}", v);
      Console.WriteLine();
      Console.WriteLine("--------------------------------------------");

      // print transitive closure
      for (int v = 0; v < G.V; v++)
      {
        Console.Write(" {0,2}: ", v);
        for (int w = 0; w < G.V; w++)
        {
          if (tc.Reachable(v, w)) Console.Write("  T");
          else Console.Write("   ");
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
