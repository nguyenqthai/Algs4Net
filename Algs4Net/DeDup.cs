/******************************************************************************
 *  File name :    DeDup.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/35applications/tinyTale.txt
 *
 *  Read in a list of words from standard input and print out
 *  each word, removing any duplicates.
 *
 *  C:\> type tinyTale.txt 
 *  it was the best of times it was the worst of times 
 *  it was the age of wisdom it was the age of foolishness 
 *  it was the epoch of belief it was the epoch of incredulity 
 *  it was the season of light it was the season of darkness 
 *  it was the spring of hope it was the winter of despair
 *
 *  C:\> algscmd DeDup < tinyTale.txt 
 *  it
 *  was
 *  the
 *  best
 *  of
 *  times
 *  worst
 *  age
 *  wisdom
 *  ...
 *  winter
 *  despair
 *
 ******************************************************************************/
using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>DeDup</c> class provides a client for reading in a sequence of
  /// words from standard input and printing each word, removing any duplicates.
  /// It is useful as a test client for various symbol table implementations.
  /// </summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/35applications">Section 3.5</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DeDup.java.html">DeDup</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DeDup
  {
    // Do not instantiate.
    private DeDup() { }

    /// <summary>
    /// Demo test for the <c>DeDup</c> client.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DeDup < tinyTale.txt")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();

      SET<string> set = new SET<string>();
      // read in strings and add to set
      while (!StdIn.IsEmpty)
      {
        string key = StdIn.ReadString();
        if (key.Equals("")) continue;
        if (!set.Contains(key))
        {
          set.Add(key);
          Console.WriteLine(key);
        }
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
