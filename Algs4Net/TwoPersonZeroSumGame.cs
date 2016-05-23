/******************************************************************************
 *  File name :    TwoPersonZeroSumGame.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Solve an M-by-N two-person zero-sum game by reducing it to
 *  linear programming. Assuming A is a strictly positive payoff
 *  matrix, the optimal row and column player strategies are x* an y*,
 *  scaled to be probability distributions.
 *
 *  (P)  max  y^T 1         (D)  min   1^T x
 *       s.t  A^T y <= 1         s.t   A x >= 1
 *                y >= 0                 x >= 0
 *
 *  Row player is x, column player is y.
 *
 * C:\> algscmd TwoPersonZeroSumGame 6 8
 * 
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>TwoPersonZeroSumGame</c> class represents a data type for
  /// computing optimal row and column strategies to two-person zero-sum games.
  /// </para><para>
  /// This implementation solves an <c>M</c>-by-<c>N</c> two-person
  /// zero-sum game by reducing it to a linear programming problem.
  /// Assuming the payoff matrix <c>A</c> is strictly positive, the
  /// optimal row and column player strategies x* and y* are obtained
  /// by solving the following primal and dual pair of linear programs,
  /// scaling the results to be probability distributions.
  /// </para>
  /// <code>
  /// (P)  max  y^T 1         (D)  min   1^T x
  ///      s.t  A^T y &lt;= 1         s.t   A x >= 1
  ///               y >= 0                 x >= 0
  /// </code>
  /// <para>
  /// If the payoff matrix <c>A</c> has any negative entries, we add
  /// the same constant to every entry so that every entry is positive.
  /// This increases the value of the game by that constant, but does not
  /// change solutions to the two-person zero-sum game.
  /// </para><para>
  /// This implementation is not suitable for large inputs, as it calls
  /// a bare-bones linear programming solver that is neither fast nor
  /// robust with respect to floating-point roundoff error.
  /// </para></summary>
  /// <remarks><para>For additional documentation, see
  /// <a href="http://algs4.cs.princeton.edu/65reductions">Section 6.5</a>
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/TwoPersonZeroSumGame.java.html">TwoPersonZeroSumGame</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class TwoPersonZeroSumGame
  {
    private const double EPSILON = 1E-8;

    private readonly int M;         // number of rows
    private readonly int N;         // number of columns
    private LinearProgramming lp;   // linear program solver
    private double constant;        // constant added to each entry in payoff matrix
                                    // (0 if all entries are strictly positive)

    /// <summary>
    /// Determines an optimal solution to the two-sum zero-sum game
    /// with the specified payoff matrix.</summary>
    /// <param name="payoff">the <c>M</c>-by-<c>N</c> payoff matrix</param>
    ///
    public TwoPersonZeroSumGame(double[,] payoff)
    {
      M = payoff.GetLength(0);
      N = payoff.GetLength(1);

      double[] c = new double[N];
      double[] b = new double[M];
      double[,] A = new double[M,N];
      for (int i = 0; i < M; i++)
        b[i] = 1.0;
      for (int j = 0; j < N; j++)
        c[j] = 1.0;

      // find smallest entry
      constant = double.PositiveInfinity;
      for (int i = 0; i < M; i++)
        for (int j = 0; j < N; j++)
          if (payoff[i,j] < constant)
            constant = payoff[i,j];

      // add constant  to every entry to make strictly positive
      if (constant <= 0) constant = -constant + 1;
      else constant = 0;
      for (int i = 0; i < M; i++)
        for (int j = 0; j < N; j++)
          A[i,j] = payoff[i,j] + constant;

      lp = new LinearProgramming(A, b, c);

      Debug.Assert(certifySolution(payoff));
    }

    /// <summary>
    /// Returns the optimal value of this two-person zero-sum game.</summary>
    /// <returns>the optimal value of this two-person zero-sum game</returns>
    ///
    public double Value()
    {
      return 1.0 / Scale() - constant;
    }

    // sum of x[j]
    private double Scale()
    {
      double[] x = lp.Primal();
      double sum = 0.0;
      for (int j = 0; j < N; j++)
        sum += x[j];
      return sum;
    }

    /// <summary>
    /// Returns the optimal row strategy of this two-person zero-sum game.</summary>
    /// <returns>the optimal row strategy <c>X</c> of this two-person zero-sum game</returns>
    ///
    public double[] Row()
    {
      double scale = Scale();
      double[] x = lp.Primal();
      for (int j = 0; j < N; j++)
        x[j] /= scale;
      return x;
    }

    /// <summary>
    /// Returns the optimal column strategy of this two-person zero-sum game.</summary>
    /// <returns>the optimal column strategy <c>Y</c> of this two-person zero-sum game</returns>
    ///
    public double[] Column()
    {
      double scale = Scale();
      double[] y = lp.Dual();
      for (int i = 0; i < M; i++)
        y[i] /= scale;
      return y;
    }

    /**************************************************************************
     *
     *  The code below is solely for testing correctness of the data type.
     *
     **************************************************************************/

    // is the row vector x primal feasible?
    private bool isPrimalFeasible()
    {
      double[] x = Row();
      double sum = 0.0;
      for (int j = 0; j < N; j++)
      {
        if (x[j] < 0)
        {
          Console.WriteLine("row vector not a probability distribution");
          Console.Write("    x[{0}] = {1:F5}\n", j, x[j]);
          return false;
        }
        sum += x[j];
      }
      if (Math.Abs(sum - 1.0) > EPSILON)
      {
        Console.WriteLine("row vector x[] is not a probability distribution");
        Console.WriteLine("    sum = " + sum);
        return false;
      }
      return true;
    }

    // is the column vector y dual feasible?
    private bool isDualFeasible()
    {
      double[] y = Column();
      double sum = 0.0;
      for (int i = 0; i < M; i++)
      {
        if (y[i] < 0)
        {
          Console.WriteLine("column vector y[] is not a probability distribution");
          Console.Write("    y[{0}] = {1:F5}\n", i, y[i]);
          return false;
        }
        sum += y[i];
      }
      if (Math.Abs(sum - 1.0) > EPSILON)
      {
        Console.WriteLine("column vector not a probability distribution");
        Console.WriteLine("    sum = " + sum);
        return false;
      }
      return true;
    }

    // is the solution a Nash equilibrium?
    private bool isNashEquilibrium(double[,] payoff)
    {
      double[] x = Row();
      double[] y = Column();
      double value = Value();

      // given row player's mixed strategy, find column player's best pure strategy
      double opt1 = double.NegativeInfinity;
      for (int i = 0; i < M; i++)
      {
        double sum = 0.0;
        for (int j = 0; j < N; j++)
        {
          sum += payoff[i,j] * x[j];
        }
        if (sum > opt1) opt1 = sum;
      }
      if (Math.Abs(opt1 - value) > EPSILON)
      {
        Console.WriteLine("Optimal value = " + value);
        Console.WriteLine("Optimal best response for column player = " + opt1);
        return false;
      }

      // given column player's mixed strategy, find row player's best pure strategy
      double opt2 = double.PositiveInfinity;
      for (int j = 0; j < N; j++)
      {
        double sum = 0.0;
        for (int i = 0; i < M; i++)
        {
          sum += payoff[i,j] * y[i];
        }
        if (sum < opt2) opt2 = sum;
      }
      if (Math.Abs(opt2 - value) > EPSILON)
      {
        Console.WriteLine("Optimal value = " + value);
        Console.WriteLine("Optimal best response for row player = " + opt2);
        return false;
      }
      return true;
    }

    private bool certifySolution(double[,] payoff)
    {
      return isPrimalFeasible() && isDualFeasible() && isNashEquilibrium(payoff);
    }


    internal static void test(string description, double[,] payoff)
    {
      Console.WriteLine();
      Console.WriteLine(description);
      Console.WriteLine("------------------------------------");
      int M = payoff.GetLength(0);
      int N = payoff.GetLength(1);
      TwoPersonZeroSumGame zerosum = new TwoPersonZeroSumGame(payoff);
      double[] x = zerosum.Row();
      double[] y = zerosum.Column();

      Console.Write("x[] = [");
      for (int j = 0; j < N - 1; j++)
        Console.Write("{0:F4}, ", x[j]);
      Console.Write("{0:F4}]\n", x[N - 1]);

      Console.Write("y[] = [");
      for (int i = 0; i < M - 1; i++)
        Console.Write("{0:F4}, ", y[i]);
      Console.Write("{0:F4}]\n", y[M - 1]);
      Console.WriteLine("value =  " + zerosum.Value());

    }

    // row = { 4/7, 3/7 }, column = { 0, 4/7, 3/7 }, value = 20/7
    // http://en.wikipedia.org/wiki/Zero-sum
    internal static void test1()
    {
      double[,] payoff = {
            { 30, -10,  20 },
            { 10,  20, -20 }
        };
      test("wikipedia", payoff);
    }

    // skew-symmetric => value = 0
    // Linear Programming by Chvatal, p. 230
    internal static void test2()
    {
      double[,] payoff = {
            {  0,  2, -3,  0 },
            { -2,  0,  0,  3 },
            {  3,  0,  0, -4 },
            {  0, -3,  4,  0 }
        };
      test("Chvatal, p. 230", payoff);
    }

    // Linear Programming by Chvatal, p. 234
    // row    = { 0, 56/99, 40/99, 0, 0, 2/99, 0, 1/99 }
    // column = { 28/99, 30/99, 21/99, 20/99 }
    // value  = 4/99
    internal static void test3()
    {
      double[,] payoff = {
            {  0,  2, -3,  0 },
            { -2,  0,  0,  3 },
            {  3,  0,  0, -4 },
            {  0, -3,  4,  0 },
            {  0,  0, -3,  3 },
            { -2,  2,  0,  0 },
            {  3, -3,  0,  0 },
            {  0,  0,  4, -4 }
        };
      test("Chvatal, p. 234", payoff);
    }

    // Linear Programming by Chvatal, p. 236
    // row    = { 0, 2/5, 7/15, 0, 2/15, 0, 0, 0 }
    // column = { 2/3, 0, 0, 1/3 }
    // value  = -1/3
    internal static void test4()
    {
      double[,] payoff = {
            {  0,  2, -1, -1 },
            {  0,  1, -2, -1 },
            { -1, -1,  1,  1 },
            { -1,  0,  0,  1 },
            {  1, -2,  0, -3 },
            {  1, -1, -1, -3 },
            {  0, -3,  2, -1 },
            {  0, -2,  1, -1 },
        };
      test("Chvatal p. 236", payoff);
    }

    // rock, paper, scissors
    // row    = { 1/3, 1/3, 1/3 }
    // column = { 1/3, 1/3, 1/3 }
    internal static void test5()
    {
      double[,] payoff = {
            {  0, -1,  1 },
            {  1,  0, -1 },
            { -1,  1,  0 }
        };
      test("rock, paper, scisssors", payoff);
    }

    /// <summary>
    /// Demo test the <c>ZeroSumGameToLP</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd TwoPersonZeroSumGame N M", "N, M-sises of N by M two-person zero-sum game")]
    public static void MainTest(string[] args)
    {
      TwoPersonZeroSumGame.test1();
      TwoPersonZeroSumGame.test2();
      TwoPersonZeroSumGame.test3();
      TwoPersonZeroSumGame.test4();
      TwoPersonZeroSumGame.test5();

      int M = 5, N = 5;
      if (args.Length == 3)
      {
        M = int.Parse(args[1]);
        N = int.Parse(args[2]);
      }
      double[,] payoff = new double[M, N];
      for (int i = 0; i < M; i++)
        for (int j = 0; j < N; j++)
          payoff[i, j] = StdRandom.Uniform(-0.5, 0.5);
      TwoPersonZeroSumGame.test("random " + M + "-by-" + N, payoff);
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
 