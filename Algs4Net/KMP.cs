/******************************************************************************
 *  File name :    KMP.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Reads in two strings, the pattern and the input text, and
 *  searches for the pattern in the input text using the
 *  KMP algorithm.
 *
 *  C:\>  algscmd KMP abc yxa3dbdkabbbacddabc2345abc2343
 *  text:    yxa3dbdkabbbacddabc2345abc2343
 *  pattern1:                 abc
 *  pattern2:                 abc
 *
 *  C:\>  algscmd KMP abc yx
 *  not found
 *  
 *  C:\>  algscmd KMP abc abababbbbccccx82kd
 *  not found
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>KMP</c> class finds the first occurrence of a pattern string
  /// in a text string.</para><para>
  /// This implementation uses a version of the Knuth-Morris-Pratt substring search
  /// algorithm. The version takes time as space proportional to
  /// <c>N</c> + <c>M R</c> in the worst case, where <c>N</c> is the length
  /// of the text string, <c>M</c> is the length of the pattern, and <c>R</c>
  /// is the alphabet size.</para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/53substring">Section 5.3</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/KMP.java.html">KMP</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class KMP
  {
    private readonly int R;   // the radix
    private int[,] dfa;      // the KMP automoton

    private char[] pattern;   // either the character array for the pattern
    private string pat;       // or the pattern string

    /// <summary>Preprocesses the pattern string.</summary>
    /// <param name="pat">pat the pattern string</param>
    ///
    public KMP(string pat)
    {
      this.R = 256;
      this.pat = pat;

      // build DFA from pattern
      int M = pat.Length;
      dfa = new int[R,M];
      dfa[pat[0],0] = 1;
      for (int X = 0, j = 1; j < M; j++)
      {
        for (int c = 0; c < R; c++)
          dfa[c,j] = dfa[c,X];    // Copy mismatch cases.
        dfa[pat[j],j] = j + 1;    // Set match case.
        X = dfa[pat[j],X];        // Update restart state.
      }
    }

    /// <summary>Preprocesses the pattern string.</summary>
    /// <param name="pattern">the pattern string</param>
    /// <param name="R">the alphabet size</param>
    ///
    public KMP(char[] pattern, int R)
    {
      this.R = R;
      this.pattern = new char[pattern.Length];
      for (int j = 0; j < pattern.Length; j++)
        this.pattern[j] = pattern[j];

      // build DFA from pattern
      int M = pattern.Length;
      dfa = new int[R,M];
      dfa[pattern[0],0] = 1;
      for (int X = 0, j = 1; j < M; j++)
      {
        for (int c = 0; c < R; c++)
          dfa[c,j] = dfa[c,X];      // Copy mismatch cases.
        dfa[pattern[j],j] = j + 1;  // Set match case.
        X = dfa[pattern[j],X];      // Update restart state.
      }
    }

    /// <summary>
    /// Returns the index of the first occurrrence of the pattern string
    /// in the text string.</summary>
    /// <param name="txt">the text string</param>
    /// <returns>the index of the first occurrence of the pattern string
    /// in the text string; N if no such match</returns>
    ///
    public int Search(string txt)
    {
      // simulate operation of DFA on text
      int M = pat.Length;
      int N = txt.Length;
      int i, j;
      for (i = 0, j = 0; i < N && j < M; i++)
      {
        j = dfa[txt[i],j];
      }
      if (j == M) return i - M;    // found
      return N;                    // not found
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
      // simulate operation of DFA on text
      int M = pattern.Length;
      int N = text.Length;
      int i, j;
      for (i = 0, j = 0; i < N && j < M; i++)
      {
        j = dfa[text[i],j];
      }
      if (j == M) return i - M;    // found
      return N;                    // not found
    }

    /// <summary>
    /// Takes a pattern string and an input string as command-line arguments;
    /// searches for the pattern string in the text string; and prints
    /// the first occurrence of the pattern string in the text string.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd KMP pattern input_text")]
    public static void MainTest(string[] args)
    {
      string pat = args[0];
      string txt = args[1];
      char[] pattern = pat.ToCharArray();
      char[] text = txt.ToCharArray();

      KMP kmp1 = new KMP(pat);
      int offset1 = kmp1.Search(txt);

      KMP kmp2 = new KMP(pattern, 256);
      int offset2 = kmp2.Search(text);

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
