/******************************************************************************
 *  File name :    LongestRepeatedSubstring.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/63suffix/tinyTale.txt
 *                http://algs4.cs.princeton.edu/63suffix/mobydick.txt
 *  
 *  Reads a text string from stdin, replaces all consecutive blocks of
 *  whitespace with a single space, and then computes the longest
 *  repeated substring in that text using a suffix array.
 * 
 *  C:\> algscmd LongestRepeatedSubstring < tinyTale.txt 
 *  'st of times it was the '
 *
 *  C:\> algscmd LongestRepeatedSubstring < mobydick.txt
 *  ',- Such a funny, sporty, gamy, jesty, joky, hoky-poky lad, is the Ocean, oh! Th'
 * 
 *  C:\> algscmd LongestRepeatedSubstring
 *  aaaaaaaaa
 *  'aaaaaaaa'
 *
 *  C:\> algscmd LongestRepeatedSubstring
 *  abcdefg
 *  ''
 *
 ******************************************************************************/

using System;
using System.Text.RegularExpressions;

namespace Algs4Net
{
  /// <summary>
  /// The <c>LongestRepeatedSubstring</c> class provides a <seealso cref="SuffixArray"/>
  /// client for computing the longest repeated substring of a string that
  /// appears at least twice. The repeated substrings may overlap (but must
  /// be distinct). See also <seealso cref="LongestCommonSubstring"/>.</summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/63suffix">Section 6.3</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/LongestRepeatedSubstring.java.html">LongestRepeatedSubstring</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class LongestRepeatedSubstring
  {

    // Do not instantiate.
    private LongestRepeatedSubstring() { }

    /// <summary>
    /// Returns the longest common string of the two specified strings.</summary>
    /// <param name="s">one string</param>
    /// <param name="t">the other string</param>
    /// <returns>the longest common string that appears as a substring</returns>
    ///
    private static string Lcs(string s, string t)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the longest repeated substring of the specified string.</summary>
    /// <param name="text">the string</param>
    /// <returns>the longest repeated substring that appears in <c>text</c>;
    ///        the empty string if no such string</returns>
    ///
    public static string Lrs(string text)
    {
      int N = text.Length;
      SuffixArray sa = new SuffixArray(text);
      string lrs = "";
      for (int i = 1; i < N; i++)
      {
        int length = sa.Lcp(i);
        if (length > lrs.Length)
        {
          lrs = text.Substring(sa.Index(i), sa.Index(i) + length - sa.Index(i));
        }
      }
      return lrs;
    }

    /// <summary>
    /// Demo test the <c>Lrs()</c> client.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd LongestRepeatedSubstring < tinyTale.txt")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();

      Regex WhiteSpace = new Regex(@"[\s]+", RegexOptions.Compiled);
      string text = StdIn.ReadAll();
      text = WhiteSpace.Replace(text, " ");
      SuffixArray suffix = new SuffixArray(text.Trim());

      //Console.WriteLine(text);
      Console.WriteLine("'" + LongestRepeatedSubstring.Lrs(text) + "'");
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
