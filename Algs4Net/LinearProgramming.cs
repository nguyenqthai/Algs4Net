/******************************************************************************
 *  File name :    LinearProgramming.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Given an M-by-N matrix A, an M-length vector b, and an
 *  N-length vector c, solve the  LP { max cx : Ax <= b, x >= 0 }.
 *  Assumes that b >= 0 so that x = 0 is a basic feasible solution.
 *
 *  Creates an (M+1)-by-(N+M+1) simplex tableaux with the 
 *  RHS in column M+N, the objective function in row M, and
 *  slack variables in columns M through M+N-1.
 *
 * C:\> algscmd LinearProgramming 6 8
 * 
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>LinearProgramming</c> class represents a data type for solving a
  /// linear program of the form { max cx : Ax &lt;= b, x &gt;= 0 }, where A is a M-by-N
  /// matrix, b is an M-length vector, and c is an N-length vector. For simplicity,
  /// we assume that A is of full rank and that b >= 0 so that x = 0 is a basic
  /// feasible soution.</para><para>
  /// The data type supplies methods for determining the optimal primal and
  /// dual solutions.</para><para>
  /// This is a bare-bones implementation of the <c>Simplex algorithm</c>.
  /// It uses Bland's rule to determing the entering and leaving variables.
  /// It is not suitable for use on large inputs. It is also not robust
  /// in the presence of floating-point roundoff error.</para></summary>
  /// <remarks><para>For additional documentation, see
  /// <a href="http://algs4.cs.princeton.edu/65reductions">Section 6.5</a>
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/LinearProgramming.java.html">LinearProgramming</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class LinearProgramming
  {
    private const double EPSILON = 1.0E-10;
    private double[,] a;    // tableaux
    private int M;          // number of constraints
    private int N;          // number of original variables

    private int[] basis;    // basis[i] = basic variable corresponding to row i
                            // only needed to print out solution, not book

    /// <summary>
    /// Determines an optimal solution to the linear program
    /// { max cx : Ax &lt;= b, x >= 0 }, where A is a M-by-N
    /// matrix, b is an M-length vector, and c is an N-length vector.</summary>
    /// <param name="A">the <c>M</c>-by-<c>N</c> matrix</param>
    /// <param name="b">the <c>M</c>-length RHS vector</param>
    /// <param name="c">the <c>N</c>-length cost vector</param>
    /// <exception cref="ArgumentException">unless b[i] >= 0 for each i</exception>
    /// <exception cref="ArithmeticException">if the linear program is unbounded</exception>
    ///
    public LinearProgramming(double[,] A, double[] b, double[] c)
    {
      M = b.Length;
      N = c.Length;
      for (int i = 0; i < M; i++)
        if (!(b[i] >= 0)) throw new ArgumentException("RHS must be nonnegative");

      a = new double[M + 1,N + M + 1];
      for (int i = 0; i < M; i++)
        for (int j = 0; j < N; j++)
          a[i,j] = A[i,j];
      for (int i = 0; i < M; i++)
        a[i,N + i] = 1.0;
      for (int j = 0; j < N; j++)
        a[M,j] = c[j];
      for (int i = 0; i < M; i++)
        a[i,M + N] = b[i];

      basis = new int[M];
      for (int i = 0; i < M; i++)
        basis[i] = N + i;

      solve();

      // check optimality conditions
      Debug.Assert(check(A, b, c));
    }

    // run simplex algorithm starting from initial BFS
    private void solve()
    {
      while (true)
      {
        // find entering column q
        int q = bland();
        if (q == -1) break;  // optimal

        // find leaving row p
        int p = minRatioRule(q);
        if (p == -1) throw new ArithmeticException("Linear program is unbounded");

        // pivot
        pivot(p, q);

        // update basis
        basis[p] = q;
      }
    }

    // lowest index of a non-basic column with a positive cost
    private int bland()
    {
      for (int j = 0; j < M + N; j++)
        if (a[M,j] > 0) return j;
      return -1;  // optimal
    }

    // index of a non-basic column with most positive cost
    private int dantzig()
    {
      int q = 0;
      for (int j = 1; j < M + N; j++)
        if (a[M,j] > a[M,q]) q = j;

      if (a[M,q] <= 0) return -1;  // optimal
      else return q;
    }

    // find row p using min ratio rule (-1 if no such row)
    // (smallest such index if there is a tie)
    private int minRatioRule(int q)
    {
      double EPSILON = 1E-12;
      int p = -1;
      for (int i = 0; i < M; i++)
      {
        // if (a[i,q] <= 0) continue;
        if (a[i,q] <= EPSILON) continue;
        else if (p == -1) p = i;
        else if ((a[i,M + N] / a[i,q]) < (a[p,M + N] / a[p,q])) p = i;
      }
      return p;
    }

    // pivot on entry (p, q) using Gauss-Jordan elimination
    private void pivot(int p, int q)
    {
      // everything but row p and column q
      for (int i = 0; i <= M; i++)
        for (int j = 0; j <= M + N; j++)
          if (i != p && j != q) a[i,j] -= a[p,j] * a[i,q] / a[p,q];

      // zero out column q
      for (int i = 0; i <= M; i++)
        if (i != p) a[i,q] = 0.0;

      // scale row p
      for (int j = 0; j <= M + N; j++)
        if (j != q) a[p,j] /= a[p,q];
      a[p,q] = 1.0;
    }

    /// <summary>
    /// Returns the optimal value of this linear program.</summary>
    /// <returns>the optimal value of this linear program</returns>
    ///
    public double Value
    {
      get { return -a[M, M + N]; }
    }

    /// <summary>
    /// Returns the optimal primal solution to this linear program.</summary>
    /// <returns>the optimal primal solution to this linear program</returns>
    ///
    public double[] Primal()
    {
      double[] x = new double[N];
      for (int i = 0; i < M; i++)
        if (basis[i] < N) x[basis[i]] = a[i,M + N];
      return x;
    }

    /// <summary>
    /// Returns the optimal dual solution to this linear program</summary>
    /// <returns>the optimal dual solution to this linear program</returns>
    ///
    public double[] Dual()
    {
      double[] y = new double[M];
      for (int i = 0; i < M; i++)
        y[i] = -a[M,N + i];
      return y;
    }


    // is the solution primal feasible?
    private bool isPrimalFeasible(double[,] A, double[] b)
    {
      double[] x = Primal();

      // check that x >= 0
      for (int j = 0; j < x.Length; j++)
      {
        if (x[j] < 0.0)
        {
          Console.WriteLine("x[" + j + "] = " + x[j] + " is negative");
          return false;
        }
      }

      // check that Ax <= b
      for (int i = 0; i < M; i++)
      {
        double sum = 0.0;
        for (int j = 0; j < N; j++)
        {
          sum += A[i,j] * x[j];
        }
        if (sum > b[i] + EPSILON)
        {
          Console.WriteLine("not primal feasible");
          Console.WriteLine("b[" + i + "] = " + b[i] + ", sum = " + sum);
          return false;
        }
      }
      return true;
    }

    // is the solution dual feasible?
    private bool isDualFeasible(double[,] A, double[] c)
    {
      double[] y = Dual();

      // check that y >= 0
      for (int i = 0; i < y.Length; i++)
      {
        if (y[i] < 0.0)
        {
          Console.WriteLine("y[" + i + "] = " + y[i] + " is negative");
          return false;
        }
      }

      // check that yA >= c
      for (int j = 0; j < N; j++)
      {
        double sum = 0.0;
        for (int i = 0; i < M; i++)
        {
          sum += A[i,j] * y[i];
        }
        if (sum < c[j] - EPSILON)
        {
          Console.WriteLine("not dual feasible");
          Console.WriteLine("c[" + j + "] = " + c[j] + ", sum = " + sum);
          return false;
        }
      }
      return true;
    }

    // check that optimal value = cx = yb
    private bool isOptimal(double[] b, double[] c)
    {
      double[] x = Primal();
      double[] y = Dual();
      double value = Value;

      // check that value = cx = yb
      double value1 = 0.0;
      for (int j = 0; j < x.Length; j++)
        value1 += c[j] * x[j];
      double value2 = 0.0;
      for (int i = 0; i < y.Length; i++)
        value2 += y[i] * b[i];
      if (Math.Abs(value - value1) > EPSILON || Math.Abs(value - value2) > EPSILON)
      {
        Console.WriteLine("value = " + value + ", cx = " + value1 + ", yb = " + value2);
        return false;
      }

      return true;
    }

    private bool check(double[,] A, double[] b, double[] c)
    {
      return isPrimalFeasible(A, b) && isDualFeasible(A, c) && isOptimal(b, c);
    }

    // print tableaux
    private void show()
    {
      Console.WriteLine("M = " + M);
      Console.WriteLine("N = " + N);
      for (int i = 0; i <= M; i++)
      {
        for (int j = 0; j <= M + N; j++)
        {
          Console.Write("{0:F} ", a[i,j]);
        }
        Console.WriteLine();
      }
      Console.WriteLine("value = {0:F}", Value);
      for (int i = 0; i < M; i++)
        if (basis[i] < N) Console.WriteLine("x_" + basis[i] + " = " + a[i,M + N]);
      Console.WriteLine();
    }


    private static void test(double[,] A, double[] b, double[] c)
    {
      LinearProgramming lp = new LinearProgramming(A, b, c);
      Console.WriteLine("value = " + lp.Value);
      double[] x = lp.Primal();
      for (int i = 0; i < x.Length; i++)
        Console.WriteLine("x[{0}] = {1:F5}", i, x[i]);
      double[] y = lp.Dual();
      for (int j = 0; j < y.Length; j++)
        Console.WriteLine("y[{0}] = {1:F5}", j, y[j]);
    }

    internal static void test1()
    {
      double[,] A = {
            { -1,  1,  0 },
            {  1,  4,  0 },
            {  2,  1,  0 },
            {  3, -4,  0 },
            {  0,  0,  1 },
        };
      double[] c = { 1, 1, 1 };
      double[] b = { 5, 45, 27, 24, 4 };
      test(A, b, c);
    }


    // x0 = 12, x1 = 28, opt = 800
    internal static void test2()
    {
      double[] c = { 13.0, 23.0 };
      double[] b = { 480.0, 160.0, 1190.0 };
      double[,] A = {
            {  5.0, 15.0 },
            {  4.0,  4.0 },
            { 35.0, 20.0 },
        };
      test(A, b, c);
    }

    // unbounded
    internal static void test3()
    {
      double[] c = { 2.0, 3.0, -1.0, -12.0 };
      double[] b = { 3.0, 2.0 };
      double[,] A = {
            { -2.0, -9.0,  1.0,  9.0 },
            {  1.0,  1.0, -1.0, -2.0 },
        };
      test(A, b, c);
    }

    // degenerate - cycles if you choose most positive objective function coefficient
    internal static void test4()
    {
      double[] c = { 10.0, -57.0, -9.0, -24.0 };
      double[] b = { 0.0, 0.0, 1.0 };
      double[,] A = {
            { 0.5, -5.5, -2.5, 9.0 },
            { 0.5, -1.5, -0.5, 1.0 },
            { 1.0,  0.0,  0.0, 0.0 },
        };
      test(A, b, c);
    }

    /// <summary>
    /// Demo test the <c>LinearProgramming</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd LinearProgramming N M", "N, M-sises of N by M matrices")]
    public static void MainTest(string[] args)
    {
      Console.WriteLine("----- test 1 --------------------");
      LinearProgramming.test1();
      Console.WriteLine("----- test 2 --------------------");
      LinearProgramming.test2();
      Console.WriteLine("----- test 3 --------------------");
      try
      {
        LinearProgramming.test3();
      }
      catch (ArithmeticException e)
      {
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
      }

      Console.WriteLine("----- test 4 --------------------");
      LinearProgramming.test4();


      Console.WriteLine("----- test random ---------------");
      int M = 5, N = 5;
      if (args.Length == 3)
      {
        M = int.Parse(args[1]);
        N = int.Parse(args[2]);
      }
      double[] c = new double[N];
      double[] b = new double[M];
      double[,] A = new double[M, N];
      for (int j = 0; j < N; j++)
        c[j] = StdRandom.Uniform(1000);
      for (int i = 0; i < M; i++)
        b[i] = StdRandom.Uniform(1000);
      for (int i = 0; i < M; i++)
        for (int j = 0; j < N; j++)
          A[i, j] = StdRandom.Uniform(100);
      try
      {
        LinearProgramming lp = new LinearProgramming(A, b, c);
        Console.WriteLine(lp.Value);
      }
      catch (ArithmeticException)
      {
        Console.WriteLine("The randomlly generated linear program is unbounded. Try again.");
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
