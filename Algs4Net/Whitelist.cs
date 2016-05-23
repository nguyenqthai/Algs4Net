/******************************************************************************
 *  File name :    Whitelist.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Data files:   http://algs4.cs.princeton.edu/11model/tinyW.txt
 *                http://algs4.cs.princeton.edu/11model/tinyT.txt
 *                http://algs4.cs.princeton.edu/11model/largeW.txt
 *                http://algs4.cs.princeton.edu/11model/largeT.txt
 *
 *  Whitelist filter.
 *
 *
 *  C:\> algscmd Whitelist tinyW.txt < tinyT.txt
 *  50
 *  99
 *  13
 *
 *  C:\> algscmd Whitelist largeW.txt < largeT.txt | more
 *  499569
 *  984875
 *  295754
 *  207807
 *  140925
 *  161828
 *  [367,966 total values]
 *
 ******************************************************************************/

using System;
using System.IO;


namespace Algs4Net
{
  class Whitelist
  {
    // Do not instantiate.
    private Whitelist() { }

    /// <summary>
    /// Reads in a sequence of integers from the whitelist file, specified as
    /// a command-line argument. Reads in integers from standard input and
    /// prints to standard output those integers that are not in the file.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Whitelist tinyW.txt < tinyT.txt",
      "Integers from a whitelist file and input integer to check the list")]
    public static void MainTest(string[] args)
    {
      TextInput input = new TextInput(args[0]);
      int[] white = input.ReadAllInts();
      input.Close();

      // remove duplicates, if any, so the ctor below will not throw an exception
      white = OrderHelper.RemoveDuplicates(white);
      StaticSETofInts set = new StaticSETofInts(white);

      TextInput StdIn = new TextInput();
      // Read key, print if not in whitelist.
      while (!StdIn.IsEmpty)
      {
        int key = StdIn.ReadInt();
        if (!set.Contains(key))
          Console.WriteLine(key);
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
