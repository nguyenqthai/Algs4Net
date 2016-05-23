/******************************************************************************
 *  File name :    StopwatchWin32.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A version of Stopwatch that uses Win32 performance counter.
 *
 *  C:\> algscmd StopwatchWin32 100000000
 *  6.666667e+11 (1.05 seconds)
 *  6.666667e+11 (7.50 seconds)
 *
 ******************************************************************************/

using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Algs4Net
{
  /// <summary>
  /// A version of Stopwatch that uses Win32 performance counter.
  /// For regular use, use the <see cref="Stopwatch"/> class. Since .NET dose not have
  /// a close equivalence of the Java ThreadMXBean class, we will not port the  
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/StopwatchCPU.java.html">StopwatchCPU</a>
  /// class. Instead, we use this class as a demonstration of an alternative Stopwatch implementation.</summary>
  /// <remarks>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/14analysis">Section 1.4</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</remarks>
  /// 
  public class StopwatchWin32
  {
    [DllImport("Kernel32.dll")]
    private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
    [DllImport("Kernel32.dll")]
    private static extern bool QueryPerformanceFrequency(out long lpFrequency);

    private long start, stop, freq;

    /// <summary>Initializes a new stopwatch.</summary>
    ///
    public StopwatchWin32()
    {
      if (QueryPerformanceFrequency(out freq) == false)
      {
        throw new Win32Exception("win32-performance counter not supported");
      }
      QueryPerformanceCounter(out start);
    }

    /// <summary>
    /// Returns the elapsed CPU time (in seconds) since the stopwatch was created.</summary>
    /// <returns>elapsed CPU time (in seconds) since the stopwatch was created</returns>
    ///
    public double ElapsedTime()
    {
      QueryPerformanceCounter(out stop);
      return (stop - start) / (double)freq;
    }

    /// <summary>
    /// Demo test for the <c></c> data type.
    /// </summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd StopwatchWin32 100000000")]
    public static void MainTest(string[] args)
    {
      int n = int.Parse(args[0]);

      // sum of square roots of integers from 1 to n using Math.sqrt(x).
      StopwatchWin32 timer1 = new StopwatchWin32();
      double sum1 = 0.0;
      for (int i = 1; i <= n; i++)
      {
        sum1 += Math.Sqrt(i);
      }
      double time1 = timer1.ElapsedTime();
      Console.Write("{0:e} ({1:F2} seconds)\n", sum1, time1);

      // sum of square roots of integers from 1 to n using Math.pow(x, 0.5).
      StopwatchWin32 timer2 = new StopwatchWin32();
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
