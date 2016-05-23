/******************************************************************************
 * File name :    LZW.cs
 * Demo test :    Use the algscmd util or Visual Studio IDE
 *           :    Enter algscmd alone for how to use the util
 *
 * Compress or expand binary input from standard input using LZW.
 *  
 * C:\> algscmd LZW - abra.txt output.txt
 * 
 * C:\> algscmd BinaryDump 60 < output.txt
 * 000001000001000001000010000001010010000001000001000001000011
 * 000001000001000001000100000100000001000100000011000000100001
 * 0001000000000000
 * 136 bits
 * 
 * C:\> algscmd LZW + output.txt abra2.txt
 * 
 * C:\> type abra2.txt
 * ABRACADABRA!
 * C:\> algscmd BinaryDump 60 < abra.txt
 * 010000010100001001010010010000010100001101000001010001000100
 * 000101000010010100100100000100100001
 * 96 bits
 * 
 * C:\> algscmd BinaryDump 60 < abra2.txt
 * BinaryDump 60
 * 
 * 010000010100001001010010010000010100001101000001010001000100
 * 000101000010010100100100000100100001
 * 96 bits
 *
 ******************************************************************************/
using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>LZW</c> class provides methods for compressing
  /// and expanding a binary input using LZW compression over the 8-bit extended
  /// ASCII alphabet with 12-bit codewords.</summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/55compress">Section 5.5</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/LZW.java.html">LZW</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class LZW
  {
    private const int R = 256;        // number of input chars
    private const int L = 4096;       // number of codewords = 2^W
    private const int W = 12;         // codeword width

    private BinaryInput input;
    private BinaryOutput output;

    // Do not instantiate.
    private LZW() { }

    /// <summary>
    /// Uses file names to direct input and output
    /// </summary>
    /// <param name="outputFileName">user input file, empty if from console</param>
    /// <param name="inputFileName">user output file</param>
    public LZW(string inputFileName, string outputFileName)
    {
      //Console.WriteLine("LZW with InFile={0} and OutFile={1}", inputFileName, outputFileName);
      input = new BinaryInput(inputFileName);
      output = new BinaryOutput(outputFileName);
    }

    /// <summary>
    /// Reads a sequence of 8-bit bytes from standard input; compresses
    /// them using LZW compression with 12-bit codewords; and writes the results
    /// to standard output.</summary>
    ///
    public void Compress()
    {
      string inputChars = input.ReadString();
      TST<int> st = new TST<int>();
      for (int i = 0; i < R; i++)
        st.Put("" + (char)i, i);
      int code = R + 1;  // R is codeword for EOF

      while (inputChars.Length > 0)
      {
        string s = st.LongestPrefixOf(inputChars);  // Find max prefix match s.
        output.Write(st[s], W);      // Print s's encoding.
        int t = s.Length;
        if (t < inputChars.Length && code < L)    // Add s to symbol table.
          st.Put(inputChars.Substring(0, t + 1), code++);
        inputChars = inputChars.Substring(t);            // Scan past s in input.
      }
      output.Write(R, W);
      output.Close();
    }

    /// <summary>
    /// Reads a sequence of bit encoded using LZW compression with
    /// 12-bit codewords from standard input; expands them; and writes
    /// the results to standard output.</summary>
    ///
    public void Expand()
    {
      string[] st = new string[L];
      int i; // next available codeword value

      // initialize symbol table with all 1-character strings
      for (i = 0; i < R; i++)
        st[i] = "" + (char)i;
      st[i++] = "";                        // (unused) lookahead for EOF

      int codeword = input.ReadInt(W);
      if (codeword == R) return;           // expanded message is empty string
      string val = st[codeword];

      while (true)
      {
        output.Write(val);
        codeword = input.ReadInt(W);
        if (codeword == R) break;
        string s = st[codeword];
        if (i == codeword) s = val + val[0];   // special case hack
        if (i < L) st[i++] = val + s[0];
        val = s;
      }
      output.Close();
    }

    /// <summary>
    /// Sample client that calls <c>compress()</c> if the command-line
    /// argument is "-" an <c>expand()</c> if it is "+".</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd LZW (-|+) input_file output_file")]
    public static void MainTest(string[] args)
    {
      LZW algorithm = new LZW(args[1], args[2]);
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
