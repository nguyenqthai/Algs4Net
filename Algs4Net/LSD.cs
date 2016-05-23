/******************************************************************************
 *  File name :    LSD.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  LSD radix sort
 *
 *    - Sort a string[] array of N extended ASCII strings (R = 256), each of length W.
 *
 *    - Sort an int[] array of N 32-bit integers, treating each integer as 
 *      a sequence of W = 4 bytes (R = 256).
 *
 *  Uses extra space proportional to N + R.
 *
 *  C:\> algscmd LSD < words3.txt
 *  all
 *  bad
 *  bed
 *  bug
 *  dad
 *  ...
 *  yes
 *  yet
 *  zoo
 *
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary>
  /// The <c>LSD</c> class provides static methods for sorting an
  /// array of W-character strings or 32-bit integers using LSD radix sort.</summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/51radix">Section 5.1</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/LSD.java.html">LSD</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class LSD
  {
    private static readonly int BitsPerByte = 8;

    // do not instantiate
    private LSD() { }

    /// <summary>
    /// Rearranges the array of W-character strings in ascending order.</summary>
    /// <param name="a">the array to be sorted</param>
    /// <param name="W">the number of characters per string</param>
    ///
    public static void Sort(string[] a, int W)
    {
      int N = a.Length;
      int R = 256;   // extend ASCII alphabet size
      string[] aux = new string[N];

      for (int d = W - 1; d >= 0; d--)
      {
        // sort by key-indexed counting on dth character

        // compute frequency counts
        int[] count = new int[R + 1];
        for (int i = 0; i < N; i++)
          count[a[i][d] + 1]++;

        // compute cumulates
        for (int r = 0; r < R; r++)
          count[r + 1] += count[r];

        // move data
        for (int i = 0; i < N; i++)
          aux[count[a[i][d]]++] = a[i];

        // copy back
        for (int i = 0; i < N; i++)
          a[i] = aux[i];
      }
    }

    /// <summary>
    /// Rearranges the array of 32-bit integers in ascending order.
    /// This is about 2-3x faster than Array.Sort().</summary>
    /// <param name="a">the array to be sorted</param>
    ///
    public static void Sort(int[] a)
    {
      int Bits = 32;                 // each int is 32 bits
      int W = Bits / BitsPerByte;  // each int is 4 bytes
      int R = 1 << BitsPerByte;    // each bytes is between 0 and 255
      int Mask = R - 1;              // 0xFF

      int N = a.Length;
      int[] aux = new int[N];

      for (int d = 0; d < W; d++)
      {

        // compute frequency counts
        int[] count = new int[R + 1];
        for (int i = 0; i < N; i++)
        {
          int c = (a[i] >> BitsPerByte * d) & Mask;
          count[c + 1]++;
        }

        // compute cumulates
        for (int r = 0; r < R; r++)
          count[r + 1] += count[r];

        // for most significant byte, 0x80-0xFF comes before 0x00-0x7F
        if (d == W - 1)
        {
          int shift1 = count[R] - count[R / 2];
          int shift2 = count[R / 2];
          for (int r = 0; r < R / 2; r++)
            count[r] += shift1;
          for (int r = R / 2; r < R; r++)
            count[r] -= shift2;
        }

        // move data
        for (int i = 0; i < N; i++)
        {
          int c = (a[i] >> BitsPerByte * d) & Mask;
          aux[count[c]++] = a[i];
        }

        // copy back
        for (int i = 0; i < N; i++)
          a[i] = aux[i];
      }
    }

    /// <summary>
    /// Reads in a sequence of fixed-length strings from standard input;
    /// LSD radix sorts them;
    /// and prints them to standard output in ascending order.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd LSD < words3.txt", "Input of all fixed-length strings to sort")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();
      int N = a.Length;

      // check that strings have fixed length
      int W = a[0].Length;
      for (int i = 0; i < N; i++)
        Debug.Assert(a[i].Length == W, "Strings must have fixed length");

      // sort the strings
      LSD.Sort(a, W);

      // print results
      for (int i = 0; i < N; i++)
        Console.WriteLine(a[i]);
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
