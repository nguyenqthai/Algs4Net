/******************************************************************************
 *  File name :    LookupCSV.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/35applications/DJIA.csv
 *                http://algs4.cs.princeton.edu/35applications/UPC.csv
 *                http://algs4.cs.princeton.edu/35applications/amino.csv
 *                http://algs4.cs.princeton.edu/35applications/elements.csv
 *                http://algs4.cs.princeton.edu/35applications/ip.csv
 *                http://algs4.cs.princeton.edu/35applications/morse.csv
 *  
 *  Reads in a set of key-value pairs from a multi-column CSV file using columns
 *  specified on the command line; then, reads in keys from standard
 *  input and prints out corresponding values.
 * 
 *  C:\> algscmd LookupCSV amino.csv 0 3     C:\> algscmd LookupCSV ip.csv 0 1 
 *  TTA                                www.google.com 
 *  Leucine                            216.239.41.99 
 *  ABC                               
 *  Not found                          C:\> algscmd LookupCSV ip.csv 1 0 
 *  TCT                                216.239.41.99 
 *  Serine                             www.google.com 
 *                                 
 *  C:\> algscmd LookupCSV amino.csv 3 0     C:\> algscmd LookupCSV DJIA.csv 0 1 
 *  Glycine                            29-Oct-29 
 *  GGG                                252.38 
 *                                     20-Oct-87 
 *                                     1738.74
 *
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>LookupCSV</c> class provides a data-driven client for reading in a
  /// key-value pairs from a file; then, printing the values corresponding to the
  /// keys found on standard input. Both keys and values are strings.
  /// The fields to serve as the key and value are taken as command-line arguments.
  /// </summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/35applications">Section 3.5</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/LookupCSV.java.html">LookupCSV</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class LookupCSV
  {
    // Do not instantiate.
    private LookupCSV() { }

    /// <summary>
    /// Demo test for the <c>LookupCSV</c> client.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd LookupCSV amino.csv col1 col2")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();

      int keyField = int.Parse(args[1]);
      int valField = int.Parse(args[2]);

      // symbol table
      ST<string, string> st = new ST<string, string>();

      // read in the data from csv file
      TextInput input = new TextInput(args[0]);

      while (input.HasNextLine())
      {
        string line = input.ReadLine();
        string[] tokens = line.Split(new char[] { ',' });
        string key = tokens[keyField];
        string val = tokens[valField];
        st[key] = val;
      }

      while (!StdIn.IsEmpty)
      {
        string s = StdIn.ReadString();
        if (st.Contains(s)) Console.WriteLine(st[s]);
        else Console.WriteLine("Not found");
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
