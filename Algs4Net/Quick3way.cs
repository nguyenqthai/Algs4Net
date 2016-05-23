/******************************************************************************
 *  File name :    Quick3way.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/23quicksort/tiny.txt
 *                http://algs4.cs.princeton.edu/23quicksort/words3.txt
 *   
 *  Sorts a sequence of strings from standard input using 3-way quicksort.
 *   
 *  C:\> type tiny.txt
 *  S O R T E X A M P L E
 *
 *  C:\> algscmd Quick3way < tiny.txt
 *  A E E L M O P R S T X                 [ one string per line ]
 *    
 *  C:\> type words3.txt
 *  bed bug dad yes zoo ... all bad yet
 *  
 *  C:\> algscmd Quick3way < words3.txt
 *  all bad bed bug dad ... yes yet zoo    [ one string per line ]
 *
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Quick3way</c> class provides static methods for sorting an
  /// array using quicksort with 3-way partitioning.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Quick3way.java.html">Quick3way</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Quick3way
  {

    // This class should not be instantiated.
    private Quick3way() { }

    /// <summary>
    /// Rearranges the array in ascending order, using the natural order.</summary>
    /// <param name="a">the array to be sorted</param>
    ///
    public static void Sort(IComparable[] a)
    {
      StdRandom.Shuffle(a);
      Sort(a, 0, a.Length - 1);
      Debug.Assert(OrderHelper.IsSorted(a));
    }

    // quicksort the subarray a[lo .. hi] using 3-way partitioning
    private static void Sort(IComparable[] a, int lo, int hi)
    {
      if (hi <= lo) return;
      int lt = lo, gt = hi;
      IComparable v = a[lo];
      int i = lo;
      while (i <= gt)
      {
        int cmp = a[i].CompareTo(v);
        if (cmp < 0) OrderHelper.Exch(a, lt++, i++);
        else if (cmp > 0) OrderHelper.Exch(a, i, gt--);
        else i++;
      }

      // a[lo..lt-1] < v = a[lt..gt] < a[gt+1..hi].
      Sort(a, lo, lt - 1);
      Sort(a, gt + 1, hi);
      Debug.Assert(OrderHelper.IsSorted(a, lo, hi));
    }

    /// <summary>
    /// Reads in a sequence of strings from standard input; 3-way
    /// quicksorts them; and prints them to standard output in ascending order. </summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Quick3way < words3.txt", "Input strings to be printed in sorted order")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();
      Quick3way.Sort(a);
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
