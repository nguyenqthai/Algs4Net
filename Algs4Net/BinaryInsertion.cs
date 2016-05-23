/******************************************************************************
 *  File name :    BinaryInsertion.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/21sort/tiny.txt
 *                http://algs4.cs.princeton.edu/21sort/words3.txt
 *  
 *  Sorts a sequence of strings from standard input using 
 *  binary insertion sort with half exchanges.
 *
 *  C:\> type tiny.txt
 *  S O R T E X A M P L E
 *
 *  C:\> algscmd BinaryInsertion < tiny.txt
 *  A E E L M O P R S T X                 [ one string per line ]
 *
 *  C:\> type words3.txt
 *  bed bug dad yes zoo ... all bad yet
 *
 *  C:\> algscmd BinaryInsertion < words3.txt
 *  all bad bed bug dad ... yes yet zoo   [ one string per line ]
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>BinaryInsertion</c> class provides a static method for sorting an
  /// array using an optimized binary insertion sort with half exchanges.
  /// </para><para>
  /// This implementation makes ~ N lg N compares for any array of length N.
  /// However, in the worst case, the running time is quadratic because the
  /// number of array accesses can be proportional to N^2 (e.g, if the array
  /// is reverse sorted). As such, it is not suitable for sorting large
  /// arrays (unOrderHelper.Less the number of inversions is small).
  /// </para><para>
  /// The sorting algorithm is stable and uses O(1) extra memory.
  /// </para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BinaryInsertion.java.html">BinaryInsertion</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class BinaryInsertion
  {
    // This class should not be instantiated.
    private BinaryInsertion() { }

    /// <summary>Rearranges the array in ascending order, using the natural order.</summary>
    /// <param name="a">the array to be sorted</param>
    ///
    public static void Sort(IComparable[] a)
    {
      int N = a.Length;
      for (int i = 1; i < N; i++)
      {
        // binary search to determine index j at which to insert a[i]
        IComparable v = a[i];
        int lo = 0, hi = i;
        while (lo < hi)
        {
          int mid = lo + (hi - lo) / 2;
          if (OrderHelper.Less(v, a[mid])) hi = mid;
          else lo = mid + 1;
        }

        // insetion sort with "half exchanges"
        // (insert a[i] at index j and shift a[j], ..., a[i-1] to right)
        for (int j = i; j > lo; --j)
          a[j] = a[j - 1];
        a[lo] = v;
      }
      Debug.Assert(OrderHelper.IsSorted(a));
    }

    /// <summary>
    /// Reads in a sequence of strings from standard input; insertion sorts them;
    /// and prints them to standard output in ascending order.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BinaryInsertion < words3.txt", "Input strings to be printed in sorted order")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();
      BinaryInsertion.Sort(a);
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
