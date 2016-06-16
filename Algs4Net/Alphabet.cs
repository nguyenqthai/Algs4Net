/******************************************************************************
 *  File name :    Alphabet.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A data type for alphabets, for use with string-processing code
 *  that must convert between an alphabet of size R and the integers
 *  0 through R-1.
 *
 *  Warning: supports only the basic multilingual plane (BMP), i.e,
 *           Unicode characters between U+0000 and U+FFFF.
 *           
 *  C:\> algscmd Alphabet
 *  NowIsTheTimeForAllGoodMen
 *  AACGAACGGTTTACCCCG
 *  01234567890123456789
 *  
 ******************************************************************************/
using System;
using System.Text;

namespace Algs4Net
{
  /// <summary>
  /// A data type for alphabets, for use with string-processing code that must 
  /// convert between an alphabet of size R and the integers 0 through R-1.
  /// </summary>
  /// <remarks><para>The members of Alphabet follows the .NET convention to use lower case to name constants.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Alphabet.java.html">Alphabet</a>
  /// implementation by the respective authors.</para></remarks>
  /// 
  public class Alphabet
  {
    /// <summary>
    /// The binary alphabet { 0, 1 }.</summary>
    ///
    public static readonly Alphabet Binary = new Alphabet("01");

    /// <summary>
    /// The octal alphabet { 0, 1, 2, 3, 4, 5, 6, 7 }.</summary>
    ///
    public static readonly Alphabet Octal = new Alphabet("01234567");

    /// <summary>
    /// The decimal alphabet { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.</summary>
    ///
    public static readonly Alphabet Decimal = new Alphabet("0123456789");

    /// <summary>
    /// The hexadecimal alphabet { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F }.</summary>
    ///
    public static readonly Alphabet Hexadecimal = new Alphabet("0123456789ABCDEF");

    /// <summary>
    /// The DNA alphabet { A, C, T, G }.</summary>
    ///
    public static readonly Alphabet Dna = new Alphabet("ACTG");

    /// <summary>
    /// The lowercase alphabet { a, b, c, ..., z }.</summary>
    ///
    public static readonly Alphabet LowerCase = new Alphabet("abcdefghijklmnopqrstuvwxyz");

    /// <summary>
    /// The uppercase alphabet { A, B, C, ..., Z }.</summary>
    ///

    public static readonly Alphabet UpperCase = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

    /// <summary>
    /// The protein alphabet { A, C, D, E, F, G, H, I, K, L, M, N, P, Q, R, S, T, V, W, Y }.</summary>
    ///
    public static readonly Alphabet Protein = new Alphabet("ACDEFGHIKLMNPQRSTVWY");

    /// <summary>
    /// The base-64 alphabet (64 characters).</summary>
    ///
    public static readonly Alphabet Base64 = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/");

    /// <summary>
    /// The ASCII alphabet (0-127).</summary>
    ///
    public static readonly Alphabet Ascii = new Alphabet(128);

    /// <summary>
    /// The extended ASCII alphabet (0-255).</summary>
    ///
    public static readonly Alphabet ExtendedAscii = new Alphabet(256);

    /// <summary>
    /// The Unicode 16 alphabet (0-65,535).</summary>
    ///
    public static readonly Alphabet Unicode16 = new Alphabet(65536);


    private char[] alphabet;      // the characters in the alphabet
    private int[] inverse;        // indices
    private int radix;            // the radix of the alphabet

    /// <summary>
    /// Initializes a new alphabet from the given set of characters.</summary>
    /// <param name="alpha">the set of characters</param>
    /// <exception cref="ArgumentException">If the given alphabet has a duplicate letter</exception>
    ///
    public Alphabet(string alpha)
    {

      // check that alphabet contains no duplicate chars
      bool[] unicode = new bool[char.MaxValue];
      for (int i = 0; i < alpha.Length; i++)
      {
        char c = alpha[i];
        if (unicode[c])
          throw new ArgumentException("Illegal alphabet: repeated character = '" + c + "'");
        unicode[c] = true;
      }

      alphabet = alpha.ToCharArray();
      radix = alpha.Length;
      inverse = new int[char.MaxValue];
      for (int i = 0; i < inverse.Length; i++)
        inverse[i] = -1;

      // can't use char since R can be as big as 65,536
      for (int c = 0; c < radix; c++)
        inverse[alphabet[c]] = c;
    }

