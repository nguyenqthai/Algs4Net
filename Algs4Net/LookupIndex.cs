/******************************************************************************
 *  File name :    LookupIndex.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/35applications/aminoI.csv
 *                http://algs4.cs.princeton.edu/35applications/movies.txt
 *
 *  C:\> algscmd LookupIndex aminoI.csv ","
 *  Serine
 *    TCT
 *    TCA
 *    TCG
 *    AGT
 *    AGC
 *  TCG
 *    Serine
 *
 *  C:\> algscmd LookupIndex movies.txt "/"
 *  Bacon, Kevin
 *    Animal House (1978)
 *    Apollo 13 (1995)
 *    Beauty Shop (2005)
 *    Diner (1982)
 *    Few Good Men, A (1992)
 *    Flatliners (1990)
 *    Footloose (1984)
 *    Friday the 13th (1980)
 *    ...
 *  Tin Men (1987)
 *    DeBoy, David
 *    Blumenfeld, Alan
 *    ...
 *
 ******************************************************************************/
using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>LookupIndex</c> class provides a data-driven client for reading in a
  /// key-value pairs from a file; then, printing the values corresponding to the
  /// keys found on standard input. Keys are strings; values are lists of strings.
  /// The separating delimiter is taken as a command-line argument. This client
  /// is sometimes known as an <c>Inverted index</c>.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/35applications">Section 3.5</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/LookupIndex.java.html">LookupIndex</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class LookupIndex
  {
    // Do not instantiate.
    private LookupIndex() { }

    /// <summary>
    /// Implementation of the <c>LookupIndex</c> client.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd LookupIndex aminoI.csv \", \"", "A csv file name and a separator")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();

      string filename = args[0];
      char[] separator = args[1].ToCharArray();
      TextInput input = new TextInput(filename);

      ST<string, LinkedQueue<string>> st = new ST<string, LinkedQueue<string>>();
      ST<string, LinkedQueue<string>> ts = new ST<string, LinkedQueue<string>>();

      while (input.HasNextLine())
      {
        string line = input.ReadLine();
        string[] fields = line.Split(separator);
        string key = fields[0];
        for (int i = 1; i < fields.Length; i++)
        {
          string val = fields[i];
          if (!st.Contains(key)) st[key] = new LinkedQueue<string>();
          if (!ts.Contains(val)) ts[val] = new LinkedQueue<string>();
          st[key].Enqueue(val);
          ts[val].Enqueue(key);
        }
      }

      Console.WriteLine("Done indexing");

      // read queries from standard input, one per line
      while (!StdIn.IsEmpty)
      {
        string query = StdIn.ReadLine();
        if (st.Contains(query))
          foreach (string vals in st[query])
            Console.WriteLine("  " + vals);
        if (ts.Contains(query))
          foreach (string keys in ts[query])
            Console.WriteLine("  " + keys);
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
