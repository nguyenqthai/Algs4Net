/******************************************************************************
 *  File name :    DoublingTest.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  C:\> algscmd DoublingTest
 *     250     0.02
 *     500     0.12
 *    1000     0.96
 *    2000     7.74
 *    4000    63.05
 *  ...
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>DoublingTest</c> class provides a client for measuring
  /// the running time of a method using a doubling test.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/14analysis">Section 1.4</a>
  ///  of <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DoublingTest.java.html">DoublingTest</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DoublingTest
  {
    private const int MAXIMUM_INTEGER = 1000000;

    // This class should not be instantiated.
    private DoublingTest() { }

    /// <summary>Returns the amount of time to call <c>ThreeSum.count()</c>
    /// with <c>N</c> random 6-digit integers.</summary>
    /// <param name="N">N the number of integers</param>
    /// <returns>amount of time (in seconds) to call <c>ThreeSum.count()</c>
    ///  with <c>N</c> random 6-digit integers</returns>
    ///
    public static double TimeTrial(int N)
    {
      int[] a = new int[N];
      for (int i = 0; i < N; i++)
      {
        a[i] = StdRandom.Uniform(-MAXIMUM_INTEGER, MAXIMUM_INTEGER);
      }
      Stopwatch timer = new Stopwatch();
      ThreeSum.Count(a);
      return timer.ElapsedTime();
    }

    /// <summary>
    /// Prints table of running times to call <c>ThreeSum.Count()</c>
    /// for arrays of size 250, 500, 1000, 2000, and so forth.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DoublingTest")]
    public static void MainTest(string[] args)
    {
      for (int N = 250; true; N += N)
      {
        double time = DoublingTest.TimeTrial(N);
        Console.Write("{0,6} {1,8:F2}\n", N, time);
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
