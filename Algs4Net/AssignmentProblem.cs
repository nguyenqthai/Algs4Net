/******************************************************************************
 *  File name :    AssignmentProblem.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Solve an N-by-N assignment problem in N^3 log N time using the
 *  successive shortest path algorithm.
 *
 *  Assumes N-by-N cost matrix is nonnegative.
 *  TODO: remove this assumption
 *
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>AssignmentProblem</c> class represents a data type for computing
  /// an optimal solution to an <c>N</c>-by-<c>N</c> <c>Assignment problem</c>.
  /// The assignment problem is to find a minimum weight matching in an
  /// edge-weighted complete bipartite graph.
  /// </para><para>
  /// The data type supplies methods for determining the optimal solution
  /// and the corresponding dual solution.
  /// </para><para>
  /// This implementation uses the <c>Successive shortest paths algorithm</c>.
  /// The order of growth of the running time in the worst case is
  /// O(<c>N</c>^3 log <c>N</c>) to solve an <c>N</c>-by-<c>N</c>
  /// instance.
  /// </para></summary>
  /// <remarks><para>For additional documentation, see
  /// <a href="http://algs4.cs.princeton.edu/65reductions">Section 6.5</a>
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/AssignmentProblem.java.html">AssignmentProblem</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class AssignmentProblem
  {
    private const int UNMATCHED = -1;

    private int N;              // number of rows and columns
    private double[,] weight;   // the N-by-N cost matrix
    private double[] px;        // px[i] = dual variable for row i
    private double[] py;        // py[j] = dual variable for col j
    private int[] xy;           // xy[i] = j means i-j is a match
    private int[] yx;           // yx[j] = i means i-j is a match

    /// <summary>
    /// Determines an optimal solution to the assignment problem.
    /// </summary>
    /// <param name="weight">the <c>N</c>-by-<c>N</c> matrix of weights</param>
    /// <exception cref="ArgumentException">unless all weights are nonnegative</exception>
    /// <exception cref="NullReferenceException">if <c>weight</c> is <c>null</c></exception>
    ///
    public AssignmentProblem(double[,] weight)
    {
      N = weight.GetLength(0);
      this.weight = new double[N, N];
      for (int i = 0; i < N; i++)
      {
        for (int j = 0; j < N; j++)
        {
          if (!(weight[i, j] >= 0.0))
            throw new ArgumentException("weights must be nonnegative");
          this.weight[i, j] = weight[i, j];
        }
      }

      // dual variables
      px = new double[N];
      py = new double[N];

      // initial matching is empty
      xy = new int[N];
      yx = new int[N];
      for (int i = 0; i < N; i++)
        xy[i] = UNMATCHED;
      for (int j = 0; j < N; j++)
        yx[j] = UNMATCHED;

      // add N edges to matching
      for (int k = 0; k < N; k++)
      {
        Debug.Assert(isDualFeasible());
        Debug.Assert(isComplementarySlack());
        augment();
      }
      Debug.Assert(certifySolution());
    }

    // find shortest augmenting path and upate
    private void augment()
    {

      // build residual graph
      EdgeWeightedDigraph G = new EdgeWeightedDigraph(2 * N + 2);
      int s = 2 * N, t = 2 * N + 1;
      for (int i = 0; i < N; i++)
      {
        if (xy[i] == UNMATCHED)
          G.AddEdge(new DirectedEdge(s, i, 0.0));
      }
      for (int j = 0; j < N; j++)
      {
        if (yx[j] == UNMATCHED)
          G.AddEdge(new DirectedEdge(N + j, t, py[j]));
      }
      for (int i = 0; i < N; i++)
      {
        for (int j = 0; j < N; j++)
        {
          if (xy[i] == j) G.AddEdge(new DirectedEdge(N + j, i, 0.0));
          else G.AddEdge(new DirectedEdge(i, N + j, reducedCost(i, j)));
        }
      }

      // compute shortest path from s to every other vertex
      DijkstraSP spt = new DijkstraSP(G, s);

      // augment along alternating path
      foreach (DirectedEdge e in spt.PathTo(t))
      {
        int i = e.From, j = e.To - N;
        if (i < N)
        {
          xy[i] = j;
          yx[j] = i;
        }
      }

      // update dual variables
      for (int i = 0; i < N; i++)
        px[i] += spt.DistTo(i);
      for (int j = 0; j < N; j++)
        py[j] += spt.DistTo(N + j);
    }

    // reduced cost of i-j
    private double reducedCost(int i, int j)
    {
      return weight[i, j] + px[i] - py[j];
    }

    /// <summary>
    /// Returns the dual optimal value for the specified row.</summary>
    /// <param name="i">the row index</param>
    /// <returns>the dual optimal value for row <c>i</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless <c>0 &lt; i &lt; N</c></exception>
    ///
    public double DualRow(int i)
    {
      validate(i);
      return px[i];
    }

    /// <summary>
    /// Returns the dual optimal value for the specified column.</summary>
    /// <param name="j">the column index</param>
    /// <returns>the dual optimal value for column <c>j</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless <c>0 &lt; j &lt; N</c></exception>
    ///
    public double DualCol(int j)
    {
      validate(j);
      return py[j];
    }

    /// <summary>
    /// Returns the column associated with the specified row in the optimal solution.</summary>
    /// <param name="i">the row index</param>
    /// <returns>the column matched to row <c>i</c> in the optimal solution</returns>
    /// <exception cref="IndexOutOfRangeException">unless <c>0 &lt; i &lt; N</c></exception>
    ///
    ///
    public int Sol(int i)
    {
      validate(i);
      return xy[i];
    }

    /// <summary>
    /// Returns the total weight of the optimal solution</summary>
    ///
    /// <returns>the total weight of the optimal solution</returns>
    ///
    ///
    public double Weight()
    {
      double total = 0.0;
      for (int i = 0; i < N; i++)
      {
        if (xy[i] != UNMATCHED)
          total += weight[i, xy[i]];
      }
      return total;
    }

    private void validate(int i)
    {
      if (i < 0 || i >= N) throw new IndexOutOfRangeException();
    }


    /**************************************************************************
     *
     *  The code below is solely for testing correctness of the data type.
     *
     **************************************************************************/

    // check that dual variables are feasible
    private bool isDualFeasible()
    {
      // check that all edges have >= 0 reduced cost
      for (int i = 0; i < N; i++)
      {
        for (int j = 0; j < N; j++)
        {
          if (reducedCost(i, j) < 0)
          {
            Console.WriteLine("Dual variables are not feasible");
            return false;
          }
        }
      }
      return true;
    }

    // check that primal and dual variables are complementary slack
    private bool isComplementarySlack()
    {

      // check that all matched edges have 0-reduced cost
      for (int i = 0; i < N; i++)
      {
        if ((xy[i] != UNMATCHED) && (reducedCost(i, xy[i]) != 0))
        {
          Console.WriteLine("Primal and dual variables are not complementary slack");
          return false;
        }
      }
      return true;
    }

    // check that primal variables are a perfect matching
    private bool isPerfectMatching()
    {

      // check that xy[] is a perfect matching
      bool[] perm = new bool[N];
      for (int i = 0; i < N; i++)
      {
        if (perm[xy[i]])
        {
          Console.WriteLine("Not a perfect matching");
          return false;
        }
        perm[xy[i]] = true;
      }

      // check that xy[] and yx[] are inverses
      for (int j = 0; j < N; j++)
      {
        if (xy[yx[j]] != j)
        {
          Console.WriteLine("xy[] and yx[] are not inverses");
          return false;
        }
      }
      for (int i = 0; i < N; i++)
      {
        if (yx[xy[i]] != i)
        {
          Console.WriteLine("xy[] and yx[] are not inverses");
          return false;
        }
      }

      return true;
    }

    // check optimality conditions
    private bool certifySolution()
    {
      return isPerfectMatching() && isDualFeasible() && isComplementarySlack();
    }

    /// <summary>
    /// Demo test the <c>AssignmentProblem</c> data type.
    /// Takes a command-line argument N; creates a random N-by-N matrix;
    /// solves the N-by-N assignment problem; and prints the optimal
    /// solution.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd AssignmentProblem N", "N-size of the N-by-N assignment problem")]
    public static void MainTest(string[] args)
    {
      // create random N-by-N matrix
      int N = int.Parse(args[0]);
      double[,] weight = new double[N, N];
      for (int i = 0; i < N; i++)
      {
        for (int j = 0; j < N; j++)
        {
          weight[i, j] = 100 + StdRandom.Uniform(900);
        }
      }

      // solve assignment problem
      AssignmentProblem assignment = new AssignmentProblem(weight);
      Console.Write("Weight = {0}\n", assignment.Weight());
      Console.WriteLine();

      // print N-by-N matrix and optimal solution
      if (N <= 20) return;
      for (int i = 0; i < N; i++)
      {
        for (int j = 0; j < N; j++)
        {
          if (j == assignment.Sol(i))
            Console.Write("*{0} ", weight[i, j]);
          else
            Console.Write(" {0} ", weight[i, j]);
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
