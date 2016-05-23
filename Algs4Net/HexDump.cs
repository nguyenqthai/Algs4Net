/******************************************************************************
 *  File name :    HexDump.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data file:    http://algs4.cs.princeton.edu/55compression/abra.txt
 *  
 *  Reads in a binary file and writes out the bytes in hex, 16 per line.
 *
 *  C:\> type abra.txt
 *  ABRACADABRA!
 *
 *  C:\> algscmd HexDump 16 < abra.txt
 *  41 42 52 41 43 41 44 41 42 52 41 21
 *  96 bits
 *
 *
 *  Remark
 *  --------------------------
 *   - Similar to the Unix utilities od (octal dump) or hexdump (hexadecimal dump).
 *
 *  % od -t x1 < abra.txt 
 *  0000000 41 42 52 41 43 41 44 41 42 52 41 21
 *  0000014
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>HexDump</c> class provides a client for displaying the contents
  /// of a binary file in hexadecimal.</para><para>
  /// See also <seealso cref="BinaryDump"/>. For more full-featured versions, 
  /// see the Unix utilities
  /// <c>od</c> (octal dump) and <c>hexdump</c> (hexadecimal dump).
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/55compress">Section 5.5</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/HexDump.java.html">HexDump</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class HexDump
  {

    // Do not instantiate.
    private HexDump() { }

    /// <summary>
    /// Reads in a sequence of bytes from standard input and writes
    /// them to standard output using hexademical notation, k hex digits
    /// per line, where k is given as a command-line integer (defaults
    /// to 16 if no integer is specified); also writes the number
    /// of bits.</summary>
    /// <param name="args">Place holder for user arguments</param>
    [HelpText("algscmd HexDump [chars_per_line] < 4runs.bin")]
    public static void MainTest(string[] args)
    {
      int bytesPerLine = 16;
      if (args.Length == 1)
      {
        bytesPerLine = int.Parse(args[0]);
      }
      BinaryInput input;
      if (args.Length == 2)
        input = new BinaryInput(args[1]);
      else
        input = new BinaryInput();
      int i;
      for (i = 0; !input.IsEmpty; i++)
      {
        if (bytesPerLine == 0)
        {
          input.ReadChar();
          continue;
        }
        if (i == 0) Console.Write("");
        else if (i % bytesPerLine == 0) Console.Write("\n", i);
        else Console.Write(" ");
        char c = input.ReadChar();
        Console.Write("{0:X2}", c & 0xff);
      }
      if (bytesPerLine != 0) Console.WriteLine();
      Console.WriteLine((i * 8) + " bits");
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
