/******************************************************************************
 *  File name :    MSD.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Sort an array of strings or integers using MSD radix sort.
 *
 *  C:\> algscmd MSD < shells.txt 
 *  are
 *  by
 *  sea
 *  seashells
 *  seashells
 *  sells
 *  sells
 *  she
 *  she
 *  shells
 *  shore
 *  surely
 *  the
 *  the
 *
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary>
  /// The <c>MSD</c> class provides static methods for sorting an
  /// array of extended ASCII strings or integers using MSD radix sort.</summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/51radix">Section 5.1</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/MSD.java.html">MSD</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class MSD
  {
    private static readonly int BitsPerByte = 8;
    private static readonly int BitsPerInt = 32;  // each Java int is 32 bits 
    private static readonly int R = 256;          // extended ASCII alphabet size
    private static readonly int CutOff = 15;      // cutoff to insertion sort

    // do not instantiate
    private MSD() { }

    /// <summary>
    /// Rearranges the array of extended ASCII strings in ascending order.</summary>
    /// <param name="a">the array to be sorted</param>
    ///
    public static void Sort(string[] a)
    {
      int N = a.Length;
      string[] aux = new string[N];
      sort(a, 0, N - 1, 0, aux);
    }

    // return dth character of s, -1 if d = length of string
    private static int charAt(string s, int d)
    {
      Debug.Assert(d >= 0 && d <= s.Length);
      if (d == s.Length) return -1;
      return s[d];
    }

    // sort from a[lo] to a[hi], starting at the dth character
    private static void sort(string[] a, int lo, int hi, int d, string[] aux)
    {

      // cutoff to insertion sort for small subarrays
      if (hi <= lo + CutOff)
      {
        insertion(a, lo, hi, d);
        return;
      }

      // compute frequency counts
      int[] count = new int[R + 2];
      for (int i = lo; i <= hi; i++)
      {
        int c = charAt(a[i], d);
        count[c + 2]++;
      }

      // transform counts to indicies
      for (int r = 0; r < R + 1; r++)
        count[r + 1] += count[r];

      // distribute
      for (int i = lo; i <= hi; i++)
      {
        int c = charAt(a[i], d);
        aux[count[c + 1]++] = a[i];
      }

      // copy back
      for (int i = lo; i <= hi; i++)
        a[i] = aux[i - lo];


      // recursively sort for each character (excludes sentinel -1)
      for (int r = 0; r < R; r++)
        sort(a, lo + count[r], lo + count[r + 1] - 1, d + 1, aux);
    }

    // insertion sort a[lo..hi], starting at dth character
    private static void insertion(string[] a, int lo, int hi, int d)
    {
      for (int i = lo; i <= hi; i++)
        for (int j = i; j > lo && less(a[j], a[j - 1], d); j--)
          exch(a, j, j - 1);
    }

    // exchange a[i] and a[j]
    private static void exch(string[] a, int i, int j)
    {
      string temp = a[i];
      a[i] = a[j];
      a[j] = temp;
    }

    // is v less than w, starting at character d
    private static bool less(string v, string w, int d)
    {
      // assert v.substring(0, d).equals(w.substring(0, d));
      for (int i = d; i < Math.Min(v.Length, w.Length); i++)
      {
        if (v[i] < w[i]) return true;
        if (v[i] > w[i]) return false;
      }
      return v.Length < w.Length;
    }


    /// <summary>
    /// Rearranges the array of 32-bit integers in ascending order.
    /// Currently assumes that the integers are nonnegative.</summary>
    /// <param name="a">the array to be sorted</param>
    ///
    public static void Sort(int[] a)
    {
      int N = a.Length;
      int[] aux = new int[N];
      sort(a, 0, N - 1, 0, aux);
    }

    // MSD sort from a[lo] to a[hi], starting at the dth byte
    private static void sort(int[] a, int lo, int hi, int d, int[] aux)
    {

      // cutoff to insertion sort for small subarrays
      if (hi <= lo + CutOff)
      {
        insertion(a, lo, hi, d);
        return;
      }

      // compute frequency counts (need R = 256)
      int[] count = new int[R + 1];
      int mask = R - 1;   // 0xFF;
      int shift = BitsPerInt - BitsPerByte * d - BitsPerByte;
      for (int i = lo; i <= hi; i++)
      {
        int c = (a[i] >> shift) & mask;
        count[c + 1]++;
      }

      // transform counts to indicies
      for (int r = 0; r < R; r++)
        count[r + 1] += count[r];

      /************* BUGGGY CODE.
              // for most significant byte, 0x80-0xFF comes before 0x00-0x7F
              if (d == 0) {
                  int shift1 = count[R] - count[R/2];
                  int shift2 = count[R/2];
                  for (int r = 0; r < R/2; r++)
                      count[r] += shift1;
                  for (int r = R/2; r < R; r++)
                      count[r] -= shift2;
              }
      ************************************/
      // distribute
      for (int i = lo; i <= hi; i++)
      {
        int c = (a[i] >> shift) & mask;
        aux[count[c]++] = a[i];
      }

      // copy back
      for (int i = lo; i <= hi; i++)
        a[i] = aux[i - lo];

      // no more bits
      if (d == 4) return;

      // recursively sort for each character
      if (count[0] > 0)
        sort(a, lo, lo + count[0] - 1, d + 1, aux);
      for (int r = 0; r < R; r++)
        if (count[r + 1] > count[r])
          sort(a, lo + count[r], lo + count[r + 1] - 1, d + 1, aux);
    }

    // insertion sort a[lo..hi], starting at dth character
    private static void insertion(int[] a, int lo, int hi, int d)
    {
      for (int i = lo; i <= hi; i++)
        for (int j = i; j > lo && a[j] < a[j - 1]; j--)
          exch(a, j, j - 1);
    }

    // exchange a[i] and a[j]
    private static void exch(int[] a, int i, int j)
    {
      int temp = a[i];
      a[i] = a[j];
      a[j] = temp;
    }

    /// <summary>
    /// Reads in a sequence of extended ASCII strings from standard input;
    /// MSD radix sorts them and prints them to standard output in 
    /// ascending order.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd MSD < shells.txt", "Input of all strings to sort")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();
      int N = a.Length;
      MSD.Sort(a);
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
