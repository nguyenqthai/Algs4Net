/******************************************************************************
 *  File name :    BinaryDump.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data file:    http://introcs.cs.princeton.edu/stdlib/abra.txt
 *  
 *  Reads in a binary file and writes out the bits, N per line.
 *
 *  C:\> type abra.txt 
 *  ABRACADABRA!
 *
 *  C:\> algscmd BinaryDump 16 < abra.txt
 *  0100000101000010
 *  0101001001000001
 *  0100001101000001
 *  0100010001000001
 *  0100001001010010
 *  0100000100100001
 *  96 bits
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>BinaryDump</c> class provides a client for displaying the contents
  /// of a binary file in binary.</para><para>
  /// For more full-featured versions, see the Unix utilities
  /// <c>od</c> (octal dump) and <c>hexdump</c> (hexadecimal dump).
  /// See also <seealso cref="HexDump"/>.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/55compress">Section 5.5</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BinaryDump.java.html">BinaryDump</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class BinaryDump
  {
    // Do not instantiate.
    private BinaryDump() { }

    /// <summary>Reads in a sequence of bytes from standard input and writes
    /// them to standard output in binary, k bits per line,
    /// where k is given as a command-line integer (defaults
    /// to 16 if no integer is specified); also writes the number
    /// of bits.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BinaryDump [chars_per_line] < 4runs.bin")]
    public static void MainTest(string[] args)
    {
      int bitsPerLine = 16;
      if (args.Length == 1)
      {
        bitsPerLine = int.Parse(args[0]);
      }
      BinaryInput input = new BinaryInput();

      int count;
      for (count = 0; !input.IsEmpty; count++)
      {
        if (bitsPerLine == 0)
        {
          input.ReadBoolean();
          continue;
        }
        else if (count != 0 && count % bitsPerLine == 0) Console.WriteLine();
        if (input.ReadBoolean()) Console.Write(1);
        else Console.Write(0);
      }
      if (bitsPerLine != 0) Console.WriteLine();
      Console.WriteLine(count + " bits");
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
