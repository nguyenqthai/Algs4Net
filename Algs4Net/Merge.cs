/******************************************************************************
 *  File name :    Merge.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/22mergesort/tiny.txt
 *                http://algs4.cs.princeton.edu/22mergesort/words3.txt
 *
 *  Sorts a sequence of strings from standard input using mergesort.
 *
 *  C:\> type tiny.txt
 *  S O R T E X A M P L E
 *
 *  C:\> algscmd Merge < tiny.txt
 *  A E E L M O P R S T X                 [ one string per line ]
 *
 *  C:\> type words3.txt
 *  bed bug dad yes zoo ... all bad yet
 *
 *  C:\> algscmd Merge < words3.txt
 *  all bad bed bug dad ... yes yet zoo    [ one string per line ]
 *
 ******************************************************************************/
using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Merge</c> class provides static methods for sorting an
  /// array using mergesort. For an optimized version, try MergeX.</summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/22mergesort">Section 2.2</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Merge.java.html">Merge</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks>
  ///
  public class Merge
  {

    // This class should not be instantiated.
    private Merge() { }

    // stably merge a[lo .. mid] with a[mid+1 ..hi] using aux[lo .. hi]
    private static void merge(IComparable[] a, IComparable[] aux, int lo, int mid, int hi)
    {
      // precondition: a[lo .. mid] and a[mid+1 .. hi] are sorted subarrays
      Debug.Assert(OrderHelper.IsSorted(a, lo, mid));
      Debug.Assert(OrderHelper.IsSorted(a, mid + 1, hi));

      // copy to aux[]
      for (int k = lo; k <= hi; k++)
      {
        aux[k] = a[k];
      }

      // merge back to a[]
      int i = lo, j = mid + 1;
      for (int k = lo; k <= hi; k++)
      {
        if (i > mid) a[k] = aux[j++];
        else if (j > hi) a[k] = aux[i++];
        else if (OrderHelper.Less(aux[j], aux[i])) a[k] = aux[j++];
        else a[k] = aux[i++];
      }

      // postcondition: a[lo .. hi] is sorted
      Debug.Assert(OrderHelper.IsSorted(a, lo, hi));
    }

    // mergesort a[lo..hi] using auxiliary array aux[lo..hi]
    private static void sort(IComparable[] a, IComparable[] aux, int lo, int hi)
    {
      if (hi <= lo) return;
      int mid = lo + (hi - lo) / 2;
      sort(a, aux, lo, mid);
      sort(a, aux, mid + 1, hi);
      merge(a, aux, lo, mid, hi);
    }

    /// <summary>
    /// Rearranges the array in ascending order, using the natural order.</summary>
    /// <param name="a">a the array to be sorted</param>
    ///
    public static void Sort(IComparable[] a)
    {
      IComparable[] aux = new IComparable[a.Length];
      sort(a, aux, 0, a.Length - 1);
      Debug.Assert(OrderHelper.IsSorted(a));
    }


    /***************************************************************************
     *  Index mergesort.
     ***************************************************************************/
    // stably merge a[lo .. mid] with a[mid+1 .. hi] using aux[lo .. hi]
    private static void merge(IComparable[] a, int[] index, int[] aux, int lo, int mid, int hi)
    {

      // copy to aux[]
      for (int k = lo; k <= hi; k++)
      {
        aux[k] = index[k];
      }

      // merge back to a[]
      int i = lo, j = mid + 1;
      for (int k = lo; k <= hi; k++)
      {
        if (i > mid) index[k] = aux[j++];
        else if (j > hi) index[k] = aux[i++];
        else if (OrderHelper.Less(a[aux[j]], a[aux[i]])) index[k] = aux[j++];
        else index[k] = aux[i++];
      }
    }

    /// <summary>
    /// Returns a permutation that gives the elements in the array in ascending order.</summary>
    /// <param name="a">a the array</param>
    /// <returns>a permutation <c>p[]</c> such that <c>a[p[0]]</c>, <c>a[p[1]]</c>,
    ///   ..., <c>a[p[N-1]]</c> are in ascending order</returns>
    ///
    public static int[] IndexSort(IComparable[] a)
    {
      int N = a.Length;
      int[] index = new int[N];
      for (int i = 0; i < N; i++)
        index[i] = i;

      int[] aux = new int[N];
      sort(a, index, aux, 0, N - 1);
      return index;
    }

    // mergesort a[lo..hi] using auxiliary array aux[lo..hi]
    private static void sort(IComparable[] a, int[] index, int[] aux, int lo, int hi)
    {
      if (hi <= lo) return;
      int mid = lo + (hi - lo) / 2;
      sort(a, index, aux, lo, mid);
      sort(a, index, aux, mid + 1, hi);
      merge(a, index, aux, lo, mid, hi);
    }

    /// <summary>
    /// Reads in a sequence of strings from standard input; mergesorts them;
    /// and prints them to standard output in ascending order.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Merge < words3.txt", "Input strings to be printed in sorted order")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();
      Merge.Sort(a);
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

