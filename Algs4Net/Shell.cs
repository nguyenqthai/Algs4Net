/******************************************************************************
 *  File name :    Shell.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/21sort/tiny.txt
 *                http://algs4.cs.princeton.edu/21sort/words3.txt
 *
 *  Sorts a sequence of strings from standard input using shellsort.
 *
 *  Uses increment sequence proposed by Sedgewick and Incerpi.
 *  The nth element of the sequence is the smallest integer >= 2.5^n
 *  that is relatively prime to all previous terms in the sequence.
 *  For example, incs[4] is 41 because 2.5^4 = 39.0625 and 41 is
 *  the next integer that is relatively prime to 3, 7, and 16.
 *
 *  C:\> type tiny.txt
 *  S O R T E X A M P L E
 *
 *  C:\> algscmd Shell < tiny.txt
 *  A E E L M O P R S T X                 [ one string per line ]
 *
 *  C:\> type words3.txt
 *  bed bug dad yes zoo ... all bad yet
 *
 *  C:\> algscmd Shell < words3.txt
 *  all bad bed bug dad ... yes yet zoo    [ one string per line ]
 *
 *
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Shell</c> class provides static methods for sorting an
  /// array using Shellsort with Knuth's increment sequence (1, 4, 13, 40, ...).</summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/21elementary">Section 2.1</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Shell.java.html">Shell</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks>
  ///
  public class Shell
  {
    // This class should not be instantiated.
    private Shell() { }

    /// <summary>
    /// Rearranges the array in ascending order, using the natural order.</summary>
    /// <param name="a">a the array to be sorted</param>
    ///
    public static void Sort(IComparable[] a)
    {
      int N = a.Length;

      // 3x+1 increment sequence:  1, 4, 13, 40, 121, 364, 1093, ...
      int h = 1;
      while (h < N / 3) h = 3 * h + 1;

      while (h >= 1)
      {
        // h-sort the array
        for (int i = h; i < N; i++)
        {
          for (int j = i; j >= h && OrderHelper.Less(a[j], a[j - h]); j -= h)
          {
            OrderHelper.Exch(a, j, j - h);
          }
        }
        Debug.Assert(IsHsorted(a, h));
        h /= 3;
      }
      Debug.Assert(OrderHelper.IsSorted(a));
    }

    /***************************************************************************
     *  Check if array is H sorted - useful for debugging.
     ***************************************************************************/

    // is the array h-sorted?
    private static bool IsHsorted(IComparable[] a, int h)
    {
      for (int i = h; i < a.Length; i++)
        if (OrderHelper.Less(a[i], a[i - h])) return false;
      return true;
    }

    /// <summary>
    /// Reads in a sequence of strings from standard input; Shellsorts them;
    /// and prints them to standard output in ascending order.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Shell < words3.txt", "Input strings to be printed in sorted order")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();
      Shell.Sort(a);
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
