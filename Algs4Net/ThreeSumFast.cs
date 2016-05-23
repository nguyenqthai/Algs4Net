/******************************************************************************
 *  File name :    ThreeSumFast.cs
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
 *  A program with N^2 log N running time. Read in N integers
 *  and counts the number of triples that sum to exactly 0.
 *
 *  Limitations
 *  -----------
 *     - we ignore integer overflow
 *     - doesn't handle case when input has duplicates
 *
 *
 *  C:\> algscmd ThreeSumFast 1Kints.txt
 *  70
 *
 *  C:\> algscmd ThreeSumFast 2Kints.txt
 *  528
 *
 *  C:\> algscmd ThreeSumFast 4Kints.txt
 *  4039
 *
 *  C:\> algscmd ThreeSumFast 8Kints.txt
 *  32074
 *
 *  C:\> algscmd ThreeSumFast 16Kints.txt
 *  255181
 *
 *  C:\> algscmd ThreeSumFast 32Kints.txt
 *  2052358
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>ThreeSumFast</c> class provides static methods for counting
  /// and printing the number of triples in an array of distinct integers that
  /// sum to 0 (ignoring integer overflow).
  /// </para><para>
  /// This implementation uses sorting and binary search and takes time
  /// proportional to N^2 log N, where N is the number of integers.
  /// </para></summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/14analysis">Section 1.4</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/ThreeSumFast.java.html">ThreeSumFast</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks>
  ///
  public class ThreeSumFast
  {

    // Do not instantiate.
    private ThreeSumFast() { }

    // returns true if the sorted array a[] contains any duplicated integers
    private static bool containsDuplicates(int[] a)
    {
      for (int i = 1; i < a.Length; i++)
        if (a[i] == a[i - 1]) return true;
      return false;
    }

    /// <summary>
    /// Prints to standard output the (i, j, k) with i &lt; j &lt; k such that a[i] + a[j] + a[k] == 0.</summary>
    /// <param name="a">a the array of integers</param>
    /// <exception cref="ArgumentException">if the array contains duplicate integers</exception>
    ///
    public static void PrintAll(int[] a)
    {
      int N = a.Length;
      Array.Sort(a);
      if (containsDuplicates(a)) throw new ArgumentException("array contains duplicate integers");
      for (int i = 0; i < N; i++)
      {
        for (int j = i + 1; j < N; j++)
        {
          int k = Array.BinarySearch(a, -(a[i] + a[j]));
          if (k > j)
            Console.WriteLine(a[i] + " " + a[j] + " " + a[k]);
        }
      }
    }

    /// <summary>
    /// Returns the number of triples (i, j, k) with i &lt; j &lt; k such that a[i] + a[j] + a[k] == 0.</summary>
    /// <param name="a">a the array of integers</param>
    /// <returns>the number of triples (i, j, k) with i &lt; j &lt; k such that a[i] + a[j] + a[k] == 0</returns>
    ///
    public static int Count(int[] a)
    {
      int N = a.Length;
      Array.Sort(a);
      if (containsDuplicates(a)) throw new ArgumentException("array contains duplicate integers");
      int cnt = 0;
      for (int i = 0; i < N; i++)
      {
        for (int j = i + 1; j < N; j++)
        {
          int k = Array.BinarySearch(a, -(a[i] + a[j]));
          if (k > j) cnt++;
        }
      }
      return cnt;
    }

    /// <summary>
    /// Reads in a sequence of distinct integers from a file, specified as a command-line argument;
    /// counts the number of triples sum to exactly zero; prints out the time to perform
    /// the computation.</summary>
    /// <param name="args">Place holder for user arguments</param>
    ///
    [HelpText("algscmd ThreeSumFast 1Kints.txt",
      "Sequence of integers from a file, of which duplicates will be removed online")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput(args[0]);
      int[] a = StdIn.ReadAllInts();
      a = OrderHelper.RemoveDuplicates(a);
      int cnt = ThreeSumFast.Count(a);
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
