/******************************************************************************
 *  File name :    Accumulator.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Mutable data type that calculates the mean, sample standard
 *  deviation, and sample variance of a stream of real numbers
 *  use a stable, one-pass algorithm.
 *  
 *  C:\> algscmd Accumulator < tinyW.txt
 *  N      = 16
 *  mean   = 42.87500
 *  stddev = 28.25804
 *  var    = 798.51667
 *  
 *  C:\> algscmd Accumulator
 *  3 3 2 5 6 8
 *  88
 *  ^Z
 *  N      = 7
 *  mean   = 16.42857
 *  stddev = 31.62729
 *  var    = 1000.28571
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>Accumulator</c> class is a data type for computing the running
  /// mean, sample standard deviation, and sample variance of a stream of real
  /// numbers. It provides an example of a mutable data type and a streaming
  /// algorithm.</para><para>
  /// This implementation uses a one-pass algorithm that is less susceptible
  /// to floating-point roundoff error than the more straightforward
  /// implementation based on saving the sum of the squares of the numbers.
  /// This technique is due to
  /// <a href = "https://en.wikipedia.org/wiki/Algorithms_for_calculating_variance#Online_algorithm">B. P. Welford</a>.
  /// Each operation takes constant time in the worst case.
  /// The amount of memory is constant - the data values are not stored.
  /// </para></summary>
  /// <remarks><para>For additional documentation, 
  /// see <a href="http://algs4.cs.princeton.edu/12oop">Section 1.2</a> of 
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Accumulator.java.html">Accumulator</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Accumulator
  {
    private int n = 0;          // number of data values
    private double sum = 0.0;   // sample variance/// (n-1)
    private double mu = 0.0;    // sample mean

    /// <summary>Initializes an accumulator.</summary>
    ///
    public Accumulator()
    {
    }

    /// <summary>
    /// Adds the specified data value to the accumulator.</summary>
    /// <param name="x">the data value</param>
    ///
    public void AddDataValue(double x)
    {
      n++;
      double delta = x - mu;
      mu += delta / n;
      sum += (double)(n - 1) / n * delta * delta;
    }

    /// <summary>
    /// Returns the mean of the data values.</summary>
    /// <returns>the mean of the data values</returns>
    ///
    public double Mean()
    {
      return mu;
    }

    /// <summary>
    /// Returns the sample variance of the data values.</summary>
    /// <returns>the sample variance of the data values</returns>
    ///
    public double Var()
    {
      return sum / (n - 1);
    }

    /// <summary>
    /// Returns the sample standard deviation of the data values.</summary>
    /// <returns>the sample standard deviation of the data values</returns>
    ///
    public double Stddev()
    {
      return Math.Sqrt(Var());
    }

    /// <summary>
    /// Returns the number of data values.</summary>
    /// <returns>the number of data values</returns>
    ///
    public int Count()
    {
      return n;
    }

    /// <summary>
    /// Demo test the <c>Accumulator</c> data type.
    /// Reads in a stream of real number from standard input;
    /// adds them to the accumulator; and prints the mean,
    /// sample standard deviation, and sample variance to standard
    /// output.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText(@"algscmd Accumulator < tinyW.txt", "Numbers separated by space or new line")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      Accumulator stats = new Accumulator();
      while (!StdIn.IsEmpty)
      {
        double x = StdIn.ReadDouble();
        stats.AddDataValue(x);
      }

      Console.Write("N      = {0}\n", stats.Count());
      Console.Write("mean   = {0:F5}\n", stats.Mean());
      Console.Write("stddev = {0:F5}\n", stats.Stddev());
      Console.Write("var    = {0:F5}\n", stats.Var());
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
