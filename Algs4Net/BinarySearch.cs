/******************************************************************************
 *  File name :    BinarySearch.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/11model/tinyW.txt
 *                http://algs4.cs.princeton.edu/11model/tinyT.txt
 *                http://algs4.cs.princeton.edu/11model/largeW.txt
 *                http://algs4.cs.princeton.edu/11model/largeT.txt
 *
 *  C:\> algscmd BinarySearch tinyW.txt < tinyT.txt
 *  50
 *  99
 *  13
 *
 *  C:\> algscmd BinarySearch largeW.txt < largeT.txt | more
 *  499569
 *  984875
 *  295754
 *  207807
 *  140925
 *  161828
 *  [367,966 total values]
 *
 ******************************************************************************/
using System;
using System.IO;

namespace Algs4Net
{
  /// <summary>
  /// <para>The <c>BinarySearch</c> class provides a static method for binary
  /// searching for an integer in a sorted array of integers.
  /// </para><para>
  /// The <c>Rank</c> operations takes logarithmic time in the worst case.
  /// </para></summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/11model">Section 1.1</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BinarySearch.java.html">BinarySearch</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.
  /// </para></remarks>

  ///
  public class BinarySearch
  {
    /// 
    /// This class should not be instantiated.
    ///
    private BinarySearch() { }

    /// <summary>
    /// Returns the index of the specified key in the specified array.</summary>
    ///
    /// <param name="a"> a the array of integers, must be sorted in ascending order</param>
    /// <param name="key"> key the search key</param>
    /// <returns>index of key in array <c>a</c> if present; <c>-1</c> otherwise</returns>
    ///
    public static int IndexOf(int[] a, int key)
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
    /// Returns the index of the specified key in the specified array.
    /// This function is poorly named because it does not give the <c>Rank</c>
    /// if the array has duplicate keys or if the key is not in the array.
    /// </summary>
    /// <param name="key"> key the search key</param>
    /// <param name="a"> a the array of integers, must be sorted in ascending order</param>
    /// <returns>index of key in array <c>a</c> if present; <c>-1</c> otherwise</returns>
    /// <remarks>This is replaced by <see cref="IndexOf(int[], int)"/>.</remarks>
    ///
    public static int Rank(int key, int[] a)
    {
      return IndexOf(a, key);
    }

    /// <summary><para>
    /// Demo test for the <c>BinarySearch</c> data type.</para>
    /// <para>Reads in a sequence of integers from the whitelist file, specified as
    /// a command-line argument; reads in integers from standard input;
    /// prints to standard output those integers that do NOT appear in the file.</para>
    /// </summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText(
      "algscmd BinarySearch tinyW.txt < tinyT.txt",
      "A text file of sorted items and items to search from default input")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      int[] whitelist = input.ReadAllInts();
      input.Close();
      // sort the array
      Array.Sort(whitelist);
      TextInput StdIn = new TextInput();
      while (!StdIn.IsEmpty)
      {
        int key = StdIn.ReadInt();
        if (BinarySearch.IndexOf(whitelist, key) == -1)
          Console.WriteLine(key);
      }
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

