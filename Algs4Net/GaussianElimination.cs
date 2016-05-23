/******************************************************************************
 *  File name :    GaussianElimination.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Gaussian elimination with partial pivoting for M-by-N system.
 *
 *  C:\> algscmd GaussianElimination N
 *  -1.000000
 *  2.000000
 *  2.000000
 *
 *  3.000000
 *  -1.000000
 *  -2.000000
 * 
 *  System is infeasible
 *
 *  -6.250000
 *  -4.500000
 *  0.000000
 *  0.000000
 *  1.000000
 *
 *  System is infeasible
 *
 *  -1.375000
 *  1.625000
 *  0.000000
 *
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>GaussianElimination</c> data type provides methods
  /// to solve a linear system of equations <c>Ax</c> = <c>B</c>,
  /// where <c>A</c> is an <c>M</c>-by-<c>N</c> matrix
  /// and <c>B</c> is a length <c>N</c> vector.
  /// </para><para>
  /// This is a bare-bones implementation that uses Gaussian elimination
  /// with partial pivoting.
  /// See <a href = "http://algs4.cs.princeton.edu/99scientific/GaussianEliminationLite.java.html">GaussianEliminationLite.java</a>
  /// for a stripped-down version that assumes the matrix <c>A</c> is square
  /// and nonsingular. See <seealso cref="GaussJordanElimination"/> for an alternate
  /// implementation that uses Gauss-Jordan elimination.
  /// For an industrial-strength numerical linear algebra library,
  /// see <a href = "http://math.nist.gov/javanumerics/jama/">JAMA</a>.
  /// </para></summary>
  /// <remarks><para>For additional documentation, see
  /// <a href="http://algs4.cs.princeton.edu/99scientific">Section 9.9</a>
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/GaussianElimination.java.html">GaussianElimination</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class GaussianElimination
  {
    private const double EPSILON = 1e-8;

    private readonly int M;   // number of rows
    private readonly int N;   // number of columns
    private double[][] a;     // M-by-N+1 augmented matrix (a jagged array)

    /// <summary>
    /// Solves the linear system of equations <c>Ax</c> = <c>B</c>,
    /// where <c>A</c> is an <c>M</c>-by-<c>N</c> matrix and <c>B</c>
    /// is a length <c>M</c> vector.</summary>
    /// <param name="A">A the <c>M</c>-by-<c>N</c> constraint matrix</param>
    /// <param name="b">the length <c>M</c> right-hand-side vector</param>
    /// <exception cref="ArgumentException">if the dimensions disagree, i.e.,
    /// the length of <c>b</c> does not equal <c>M</c></exception>
    ///
    public GaussianElimination(double[][] A, double[] b)
    {
      M = A.GetLength(0);
      N = A[0].Length;

      if (b.Length != M) throw new ArgumentException("Dimensions disagree");

      // build augmented matrix
      a = new double[M][];
      for (int i = 0; i < M; i++)
      {
        a[i] = new double[N + 1];
        for (int j = 0; j < N; j++)
        {
          a[i][j] = A[i][j];
        }
        a[i][N] = b[i];
      }

      forwardElimination();

      Debug.Assert(certifySolution(A, b));
    }

    // forward elimination
    private void forwardElimination()
    {
      for (int p = 0; p < Math.Min(M, N); p++)
      {
        // find pivot row using partial pivoting
        int max = p;
        for (int i = p + 1; i < M; i++)
        {
          if (Math.Abs(a[i][p]) > Math.Abs(a[max][p]))
          {
            max = i;
          }
        }

        // swap
        swap(p, max);

        // singular or nearly singular
        if (Math.Abs(a[p][p]) <= EPSILON)
        {
          continue;
        }

        // pivot
        pivot(p);
      }
    }

    // swap row1 and row2
    private void swap(int row1, int row2)
    {
      double[] temp = a[row1];
      a[row1] = a[row2];
      a[row2] = temp;
    }

    // pivot on a[p][p]
    private void pivot(int p)
    {
      for (int i = p + 1; i < M; i++)
      {
        double alpha = a[i][p] / a[p][p];
        for (int j = p; j <= N; j++)
        {
          a[i][j] -= alpha * a[p][j];
        }
      }
    }

    /// <summary>
    /// Returns a solution to the linear system of equations <c>Ax</c> = <c>B</c>.</summary>
    /// <returns>a solution <c>X</c> to the linear system of equations</returns>
    ///        <c>Ax</c> = <c>B</c>; <c>null</c> if no such solution
    ///
    public double[] Primal()
    {
      // back substitution
      double[] x = new double[N];
      for (int i = Math.Min(N - 1, M - 1); i >= 0; i--)
      {
        double sum = 0.0;
        for (int j = i + 1; j < N; j++)
        {
          sum += a[i][j] * x[j];
        }

        if (Math.Abs(a[i][i]) > EPSILON)
          x[i] = (a[i][N] - sum) / a[i][i];
        else if (Math.Abs(a[i][N] - sum) > EPSILON)
          return null;
      }

      // redundant rows
      for (int i = N; i < M; i++)
      {
        double sum = 0.0;
        for (int j = 0; j < N; j++)
        {
          sum += a[i][j] * x[j];
        }
        if (Math.Abs(a[i][N] - sum) > EPSILON)
          return null;
      }
      return x;
    }

    /// <summary>
    /// Returns true if there exists a solution to the linear system of
    /// equations <c>Ax</c> = <c>B</c>.</summary>
    /// <returns><c>true</c> if there exists a solution to the linear system
    /// of equations <c>Ax</c> = <c>B</c>; <c>false</c> otherwise</returns>
    ///
    public bool IsFeasible
    {
      get { return Primal() != null; }
    }


    // check that Ax = b
    private bool certifySolution(double[][] A, double[] b)
    {
      if (!IsFeasible) return true;
      double[] x = Primal();
      for (int i = 0; i < M; i++)
      {
        double sum = 0.0;
        for (int j = 0; j < N; j++)
        {
          sum += A[i][j] * x[j];
        }
        if (Math.Abs(sum - b[i]) > EPSILON)
        {
          Console.WriteLine("not feasible");
          Console.WriteLine("b[" + i + "] = " + b[i] + ", sum = " + sum);
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Demo test the <c>GaussianElimination</c> data type.</summary>
    ///
    internal static void test(string name, double[][] A, double[] b)
    {
      Console.WriteLine("----------------------------------------------------");
      Console.WriteLine(name);
      Console.WriteLine("----------------------------------------------------");
      GaussianElimination gaussian = new GaussianElimination(A, b);
      double[] x = gaussian.Primal();
      if (gaussian.IsFeasible)
      {
        for (int i = 0; i < x.Length; i++)
        {
          Console.Write("{0:F6}\n", x[i]);
        }
      }
      else
      {
        Console.WriteLine("System is infeasible");
      }
      Console.WriteLine();
      Console.WriteLine();
    }


    // 3-by-3 nonsingular system
    internal static void test1()
    {
      double[][] A = {
        new double[]{ 0, 1,  1 },
        new double[]{ 2, 4, -2 },
        new double[]{ 0, 3, 15 }
      };
      double[] b = { 4, 2, 36 };
      test("test 1 (3-by-3 system, nonsingular)", A, b);
    }

    // 3-by-3 nonsingular system
    internal static void test2()
    {
      double[][] A = {
        new double[]{  1, -3,   1 },
        new double[]{  2, -8,   8 },
        new double[]{ -6,  3, -15 }
      };
      double[] b = { 4, -2, 9 };
      test("test 2 (3-by-3 system, nonsingular)", A, b);
    }

    // 5-by-5 singular: no solutions
    internal static void test3()
    {
      double[][] A = {
        new double[]{  2, -3, -1,  2,  3 },
        new double[]{  4, -4, -1,  4, 11 },
        new double[]{  2, -5, -2,  2, -1 },
        new double[]{  0,  2,  1,  0,  4 },
        new double[]{ -4,  6,  0,  0,  7 },
      };
      double[] b = { 4, 4, 9, -6, 5 };
      test("test 3 (5-by-5 system, no solutions)", A, b);
    }

    // 5-by-5 singular: infinitely many solutions
    internal static void test4()
    {
      double[][] A = {
        new double[]{  2, -3, -1,  2,  3 },
        new double[]{  4, -4, -1,  4, 11 },
        new double[]{  2, -5, -2,  2, -1 },
        new double[]{  0,  2,  1,  0,  4 },
        new double[]{ -4,  6,  0,  0,  7 },
      };
      double[] b = { 4, 4, 9, -5, 5 };
      test("test 4 (5-by-5 system, infinitely many solutions)", A, b);
    }

    // 3-by-3 singular: no solutions
    internal static void test5()
    {
      double[][] A = {
        new double[]{  2, -1,  1 },
        new double[]{  3,  2, -4 },
        new double[]{ -6,  3, -3 },
      };
      double[] b = { 1, 4, 2 };
      test("test 5 (3-by-3 system, no solutions)", A, b);
    }

    // 3-by-3 singular: infinitely many solutions
    internal static void test6()
    {
      double[][] A = {
        new double[]{  1, -1,  2 },
        new double[]{  4,  4, -2 },
        new double[]{ -2,  2, -4 },
      };
      double[] b = { -3, 1, 6 };
      test("test 6 (3-by-3 system, infinitely many solutions)", A, b);
    }

    // 4-by-3 full rank and feasible system
    internal static void test7()
    {
      double[][] A = {
        new double[]{ 0, 1,  1 },
        new double[]{ 2, 4, -2 },
        new double[]{ 0, 3, 15 },
        new double[]{ 2, 8, 14 }
      };
      double[] b = { 4, 2, 36, 42 };
      test("test 7 (4-by-3 system, full rank)", A, b);
    }

    // 4-by-3 full rank and infeasible system
    internal static void test8()
    {
      double[][] A = {
        new double[]{ 0, 1,  1 },
        new double[]{ 2, 4, -2 },
        new double[]{ 0, 3, 15 },
        new double[]{ 2, 8, 14 }
      };
      double[] b = { 4, 2, 36, 40 };
      test("test 8 (4-by-3 system, no solution)", A, b);
    }

    // 3-by-4 full rank system
    internal static void test9()
    {
      double[][] A = {
        new double[]{  1, -3,   1,  1 },
        new double[]{  2, -8,   8,  2 },
        new double[]{ -6,  3, -15,  3 }
      };
      double[] b = { 4, -2, 9 };
      test("test 9 (3-by-4 system, full rank)", A, b);
    }

    /// <summary>
    /// Demo test the <c>GaussianElimination</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd GaussianElimination N", "N - the size of N by N matrices")]
    public static void MainTest(string[] args)
    {
      GaussianElimination.test1();
      GaussianElimination.test2();
      GaussianElimination.test3();
      GaussianElimination.test4();
      GaussianElimination.test5();
      GaussianElimination.test6();
      GaussianElimination.test7();
      GaussianElimination.test8();
      GaussianElimination.test9();

      // N-by-N random system
      int N = int.Parse(args[0]);

      // build augmented matrix
      double[][] A = new double[N][];

      for (int i = 0; i < N; i++)
      {
        A[i] = new double[N];
        for (int j = 0; j < N; j++)
        {
          A[i][j] = StdRandom.Uniform(1000);
        }
      }

      double[] b = new double[N];
      for (int i = 0; i < N; i++)
        b[i] = StdRandom.Uniform(1000);

      GaussianElimination.test(N + "-by-" + N + " (probably nonsingular)", A, b);
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
