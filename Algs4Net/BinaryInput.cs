/******************************************************************************
 *  File name :    BinaryStdIn.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Supports reading binary data from standard input.
 *
 * C:\> algscmd BinaryInput output.bin < 4runs.bin
 * 
 * C:\> algscmd HexDump < output.bin
 * 00 01 FC 07 FF
 * 40 bits
 *
 ******************************************************************************/

using System;
using System.IO;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// <c>Binary standard input</c>. This class provides methods for reading
  /// in bits from standard input, either one bit at a time (as a <c>boolean</c>),
  /// 8 bits at a time (as a <c>byte</c> or <c>char</c>),
  /// 16 bits at a time (as a <c>short</c>), 32 bits at a time
  /// (as an <c>int</c> or <c>float</c>), or 64 bits at a time (as a
  /// <c>double</c> or <c>long</c>).</para>
  /// <para>All primitive types are assumed to be represented using their 
  /// standard .NET representations, in little-endian (least significant
  /// byte first) order.</para><para>
  /// The client should not intermix calls to <c>BinaryStdIn</c> with calls
  /// to <c>StdIn</c> or <c>Console.In</c>;
  /// otherwise unexpected behavior will result.</para></summary>
  /// <remarks>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BinaryStdIn.java.html">BinaryStdIn</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</remarks>
  ///
  public sealed class BinaryInput : IDisposable
  {
    private BinaryReader input; // input stream instance
    private const int EOF = -1;   // end of file

    private int buffer;           // one byte buffer
    private int n;                // number of bits left in buffer

    private bool disposed = false;
    /// <summary>
    /// Represents an input stream from a source, which may not be the standard input
    /// </summary>
    /// <param name="inputFileName">the input file name, empty if console</param>
    /// <exception cref="IOException">if a reading error is encoutered</exception>
    /// 
    public BinaryInput(string inputFileName = "")
    {
      if (inputFileName.Equals(""))
      {
        input = new BinaryReader(Console.OpenStandardInput());
      }
      else
      {
        try
        {
          input = new BinaryReader(File.OpenRead(inputFileName));
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("File open error: {0}", ex.Message);
        }
      }
      fillBuffer();
    }

    private void fillBuffer()
    {
      try
      {
        buffer = input.ReadByte();
        n = (buffer == EOF) ? -1 : 8;
      }
      catch (IOException)
      {
        buffer = EOF;
        n = -1;
      }
    }

    /// <summary>
    /// Close this input stream and release any associated system resources.</summary>
    ///
    public void Close()
    {
      try
      {
        input.Close();
      }
      catch (IOException e)
      {
        throw new IOException(string.Format("BinaryStdIn error {0}", e.Message));
      }
    }

    /// <summary>
    /// Returns true if standard input is empty.</summary>
    /// <returns>true if and only if standard input is empty</returns>
    ///
    public bool IsEmpty
    {
      get { return buffer == EOF; }
    }

    /// <summary>
    /// Reads the next bit of data from standard input and return as a boolean.</summary>
    /// <returns>the next bit of data from standard input as a <c>boolean</c></returns>
    /// <exception cref="IOException">if standard input is empty</exception>
    ///
    public bool ReadBoolean()
    {
      if (IsEmpty) throw new IOException("Reading from empty input stream");
      n--;
      bool bit = ((buffer >> n) & 1) == 1;
      if (n == 0) fillBuffer();
      return bit;
    }

    /// <summary>
    /// Reads the next 8 bits from standard input and return as an 8-bit char.
    /// Note that <c>char</c> is a 16-bit type;
    /// to read the next 16 bits as a char, use <c>readChar(16)</c>.</summary>
    /// <returns>the next 8 bits of data from standard input as a <c>char</c></returns>
    /// <exception cref="IOException">if there are fewer than 8 bits available on standard input</exception>
    ///
    public char ReadChar()
    {
      if (IsEmpty) throw new IOException("Reading from empty input stream");

      // special case when aligned byte
      int x = default(int);
      if (n == 8)
      {
        x = buffer;
        fillBuffer();
        return (char)(x & 0xff);
      }

      // combine last n bits of current buffer with first 8-n bits of new buffer
      x = buffer;
      x <<= (8 - n);
      int oldN = n;
      fillBuffer();
      if (IsEmpty) throw new IOException("Reading from empty input stream");
      n = oldN;
      x |= (buffer >> n);
      return (char)(x & 0xff);
      // the above code doesn't quite work for the last character if n = 8
      // because buffer will be -1, so there is a special case for aligned byte
    }

    /// <summary>
    /// Reads the next r bits from standard input and return as an r-bit character.</summary>
    /// <param name="r">number of bits to read.</param>
    /// <returns>the next r bits of data from standard input as a <c>char</c></returns>
    /// <exception cref="ArgumentException">if there are fewer than r bits available on standard input</exception>
    /// <exception cref="ArgumentException">unless 1 &lt;= r &lt;= 16</exception>
    ///
    public char ReadChar(int r)
    {
      if (r < 1 || r > 16) throw new ArgumentException("Illegal value of r = " + r);

      // optimize r = 8 case
      if (r == 8) return ReadChar();

      int x = 0;
      for (int i = 0; i < r; i++)
      {
        x <<= 1;
        bool bit = ReadBoolean();
        if (bit) x |= 1;
      }
      return (char)(x & 0xffff);
    }

    /// <summary>
    /// Reads the remaining bytes of data from standard input and return as a 
    /// string. The method is similar to the <see cref="System.IO.TextReader.ReadToEnd"/> 
    /// method of the .NET Framework.</summary>
    /// <returns>the remaining bytes of data from standard input as a <c>string</c></returns>
    /// <exception cref="IOException">if standard input is empty or if the number of bits
    /// available on standard input is not a multiple of 8 (byte-aligned)</exception>
    ///
    public string ReadString()
    {
      // Debug 16-bit char behavior in .NET
      if (IsEmpty) throw new IOException("Reading from empty input stream");

      StringBuilder sb = new StringBuilder();
      while (!IsEmpty)
      {
        char c = ReadChar();
        sb.Append(c);
      }
      return sb.ToString();
    }

    /// <summary>
    /// Reads the next 16 bits from standard input and return as a 16-bit short.</summary>
    /// <returns>the next 16 bits of data from standard input as a <c>short</c></returns>
    /// <exception cref="IOException">if there are fewer than 16 bits available on standard input</exception>
    ///
    public short ReadShort()
    {
      int x = 0;
      for (int i = 0; i < 2; i++)
      {
        int c = ReadChar();
        x <<= 8;
        x |= c;
      }
      return (short)x;
    }

    /// <summary>
    /// Reads the next 32 bits from standard input and return as a 32-bit int.</summary>
    /// <returns>the next 32 bits of data from standard input as a <c>int</c></returns>
    /// <exception cref="IOException">if there are fewer than 32 bits available on standard input</exception>
    ///
    public int ReadInt()
    {
      int x = 0;
      for (int i = 0; i < 4; i++)
      {
        char c = ReadChar();
        x <<= 8;
        x |= c;
      }
      return x;
    }

    /// <summary>
    /// Reads the next r bits from standard input and return as an r-bit int.</summary>
    /// <param name="r">number of bits to read.</param>
    /// <returns>the next r bits of data from standard input as a <c>int</c></returns>
    /// <exception cref="ArgumentException">if there are fewer than r bits available on standard input</exception>
    /// <exception cref="ArgumentException">unless 1 &lt;= r &lt;= 32</exception>
    ///
    public int ReadInt(int r)
    {
      if (r < 1 || r > 32) throw new ArgumentException("Illegal value of r = " + r);

      // optimize r = 32 case
      if (r == 32) return ReadInt();

      int x = 0;
      for (int i = 0; i < r; i++)
      {
        x <<= 1;
        bool bit = ReadBoolean();
        if (bit) x |= 1;
      }
      return x;
    }

    /// <summary>
    /// Reads the next 64 bits from standard input and return as a 64-bit long.</summary>
    /// <returns>the next 64 bits of data from standard input as a <c>long</c></returns>
    /// <exception cref="IOException">if there are fewer than 64 bits available on standard input</exception>
    ///
    public long ReadLong()
    {
      long x = 0;
      for (int i = 0; i < 8; i++)
      {
        char c = ReadChar();
        x <<= 8;
        x |= c;
      }
      return x;
    }


    /// <summary>
    /// Reads the next 64 bits from standard input and return as a 64-bit double.</summary>
    /// <returns>the next 64 bits of data from standard input as a <c>double</c></returns>
    /// <exception cref="IOException">if there are fewer than 64 bits available on standard input</exception>
    ///
    public double ReadDouble()
    {
      long bit64 = ReadLong();
      return BitConverter.Int64BitsToDouble(bit64);
    }

    /// <summary>
    /// Reads the next 32 bits from standard input and return as a 32-bit float.</summary>
    /// <returns>the next 32 bits of data from standard input as a <c>float</c></returns>
    /// <exception cref="NotImplementedException">not implemented</exception>
    ///
    public static float ReadFloat()
    {
      throw new NotImplementedException("ReadFloat to be implemented");
    }

    /// <summary>
    /// Reads the next 8 bits from standard input and return as an 8-bit byte.</summary>
    /// <returns>the next 8 bits of data from standard input as a <c>byte</c></returns>
    /// <exception cref="IOException">if there are fewer than 8 bits available on standard input</exception>
    ///
    public byte ReadByte()
    {
      char c = ReadChar();
      byte x = (byte)(c & 0xff);
      return x;
    }

    /// <summary>
    /// Cleans up the input stream
    /// </summary>
    public void Dispose()
    {
      DisposeIt(true);
      // This object will be cleaned up by the Dispose method.
      // Therefore, you should call GC.SupressFinalize to
      // take this object off the finalization queue
      // and prevent finalization code for this object
      // from executing a second time.
      GC.SuppressFinalize(this);
    }

    void DisposeIt(bool disposing)
    {
      // Check to see if Dispose has already been called.
      if (!this.disposed)
      {
        // If disposing equals true, dispose all managed
        // and unmanaged resources.
        if (disposing)
        {
          // Dispose managed resources.
          input.Dispose();
        }
        // Note disposing has been done.
        disposed = true;

      }
    }

    /// <summary>
    /// Closes the internal reader if not alreay closed 
    /// </summary>
    ~BinaryInput()
    {
      // Do not re-create Dispose clean-up code here.
      // Calling Dispose(false) is optimal in terms of
      // readability and maintainability.
      DisposeIt(false);
    }
    /// <summary>
    /// Test client. Reads in a binary input file from standard input and writes
    /// it to standard output.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BinaryInput output.bin < 4runs.bin", "Reads from user input and write to a file")]
    public static void MainTest(string[] args)
    {
      BinaryInput input = new BinaryInput();
      BinaryOutput output = new BinaryOutput(args[0]);
      // read one 8-bit char at a time
      while (!input.IsEmpty)
      {
        int c = input.ReadChar();
        if (c == -1) break;
        output.Write((byte)c);
      }
      output.Close();
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
