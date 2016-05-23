/******************************************************************************
 *  Compilation:  javac CPM.java
 *  Execution:    java CPM < input.txt
 *  Dependencies: EdgeWeightedDigraph.java AcyclicDigraphLP.java Console.java
 *  Data files:   http://algs4.cs.princeton.edu/44sp/jobsPC.txt
 *
 *  Critical path method.
 *
 *  C:\> algscmd CPM < jobsPC.txt
 *   job   start  finish
 *  --------------------
 *     0     0.0    41.0
 *     1    41.0    92.0
 *     2   123.0   173.0
 *     3    91.0   127.0
 *     4    70.0   108.0
 *     5     0.0    45.0
 *     6    70.0    91.0
 *     7    41.0    73.0
 *     8    91.0   123.0
 *     9    41.0    70.0
 *  Finish time:   173.0
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>CPM</c> class provides a client that solves the
  /// parallel precedence-constrained job scheduling problem
  /// via the <c>Critical path method</c>. It reduces the problem
  /// to the longest-paths problem in edge-weighted DAGs.
  /// It builds an edge-weighted digraph (which must be a DAG)
  /// from the job-scheduling problem specification,
  /// finds the longest-paths tree, and computes the longest-paths
  /// lengths (which are precisely the start times for each job).
  /// </para><para>
  /// This implementation uses <seealso cref="AcyclicLP"/> to find a longest
  /// path in a DAG.
  /// The running time is proportional to <c>V</c> + <c>E</c>,
  /// where <c>V</c> is the number of jobs and <c>E</c> is the
  /// number of precedence constraints.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/44sp">Section 4.4</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/CPM.java.html">CPM</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class CPM
  {
    // this class cannot be instantiated
    private CPM() { }

    /// <summary>
    /// Reads the precedence constraints from standard input
    /// and prints a feasible schedule to standard output.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd CPM < jobsPC.txt", "Input with the given job scheduling format")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      // number of jobs
      int N = StdIn.ReadInt();

      // source and sink
      int source = 2 * N;
      int sink = 2 * N + 1;

      // build network
      EdgeWeightedDigraph G = new EdgeWeightedDigraph(2 * N + 2);
      for (int i = 0; i < N; i++)
      {
        double duration = StdIn.ReadDouble();
        G.AddEdge(new DirectedEdge(source, i, 0.0));
        G.AddEdge(new DirectedEdge(i + N, sink, 0.0));
        G.AddEdge(new DirectedEdge(i, i + N, duration));

        // precedence constraints
        int M = StdIn.ReadInt();
        for (int j = 0; j < M; j++)
        {
          int precedent = StdIn.ReadInt();
          G.AddEdge(new DirectedEdge(N + i, precedent, 0.0));
        }
      }

      // compute longest path
      AcyclicLP lp = new AcyclicLP(G, source);

      // print results
      Console.WriteLine(" job   start  finish");
      Console.WriteLine("--------------------");
      for (int i = 0; i < N; i++)
      {
        Console.Write("{0,4} {1,7:F1} {2,7:F1}\n", i, lp.DistTo(i), lp.DistTo(i + N));
      }
      Console.Write("Finish time: {0,7:F1}\n", lp.DistTo(sink));
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
