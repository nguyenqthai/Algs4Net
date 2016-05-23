/******************************************************************************
 *  File name :    Genome.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Compress or expand a genomic sequence using a 2-bit code.
 *  
 *  C:\> type output.txt
 *  ATAGATGCATAGCGCATAGCTAGATGTGCTAGC
 *  
 *  C:\> algscmd Genome - genomeTiny.txt output.bin
 *  
 *  C:\> algscmd HexDump 16 < output.bin
 *  00 00 00 21 32 39 32 64 C9 C8 EE 72 40
 *  104 bits
 *  C\> algscmd Genome + output.bin output.txt
 *  
 *  C:\> type output.txt
 *  ATAGATGCATAGCGCATAGCTAGATGTGCTAGC
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Genome</c> class provides static methods for compressing
  /// and expanding a genomic sequence using a 2-bit code.</summary>
  /// <remarks><para>
  /// For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/55compress">Section 5.5</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Genome.java.html">Genome</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Genome
  {
    private static readonly Alphabet DNA = new Alphabet("ACGT"); // could use Alphabet.Dna
    private BinaryInput input;
    private BinaryOutput output;

    // Do not instantiate.
    private Genome() { }

    /// <summary>
    /// Uses file names to direct input and output
    /// </summary>
    /// <param name="outputFileName">user input file, empty if from console</param>
    /// <param name="inputFileName">user output file</param>
    public Genome(string inputFileName, string outputFileName)
    {
      //Console.WriteLine("Genome with InFile={0} and OutFile={1}", inputFileName, outputFileName);
      input = new BinaryInput(inputFileName);
      output = new BinaryOutput(outputFileName);
    }

    /// <summary>
    /// Reads a sequence of 8-bit extended ASCII characters over the alphabet
    /// { A, C, T, G } from standard input; compresses them using two bits per
    /// character; and writes the results to standard output.</summary>
    ///
    public void Compress()
    {
      string s = input.ReadString();
      int N = s.Length;
      output.Write(N);

      // Write two-bit code for char.
      for (int i = 0; i < N; i++)
      {
        int d = DNA.ToIndex(s[i]);
        output.Write(d, 2);
      }
      output.Close();
      input.Close();
    }

    /// <summary>
    /// Reads a binary sequence from standard input; converts each two bits
    /// to an 8-bit extended ASCII character over the alphabet { A, C, T, G };
    /// and writes the results to standard output.</summary>
    ///
    public void Expand()
    {
      int N = input.ReadInt();
      // Read two bits; write char.
      for (int i = 0; i < N; i++)
      {
        char c = input.ReadChar(2);
        output.Write(DNA.ToChar(c), 8);
      }
      output.Close();
      input.Close();
    }

    /// <summary>
    /// Sample client that calls <c>compress()</c> if the command-line
    /// argument is "-" an <c>expand()</c> if it is "+".</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Genome (-|+) input_file output_file")]
    public static void MainTest(string[] args)
    {
      Genome algorithm = new Genome(args[1], args[2]);
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