    /// <summary>
    /// Initializes a new alphabet using characters 0 through R-1.</summary>
    /// <param name="R">the number of characters in the alphabet (the radix)</param>
    ///
    private Alphabet(int R)
    {
      alphabet = new char[R];
      inverse = new int[R];
      radix = R;

      // can't use char since R can be as big as 65,536
      for (int i = 0; i < R; i++)
        alphabet[i] = (char)i;
      for (int i = 0; i < R; i++)
        inverse[i] = i;
    }

    /// <summary>
    /// Initializes a new alphabet using characters 0 through 255.</summary>
    ///
    public Alphabet() : this(256)
    {
    }

    /// <summary>
    /// Returns true if the argument is a character in this alphabet.</summary>
    /// <param name="c">the character</param>
    /// <returns><c>true</c> if <c>c</c> is a character in this alphabet;
    ///        <c>false</c> otherwise</returns>
    ///
    public bool Contains(char c)
    {
      return inverse[c] != -1;
    }

    /// <summary>
    /// Returns the number of characters in this alphabet (the radix).</summary>
    /// <returns>the number of characters in this alphabet</returns>
    ///
    public int Radix
    {
      get { return radix; }
    }

    /// <summary>
    /// Returns the binary logarithm of the number of characters in this alphabet.</summary>
    /// <returns>the binary logarithm (rounded up) of the number of characters in this alphabet</returns>
    ///
    public int LgR
    {
      get
      {
        int lgR = 0;
        for (int t = radix - 1; t >= 1; t /= 2)
          lgR++;
        return lgR;
      }
    }

    /// <summary>
    /// Returns the index corresponding to the argument character.</summary>
    /// <param name="c">the character</param>
    /// <returns>the index corresponding to the character <c>c</c></returns>
    /// <exception cref="ArgumentException">unless <c>c</c> is a character in this alphabet</exception>
    ///
    public int ToIndex(char c)
    {
      if (c >= inverse.Length || inverse[c] == -1)
      {
        throw new ArgumentException("Character " + c + " not in alphabet");
      }
      return inverse[c];
    }

    /// <summary>
    /// Returns the indices corresponding to the argument characters.</summary>
    /// <param name="s">the characters</param>
    /// <returns>the indices corresponding to the characters <c>s</c></returns>
    /// <exception cref="ArgumentException">unless every character in <c>s</c>
    ///        is a character in this alphabet</exception>
    ///
    public int[] ToIndices(string s)
    {
      char[] source = s.ToCharArray();
      int[] target = new int[s.Length];
      for (int i = 0; i < source.Length; i++)
        target[i] = ToIndex(source[i]);
      return target;
    }

    /// <summary>
    /// Returns the character corresponding to the argument index.</summary>
    /// <param name="index">the index</param>
    /// <returns>the character corresponding to the index <c>index</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless <c>index</c> is between <c>0</c>
    ///        and <c>R - 1</c></exception>
    ///
    public char ToChar(int index)
    {
      if (index < 0 || index >= radix)
      {
        throw new IndexOutOfRangeException("Alphabet index out of bounds");
      }
      return alphabet[index];
    }

    /// <summary>
    /// Returns the characters corresponding to the argument indices.</summary>
    /// <param name="indices">the indices</param>
    /// <returns>the characters corresponding to the indices <c>indices</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless every index is between <c>0</c>
    /// and <c>R - 1</c></exception>
    ///
    public string ToChars(int[] indices)
    {
      StringBuilder s = new StringBuilder(indices.Length);
      for (int i = 0; i < indices.Length; i++)
        s.Append(ToChar(indices[i]));
      return s.ToString();
    }

    /// <summary>
    /// Demo test the <c>Alphabet</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    public static void MainTest(string[] args)
    {
      int[] encoded1 = Alphabet.Base64.ToIndices("NowIsTheTimeForAllGoodMen");
      string decoded1 = Alphabet.Base64.ToChars(encoded1);
      Console.WriteLine(decoded1);

      int[] encoded2 = Alphabet.Dna.ToIndices("AACGAACGGTTTACCCCG");
      string decoded2 = Alphabet.Dna.ToChars(encoded2);
      Console.WriteLine(decoded2);

      int[] encoded3 = Alphabet.Decimal.ToIndices("01234567890123456789");
      string decoded3 = Alphabet.Decimal.ToChars(encoded3);
      Console.WriteLine(decoded3);
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

