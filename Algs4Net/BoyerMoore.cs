/******************************************************************************
 *  File name :    BoyerMoore.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Reads in two strings, the pattern and the input text, and
 *  searches for the pattern in the input text using the
 *  bad-character rule part of the Boyer-Moore algorithm.
 *  (does not implement the strong good suffix rule)
 *
 *  C:\> algscmd BoyerMoore abc yxa3dbdkabbbac
 *  BoyerMoore abc yxa3dbdkabbbac
 *  not found
 *  
 *  C:\> algscmd BoyerMoore abc yxa3dbdkabbbacddabc2345abc2343
 *  text:    yxa3dbdkabbbacddabc2345abc2343
 *  pattern1:                 abc
 *  pattern2:                 abc
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>BoyerMoore</c> class finds the first occurrence of a pattern string
  /// in a text string.</para><para>
  /// This implementation uses the Boyer-Moore algorithm (with the bad-character
  /// rule, but not the strong good suffix rule).</para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/53substring">Section 5.3</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BoyerMoore.java.html">BoyerMoore</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class BoyerMoore
  {
    private readonly int R;   // the radix
    private int[] right;      // the bad-character skip array

    private char[] pattern;   // store the pattern as a character array
    private string pat;       // or as a string

    /// <summary>Preprocesses the pattern string.</summary>
    ///
    /// <param name="pat">the pattern string</param>
    ///
    public BoyerMoore(string pat)
    {
      this.R = 256;
      this.pat = pat;

      // position of rightmost occurrence of c in the pattern
      right = new int[R];
      for (int c = 0; c < R; c++)
        right[c] = -1;
      for (int j = 0; j < pat.Length; j++)
        right[pat[j]] = j;
    }

    /// <summary>Preprocesses the pattern string.</summary>
    /// <param name="pattern">the pattern string</param>
    /// <param name="R">the alphabet size</param>
    ///
    public BoyerMoore(char[] pattern, int R)
    {
      this.R = R;
      this.pattern = new char[pattern.Length];
      for (int j = 0; j < pattern.Length; j++)
        this.pattern[j] = pattern[j];

      // position of rightmost occurrence of c in the pattern
      right = new int[R];
      for (int c = 0; c < R; c++)
        right[c] = -1;
      for (int j = 0; j < pattern.Length; j++)
        right[pattern[j]] = j;
    }

    /// <summary>
    /// Returns the index of the first occurrrence of the pattern string
    /// in the text string.</summary>
    /// <param name="txt"> txt the text string</param>
    /// <returns>the index of the first occurrence of the pattern string
    /// in the text string; N if no such match</returns>
    ///
    public int Search(string txt)
    {
      int M = pat.Length;
      int N = txt.Length;
      int skip;
      for (int i = 0; i <= N - M; i += skip)
      {
        skip = 0;
        for (int j = M - 1; j >= 0; j--)
        {
          if (pat[j] != txt[i + j])
          {
            skip = Math.Max(1, j - right[txt[i + j]]);
            break;
          }
        }
        if (skip == 0) return i;    // found
      }
      return N;                       // not found
    }


    /// <summary>
    /// Returns the index of the first occurrrence of the pattern string
    /// in the text string.</summary>
    /// <param name="text">the text string</param>
    /// <returns>the index of the first occurrence of the pattern string
    /// in the text string; N if no such match</returns>
    ///
    public int Search(char[] text)
    {
      int M = pattern.Length;
      int N = text.Length;
      int skip;
      for (int i = 0; i <= N - M; i += skip)
      {
        skip = 0;
        for (int j = M - 1; j >= 0; j--)
        {
          if (pattern[j] != text[i + j])
          {
            skip = Math.Max(1, j - right[text[i + j]]);
            break;
          }
        }
        if (skip == 0) return i;    // found
      }
      return N;                       // not found
    }
    /// <summary>
    /// Takes a pattern string and an input string as command-line arguments;
    /// searches for the pattern string in the text string; and prints
    /// the first occurrence of the pattern string in the text string.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BoyerMoore pattern string")]
    public static void MainTest(string[] args)
    {
      string pat = args[0];
      string txt = args[1];
      char[] pattern = pat.ToCharArray();
      char[] text = txt.ToCharArray();

      BoyerMoore boyermoore1 = new BoyerMoore(pat);
      BoyerMoore boyermoore2 = new BoyerMoore(pattern, 256);
      int offset1 = boyermoore1.Search(txt);
      int offset2 = boyermoore2.Search(text);

      if (offset2 != offset1)
        Console.WriteLine("inconsistent offsets {0} vs {1}", offset1, offset2);
      else if (offset1 == txt.Length)
        Console.WriteLine("not found");
      else
      {
        // print results
        Console.WriteLine("text:    " + txt);

        Console.Write("pattern1: ");
        for (int i = 0; i < offset1; i++)
          Console.Write(" ");
        Console.WriteLine(pat);

        Console.Write("pattern2: ");
        for (int i = 0; i < offset2; i++)
          Console.Write(" ");
        Console.WriteLine(pat);
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
