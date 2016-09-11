/******************************************************************************
 * File name :    BinaryOutput.cs
 * Demo test :    Use the algscmd util or Visual Studio IDE
 *           :    Enter algscmd alone for how to use the util
 *
 * Write binary data to standard output, either one 1-bit boolean,
 * one 8-bit char, one 32-bit int, one 64-bit double, one 32-bit float,
 * or one 64-bit long at a time.
 *
 * The bytes written are not aligned.
 * C:\> algscmd BinaryOutput 5 output.bin
 *  
 * C:\> algscmd HexDump < output.bin
 * 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03
 * 00 00 00 04
 * 160 bits
 *
 ******************************************************************************/

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// <c>Binary standard output</c>. This class provides methods for converting
  /// primtive type variables (<c>boolean</c>, <c>byte</c>, <c>char</c>,
  /// <c>int</c>, <c>long</c>, <c>float</c>, and <c>double</c>)
  /// to sequences of bits and writing them to standard output.
  /// Uses .NET representations, in little-endian (least-significant byte first).</para><para>
  /// The client must <c>Flush()</c> the output stream when finished writing bits and 
  /// should not intermixing calls to <c>output</c> with calls
  /// to <c>Console</c> or <c>Console.Out</c>; otherwise unexpected behavior 
  /// will result.</para></summary>
  /// <remarks>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BinaryStdOut.java.html">output</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</remarks>
  ///
  public sealed class BinaryOutput
  {
    private BinaryWriter output;

    private static int buffer;     // 8-bit buffer of bits to write out
    private static int n;          // number of bits remaining in buffer

    private BinaryOutput() { }      // do not use

    /// <summary>
    /// Represents an output stream from a source, which may not be the standard output
    /// </summary>
    /// <param name="outputFileName">the output file name</param>
    /// 
    public BinaryOutput(string outputFileName)
    {
      try
      {
        output = new BinaryWriter(File.Create(outputFileName));
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine("File create with error: {0}", ex.Message);
      }
    }

    /// Write the specified bit to standard output.
    ///
    private void writeBit(bool bit)
    {
      // add bit to buffer
      buffer <<= 1;
      if (bit) buffer |= 1;

      // if buffer is full (8 bits), write out as a single byte
      n++;
      if (n == 8) clearBuffer();
    }

    /// <summary>
    /// Write the 8-bit byte to standard output.</summary>
    ///
    private void writeByte(int x)
    {
      Debug.Assert(x >= 0 && x < 256);

      // optimized if byte-aligned
      if (n == 0)
      {
        try
        {
          output.Write((byte)x);
          //string s = string.Format("{0:D8}", Convert.ToString((byte)x, 2));
          //log.Append(s);
        }
        catch (IOException e)
        {
          Console.Error.WriteLine(e.StackTrace);
        }
        return;
      }
      // otherwise write one bit at a time
      for (int i = 0; i < 8; i++)
      {
        bool bit = ((x >> (8 - i - 1)) & 1) == 1;
        writeBit(bit);
      }
    }

    // write out any remaining bits in buffer to standard output, padding with 0s
    private void clearBuffer()
    {
      if (n == 0) return;
      if (n > 0) buffer <<= (8 - n);
      try
      {
        output.Write((byte)buffer);
        //string s = string.Format("{0:D8}", Convert.ToString((byte)buffer, 2));
        //log.Append(s);

      }
      catch (IOException e)
      {
        Console.Error.WriteLine(e.StackTrace);
      }
      n = 0;
      buffer = 0;
    }

    /// <summary>
    /// Flush standard output, padding 0s if number of bits written so far
    /// is not a multiple of 8.</summary>
    ///
    public void Flush()
    {
      clearBuffer();
      try
      {
        output.Flush();
      }
      catch (IOException e)
      {
        Console.Error.WriteLine(e.StackTrace);
      }
    }

    /// <summary>
    /// Flush and close standard output. Once standard output is closed, you can no
    /// longer write bits to it.</summary>
    ///
    public void Close()
    {
      Flush();
      try
      {
        output.Close();
      }
      catch (IOException e)
      {
        Console.Error.WriteLine(e.StackTrace);
      }
    }


    /// <summary>
    /// Write the specified bit to standard output.</summary>
    /// <param name="x">the <c>bool</c> to write.</param>
    ///
    public void Write(bool x)
    {
      writeBit(x);
    }

    /// <summary>
    /// Write the 8-bit byte to standard output.</summary>
    /// <param name="x">the <c>byte</c> to write.</param>
    ///
    public void Write(byte x)
    {
      writeByte(x & 0xff);
    }

    /// <summary>
    /// Write the 32-bit int to standard output.</summary>
    /// <param name="x">the <c>int</c> to write.</param>
    ///
    public void Write(int x)
    {
      writeByte((x >> 24) & 0xff);
      writeByte((x >> 16) & 0xff);
      writeByte((x >> 8) & 0xff);
      writeByte((x >> 0) & 0xff);
    }

    /// <summary>
    /// Write the r-bit int to standard output.</summary>
    /// <param name="x">the <c>int</c> to write.</param>
    /// <param name="r">the number of relevant bits in the char.</param>
    /// <exception cref="ArgumentException">if <c>r</c> is not between 1 and 32.</exception>
    /// <exception cref="ArgumentException">if <c>x</c> is not between 0 and 2<sup>r</sup> - 1.</exception>
    ///
    public void Write(int x, int r)
    {
      if (r == 32)
      {
        Write(x);
        return;
      }
      if (r < 1 || r > 32) throw new ArgumentException("Illegal value for r = " + r);
      if (x < 0 || x >= (1 << r)) throw new ArgumentException("Illegal " + r + "-bit char = " + x);
      for (int i = 0; i < r; i++)
      {
        bool bit = ((x >> (r - i - 1)) & 1) == 1;
        writeBit(bit);
      }
    }

    /// <summary>
    /// Write the 64-bit double to standard output.</summary>
    /// <param name="x">the <c>double</c> to write.</param>
    ///
    public void Write(double x)
    {
      Write(BitConverter.DoubleToInt64Bits(x));
    }

    /// <summary>
    /// Write the 64-bit long to standard output.</summary>
    /// <param name="x">the <c>long</c> to write.</param>
    ///
    public void Write(long x)
    {
      writeByte((int)((x >> 56) & 0xff));
      writeByte((int)((x >> 48) & 0xff));
      writeByte((int)((x >> 40) & 0xff));
      writeByte((int)((x >> 32) & 0xff));
      writeByte((int)((x >> 24) & 0xff));
      writeByte((int)((x >> 16) & 0xff));
      writeByte((int)((x >> 8) & 0xff));
      writeByte((int)((x >> 0) & 0xff));
    }

    /// <summary>
    /// Write the 32-bit float to standard output.</summary>
    /// <param name="x">the <c>float</c> to write.</param>
    ///
    public void Write(float x)
    {
      throw new NotImplementedException("Write float not yet implemented");
    }

    /// <summary>
    /// Write the 16-bit int to standard output.</summary>
    /// <param name="x">the <c>short</c> to write.</param>
    ///
    public void Write(short x)
    {
      writeByte((x >> 8) & 0xff);
      writeByte((x >> 0) & 0xff);
    }

    /// <summary>
    /// Write the 8-bit char to standard output.</summary>
    /// <param name="x">the <c>char</c> to write.</param>
    /// <exception cref="ArgumentException">if <c>x</c> is not betwen 0 and 255.</exception>
    ///
    public void Write(char x)
    {
      if (x < 0 || x >= 256) throw new ArgumentException("Illegal 8-bit char = " + x);
      writeByte(x);
    }

    /// <summary>
    /// Write the r-bit char to standard output.</summary>
    /// <param name="x">the <c>char</c> to write.</param>
    /// <param name="r">the number of relevant bits in the char.</param>
    /// <exception cref="ArgumentException">if <c>r</c> is not between 1 and 16.</exception>
    /// <exception cref="ArgumentException">if <c>x</c> is not between 0 and 2<sup>r</sup> - 1.</exception>
    ///
    public void Write(char x, int r)
    {
      if (r == 8)
      {
        Write(x);
        return;
      }
      if (r < 1 || r > 16) throw new ArgumentException("Illegal value for r = " + r);
      if (x >= (1 << r)) throw new ArgumentException("Illegal " + r + "-bit char = " + x);
      for (int i = 0; i < r; i++)
      {
        bool bit = ((x >> (r - i - 1)) & 1) == 1;
        writeBit(bit);
      }
    }

    /// <summary>
    /// Write the string of 8-bit characters to standard output.</summary>
    /// <param name="s">the <c>string</c> to write.</param>
    /// <exception cref="ArgumentException">if any character in the string is not
    /// between 0 and 255.</exception>
    ///
    public void Write(string s)
    {
      for (int i = 0; i < s.Length; i++)
        Write(s[i]);
    }

    /// <summary>
    /// Write the string of r-bit characters to standard output.</summary>
    /// <param name="s">the <c>string</c> to write.</param>
    /// <param name="r">the number of relevants bits in each character.</param>
    /// <exception cref="ArgumentException">if r is not between 1 and 16.</exception>
    /// <exception cref="ArgumentException">if any character in the string is not
    /// between 0 and 2<sup>r</sup> - 1.</exception>
    ///
    public void Write(string s, int r)
    {
      for (int i = 0; i < s.Length; i++)
        Write(s[i], r);
    }

    // TODO: Add this to a unit test
    private static void BinaryStdOutTest2(string[] args)
    {
      // prepare the memory stream
      MemoryStream m = new MemoryStream();

      byte[] bytes;
      m.Write(new byte[9] { 0x88, 0x80, 0x81, 0x03, 0x04, 0x81, 0x44, 0x22, 0x55 }, 0, 9);
      foreach (char c in "Hello µ-λ-β!")
      {
        bytes = BitConverter.GetBytes(c);
        m.Write(bytes, 0, bytes.Length);
      }
      double inf = double.PositiveInfinity;
      bytes = BitConverter.GetBytes(inf);
      m.Write(bytes, 0, bytes.Length);

      float inf32 = float.PositiveInfinity;
      bytes = BitConverter.GetBytes(inf32);
      m.Write(bytes, 0, bytes.Length);
      m.Position = 0;
      // bytes = m.ToArray() TBD
      using (BufferedStream reader = new BufferedStream(m))
      {
        BinaryOutput output = new BinaryOutput(args[0]);
        while (reader.CanRead)
        {
          int b = reader.ReadByte();
          if (b == -1) break;
          output.Write((byte)b);
        }
        output.Flush();
      }
    }

    /// <summary>Test client.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BinaryOutput N output.bin", "Writes N integers to a binary file")]
    public static void MainTest(string[] args)
    {
      int T = int.Parse(args[0]);
      // write to output file
      BinaryOutput writer = new BinaryOutput(args[1]);
      for (int i = 0; i < T; i++)
      {
        writer.Write(i);
      }
      writer.Flush();
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
