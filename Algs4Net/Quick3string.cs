/******************************************************************************
 *  File name :    Quick3string.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Reads string from standard input and 3-way string quicksort them.
 *
 *  C:\> algscmd Quick3string < shells.txt
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
  /// The <c>Quick3string</c> class provides static methods for sorting an
  /// array of strings using 3-way radix quicksort.</summary>
  /// <remarks><para>
  /// For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/51radix">Section 5.1</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Quick3string.java.html">Quick3string</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Quick3string
  {
    private static readonly int CutOff = 15;   // cutoff to insertion sort

    // do not instantiate
    private Quick3string() { }

    /// <summary>
    /// Rearranges the array of strings in ascending order.</summary>
    /// <param name="a">the array to be sorted</param>
    ///
    public static void Sort(string[] a)
    {
      StdRandom.Shuffle(a);
      sort(a, 0, a.Length - 1, 0);
      Debug.Assert(isSorted(a));
    }

    // return the dth character of s, -1 if d = length of s
    private static int charAt(string s, int d)
    {
      Debug.Assert( d >= 0 && d <= s.Length);
      if (d == s.Length) return -1;
      return s[d];
    }

    // 3-way string quicksort a[lo..hi] starting at dth character
    private static void sort(string[] a, int lo, int hi, int d)
    {
      // cutoff to insertion sort for small subarrays
      if (hi <= lo + CutOff)
      {
        insertion(a, lo, hi, d);
        return;
      }

      int lt = lo, gt = hi;
      int v = charAt(a[lo], d);
      int i = lo + 1;
      while (i <= gt)
      {
        int t = charAt(a[i], d);
        if (t < v) exch(a, lt++, i++);
        else if (t > v) exch(a, i, gt--);
        else i++;
      }

      // a[lo..lt-1] < v = a[lt..gt] < a[gt+1..hi].
      sort(a, lo, lt - 1, d);
      if (v >= 0) sort(a, lt, gt, d + 1);
      sort(a, gt + 1, hi, d);
    }

    // sort from a[lo] to a[hi], starting at the dth character
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
    // DEPRECATED BECAUSE OF SLOW SUBSTRING EXTRACTION IN JAVA 7
    // private static bool less(String v, String w, int d) {
    //    assert v.substring(0, d).equals(w.substring(0, d));
    //    return v.substring(d).compareTo(w.substring(d)) < 0;
    // }

    // is v less than w, starting at character d
    private static bool less(string v, string w, int d)
    {
      Debug.Assert(v.Substring(0, d).Equals(w.Substring(0, d)));
      for (int i = d; i < Math.Min(v.Length, w.Length); i++)
      {
        if (v[i] < w[i]) return true;
        if (v[i] > w[i]) return false;
      }
      return v.Length < w.Length;
    }

    // is the array sorted
    private static bool isSorted(string[] a)
    {
      for (int i = 1; i < a.Length; i++)
        if (a[i].CompareTo(a[i - 1]) < 0) return false;
      return true;
    }

    /// <summary>Reads in a sequence of fixed-length strings from 
    /// standard input, 3-way radix quicksorts them, and prints them 
    /// to standard output in ascending order.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Quick3string < shells.txt", "Input of all strings to sort")]
    public static void MainTest(string[] args)
    {
      // read in the strings from standard input
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();
      int N = a.Length;

      // sort the strings
      Quick3string.Sort(a);

      // print the results
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
