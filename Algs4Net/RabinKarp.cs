/******************************************************************************
 *  File name :    RabinKarp.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Reads in two strings, the pattern and the input text, and
 *  searches for the pattern in the input text using the
 *  Las Vegas version of the Rabin-Karp algorithm.
 *
 *  C:\> algscmd RabinKarp abc abababbbbcccccabcx82kd
 *  text:    abababbbbcccccabcx82kd
 *  pattern:               abc
 *  C:\> algscmd RabinKarp abc abababbbbccccx82kd
 *  text:    abababbbbccccx82kd
 *  not found
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>RabinKarp</c> class finds the first occurrence of a pattern string
  /// in a text string. This implementation uses the Rabin-Karp algorithm.</summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/53substring">Section 5.3</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/RabinKarp.java.html">RabinKarp</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class RabinKarp
  {
    private string pat;      // the pattern  // needed only for Las Vegas
    private long patHash;    // pattern hash value
    private int M;           // pattern length
    private long Q;          // a large prime, small enough to avoid long overflow
    private int R;           // radix
    private long RM;         // R^(M-1) % Q

    /// <summary>
    /// Preprocesses the pattern string.</summary>
    /// <param name="pattern">the pattern string</param>
    /// <param name="R">the alphabet size</param>
    ///
    internal RabinKarp(char[] pattern, int R)
    {
      throw new NotImplementedException("Operation not supported yet");
    }

    /// <summary>
    /// Preprocesses the pattern string.</summary>
    /// <param name="pat">the pattern string</param>
    ///
    public RabinKarp(string pat)
    {
      this.pat = pat;      // save pattern (needed only for Las Vegas)
      R = 256;
      M = pat.Length;
      Q = longRandomPrime();

      // precompute R^(M-1) % Q for use in removing leading digit
      RM = 1;
      for (int i = 1; i <= M - 1; i++)
        RM = (R * RM) % Q;
      patHash = hash(pat, M);
    }

    // Compute hash for key[0..M-1].
    private long hash(string key, int M)
    {
      long h = 0;
      for (int j = 0; j < M; j++)
        h = (R * h + key[j]) % Q;
      return h;
    }

    // Las Vegas version: does pat[] match txt[i..i-M+1] ?
    private bool check(string txt, int i)
    {
      for (int j = 0; j < M; j++)
        if (pat[j] != txt[i + j])
          return false;
      return true;
    }

    // Monte Carlo version: always return true
    private bool check(int i)
    {
      return true;
    }

    /// <summary>
    /// Returns the index of the first occurrrence of the pattern string
    /// in the text string.</summary>
    /// <param name="txt">the text string</param>
    /// <returns>the index of the first occurrence of the pattern string</returns>
    ///        in the text string; N if no such match
    ///
    public int Search(string txt)
    {
      int N = txt.Length;
      if (N < M) return N;
      long txtHash = hash(txt, M);

      // check for match at offset 0
      if ((patHash == txtHash) && check(txt, 0))
        return 0;

      // check for hash match; if hash match, check for exact match
      for (int i = M; i < N; i++)
      {
        // Remove leading digit, add trailing digit, check for match.
        txtHash = (txtHash + Q - RM * txt[i - M] % Q) % Q;
        txtHash = (txtHash * R + txt[i]) % Q;

        // match
        int offset = i - M + 1;
        if ((patHash == txtHash) && check(txt, offset))
          return offset;
      }

      // no match
      return N;
    }


    // simulate a random 31-bit prime as .NET does not have a facility to
    // generate probably large random primes.
    private static long longRandomPrime()
    {
      // The list was taken from https://primes.utm.edu/lists/small/small.html#10
      long[] largePrimes = {
        5915587277, 1500450271, 3267000013, 5754853343, 4093082899, 9576890767, 3628273133, 2860486313, 5463458053, 3367900313
      };
      Random rnd = new Random();
      return largePrimes[rnd.Next() % largePrimes.Length];
    }

    /// <summary>
    /// Takes a pattern string and an input string as command-line arguments;
    /// searches for the pattern string in the text string; and prints
    /// the first occurrence of the pattern string in the text string.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd RabinKarp pattern input_text")]
    public static void MainTest(string[] args)
    {
      string pat = args[0];
      string txt = args[1];

      RabinKarp searcher = new RabinKarp(pat);
      int offset = searcher.Search(txt);

      // print results
      Console.WriteLine("text:    " + txt);
      if (offset == txt.Length) Console.WriteLine("not found");
      else
      {
        // from brute force search method 1
        Console.Write("pattern: ");
        for (int i = 0; i < offset; i++)
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

