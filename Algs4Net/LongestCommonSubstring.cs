/******************************************************************************
 *  File name :    LongestCommonSubstring.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Read in two text files and find the longest substring that
 *  appears in both texts.
 * 
 *  C:\> algscmd LongestCommonSubstring tale.txt mobydick.txt
 *  ' seemed on the point of being '
 *
 ******************************************************************************/
using System;
using System.Text.RegularExpressions;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>LongestCommonSubstring</c> class provides a <seealso cref="SuffixArray"/>
  /// client for computing the longest common substring that appears in two
  /// given strings.</para><para>
  /// This implementation computes the suffix array of each string and applies a
  /// merging operation to determine the longest common substring.
  /// For an alternate implementation, see
  /// <a href = "http://algs4.cs.princeton.edu/63suffix/LongestCommonSubstringConcatenate.java.html">LongestCommonSubstringConcatenate.java</a>.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/63suffix">Section 6.3</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/LongestCommonSubstring.java.html">LongestCommonSubstring</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class LongestCommonSubstring
  {

    // Do not instantiate.
    private LongestCommonSubstring() { }

    // return the longest common prefix of suffix s[p..] and suffix t[q..]
    private static string lcp(string s, int p, string t, int q)
    {
      int n = Math.Min(s.Length - p, t.Length - q);
      for (int i = 0; i < n; i++)
      {
        if (s[p + i] != t[q + i])
          return s.Substring(p, p + i - p);
      }
      return s.Substring(p, p + n - p);
    }

    // compare suffix s[p..] and suffix t[q..]
    private static int compare(string s, int p, string t, int q)
    {
      int n = Math.Min(s.Length - p, t.Length - q);
      for (int i = 0; i < n; i++)
      {
        if (s[p + i] != t[q + i])
          return s[p + i] - t[q + i];
      }
      if (s.Length - p < t.Length - q) return -1;
      else if (s.Length - p > t.Length - q) return +1;
      else return 0;
    }

    /// <summary>
    /// Returns the longest common string of the two specified strings.
    /// </summary>
    /// <param name="s">one string</param>
    /// <param name="t">the other string</param>
    /// <returns>the longest common string that appears as a substring
    /// in both <c>s</c> and <c>t</c>; the empty string
    /// if no such string</returns>
    ///
    public static string Lcs(string s, string t)
    {
      SuffixArray suffix1 = new SuffixArray(s);
      SuffixArray suffix2 = new SuffixArray(t);

      // find longest common substring by "merging" sorted suffixes
      string lcs = "";
      int i = 0, j = 0;
      while (i < s.Length && j < t.Length)
      {
        int p = suffix1.Index(i);
        int q = suffix2.Index(j);
        string x = lcp(s, p, t, q);
        if (x.Length > lcs.Length) lcs = x;
        if (compare(s, p, t, q) < 0) i++;
        else j++;
      }
      return lcs;
    }

    /// <summary>
    /// Demo test the <c>Lcs()</c> client.
    /// Reads in two strings from files specified as command-line arguments;
    /// computes the longest common substring; and prints the results to
    /// standard output.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd LongestCommonSubstring text1.txt text2.txt")]
    public static void MainTest(string[] args)
    {
      TextInput in1 = new TextInput(args[0]);
      TextInput in2 = new TextInput(args[1]);
      Regex WhiteSpace = new Regex(@"[\s]+", RegexOptions.Compiled);
      string s = in1.ReadAll().Trim();
      s = WhiteSpace.Replace(s, " ");
      string t = in2.ReadAll().Trim();
      t = WhiteSpace.Replace(t, " ");
      Console.WriteLine("'" + LongestCommonSubstring.Lcs(s, t) + "'");
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
