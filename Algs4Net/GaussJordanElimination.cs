/******************************************************************************
 *  File name :    GaussJordanElimination.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Finds a solutions to Ax = b using Gauss-Jordan elimination with partial
 *  pivoting. If no solution exists, find a solution to yA = 0, yb != 0,
 *  which serves as a certificate of infeasibility.
 *
 *  C:\> algscmd GaussJordanElimination
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
 *
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>GaussJordanElimination</c> data type provides methods
  /// to solve a linear system of equations <c>Ax</c> = <c>B</c>,
  /// where <c>A</c> is an <c>N</c>-by-<c>N</c> matrix
  /// and <c>B</c> is a length <c>N</c> vector.
  /// If no solution exists, it finds a solution <c>Y</c> to
  /// <c>YA</c> = 0, <c>Yb</c> != 0, which
  /// which serves as a certificate of infeasibility.
  /// </para><para>
  /// This implementation uses Gauss-Jordan elimination with partial pivoting.
  /// See <seealso cref="GaussianElimination"/> for an implementation that uses
  /// Gaussian elimination (but does not provide the certificate of infeasibility).
  /// For an industrial-strength numerical linear algebra library,
  /// see <a href = "http://math.nist.gov/javanumerics/jama/">JAMA</a>. 
  /// </para></summary>
  /// <remarks><para>For additional documentation, see
  /// <a href="http://algs4.cs.princeton.edu/99scientific">Section 9.9</a>
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/GaussJordanElimination.java.html">GaussJordanElimination</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class GaussJordanElimination
  {
    private const double EPSILON = 1e-8;

    private readonly int N;     // N-by-N system
    private double[][] a;       // N-by-N+1 augmented matrix

    // Gauss-Jordan elimination with partial pivoting
    /// <summary>
    /// Solves the linear system of equations <c>Ax</c> = <c>B</c>
    /// where <c>A</c> is an <c>N</c>-by-<c>N</c> matrix and <c>B</c>
    /// is a length <c>N</c> vector.</summary>
    /// <param name="A">the <c>N</c>-by-<c>N</c> constraint matrix</param>
    /// <param name="b">the length <c>N</c> right-hand-side vector</param>
    ///
    public GaussJordanElimination(double[][] A, double[] b)
    {
      N = b.Length;

      // build augmented matrix
      a = new double[N][];
      for (int i = 0; i < N; i++) {
        a[i] = new double[N + N + 1];
        for (int j = 0; j < N; j++)
          a[i][j] = A[i][j];
      }
      // only needed if you want to find certificate of infeasibility (or compute inverse)
      for (int i = 0; i < N; i++)
        a[i][N + i] = 1.0;

      for (int i = 0; i < N; i++)
        a[i][N + N] = b[i];

      solve();

      Debug.Assert(certifySolution(A, b));
    }

    private void solve()
    {

      // Gauss-Jordan elimination
      for (int p = 0; p < N; p++)
      {
        // show();

        // find pivot row using partial pivoting
        int max = p;
        for (int i = p + 1; i < N; i++)
        {
          if (Math.Abs(a[i][p]) > Math.Abs(a[max][p]))
          {
            max = i;
          }
        }

        // exchange row p with row max
        swap(p, max);

        // singular or nearly singular
        if (Math.Abs(a[p][p]) <= EPSILON)
        {
          continue;
          // throw new ArithmeticException("Matrix is singular or nearly singular");
        }

        // pivot
        pivot(p, p);
      }
      // show();
    }

    // swap row1 and row2
    private void swap(int row1, int row2)
    {
      double[] temp = a[row1];
      a[row1] = a[row2];
      a[row2] = temp;
    }


    // pivot on entry (p, q) using Gauss-Jordan elimination
    private void pivot(int p, int q)
    {

      // everything but row p and column q
      for (int i = 0; i < N; i++)
      {
        double alpha = a[i][q] / a[p][q];
        for (int j = 0; j <= N + N; j++)
        {
          if (i != p && j != q) a[i][j] -= alpha * a[p][j];
        }
      }

      // zero out column q
      for (int i = 0; i < N; i++)
        if (i != p) a[i][q] = 0.0;

      // scale row p (ok to go from q+1 to N, but do this for consistency with simplex pivot)
      for (int j = 0; j <= N + N; j++)
        if (j != q) a[p][j] /= a[p][q];
      a[p][q] = 1.0;
    }

    /// <summary>
    /// Returns a solution to the linear system of equations <c>Ax</c> = <c>B</c>.</summary>
    /// <returns>a solution <c>X</c> to the linear system of equations
    ///        <c>Ax</c> = <c>B</c>; <c>null</c> if no such solution</returns>
    ///
    public double[] Primal()
    {
      double[] x = new double[N];
      for (int i = 0; i < N; i++)
      {
        if (Math.Abs(a[i][i]) > EPSILON)
          x[i] = a[i][N + N] / a[i][i];
        else if (Math.Abs(a[i][N + N]) > EPSILON)
          return null;
      }
      return x;
    }

    /// <summary>
    /// Returns a solution to the linear system of equations <c>YA</c> = 0,
    /// <c>Yb</c> != 0.</summary>
    /// <returns>a solution <c>Y</c> to the linear system of equations
    /// <c>YA</c> = 0, <c>Yb</c> != 0; <c>null</c> if no such solution</returns>
    ///
    public double[] Dual()
    {
      double[] y = new double[N];
      for (int i = 0; i < N; i++)
      {
        if ((Math.Abs(a[i][i]) <= EPSILON) && (Math.Abs(a[i][N + N]) > EPSILON))
        {
          for (int j = 0; j < N; j++)
            y[j] = a[i][N + j];
          return y;
        }
      }
      return null;
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

    // print the tableaux
    private void show()
    {
      for (int i = 0; i < N; i++)
      {
        for (int j = 0; j < N; j++)
        {
          Console.Write("{0,8:F3} ", a[i][j]);
        }
        Console.Write("| ");
        for (int j = N; j < N + N; j++)
        {
          Console.Write("{0,8:F3} ", a[i][j]);
        }
        Console.Write("| {0,8:F3}\n", a[i][N + N]);
      }
      Console.WriteLine();
    }


    // check that Ax = b or yA = 0, yb != 0
    private bool certifySolution(double[][] A, double[] b)
    {

      // check that Ax = b
      if (IsFeasible)
      {
        double[] x = Primal();
        for (int i = 0; i < N; i++)
        {
          double sum = 0.0;
          for (int j = 0; j < N; j++)
          {
            sum += A[i][j] * x[j];
          }
          if (Math.Abs(sum - b[i]) > EPSILON)
          {
            Console.WriteLine("not feasible");
            Console.Write("b[{0}] = {1,8:F3}, sum = {2,8:F3}\n", i, b[i], sum);
            return false;
          }
        }
        return true;
      }

      // or that yA = 0, yb != 0
      else
      {
        double[] y = Dual();
        for (int j = 0; j < N; j++)
        {
          double sum = 0.0;
          for (int i = 0; i < N; i++)
          {
            sum += A[i][j] * y[i];
          }
          if (Math.Abs(sum) > EPSILON)
          {
            Console.WriteLine("invalid certificate of infeasibility");
            Console.Write("sum = {0,8:F3}\n", sum);
            return false;
          }
        }
        double sum1 = 0.0;
        for (int i = 0; i < N; i++)
        {
          sum1 += y[i] * b[i];
        }
        if (Math.Abs(sum1) < EPSILON)
        {
          Console.WriteLine("invalid certificate of infeasibility");
          Console.Write("yb  = {0,8:F3}\n", sum1);
          return false;
        }
        return true;
      }
    }

    internal static void test(string name, double[][] A, double[] b)
    {
      Console.WriteLine("----------------------------------------------------");
      Console.WriteLine(name);
      Console.WriteLine("----------------------------------------------------");
      GaussJordanElimination gaussian = new GaussJordanElimination(A, b);
      if (gaussian.IsFeasible)
      {
        Console.WriteLine("Solution to Ax = b");
        double[] x = gaussian.Primal();
        for (int i = 0; i < x.Length; i++)
        {
          Console.Write("{0,10:F6}\n", x[i]);
        }
      }
      else
      {
        Console.WriteLine("Certificate of infeasibility");
        double[] y = gaussian.Dual();
        for (int j = 0; j < y.Length; j++)
        {
          Console.Write("{0,10:F6}\n", y[j]);
        }
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
      test("test 1", A, b);
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
      test("test 2", A, b);
    }

    // 5-by-5 singular: no solutions
    // y = [ -1, 0, 1, 1, 0 ]
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
      test("test 3", A, b);
    }

    // 5-by-5 singluar: infinitely many solutions
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
      test("test 4", A, b);
    }

    // 3-by-3 singular: no solutions
    // y = [ 1, 0, 1/3 ]
    internal static void test5()
    {
      double[][] A = {
        new double[]{  2, -1,  1 },
        new double[]{  3,  2, -4 },
        new double[]{ -6,  3, -3 },
      };
      double[] b = { 1, 4, 2 };
      test("test 5", A, b);
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
      test("test 6 (infinitely many solutions)", A, b);
    }

    /// <summary>
    /// Demo test the <c>GaussJordanElimination</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd GaussJordanElimination")]
    public static void MainTest(string[] args)
    {

      GaussJordanElimination.test1();
      GaussJordanElimination.test2();
      GaussJordanElimination.test3();
      GaussJordanElimination.test4();
      GaussJordanElimination.test5();
      GaussJordanElimination.test6();

      // N-by-N random system (likely full rank)
      int N = int.Parse(args[0]);

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
      GaussJordanElimination.test("random " + N + "-by-" + N + " (likely full rank)", A, b);

      // N-by-N random system (likely infeasible)
      A = new double[N][];
      for (int i = 0; i < N - 1; i++)
      {
        A[i] = new double[N];
        for (int j = 0; j < N; j++)
        {
          A[i][j] = StdRandom.Uniform(1000);
        }
      }
      A[N - 1] = new double[N];
      for (int i = 0; i < N - 1; i++)
      {
        double alpha = StdRandom.Uniform(11) - 5.0;
        for (int j = 0; j < N; j++)
        {
          A[N - 1][j] += alpha * A[i][j];
        }
      }
      b = new double[N];
      for (int i = 0; i < N; i++)
        b[i] = StdRandom.Uniform(1000);
      GaussJordanElimination.test("random " + N + "-by-" + N + " (likely infeasible)", A, b);

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
