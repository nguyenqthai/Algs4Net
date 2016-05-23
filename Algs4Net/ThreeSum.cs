/******************************************************************************
 *  File name :    ThreeSum.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/14analysis/1Kints.txt
 *                http://algs4.cs.princeton.edu/14analysis/2Kints.txt
 *                http://algs4.cs.princeton.edu/14analysis/4Kints.txt
 *                http://algs4.cs.princeton.edu/14analysis/8Kints.txt
 *                http://algs4.cs.princeton.edu/14analysis/16Kints.txt
 *                http://algs4.cs.princeton.edu/14analysis/32Kints.txt
 *                http://algs4.cs.princeton.edu/14analysis/1Mints.txt
 *
 *  A program with cubic running time. Read in N integers
 *  and counts the number of triples that sum to exactly 0
 *  (ignoring integer overflow).
 *
 *  C:\> algscmd ThreeSum 1Kints.txt
 *  Elapsed time = 0.9647951
 *  70
 *  
 *  C:\> algscmd ThreeSum 2Kints.txt
 *  Elapsed time = 7.6489965
 *  528
 *  
 *  C:\> algscmd ThreeSum 4Kints.txt
 *  Elapsed time = 62.471492
 *  4039
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>ThreeSum</c> class provides static methods for counting
  /// and printing the number of triples in an array of integers that sum to 0
  /// (ignoring integer overflow).</para><para>
  /// This implementation uses a triply nested loop and takes proportional to N^3,
  /// where N is the number of integers.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/14analysis">Section 1.4</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/ThreeSum.java.html">ThreeSum</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class ThreeSum
  {
    // Do not instantiate.
    private ThreeSum() { }

    /// <summary>
    /// Prints to standard output the (i, j, k) with i &lt; j &lt; k such that 
    /// a[i] + a[j] + a[k] == 0.</summary>
    /// <param name="a">the array of integers</param>
    ///
    public static void PrintAll(int[] a)
    {
      int N = a.Length;
      for (int i = 0; i < N; i++)
      {
        for (int j = i + 1; j < N; j++)
        {
          for (int k = j + 1; k < N; k++)
          {
            if (a[i] + a[j] + a[k] == 0)
            {
              Console.WriteLine(a[i] + " " + a[j] + " " + a[k]);
            }
          }
        }
      }
    }

    /// <summary>
    /// Returns the number of triples (i, j, k) with i &lt; j &lt; k such that a[i] + a[j] + a[k] == 0.</summary>
    /// <param name="a">the array of integers</param>
    /// <returns>the number of triples (i, j, k) with i &lt; j &lt; k such that a[i] + a[j] + a[k] == 0</returns>
    ///
    public static int Count(int[] a)
    {
      int N = a.Length;
      int cnt = 0;
      for (int i = 0; i < N; i++)
      {
        for (int j = i + 1; j < N; j++)
        {
          for (int k = j + 1; k < N; k++)
          {
            if (a[i] + a[j] + a[k] == 0)
            {
              cnt++;
            }
          }
        }
      }
      return cnt;
    }
    /// <summary>
    /// Reads in a sequence of integers from a file, specified as a command-line argument;
    /// counts the number of triples sum to exactly zero; prints out the time to perform
    /// the computation.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd ThreeSum 1Kints.txt", "Sequence of integers from a file")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput(args[0]);
      int[] a = StdIn.ReadAllInts();

      Stopwatch timer = new Stopwatch();
      int cnt = ThreeSum.Count(a);
      Console.WriteLine("Elapsed time = " + timer.ElapsedTime());
      Console.WriteLine(cnt);
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
