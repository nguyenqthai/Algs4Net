/******************************************************************************
 *  File name :    FileIndex.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/35applications/ex1.txt
 *                http://algs4.cs.princeton.edu/35applications/ex2.txt
 *                http://algs4.cs.princeton.edu/35applications/ex3.txt
 *                http://algs4.cs.princeton.edu/35applications/ex4.txt
 *
 *  C:\> algscmd FileIndex ex*.txt
 *  age
 *   ex3.txt
 *   ex4.txt 
 * best
 *   ex1.txt 
 * was
 *   ex1.txt
 *   ex2.txt
 *   ex3.txt
 *   ex4.txt 
 * ^Z
 * 
 *  C:\> algscmd FileIndex *.txt
 *
 *  C:\> algscmd FileIndex *.java
 *
 ******************************************************************************/

using System;
using System.IO;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary>
  /// The <c>FileIndex</c> class provides a client for indexing a set of files,
  /// specified as command-line arguments. It takes queries from standard input
  /// and prints each file that contains the given query.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/35applications">Section 3.5</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/FileIndex.java.html">FileIndex</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class FileIndex
  {
    private FileIndex() { }

    /// <summary>
    /// Returns the list of files from the name-pattern list in the current directory
    /// </summary>
    /// <param name="namePatterns">file name that may consist of wildcard characters</param>
    /// <returns>the list of files</returns>
    public static string[] GetFileNames(string[] namePatterns)
    {
      DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory);
      List<string> fileNames = new List<string>();
      foreach (var pattern in namePatterns)
      {
        FileInfo[] fileInfos = di.GetFiles(pattern);
        foreach (var fi in fileInfos)
        {
          fileNames.Add(fi.Name);
        }
      }
      return fileNames.ToArray();
    }

    /// <summary>
    /// Implementation of the <c>FileIndex</c> client.
    /// </summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd FileIndex ex*.txt < keywords.txt")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      // key = word, value = set of files containing that word
      ST<string, SET<string>> st = new ST<string, SET<string>>();

      // create inverted index of all files
      Console.WriteLine("Indexing files");
      string[] allFileNames = FileIndex.GetFileNames(args);

      foreach (string filename in allFileNames)
      {
        Console.WriteLine("  " + filename);
        TextInput input = new TextInput(filename);
        while (!input.IsEmpty)
        {
          string word = input.ReadString();
          if (!st.Contains(word)) st[word] = new SET<string>();
          SET<string> set = st[word];
          set.Add(filename);
        }
      }

      // read queries from standard input, one per line
      while (!StdIn.IsEmpty)
      {
        string query = StdIn.ReadString();
        if (st.Contains(query))
        {
          SET<string> set = st[query];
          foreach (string filename in set)
          {
            Console.WriteLine("  " + filename);
          }
        }
      }
    }

  }

}

/******************************************************************************
/// Copyright 2002-2015, Robert Sedgewick and Kevin Wayne.
///
/// This file is part of algs4.jar, which accompanies the textbook
///
///     Algorithms, 4th edition by Robert Sedgewick and Kevin Wayne,
///     Addison-Wesley Professional, 2011, ISBN 0-321-57351-X.
///     http://algs4.cs.princeton.edu
///
///
/// algs4.jar is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// algs4.jar is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with algs4.jar.  If not, see http://www.gnu.org/licenses.
 ******************************************************************************/
