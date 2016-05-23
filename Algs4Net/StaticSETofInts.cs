/******************************************************************************
 *  File name :    StaticSETofInts.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Data type to store a set of integers.
 *  
 *  C:\> algscmd StaticSETofInts
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>StaticSETofInts</c> class represents a set of integers.
  /// It supports searching for a given integer is in the set. It accomplishes
  /// this by keeping the set of integers in a sorted array and using
  /// binary search to find the given integer.</para>
  /// <para>The <c>Rank</c> and <c>Contains</c> operations take
  /// logarithmic time in the worst case.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/12oop">Section 1.2</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/StaticSETofInts.java.html">StaticSETofInts</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class StaticSETofInts
  {
    private int[] a;

    /// <summary>Initializes a set of integers specified by the integer array.</summary>
    /// <param name="keys">the array of integers</param>
    /// <exception cref="ArgumentException">if the array contains duplicate integers</exception>
    ///
    public StaticSETofInts(int[] keys)
    {
      // defensive copy
      a = new int[keys.Length];
      keys.CopyTo(a, 0);

      // sort the integers
      Array.Sort(a);
      // check for duplicates
      for (int i = 1; i < a.Length; i++)
        if (a[i] == a[i - 1])
          throw new ArgumentException("Argument arrays contains duplicate keys.");
    }

    /// <summary>
    /// Is the key in this set of integers?</summary>
    /// <param name="key">the search key</param>
    /// <returns>true if the set of integers contains the key; false otherwise</returns>
    ///
    public bool Contains(int key)
    {
      return Rank(key) != -1;
    }

    /// <summary>
    /// Returns either the index of the search key in the sorted array
    /// (if the key is in the set) or -1 (if the key is not in the set).</summary>
    /// <param name="key">the search key</param>
    /// <returns>the number of keys in this set less than the key (if the key is in the set)
    /// or -1 (if the key is not in the set).</returns>
    ///
    public int Rank(int key)
    {
      int lo = 0;
      int hi = a.Length - 1;
      while (lo <= hi)
      {
        // Key is in a[lo..hi] or not present.
        int mid = lo + (hi - lo) / 2;
        if (key < a[mid]) hi = mid - 1;
        else if (key > a[mid]) lo = mid + 1;
        else return mid;
      }
      return -1;
    }

    /// <summary>
    /// Demo test for the <c>StaticSETofInts</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    ///
    [HelpText("algscmd StaticSETofInts")]
    public static void MainTest(string[] args)
    {
      int[] a = { 33, 22, 11, 31, 35, 27, 24, 25 };
      StaticSETofInts set = new StaticSETofInts(a);
      Console.WriteLine("Set contains:");
      OrderHelper.Show(a);
      Console.WriteLine("Set contains {0}: {1}", 11, set.Contains(11));
      Console.WriteLine("Rank of {0}: {1} with same rank: {2}", 35, set.Rank(35), (a.Length - 1) == set.Rank(35));
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
