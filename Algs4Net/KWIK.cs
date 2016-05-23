/******************************************************************************
 *  File name :    KWIK.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/63suffix/tale.txt
 *
 *  Keyword-in-context search.
 *
 *  C:\> algscmd KWIK tale.txt 15
 *  majesty
 *   most gracious majesty king george th
 *  rnkeys and the majesty of the law fir
 *  on against the majesty of the people 
 *  se them to his majestys chief secreta
 *  h lists of his majestys forces and of
 *
 *  the worst
 *  w the best and the worst are known to y
 *  f them give me the worst first there th
 *  for in case of the worst is a friend in
 *  e roomdoor and the worst is over then a
 *  pect mr darnay the worst its the wisest
 *  is his brother the worst of a bad race 
 *  ss in them for the worst of health for 
 *   you have seen the worst of her agitati
 *  cumwented into the worst of luck buuust
 *  n your brother the worst of the bad rac
 *   full share in the worst of the day pla
 *  mes to himself the worst of the strife 
 *  f times it was the worst of times it wa
 *  ould hope that the worst was over well 
 *  urage business the worst will be over i
 *  clesiastics of the worst world worldly 
 *
 ******************************************************************************/

using System;
using System.Text.RegularExpressions;

namespace Algs4Net
{
  /// <summary>
  /// The <c>KWIK</c> class provides a <seealso cref="SuffixArray"/> client for computing
  /// all occurrences of a keyword in a given string, with surrounding context.
  /// This is known as <c>Keyword-in-context search</c>.</summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/63suffix">Section 6.3</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/KWIK.java.html">KWIK</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class KWIK
  {
    // Do not instantiate.
    private KWIK() { }

    /// <summary>
    /// Reads a string from a file specified as the first
    /// command-line argument; read an integer k specified as the
    /// second command line argument; then repeatedly processes
    /// use queries, printing all occurrences of the given query
    /// string in the text string with k characters of surrounding
    /// context on either side.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd KWIK tale.txt 15", "Followed by iterative query strings, i.e. majesty")]
    public static void MainTest(string[] args)
    {
      Regex WhiteSpace = new Regex(@"[\s]+", RegexOptions.Compiled);

      TextInput input = new TextInput(args[0]);
      int context = int.Parse(args[1]);

      // read in text
      string text = input.ReadAll();
      text = WhiteSpace.Replace(text, " ");
      int N = text.Length;

      // build suffix array
      SuffixArray sa = new SuffixArray(text);
      TextInput StdIn = new TextInput();
      // find all occurrences of queries and give context
      while (StdIn.HasNextLine())
      {
        string query = StdIn.ReadLine();
        for (int i = sa.Rank(query); i < N; i++)
        {
          int from1 = sa.Index(i);
          int to1 = Math.Min(N, from1 + query.Length);
          if (!query.Equals(text.Substring(from1, to1 - from1))) break;
          int from2 = Math.Max(0, sa.Index(i) - context);
          int to2 = Math.Min(N, sa.Index(i) + context + query.Length);
          Console.WriteLine(text.Substring(from2, to2 - from2));
        }
        Console.WriteLine();
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
