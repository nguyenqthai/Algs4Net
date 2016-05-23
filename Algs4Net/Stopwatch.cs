/******************************************************************************
 *  File name :    Stopwatch.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A utility class to measure the running time (wall clock) of a program.
 *
 *  C:\> algscmd Stopwatch 100000000
 *  6.666667e+011 (0.99 seconds)
 *  6.666667e+011 (5.55 seconds)
 *
 ******************************************************************************/
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Stopwatch</c> data type is for measuring the time that elapses 
  /// between the start and end of a programming task (wall-clock time). 
  /// For an alternative, see <see cref="StopwatchWin32"/>.
  /// </summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/14analysis">Section 1.4</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Stopwatch.java.html">Stopwatch</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Stopwatch
  {
    private readonly DateTime start;

    /// <summary>
    /// Initializes a new stopwatch.</summary>
    ///
    public Stopwatch()
    {
      Thread.Sleep(0);
      start = DateTime.Now;
    }

    /// <summary>
    /// Returns the elapsed CPU time (in seconds) since the stopwatch was created.</summary>
    /// <returns>elapsed CPU time (in seconds) since the stopwatch was created</returns>
    ///
    public double ElapsedTime()
    {
      DateTime now = DateTime.Now;
      return now.Subtract(start).TotalSeconds;
    }

    /// <summary>
    /// Demo test the <c>Stopwatch</c> data type.
    /// Takes a command-line argument <c>n</c> and computes the 
    /// sum of the square roots of the first <c>n</c> positive integers,
    /// first using <c>Math.Sqrt()</c>, then using <c>Math.Pow()</c>.
    /// It prints to standard output the sum and the amount of time to
    /// compute the sum. Note that the discrete sum can be approximated by
    /// an integral - the sum should be approximately 2/3 power (n^(3/2) - 1).</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Stopwatch 100000000")]
    public static void MainTest(string[] args)
    {
      int n = int.Parse(args[0]);

      // sum of square roots of integers from 1 to n using Math.sqrt(x).
      Stopwatch timer1 = new Stopwatch();
      double sum1 = 0.0;
      for (int i = 1; i <= n; i++)
      {
        sum1 += Math.Sqrt(i);
      }
      double time1 = timer1.ElapsedTime();
      Console.Write("{0:e} ({1:F2} seconds)\n", sum1, time1);

      // sum of square roots of integers from 1 to n using Math.pow(x, 0.5).
      Stopwatch timer2 = new Stopwatch();
      double sum2 = 0.0;
      for (int i = 1; i <= n; i++)
      {
        sum2 += Math.Pow(i, 0.5);
      }
      double time2 = timer2.ElapsedTime();
      Console.Write("{0:e} ({1:F2} seconds)\n", sum2, time2);
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
