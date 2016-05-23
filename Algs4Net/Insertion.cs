/******************************************************************************
 *  File name :    Insertion.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/21sort/tiny.txt
 *                http://algs4.cs.princeton.edu/21sort/words3.txt
 *
 *  Sorts a sequence of strings from standard input using insertion sort.
 *
 *  C:\> type tiny.txt
 *  S O R T E X A M P L E
 *
 *  C:\> algscmd Insertion < tiny.txt
 *  A E E L M O P R S T X                 [ one string per line ]
 *
 *  C:\> type words3.txt
 *  bed bug dad yes zoo ... all bad yet
 *
 *  C:\> algscmd Insertion < words3.txt
 *  all bad bed bug dad ... yes yet zoo   [ one string per line ]
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>Insertion</c> class provides static methods for sorting an
  /// array using insertion sort.
  /// </para><para>
  /// This implementation makes ~ 1/2 N^2 compares and OrderHelper.Exchanges in
  /// the worst case, so it is not suitable for sorting large arbitrary arrays.
  /// More precisely, the number of OrderHelper.Exchanges is exactly equal to the number
  /// of inversions. So, for example, it sorts a partially-sorted array
  /// in linear time.
  /// </para><para>
  /// The sorting algorithm is stable and uses O(1) extra memory.
  /// </para>
  /// <para>
  /// See <a href="http://algs4.cs.princeton.edu/21elementary/InsertionPedantic.java.html">InsertionPedantic.java</a>
  /// for a version that eliminates the compiler warning.
  /// </para></summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Insertion.java.html">Insertion</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.
  /// </para></remarks>
  ///
  public class Insertion
  {

    // This class should not be instantiated.
    private Insertion() { }

    /// <summary>
    /// Rearranges the array in ascending order, using the natural order.</summary>
    /// <param name="a">a the array to be sorted</param>
    ///
    public static void Sort(IComparable[] a)
    {
      int N = a.Length;
      for (int i = 0; i < N; i++)
      {
        for (int j = i; j > 0 && OrderHelper.Less(a[j], a[j - 1]); j--)
        {
          OrderHelper.Exch(a, j, j - 1);
        }
        Debug.Assert(OrderHelper.IsSorted(a, 0, i));
      }
      Debug.Assert(OrderHelper.IsSorted(a));
    }

    /// <summary>
    /// Rearranges the subarray a[lo..hi] in ascending order, using the natural order.</summary>
    /// <param name="a">a the array to be sorted</param>
    /// <param name="lo">lo left endpoint</param>
    /// <param name="hi">hi right endpoint</param>
    ///
    public static void Sort(IComparable[] a, int lo, int hi)
    {
      for (int i = lo; i <= hi; i++)
      {
        for (int j = i; j > lo && OrderHelper.Less(a[j], a[j - 1]); j--)
        {
          OrderHelper.Exch(a, j, j - 1);
        }
      }
      Debug.Assert(OrderHelper.IsSorted(a, lo, hi));
    }

    /// <summary>
    /// Rearranges the array in ascending order, using a comparator.</summary>
    /// <param name="a">a the array</param>
    /// <param name="comparator">comparator the comparator specifying the order</param>
    ///
    public static void Sort(object[] a, System.Collections.Comparer comparator)
    {
      int N = a.Length;
      for (int i = 0; i < N; i++)
      {
        for (int j = i; j > 0 && OrderHelper.Less(a[j], a[j - 1], comparator); j--)
        {
          OrderHelper.Exch(a, j, j - 1);
        }
        Debug.Assert(OrderHelper.IsSorted(a, 0, i, comparator));
      }
      Debug.Assert(OrderHelper.IsSorted(a, comparator));
    }

    /// <summary>
    /// Rearranges the subarray a[lo..hi] in ascending order, using a generic comparator.</summary>
    /// <param name="a">a the array</param>
    /// <param name="lo">lo left endpoint</param>
    /// <param name="hi">hi right endpoint</param>
    /// <param name="comparator">comparator the comparator specifying the order</param>
    ///
    public static void Sort<T>(T[] a, int lo, int hi, Comparer<T> comparator)
    {
      for (int i = lo; i <= hi; i++)
      {
        for (int j = i; j > lo && OrderHelper.Less(a[j], a[j - 1], comparator); j--)
        {
          OrderHelper.Exch<T>(a, j, j - 1);
        }
      }
      Debug.Assert(OrderHelper.IsSorted(a, lo, hi, comparator));
    }

    /// <summary>
    /// Returns a permutation that gives the elements in the array in ascending order,
    /// while not changing the original array a[]</summary>
    /// <param name="a">a the array</param>
    /// <returns>a permutation <c>p[]</c> such that <c>a[p[0]]</c>, <c>a[p[1]]</c>,
    /// ..., <c>a[p[N-1]]</c> are in ascending order</returns>
    ///
    public static int[] IndexSort(int[] a)
    {
      int N = a.Length;
      int[] index = new int[N];
      for (int i = 0; i < N; i++)
        index[i] = i;

      for (int i = 0; i < N; i++)
        for (int j = i; j > 0 && OrderHelper.Less(a[index[j]], a[index[j - 1]]); j--)
          OrderHelper.Exch(index, j, j - 1);

      return index;
    }

    /// <summary>
    /// Reads in a sequence of strings from standard input; insertion sorts them;
    /// and prints them to standard output in ascending order.</summary>
    /// <param name="args">Place holder for user arguments</param>
    ///
    [HelpText("algscmd Insertion < words3.txt", "Input strings to be printed in sorted order")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();
      Insertion.Sort(a);
      OrderHelper.Show(a);
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
