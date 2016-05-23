/******************************************************************************
 *  File name :    SuffixArrayX.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A data type that computes the suffix array of a string using 3-way
 *  radix quicksort.
 *
 *  C:\> algscmd SuffixArrayX < abra.txt 
 *    i ind lcp rnk  select
 *  ---------------------------
 *    0  11   -   0  !
 *    1  10   0   1  A!
 *    2   7   1   2  ABRA!
 *    3   0   4   3  ABRACADABRA!
 *    4   3   1   4  ACADABRA!
 *    5   5   1   5  ADABRA!
 *    6   8   0   6  BRA!
 *    7   1   3   7  BRACADABRA!
 *    8   4   0   8  CADABRA!
 *    9   6   0   9  DABRA!
 *   10   9   0  10  RA!
 *   11   2   2  11  RACADABRA!
 *
 *
 ******************************************************************************/
 
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>SuffixArrayX</c> class represents a suffix array of a string of
  /// length <c>N</c>.
  /// It supports the <c>Selecting</c> the <c>I</c>th smallest suffix,
  /// getting the <c>Index</c> of the <c>I</c>th smallest suffix,
  /// computing the length of the <c>Longest common prefix</c> between the
  /// <c>I</c>th smallest suffix and the <c>I</c>-1st smallest suffix,
  /// and determining the <c>Rank</c> of a query string (which is the number
  /// of suffixes strictly less than the query string).
  /// </para><para>
  /// This implementation uses 3-way radix quicksort to sort the array of suffixes.
  /// For a simpler (but less efficient) implementations of the same API, see
  /// <seealso cref="SuffixArray"/>.
  /// The <c>Index</c> and <c>Length</c> operations takes constant time
  /// in the worst case. The <c>Lcp</c> operation takes time proportional to the
  /// length of the longest common prefix.
  /// The <c>Select</c> operation takes time proportional
  /// to the length of the suffix and should be used primarily for debugging.
  /// </para><para>
  /// This implementation uses '\0' as a sentinel and assumes that the charater
  /// '\0' does not appear in the text.
  /// </para><para>
  /// In practice, this algorithm runs very fast. However, in the worst-case
  /// it can be very poor (e.g., a string consisting of N copies of the same
  /// character. We do not shuffle the array of suffixes before sorting because
  /// shuffling is relatively expensive and a pathologial input for which 
  /// the suffixes start out in a bad order (e.g., sorted) is likely to be
  /// a bad input for this algorithm with or without the shuffle.
  /// </para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/63suffix">Section 6.3</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/SuffixArrayX.java.html">SuffixArrayX</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class SuffixArrayX
  {
    private static readonly int CUTOFF = 5;   // cutoff to insertion sort (any value between 0 and 12)

    private readonly char[] text;
    private readonly int[] index;   // index[i] = j means text.substring(j) is ith largest suffix
    private readonly int N;         // number of characters in text

    /// <summary>
    /// Initializes a suffix array for the given <c>text</c> string.</summary>
    /// <param name="text">the input string</param>
    ///
    public SuffixArrayX(string text)
    {
      N = text.Length;
      text = text + '\0';
      this.text = text.ToCharArray();
      this.index = new int[N];
      for (int i = 0; i < N; i++)
        index[i] = i;

      sort(0, N - 1, 0);
    }

    // 3-way string quicksort lo..hi starting at dth character
    private void sort(int lo, int hi, int d)
    {

      // cutoff to insertion sort for small subarrays
      if (hi <= lo + CUTOFF)
      {
        insertion(lo, hi, d);
        return;
      }

      int lt = lo, gt = hi;
      char v = text[index[lo] + d];
      int i = lo + 1;
      while (i <= gt)
      {
        char t = text[index[i] + d];
        if (t < v) exch(lt++, i++);
        else if (t > v) exch(i, gt--);
        else i++;
      }

      // a[lo..lt-1] < v = a[lt..gt] < a[gt+1..hi].
      sort(lo, lt - 1, d);
      if (v > 0) sort(lt, gt, d + 1);
      sort(gt + 1, hi, d);
    }

    // sort from a[lo] to a[hi], starting at the dth character
    private void insertion(int lo, int hi, int d)
    {
      for (int i = lo; i <= hi; i++)
        for (int j = i; j > lo && less(index[j], index[j - 1], d); j--)
          exch(j, j - 1);
    }

    // is text[i+d..N) < text[j+d..N) ?
    private bool less(int i, int j, int d)
    {
      if (i == j) return false;
      i = i + d;
      j = j + d;
      while (i < N && j < N)
      {
        if (text[i] < text[j]) return true;
        if (text[i] > text[j]) return false;
        i++;
        j++;
      }
      return i > j;
    }

    // exchange index[i] and index[j]
    private void exch(int i, int j)
    {
      int swap = index[i];
      index[i] = index[j];
      index[j] = swap;
    }

    /// <summary>
    /// Returns the length of the input string.</summary>
    /// <returns>the length of the input string</returns>
    ///
    public int Length
    {
      get { return N; }
    }


    /// <summary>
    /// Returns the index into the original string of the <c>I</c>th smallest suffix.
    /// That is, <c>text.substring(sa.index(i))</c> is the <c>I</c> smallest suffix.</summary>
    /// <param name="i">an integer between 0 and <c>N</c>-1</param>
    /// <returns>the index into the original string of the <c>I</c>th smallest suffix</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>N</c></exception>
    ///
    public int Index(int i)
    {
      if (i < 0 || i >= N) throw new IndexOutOfRangeException();
      return index[i];
    }

    /// <summary>
    /// Returns the length of the longest common prefix of the <c>I</c>th
    /// smallest suffix and the <c>I</c>-1st smallest suffix.</summary>
    /// <param name="i">an integer between 1 and <c>N</c>-1</param>
    /// <returns>the length of the longest common prefix of the <c>I</c>th</returns>
    /// smallest suffix and the <c>I</c>-1st smallest suffix.
    /// <exception cref="IndexOutOfRangeException">unless 1 &lt;= <c>i</c> &lt; <c>N</c></exception>
    ///
    public int Lcp(int i)
    {
      if (i < 1 || i >= N) throw new IndexOutOfRangeException();
      return lcp(index[i], index[i - 1]);
    }

    // longest common prefix of text[i..N) and text[j..N)
    private int lcp(int i, int j)
    {
      int length = 0;
      while (i < N && j < N)
      {
        if (text[i] != text[j]) return length;
        i++;
        j++;
        length++;
      }
      return length;
    }

    /// <summary>
    /// Returns the <c>I</c>th smallest suffix as a string.</summary>
    /// <param name="i">the index</param>
    /// <returns>the <c>i</c> smallest suffix as a string</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>N</c></exception>
    ///
    public string Select(int i)
    {
      if (i < 0 || i >= N) throw new IndexOutOfRangeException();
      return new string(text, index[i], N - index[i]);
    }

    /// <summary>
    /// Returns the number of suffixes strictly less than the <c>query</c> string.
    /// We note that <c>rank(select(i))</c> equals <c>i</c> for each <c>i</c>
    /// between 0 and <c>N</c>-1.</summary>
    /// <param name="query">the query string</param>
    /// <returns>the number of suffixes strictly less than <c>query</c></returns>
    ///
    public int Rank(string query)
    {
      int lo = 0, hi = N - 1;
      while (lo <= hi)
      {
        int mid = lo + (hi - lo) / 2;
        int cmp = compare(query, index[mid]);
        if (cmp < 0) hi = mid - 1;
        else if (cmp > 0) lo = mid + 1;
        else return mid;
      }
      return lo;
    }

    // is query < text[i..N) ?
    private int compare(string query, int i)
    {
      int M = query.Length;
      int j = 0;
      while (i < N && j < M)
      {
        if (query[j] != text[i]) return query[j] - text[i];
        i++;
        j++;

      }
      if (i < N) return -1;
      if (j < M) return +1;
      return 0;
    }

    /// <summary>
    /// Demo test the <c>SuffixArrayx</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd SuffixArrayX < abra.txt")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();

      Regex WhiteSpace = new Regex(@"[\s]+", RegexOptions.Compiled);
      string s = StdIn.ReadAll().Trim();
      s = WhiteSpace.Replace(s, " ");

      SuffixArray suffix2 = new SuffixArray(s);
      SuffixArrayX suffix1 = new SuffixArrayX(s);
      bool check = true;
      for (int i = 0; check && i < s.Length; i++)
      {
        if (suffix1.Index(i) != suffix2.Index(i))
        {
          Console.WriteLine("suffix1(" + i + ") = " + suffix1.Index(i));
          Console.WriteLine("suffix2(" + i + ") = " + suffix2.Index(i));
          string ith = "\"" + s.Substring(suffix1.Index(i), Math.Min(suffix1.Index(i) + 50, s.Length) - suffix1.Index(i)) + "\"";
          string jth = "\"" + s.Substring(suffix2.Index(i), Math.Min(suffix2.Index(i) + 50, s.Length) - suffix2.Index(i)) + "\"";
          Console.WriteLine(ith);
          Console.WriteLine(jth);
          check = false;
        }
      }

      Console.WriteLine("  i ind lcp rnk  select");
      Console.WriteLine("---------------------------");

      for (int i = 0; i < s.Length; i++)
      {
        int index = suffix2.Index(i);
        string ith = "\"" + s.Substring(index, Math.Min(index + 50, s.Length) - index) + "\"";
        Debug.Assert(s.Substring(index).Equals(suffix2.Select(i)));
        int rank = suffix2.Rank(s.Substring(index));

        if (i == 0)
        {
          Console.Write("{0,3} {1,3} {2,3} {3,3} {4}\n", i, index, "-", rank, ith);
        }
        else
        {
          // int lcp  = suffix.lcp(suffix2.index(i), suffix2.index(i-1));
          int lcp = suffix2.Lcp(i);
          Console.Write("{0,3} {1,3} {2,3} {3,3} {4}\n", i, index, lcp, rank, ith);
        }
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
