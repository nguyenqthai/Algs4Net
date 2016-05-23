/******************************************************************************
 *  File name :    Huffman.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/55compression/abra.txt
 *                http://algs4.cs.princeton.edu/55compression/tinytinyTale.txt
 *
 *  Compress or expand a binary input stream using the Huffman algorithm.
 *
 *  C:\> algscmd Huffman - abra.txt output.txt 
 *  
 *  C:\> algscmd BinaryDump 60 < output.txt
 *  010100000100101000100010010000110100001101010100101010000100
 *  000000000000000000000000000110001111100101101000111110010100
 *  120 bits
 *
 *  C:\> algscmd Huffman + output.txt abra2.txt
 *  
 *  C:\> type abra2.txt
 *  ABRACADABRA!
 *
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Huffman</c> class provides methods for compressing
  /// and expanding a binary input using Huffman codes over the 8-bit extended
  /// ASCII alphabet.</summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/55compress">Section 5.5</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Huffman.java.html">Huffman</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Huffman
  {
    private const char ONE  = '\u0001';
    private const char ZERO = '\u0000';
    // alphabet size of extended ASCII
    private const int R = 256;

    private BinaryInput input;
    private BinaryOutput output;

    // Do not instantiate.
    private Huffman() { }

    /// <summary>
    /// Uses file names to direct input and output
    /// </summary>
    /// <param name="outputFileName">user input file, empty if from console</param>
    /// <param name="inputFileName">user output file</param>
    public Huffman(string inputFileName, string outputFileName)
    {
      //Console.WriteLine("Huffman with InFile={0} and OutFile={1}", inputFileName, outputFileName);
      input = new BinaryInput(inputFileName);
      output = new BinaryOutput(outputFileName);
    }

    // Huffman trie node
    private class Node : IComparable<Node>
    {
      public readonly char ch;
      public readonly int freq;
      public readonly Node left, right;

      public Node(char ch, int freq, Node left, Node right)
      {
        this.ch = ch;
        this.freq = freq;
        this.left = left;
        this.right = right;
      }

      // is the node a leaf node?
      public bool IsLeaf
      {
        get
        {
          Debug.Assert(((left == null) && (right == null)) || ((right != null) && (right != null)));
          return (left == null) && (right == null);
        }
      }

      // compare, based on frequency
      public int CompareTo(Node other)
      {
        return freq - other.freq;
      }
    }

    /// <summary>
    /// Reads a sequence of 8-bit bytes from standard input; compresses them
    /// using Huffman codes with an 8-bit alphabet; and writes the results
    /// to standard output.</summary>
    ///
    public void Compress()
    {
      // read the input
      string s = input.ReadString();
      char[] inputChars = s.ToCharArray();

      // tabulate frequency counts
      int[] freq = new int[R];
      for (int i = 0; i < inputChars.Length; i++)
        freq[inputChars[i]]++;

      // build Huffman trie
      Node root = buildTrie(freq);

      // build code table
      string[] st = new string[R];
      buildCode(st, root, "");

      // print trie for decoder
      writeTrie(root);

      // print number of bytes in original uncompressed message
      output.Write(inputChars.Length);

      // use Huffman code to encode input
      for (int i = 0; i < inputChars.Length; i++)
      {
        string code = st[inputChars[i]];
        for (int j = 0; j < code.Length; j++)
        {
          if (code[j] == '0')
          {
            output.Write(false);
          }
          else if (code[j] == '1')
          {
            output.Write(true);
          }
          else throw new InvalidOperationException("Illegal state");
        }
      }

      // close output stream
      output.Close();
    }

    // build the Huffman trie given frequencies
    private static Node buildTrie(int[] freq)
    {

      // initialze priority queue with singleton trees
      MinPQ<Node> pq = new MinPQ<Node>();
      for (int i = 0; i < R; i++)
        if (freq[i] > 0)
          pq.Insert(new Node((char)i, freq[i], null, null));

      // special case in case there is only one character with a nonzero frequency
      if (pq.Count == 1)
      {
        if (freq[ZERO] == 0) pq.Insert(new Node(ZERO, 0, null, null));
        else pq.Insert(new Node(ONE, 0, null, null));
      }

      // merge two smallest trees
      while (pq.Count > 1)
      {
        Node left = pq.DelMin();
        Node right = pq.DelMin();
        Node parent = new Node(ZERO, left.freq + right.freq, left, right);
        pq.Insert(parent);
      }
      return pq.DelMin();
    }


    // write bitstring-encoded trie to standard output
    private void writeTrie(Node x)
    {
      if (x.IsLeaf)
      {
        output.Write(true);
        output.Write(x.ch, 8);
        return;
      }
      output.Write(false);
      writeTrie(x.left);
      writeTrie(x.right);
    }

    // make a lookup table from symbols and their encodings
    private static void buildCode(string[] st, Node x, string s)
    {
      if (!x.IsLeaf)
      {
        buildCode(st, x.left, s + '0');
        buildCode(st, x.right, s + '1');
      }
      else
      {
        st[x.ch] = s;
      }
    }

    /// <summary>
    /// Reads a sequence of bits that represents a Huffman-compressed message from
    /// standard input; expands them; and writes the results to standard output.</summary>
    ///
    public void Expand()
    {

      // read in Huffman trie from input stream
      Node root = readTrie();

      // number of bytes to write
      int length = input.ReadInt();

      // decode using the Huffman trie
      for (int i = 0; i < length; i++)
      {
        Node x = root;
        while (!x.IsLeaf)
        {
          bool bit = input.ReadBoolean();
          if (bit) x = x.right;
          else x = x.left;
        }
        output.Write(x.ch, 8);
      }
      output.Close();
    }


    private Node readTrie()
    {
      bool isLeaf = input.ReadBoolean();
      if (isLeaf)
      {
        return new Node(input.ReadChar(), -1, null, null);
      }
      else
      {
        return new Node(ZERO, -1, readTrie(), readTrie());
      }
    }
    /// <summary>
    /// Sample client that calls <c>Compress()</c> if the command-line
    /// argument is "-" an <c>Expand()</c> if it is "+".</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Huffman (-|+) input_file output_file")]
    public static void MainTest(string[] args)
    {
      Huffman algorithm = new Huffman(args[1], args[2]);
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
