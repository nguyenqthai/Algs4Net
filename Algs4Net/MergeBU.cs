/******************************************************************************
 *  File name :    MergeBU.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/22mergesort/tiny.txt
 *                http://algs4.cs.princeton.edu/22mergesort/words3.txt
 *   
 *  Sorts a sequence of strings from standard input using
 *  bottom-up mergesort.
 *   
 *  C:\> type tiny.txt
 *  S O R T E X A M P L E
 *
 *  C:\> algscmd MergeBU < tiny.txt
 *  A E E L M O P R S T X                 [ one string per line ]
 *    
 *  C:\> type words3.txt
 *  bed bug dad yes zoo ... all bad yet
 *  
 *  C:\> algscmd MergeBU < words3.txt
 *  all bad bed bug dad ... yes yet zoo    [ one string per line ]
 *
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary>
  /// The <c>MergeBU</c> class provides static methods for sorting an
  /// array using bottom-up mergesort.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/MergeBU.java.html">MergeBU</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class MergeBU
  {
    // This class should not be instantiated.
    private MergeBU() { }

    // stably merge a[lo..mid] with a[mid+1..hi] using aux[lo..hi]
    private static void merge(IComparable[] a, IComparable[] aux, int lo, int mid, int hi)
    {
      // copy to aux[]
      for (int k = lo; k <= hi; k++)
      {
        aux[k] = a[k];
      }

      // merge back to a[]
      int i = lo, j = mid + 1;
      for (int k = lo; k <= hi; k++)
      {
        if (i > mid) a[k] = aux[j++];  // this copying is unneccessary
        else if (j > hi) a[k] = aux[i++];
        else if (OrderHelper.Less(aux[j], aux[i])) a[k] = aux[j++];
        else a[k] = aux[i++];
      }

    }

    /// <summary>
    /// Rearranges the array in ascending order, using the natural order.</summary>
    /// <param name="a">the array to be sorted</param>
    ///
    public static void Sort(IComparable[] a)
    {
      int N = a.Length;
      IComparable[] aux = new IComparable[N];
      for (int n = 1; n < N; n = n + n)
      {
        for (int i = 0; i < N - n; i += n + n)
        {
          int lo = i;
          int m = i + n - 1;
          int hi = Math.Min(i + n + n - 1, N - 1);
          merge(a, aux, lo, m, hi);
        }
      }
      Debug.Assert(OrderHelper.IsSorted(a));
    }

    /// <summary>
    /// Reads in a sequence of strings from standard input; mergesorts them;
    /// and prints them to standard output in ascending order.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd MergeBU < words3.txt", "Input strings to be printed in sorted order")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();

      MergeBU.Sort(a);
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
