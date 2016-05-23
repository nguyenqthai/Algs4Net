/******************************************************************************
 *  File name :    Selection.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/21sort/tiny.txt
 *                http://algs4.cs.princeton.edu/21sort/words3.txt
 *
 *  Sorts a sequence of strings from standard input using selection sort.
 *
 *  C:\> type tiny.txt
 *  S O R T E X A M P L E
 *
 *  C:\> algscmd Selection < tiny.txt
 *  A E E L M O P R S T X                 [ one string per line ]
 *
 *  C:\> type words3.txt
 *  bed bug dad yes zoo ... all bad yet
 *
 *  C:\> algscmd Selection < words3.txt
 *  all bad bed bug dad ... yes yet zoo    [ one string per line ]
 *
 ******************************************************************************/

using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Selection</c> class provides static methods for sorting an
  /// array using selection sort.</summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Selection.java.html">Selection</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks> 
  ///
  public class Selection
  {
    // This class should not be instantiated.
    private Selection() { }

    /// <summary>
    /// Rearranges the array in ascending order, using the natural order.</summary>
    /// <param name="a">a the array to be sorted</param>
    ///
    public static void Sort(IComparable[] a)
    {
      int N = a.Length;
      for (int i = 0; i < N; i++)
      {
        int min = i;
        for (int j = i + 1; j < N; j++)
        {
          if (OrderHelper.Less(a[j], a[min])) min = j;
        }
        OrderHelper.Exch(a, i, min);
        Debug.Assert(OrderHelper.IsSorted(a, 0, i));
      }
      Debug.Assert(OrderHelper.IsSorted(a));
    }

    /// <summary>
    /// Rearranges the array in ascending order, using a comparator.</summary>
    /// <param name="a">a the array</param>
    /// <param name="c">c the comparator specifying the order</param>
    ///
    public static void Sort<T>(T[] a, Comparer<T> c)
    {
      int N = a.Length;
      for (int i = 0; i < N; i++)
      {
        int min = i;
        for (int j = i + 1; j < N; j++)
        {
          if (OrderHelper.Less<T>(a[j], a[min], c)) min = j;
        }
        OrderHelper.Exch(a, i, min);
        Debug.Assert(OrderHelper.IsSorted(a, 0, i, c));
      }
      Debug.Assert(OrderHelper.IsSorted(a, c));
    }

    /// <summary>
    /// Reads in a sequence of strings from standard input; selection sorts them;
    /// and prints them to standard output in ascending order.</summary>
    /// <param name="args">Place holder for user arguments</param>
    ///
    [HelpText("algscmd Selection < words3.txt", "Input strings to be printed in sorted order")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();
      Selection.Sort(a);
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
