/******************************************************************************
 *  File name :    SuffixArray.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A data type that computes the suffix array of a string.
 *
 *  C:\> algscmd SuffixArray < abra.txt
 *   i ind lcp rnk  select
 *  ---------------------------
 *   0  11   -   0  "!"
 *   1  10   0   1  "A!"
 *   2   7   1   2  "ABRA!"
 *   3   0   4   3  "ABRACADABRA!"
 *   4   3   1   4  "ACADABRA!"
 *   5   5   1   5  "ADABRA!"
 *   6   8   0   6  "BRA!"
 *   7   1   3   7  "BRACADABRA!"
 *   8   4   0   8  "CADABRA!"
 *   9   6   0   9  "DABRA!"
 *  10   9   0  10  "RA!"
 *  11   2   2  11  "RACADABRA!"
 *
 ******************************************************************************/

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>SuffixArray</c> class represents a suffix array of a string of
  /// length <c>N</c>.
  /// It supports the <c>Selecting</c> the <c>I</c>th smallest suffix,
  /// getting the <c>Index</c> of the <c>I</c>th smallest suffix,
  /// computing the length of the <c>Longest common prefix</c> between the
  /// <c>I</c>th smallest suffix and the <c>I</c>-1st smallest suffix,
  /// and determining the <c>Rank</c> of a query string (which is the number
  /// of suffixes strictly less than the query string).</para><para>
  /// This implementation uses a nested class <c>Suffix</c> to represent
  /// a suffix of a string (using constant time and space) and
  /// <c>Array.Sort()</c> to sort the array of suffixes.
  /// The <c>Index</c> and <c>Length</c> operations takes constant time 
  /// in the worst case. The <c>Lcp</c> operation takes time proportional to the
  /// length of the longest common prefix.
  /// The <c>Select</c> operation takes time proportional
  /// to the length of the suffix and should be used primarily for debugging.
  /// </para><para>For alternate implementations of the same API, see
  /// <seealso cref="SuffixArrayX"/>, which is faster in practice (uses 3-way radix quicksort)
  /// and uses less memory (does not create <c>Suffix</c> objects)</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/63suffix">Section 6.3</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/SuffixArray.java.html">SuffixArray</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class SuffixArray
  {
    private Suffix[] suffixes;

    /// <summary>
    /// Initializes a suffix array for the given <c>text</c> string.</summary>
    /// <param name="text">the input string</param>
    ///
    public SuffixArray(string text)
    {
      int N = text.Length;
      suffixes = new Suffix[N];
      for (int i = 0; i < N; i++)
        suffixes[i] = new Suffix(text, i);
      Array.Sort(suffixes);
    }

    private class Suffix : IComparable<Suffix>
    {
      private readonly string text;
      private readonly int index;

      public Suffix(string text, int index)
      {
        this.text = text;
        this.index = index;
      }

      public int Index
      {
        get { return index; }
      }

      public int Length
      {
        get { return text.Length - index; }
      }

      public char this[int i]
      {
        get { return text[index + i]; }
      }

      public int CompareTo(Suffix that)
      {
        if (this == that) return 0;  // optimization
        int N = Math.Min(Length, that.Length);
        for (int i = 0; i < N; i++)
        {
          if (this[i] < that[i]) return -1;
          if (this[i] > that[i]) return +1;
        }
        return Length - that.Length;
      }

      public override string ToString()
      {
        return text.Substring(index);
      }
    }

    /// <summary>
    /// Returns the length of the input string.</summary>
    /// <returns>the length of the input string</returns>
    ///
    public int Length
    {
      get { return suffixes.Length; }
    }

    /// <summary>
    /// Returns the index into the original string of the <c>i</c>th smallest suffix.
    /// That is, <c>text.Substring(sa.Index(i))</c> is the <c>i</c>th smallest suffix.</summary>
    /// <param name="i">an integer between 0 and <c>N</c>-1</param>
    /// <returns>the index into the original string of the <c>i</c>th smallest suffix</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>N</c></exception>
    ///
    public int Index(int i)
    {
      if (i < 0 || i >= suffixes.Length) throw new IndexOutOfRangeException();
      return suffixes[i].Index;
    }


    /// <summary>
    /// Returns the length of the longest common prefix of the <c>I</c>th
    /// smallest suffix and the <c>i</c>-1st smallest suffix.</summary>
    /// <param name="i">an integer between 1 and <c>N</c>-1</param>
    /// <returns>the length of the longest common prefix of the <c>I</c>th</returns>
    /// smallest suffix and the <c>i</c>-1st smallest suffix.
    /// <exception cref="IndexOutOfRangeException">unless 1 &lt;= <c>I</c> &lt; <c>N</c></exception>
    ///
    public int Lcp(int i)
    {
      if (i < 1 || i >= suffixes.Length) throw new IndexOutOfRangeException();
      return lcp(suffixes[i], suffixes[i - 1]);
    }

    // longest common prefix of s and t
    private static int lcp(Suffix s, Suffix t)
    {
      int N = Math.Min(s.Length, t.Length);
      for (int i = 0; i < N; i++)
      {
        if (s[i] != t[i]) return i;
      }
      return N;
    }

    /// <summary>
    /// Returns the <c>i</c>th smallest suffix as a string.</summary>
    /// <param name="i">the index</param>
    /// <returns>the <c>I</c> smallest suffix as a string</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>N</c></exception>
    ///
    public string Select(int i)
    {
      if (i < 0 || i >= suffixes.Length) throw new IndexOutOfRangeException();
      return suffixes[i].ToString();
    }

    /// <summary>
    /// Returns the number of suffixes strictly less than the <c>query</c> string.
    /// We note that <c>Rank(Select(i))</c> equals <c>i</c> for each <c>i</c>
    /// between 0 and <c>N</c>-1.</summary>
    /// <param name="query">the query string</param>
    /// <returns>the number of suffixes strictly less than <c>query</c></returns>
    ///
    public int Rank(string query)
    {
      int lo = 0, hi = suffixes.Length - 1;
      while (lo <= hi)
      {
        int mid = lo + (hi - lo) / 2;
        int cmp = compare(query, suffixes[mid]);
        if (cmp < 0) hi = mid - 1;
        else if (cmp > 0) lo = mid + 1;
        else return mid;
      }
      return lo;
    }

    // compare query string to suffix
    private static int compare(string query, Suffix suffix)
    {
      int N = Math.Min(query.Length, suffix.Length);
      for (int i = 0; i < N; i++)
      {
        if (query[i] < suffix[i]) return -1;
        if (query[i] > suffix[i]) return +1;
      }
      return query.Length - suffix.Length;
    }

    /// <summary>
    /// Demo test the <c>SuffixArray</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd SuffixArray < abra.txt")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();

      Regex WhiteSpace = new Regex(@"[\s]+", RegexOptions.Compiled);
      string s = StdIn.ReadAll().Trim();
      s = WhiteSpace.Replace(s, " ");
      SuffixArray suffix = new SuffixArray(s.Trim());

      Console.WriteLine("  i ind lcp rnk select");
      Console.WriteLine("---------------------------");

      for (int i = 0; i < s.Length; i++)
      {
        int index = suffix.Index(i);
        string ith = "\"" + s.Substring(index, Math.Min(index + 50, s.Length) - index) + "\"";
        Debug.Assert(s.Substring(index).Equals(suffix.Select(i)));
        int rank = suffix.Rank(s.Substring(index));
        if (i == 0)
        {
          Console.WriteLine("{0,3} {1,3} {2,3} {3,3} {4}", i, index, "-", rank, ith);
        }
        else
        {
          int lcp = suffix.Lcp(i);
          Console.WriteLine("{0,3} {1,3} {2,3} {3,3} {4}", i, index, lcp, rank, ith);
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
