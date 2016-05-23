/******************************************************************************
 *  File name :    FrequencyCounter.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/31elementary/tnyTale.txt
 *                http://algs4.cs.princeton.edu/31elementary/tale.txt
 *                http://algs4.cs.princeton.edu/31elementary/leipzig100K.txt
 *                http://algs4.cs.princeton.edu/31elementary/leipzig300K.txt
 *                http://algs4.cs.princeton.edu/31elementary/leipzig1M.txt
 *
 *  Read in a list of words from standard input and print out
 *  the most frequently occurring word that has length greater than
 *  a given threshold.
 *
 *  C:\> algscmd FrequencyCounter 1 < tinyTale.txt
 *  it 10
 *
 *  C:\> algscmd FrequencyCounter 8 < tale.txt
 *  business 122
 *
 *  C:\> algscmd FrequencyCounter 10 < leipzig1M.txt
 *  government 24763
 *
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>FrequencyCounter</c> class provides a client for
  /// reading in a sequence of words and printing a word (exceeding
  /// a given length) that occurs most frequently. It is useful as
  /// a test client for various symbol table implementations.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/31elementary">Section 3.1</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/FrequencyCounter.java.html">FrequencyCounter</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class FrequencyCounter
  {

    // Do not instantiate. Place holder for future work
    private FrequencyCounter() { }

    /// <summary>
    /// Reads in a command-line integer and sequence of words from
    /// standard input and prints out a word (whose length exceeds
    /// the threshold) that occurs most frequently to standard output.
    /// It also prints out the number of words whose length exceeds
    /// the threshold and the number of distinct such words.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd FrequencyCounter Max < tinyTale.txt", "Max word length and input text")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput("tale.txt");

      int distinct = 0, words = 0;
      int minlen = int.Parse(args[0]);
      ST<string, int> st = new ST<string, int>();

      int count = 0;
      // compute frequency counts
      while (!StdIn.IsEmpty)
      {
        string key = StdIn.ReadString();
        count++;
        if (key.Length < minlen) continue;
        words++;
        if (st.Contains(key))
        {
          st[key] = st[key] + 1;
        }
        else
        {
          st[key] = 1;
          distinct++;
        }
      }

      // find a key with the highest frequency count
      string max = "";
      st[max] = 0; ;
      foreach (string word in st.Keys())
      {
        if (st[word] > st[max])
          max = word;
      }

      Console.WriteLine(max + " " + st[max]);
      Console.WriteLine("distinct = " + distinct);
      Console.WriteLine("words    = " + words);
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
