/******************************************************************************
 *  File name :    Count.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Create an alphabet specified on the command line, read in a 
 *  sequence of characters over that alphabet (ignoring characters
 *  not in the alphabet), computes the frequency of occurrence of
 *  each character, and print out the results.
 *
 *  C:\> algscmd Count ABCDR < abra.txt 
 *  A 5
 *  B 2
 *  C 1
 *  D 1
 *  R 2
 *
 *  C:\> algscmd Count 0123456789 < pi.txt
 *  0 9999
 *  1 10137
 *  2 9908
 *  3 10026
 *  4 9971
 *  5 10026
 *  6 10028
 *  7 10025
 *  8 9978
 *  9 9902
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Count</c> class provides an <seealso cref="Alphabet"/> client for reading
  /// in a piece of text and computing the frequency of occurrence of each
  /// character over a given alphabet.</summary>
  /// <remarks><para>
  /// For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/55compress">Section 5.5</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Count.java.html">Count</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Count
  {
    private Count() {}

    /// <summary>Reads in text from standard input; calculates the frequency of
    /// occurrence of each character over the alphabet specified as a
    /// commmand-line argument; and prints the frequencies to standard
    /// output.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Count ABCDR < abra.txt", "alphas-the string with all the alphabet's chars")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      Alphabet alphabet = new Alphabet(args[0]);
      int R = alphabet.Radix;
      int[] count = new int[R];
      while (StdIn.HasNextChar())
      {
        char c = StdIn.ReadChar();
        if (alphabet.Contains(c))
          count[alphabet.ToIndex(c)]++;
      }
      for (int c = 0; c < R; c++)
        Console.WriteLine(alphabet.ToChar(c) + " " + count[c]);
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
