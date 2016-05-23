/******************************************************************************
 *  File name :    TrieSET.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  An set for extended ASCII strings, implemented  using a 256-way trie.
 *
 *  Sample client reads in a list of words from standard input and
 *  prints out each word, removing any duplicates.
 *
 ******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>TrieSET</c> class represents an ordered set of strings over
  /// the extended ASCII alphabet.
  /// It supports the usual <c>Add</c>, <c>Contains</c>, and <c>Delete</c>
  /// methods. It also provides character-based methods for finding the string
  /// in the set that is the <c>Longest prefix</c> of a given prefix,
  /// finding all strings in the set that <c>Start with</c> a given prefix,
  /// and finding all strings in the set that <c>Match</c> a given pattern.
  /// </para><para>
  /// This implementation uses a 256-way trie.
  /// The <c>Add</c>, <c>Contains</c>, <c>Delete</c>, and
  /// <c>Longest prefix</c> methods take time proportional to the length
  /// of the key (in the worst case). Construction takes constant time.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/55compress">Section 5.5</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/TrieSET.java.html">Huffman</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class TrieSET : IEnumerable<string> {
    /// <summary>
    /// The default alphabet for the parameterless constructor
    /// </summary>
    public static readonly Alphabet ExtendedASSCII = Alphabet.ExtendedAscii;

    private readonly Alphabet alphabet;
    private readonly int R;   // extracted radix from the alphabet
    private Node root;        // root of trie
    private int N;            // number of keys in trie

    // R-way trie node
    private class Node
    {
      private bool isString;
      private Node[] next;

      private Node() { } // do not call this
      public Node(int Radix)
      {
        isString = false;
        next = new Node[Radix];
      }

      public bool IsString
      {
        get { return isString; }
        set { isString = value; }
      }

      public Node this[int n]
      {
        get
        {
          Debug.Assert((0 <= n) && (n < next.Length));
          return next[n];
        }

        set
        {
          Debug.Assert((0 <= n) && (n < next.Length));
          next[n] = value;
        }
      }
    }


    /// <summary>
    /// Initializes an empty string symbol table.</summary>
    /// <param name="alphabet">The alphabet to use in place of the default</param>
    ///
    public TrieSET(Alphabet alphabet)
    {
      this.alphabet = alphabet;
      R = this.alphabet.Radix;
      root = null;
      N = 0;
    }

    /// <summary>
    /// Initializes an empty set of strings.</summary>
    ///
    public TrieSET()
    {
      this.alphabet = ExtendedASSCII;
      R = this.alphabet.Radix;
      root = null;
      N = 0;
    }

    /// <summary>
    /// Does the set contain the given key?</summary>
    /// <param name="key">the key</param>
    /// <returns><c>true</c> if the set contains <c>key</c> and
    /// <c>false</c> otherwise</returns>
    /// <exception cref="NullReferenceException">if <c>key</c> is <c>null</c></exception>
    ///
    public bool Contains(string key)
    {
      Node x = get(root, key, 0);
      if (x == null) return false;
      return x.IsString;
    }

    private Node get(Node x, string key, int d)
    {
      if (x == null) return null;
      if (d == key.Length) return x;
      char c = key[d];
      return get(x[alphabet.ToIndex(c)], key, d + 1);
    }

    /// <summary>
    /// Adds the key to the set if it is not already present.</summary>
    /// <param name="key">the key to add</param>
    /// <exception cref="NullReferenceException">if <c>key</c> is <c>null</c></exception>
    ///
    public void Add(string key)
    {
      root = add(root, key, 0);
    }

    private Node add(Node x, string key, int d)
    {
      if (x == null) x = new Node(alphabet.Radix);
      if (d == key.Length)
      {
        if (!x.IsString) N++;
        x.IsString = true;
      }
      else
      {
        char c = key[d];
        int idx = alphabet.ToIndex(c);
        x[idx] = add(x[idx], key, d + 1);
      }
      return x;
    }

    /// <summary>
    /// Returns the number of strings in the set.</summary>
    /// <returns>the number of strings in the set</returns>
    ///
    public int Count
    {
      get { return N; }
    }

    /// <summary>
    /// Is the set empty?</summary>
    /// <returns><c>true</c> if the set is empty, and <c>false</c> otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return Count == 0; }
    }

    /// <summary>
    /// Returns all of the keys in the set, as an iterator.
    /// To iterate over all of the keys in a set named <c>set</c>, use the
    /// foreach notation: <c>foreach (Key key in set)</c>.</summary>
    /// <returns>an iterator to all of the keys in the set</returns>
    ///
    public IEnumerator<string> GetEnumerator()
    {
      return KeysWithPrefix("").GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    /// <summary>
    /// Returns all of the keys in the set that start with <c>prefix</c>.</summary>
    /// <param name="prefix">the prefix</param>
    /// <returns>all of the keys in the set that start with <c>prefix</c>,
    ///  as an iterable</returns>
    ///
    public IEnumerable<string> KeysWithPrefix(string prefix)
    {
      LinkedQueue<string> results = new LinkedQueue<string>();
      Node x = get(root, prefix, 0);
      collect(x, new StringBuilder(prefix), results);
      return results;
    }

    private void collect(Node x, StringBuilder prefix, LinkedQueue<string> results)
    {
      if (x == null) return;
      if (x.IsString) results.Enqueue(prefix.ToString());
      for (int i = 0; i < R; i++)
      {
        prefix.Append(alphabet.ToChar(i));
        collect(x[i], prefix, results);
        prefix.Remove(prefix.Length - 1, 1);
      }
    }

    /// <summary>
    /// Returns all of the keys in the set that match <c>pattern</c>,
    /// where . symbol is treated as a wildcard character.</summary>
    /// <param name="pattern">the pattern</param>
    /// <returns>all of the keys in the set that match <c>pattern</c>,
    /// as an iterable, where . is treated as a wildcard character.</returns>
    ///
    public IEnumerable<string> KeysThatMatch(string pattern)
    {
      LinkedQueue<string> results = new LinkedQueue<string>();
      StringBuilder prefix = new StringBuilder();
      collect(root, prefix, pattern, results);
      return results;
    }

    private void collect(Node x, StringBuilder prefix, string pattern, LinkedQueue<string> results)
    {
      if (x == null) return;
      int d = prefix.Length;
      if (d == pattern.Length && x.IsString)
        results.Enqueue(prefix.ToString());
      if (d == pattern.Length)
        return;
      char c = pattern[d];
      if (c == '.')
      {
        for (int i = 0; i < R; i++)
        {
          prefix.Append(alphabet.ToChar(i));
          collect(x[i], prefix, pattern, results);
          prefix.Remove(prefix.Length - 1, 1);
        }
      }
      else
      {
        prefix.Append(c);
        collect(x[alphabet.ToIndex(c)], prefix, pattern, results);
        prefix.Remove(prefix.Length - 1, 1);
      }
    }

    /// <summary>
    /// Returns the string in the set that is the longest prefix of <c>query</c>,
    /// or <c>null</c>, if no such string.</summary>
    /// <param name="query">the query string</param>
    /// <returns>the string in the set that is the longest prefix of <c>query</c>,
    /// or <c>null</c> if no such string</returns>
    /// <exception cref="NullReferenceException">if <c>query</c> is <c>null</c></exception>
    ///
    public string LongestPrefixOf(string query)
    {
      int length = longestPrefixOf(root, query, 0, -1);
      if (length == -1) return null;
      return query.Substring(0, length);
    }

    // returns the length of the longest string key in the subtrie
    // rooted at x that is a prefix of the query string,
    // assuming the first d character match and we have already
    // found a prefix match of length length
    private int longestPrefixOf(Node x, string query, int d, int length)
    {
      if (x == null) return length;
      if (x.IsString) length = d;
      if (d == query.Length) return length;
      char c = query[d];
      return longestPrefixOf(x[alphabet.ToIndex(c)], query, d + 1, length);
    }

    /// <summary>
    /// Removes the key from the set if the key is present.</summary>
    /// <param name="key">the key</param>
    /// <exception cref="NullReferenceException">if <c>key</c> is <c>null</c></exception>
    ///
    public void Delete(string key)
    {
      root = delete(root, key, 0);
    }

    private Node delete(Node x, string key, int d)
    {
      if (x == null) return null;
      if (d == key.Length)
      {
        if (x.IsString) N--;
        x.IsString = false;
      }
      else
      {
        char c = key[d];
        x[alphabet.ToIndex(c)] = delete(x[alphabet.ToIndex(c)], key, d + 1);
      }

      // remove subtrie rooted at x if it is completely empty
      if (x.IsString) return x;
      for (int i = 0; i < R; i++)
        if (x[i] != null)
          return x;
      return null;
    }

    /// <summary>
    /// Demo test the <c>TrieSET</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd TrieSET < shellsST.txt", "Input symbols, duplicates ignored")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();

      TrieSET set = new TrieSET();
      while (!StdIn.IsEmpty)
      {
        string key = StdIn.ReadString();
        set.Add(key);
      }

      // print results
      if (set.Count < 100)
      {
        Console.WriteLine("keys(\"\"):");
        foreach (string key in set)
        {
          Console.WriteLine(key);
        }
        Console.WriteLine();
      }

      Console.WriteLine("longestPrefixOf(\"shellsort\"):");
      Console.WriteLine(set.LongestPrefixOf("shellsort"));
      Console.WriteLine();

      Console.WriteLine("longestPrefixOf(\"xshellsort\"):");
      Console.WriteLine(set.LongestPrefixOf("xshellsort"));
      Console.WriteLine();

      Console.WriteLine("keysWithPrefix(\"shor\"):");
      foreach (string s in set.KeysWithPrefix("shor"))
        Console.WriteLine(s);
      Console.WriteLine();

      Console.WriteLine("keysWithPrefix(\"shortening\"):");
      foreach (string s in set.KeysWithPrefix("shortening"))
        Console.WriteLine(s);
      Console.WriteLine();

      Console.WriteLine("keysThatMatch(\".he.l.\"):");
      foreach (string s in set.KeysThatMatch(".he.l."))
        Console.WriteLine(s);
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
