/******************************************************************************
 *  File name :    QuickX.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Uses the Bentley-McIlroy 3-way partitioning scheme,
 *  chooses the partitioning element using Tukey's ninther,
 *  and cuts off to insertion sort.
 *
 *  Reference: Engineering a Sort Function by Jon L. Bentley
 *  and M. Douglas McIlroy. Softwae-Practice and Experience,
 *  Vol. 23 (11), 1249-1265 (November 1993).
 *  
 *  C:\> algscmd Quick < tiny.txt
 *  ...
 *  C:\> algscmd Quick3way < words3.txt
 *  ...
 *
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary>
  /// The <c>QuickX</c> class provides static methods for sorting an
  /// array using an optimized version of quicksort (using Bentley-McIlroy
  /// 3-way partitioning, Tukey's ninther, and cutoff to insertion sort).</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/QuickX.java.html">QuickX</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class QuickX
  {
    private const int CUTOFF = 8;  // cutoff to insertion sort, must be >= 1

    // This class should not be instantiated.
    private QuickX() { }

    /// <summary>
    /// Rearranges the array in ascending order, using the natural order.</summary>
    /// <param name="a">the array to be sorted</param>
    ///
    public static void Sort(IComparable[] a)
    {
      Sort(a, 0, a.Length - 1);
    }

    private static void Sort(IComparable[] a, int lo, int hi)
    {
      int N = hi - lo + 1;

      // cutoff to insertion sort
      if (N <= CUTOFF)
      {
        Insertion.Sort(a, lo, hi);
        return;
      }

      // use median-of-3 as partitioning element
      else if (N <= 40)
      {
        int m = median3(a, lo, lo + N / 2, hi);
        OrderHelper.Exch(a, m, lo);
      }

      // use Tukey ninther as partitioning element
      else
      {
        int eps = N / 8;
        int mid = lo + N / 2;
        int m1 = median3(a, lo, lo + eps, lo + eps + eps);
        int m2 = median3(a, mid - eps, mid, mid + eps);
        int m3 = median3(a, hi - eps - eps, hi - eps, hi);
        int ninther = median3(a, m1, m2, m3);
        OrderHelper.Exch(a, ninther, lo);
      }

      // Bentley-McIlroy 3-way partitioning
      int i = lo, j = hi + 1;
      int p = lo, q = hi + 1;
      IComparable v = a[lo];
      while (true)
      {
        while (OrderHelper.Less(a[++i], v))
          if (i == hi) break;
        while (OrderHelper.Less(v, a[--j]))
          if (j == lo) break;

        // pointers cross
        if (i == j && OrderHelper.Eq(a[i], v))
          OrderHelper.Exch(a, ++p, i);
        if (i >= j) break;

        OrderHelper.Exch(a, i, j);
        if (OrderHelper.Eq(a[i], v)) OrderHelper.Exch(a, ++p, i);
        if (OrderHelper.Eq(a[j], v)) OrderHelper.Exch(a, --q, j);
      }


      i = j + 1;
      for (int k = lo; k <= p; k++)
        OrderHelper.Exch(a, k, j--);
      for (int k = hi; k >= q; k--)
        OrderHelper.Exch(a, k, i++);

      Sort(a, lo, j);
      Sort(a, i, hi);
    }

    // return the index of the median element among a[i], a[j], and a[k]
    private static int median3(IComparable[] a, int i, int j, int k)
    {
      return (OrderHelper.Less(a[i], a[j]) ?
             (OrderHelper.Less(a[j], a[k]) ? j : OrderHelper.Less(a[i], a[k]) ? k : i) :
             (OrderHelper.Less(a[k], a[j]) ? j : OrderHelper.Less(a[k], a[i]) ? k : i));
    }

    /// <summary>
    /// Reads in a sOrderHelper.Equence of strings from standard input; quicksorts them
    /// (using an optimized version of quicksort); 
    /// and prints them to standard output in ascending order.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd QuickX < words3.txt", "Input strings to be printed in sorted order")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();
      QuickX.Sort(a);
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
