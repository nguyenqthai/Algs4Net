/******************************************************************************
 *  File name :    Heap.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/24pq/tiny.txt
 *                http://algs4.cs.princeton.edu/24pq/words3.txt
 *
 *  Sorts a sequence of strings from standard input using heapsort.
 *
 *  C:\> type tiny.txt
 *  S O R T E X A M P L E
 *
 *  C:\> algscmd Heap < tiny.txt
 *  A E E L M O P R S T X                 [ one string per line ]
 *
 *  C:\> type words3.txt
 *  bed bug dad yes zoo ... all bad yet
 *
 *  C:\> algscmd Heap < words3.txt
 *  all bad bed bug dad ... yes yet zoo   [ one string per line ]
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Heap</c> class provides a static methods for heapsorting an array.</summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/24pq">Section 2.4</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Heap.java.html">Heap</a> implementation by
  /// Robert Sedgewick, and Kevin Wayne.
  /// </para></remarks>
  ///
  public class Heap
  {
    // This class should not be instantiated.
    private Heap() { }

    /// <summary>
    /// Rearranges the array in ascending order, using the natural order.</summary>
    /// <param name="pq">pq the array to be sorted</param>
    ///
    public static void Sort(IComparable[] pq)
    {
      int N = pq.Length;
      for (int k = N / 2; k >= 1; k--)
        sink(pq, k, N);
      while (N > 1)
      {
        exch(pq, 1, N--);
        sink(pq, 1, N);
      }
    }

    /***************************************************************************
     * Helper functions to restore the heap invariant.
     ***************************************************************************/

    private static void sink(IComparable[] pq, int k, int N)
    {
      while (2 * k <= N)
      {
        int j = 2 * k;
        if (j < N && less(pq, j, j + 1)) j++;
        if (!less(pq, k, j)) break;
        exch(pq, k, j);
        k = j;
      }
    }

    /***************************************************************************
     * Helper functions for comparisons and swaps.
     * Indices are "off-by-one" to support 1-based indexing.
     ***************************************************************************/
    private static bool less(IComparable[] pq, int i, int j)
    {
      return pq[i - 1].CompareTo(pq[j - 1]) < 0;
    }

    private static void exch(Object[] pq, int i, int j)
    {
      Object swap = pq[i - 1];
      pq[i - 1] = pq[j - 1];
      pq[j - 1] = swap;
    }

    // is v < w ?
    private static bool less(IComparable v, IComparable w)
    {
      return v.CompareTo(w) < 0;
    }


    /***************************************************************************
     *  Check if array is sorted - useful for debugging.
     ***************************************************************************/
    private static bool isSorted(IComparable[] a)
    {
      for (int i = 1; i < a.Length; i++)
        if (less(a[i], a[i - 1])) return false;
      return true;
    }

    /// <summary>
    /// Reads in a sequence of strings from standard input; heapsorts them;
    /// and prints them to standard output in ascending order.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Heap < words3.txt", "Input strings to be printed in sorted order")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();
      Heap.Sort(a);
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

