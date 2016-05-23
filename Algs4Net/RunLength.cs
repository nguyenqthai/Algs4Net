/******************************************************************************
 *  File name :    RunLength.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Compress or expand binary input from standard input using
 *  run-length encoding.
 *
 *  C:\> algscmd BinaryDump 40 < 4runs.bin 
 *  0000000000000001111111000000011111111111
 *  40 bits
 *
 *  This has runs of 15 0s, 7 1s, 7 0s, and 11 1s.
 *
 *  C:\> algscmd RunLength - 4runs.bin output.bin
 *  
 *  C:\> algscmd HexDump < output.bin
 *  0F 07 07 0B
 *  32 bits
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>RunLength</c> class provides methods for compressing
  /// and expanding a binary input using run-length coding with 8-bit
  /// run lengths.</summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/55compress">Section 5.5</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/RunLength.java.html">RunLength</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public sealed class RunLength
  {
    private const int R = 256;
    private const int LG_R = 8;

    private BinaryInput input;
    private BinaryOutput output;

    /// <summary>
    /// Uses file names to direct input and output
    /// </summary>
    /// <param name="outputFileName">user input file, empty if from console</param>
    /// <param name="inputFileName">user output file</param>
    public RunLength(string inputFileName, string outputFileName)
    {
      //Console.WriteLine("RunLength with InFile={0} and OutFile={1}", inputFileName, outputFileName);
      input = new BinaryInput(inputFileName);
      output = new BinaryOutput(outputFileName);
    }

    /// <summary>Reads a sequence of bits from standard input (that are encoded
    /// using run-length encoding with 8-bit run lengths); decodes them;
    /// and writes the results to standard output.</summary>
    ///
    public void Expand()
    {
      bool b = false;
      while (!input.IsEmpty)
      {
        int run = input.ReadInt(LG_R);
        for (int i = 0; i < run; i++)
          output.Write(b);
        b = !b;
      }
      output.Close();
    }

    /// <summary>
    /// Reads a sequence of bits from standard input; compresses
    /// them using run-length coding with 8-bit run lengths; and writes the
    /// results to standard output.</summary>
    ///
    public void Compress()
    {
      int run = 0;
      bool old = false;
      while (!input.IsEmpty)
      {
        bool b = input.ReadBoolean();
        if (b != old)
        {
          output.Write(run, LG_R);
          run = 1;
          old = !old;
        }
        else
        {
          if (run == R - 1)
          {
            output.Write(run, LG_R);
            run = 0;
            output.Write(run, LG_R);
          }
          run++;
        }
      }
      output.Write(run, LG_R);
      output.Close();
    }

    /// <summary>
    /// Sample client that calls <c>compress()</c> if the command-line
    /// argument is "-" an <c>expand()</c> if it is "+".</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd RunLength (-|+) input_file output_file")]
    public static void MainTest(string[] args)
    {
      RunLength algorithm = new RunLength(args[1], args[2]);
      if (args[0].Equals("-")) algorithm.Compress();
      else if (args[0].Equals("+")) algorithm.Expand();
      else throw new ArgumentException("Illegal command line argument");
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
