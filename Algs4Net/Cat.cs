/******************************************************************************
 *  File name :    Cat.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Reads in text files specified as the first command-line 
 *  arguments, concatenates them, and writes the result to
 *  filename specified as the last command-line arguments.
 *
 *  C:\> type in1.txt
 *  This is
 *
 *  C:\> type in2.txt 
 *  a tiny
 *  test.
 * 
 *  C:\> algscmd Cat in1.txt in2.txt out.txt
 *
 *  C:\> type out.txt
 *  This is
 *  a tiny
 *  test.
 *
 ******************************************************************************/

using System;
using System.IO;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Cat</c> class provides a client for concatenating the results
  /// of several text files.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/11model">Section 1.1</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Cat.java.html">Cat</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Cat
  {
    // this class should not be instantiated
    private Cat() { }

    /// <summary>
    /// Reads in a sequence of text files specified as the first command-line
    /// arguments, concatenates them, and writes the results to the file
    /// specified as the last command-line argument. The code demonstrates
    /// .NET's using statements and Stream I/O classes. If a single file name
    /// is provided, the file is rewritten using its content (unchanged).</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Cat in1.txt in2.txt out.txt",
      "Input file names followed by an output file name")]
    public static void MainTest(string[] args)
    {
      using (StreamWriter output = new StreamWriter(args[args.Length - 1]))
      {
        for (int i = 0; i < args.Length - 1; i++)
        {
          using (StreamReader input = new StreamReader(args[i]))
          {
            string s = input.ReadToEnd().TrimEnd(); // remove EOF space chars
            output.WriteLine(s);
          }
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
