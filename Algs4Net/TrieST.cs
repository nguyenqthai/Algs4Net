/******************************************************************************
 *  File name :    TrieST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A string symbol table for extended ASCII strings, implemented
 *  using a 256-way trie.
 *
 *  C:\> algscmd TrieST < shellsST.txt 
 *  by 4
 *  sea 6
 *  sells 1
 *  she 0
 *  shells 3
 *  shore 7
 *  the 5
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>TrieST</c> class represents an symbol table of key-value
  /// pairs, with string keys and generic values.
  /// It supports the usual <c>Put</c>, <c>Get</c>, <c>Indexer</c>, <c>Contains</c>,
  /// <c>Delete</c>, <c>Count</c>, and <c>IsEmpty</c> methods.
  /// It also provides character-based methods for finding the string
  /// in the symbol table that is the <c>Longest prefix</c> of a given prefix,
  /// finding all strings in the symbol table that <c>Start with</c> a given prefix,
  /// and finding all strings in the symbol table that <c>Match</c> a given pattern.
  /// A symbol table implements the <c>Associative array</c> abstraction:
  /// when associating a value with a key that is already in the symbol table,
  /// the convention is to replace the old value with the new value.
  /// Unlike <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>, this class uses the convention that
  /// values cannot be <c>null</c>. Setting the value associated with a 
  /// key to <c>null</c> is equivalent to deleting the key from the symbol table.</para>
  /// <para>This implementation uses a 256-way trie. The <c>Put</c>, <c>Contains</c>, 
  /// <c>Delete</c>, and <c>Longest prefix</c> operations take time proportional to 
  /// the length of the key (in the worst case). Construction takes constant time.
  /// The <c>Count</c>, and <c>IsEmpty</c> operations take constant time.
  /// Construction takes constant time.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/52trie">Section 5.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/TrieST.java.html">TrieST</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class TrieST<Value>
  {
    /// <summary>
    /// The default alphabet for the parameterless constructor
    /// </summary>
    public static readonly Alphabet ExtendedASSCII = Alphabet.ExtendedAscii;

    private readonly Alphabet alphabet;
    private readonly int R;   // extracted radix from the alphabet
    private Node root;        // root of trie
    private int N;            // number of keys in trie

    /// <summary>
    /// Initializes an empty string symbol table.</summary>
    /// <param name="alphabet">The alphabet to use in place of the default</param>
    ///
    public TrieST(Alphabet alphabet)
    {
      this.alphabet = alphabet;
      R = this.alphabet.Radix;
      root = null;
      N = 0;
    }

    /// <summary>
    /// Initializes an empty string symbol table using the default alphabet.</summary>
    /// 
    public TrieST()
    {
      this.alphabet = ExtendedASSCII;
      R = this.alphabet.Radix;
      root = null;
      N = 0;
    }

    // R-way trie node
    private class Node
    {
      private object val;
      private Node[] next;

      private Node() { } // do not call this
      public Node(int Radix)
      {
        val = null;
        next = new Node[Radix];
      }

      public object Value
      {
        get { return val; }
        set { val = value; }
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
    /// Indexer wrapping <c>Get</c> and <c>Put</c> for convenient syntax
    /// </summary>
    /// <param name="key">key the key </param>
    /// <returns>value associated with the key</returns>
    /// <exception cref="NullReferenceException">null reference being used for value type</exception>
    public Value this[string key]
    {
      get
      {
        object value = Get(key);
        if (value == null)
        {
          if (default(Value) == null) return (Value)value;
          else throw new NullReferenceException("null reference being used for value type");
        }
        return (Value)value;
      }

      set { Put(key, value); }
    }

    /// <summary>
    /// Returns the value associated with the given key.</summary>
    /// <param name="key">the key</param>
    /// <returns>the value associated with the given key if the key is in the 
    /// symbol table and <c>null</c> if the key is not in the symbol table</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public object Get(string key)
    {
      if (key == null) throw new ArgumentNullException();
      Node x = get(root, key, 0);
      if (x == null) return null;
      return x.Value;
    }

    /// <summary>
    /// Does this symbol table contain the given key?</summary>
    /// <param name="key">the key</param>
    /// <returns><c>true</c> if this symbol table contains <c>key</c> and
    /// <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public bool Contains(string key)
    {
      if (key == null) throw new ArgumentNullException();
      return Get(key) != null;
    }

    private Node get(Node x, string key, int d)
    {
      if (x == null) return null;
      if (d == key.Length) return x;
      char c = key[d];
      return get(x[alphabet.ToIndex(c)], key, d + 1);
    }

    /// <summary>
    /// Inserts the key-value pair into the symbol table, overwriting the old value
    /// with the new value if the key is already in the symbol table.
    /// If the value is <c>null</c>, this effectively deletes the key from the symbol table.</summary>
    /// <param name="key">the key</param>
    /// <param name="val">the value</param>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public void Put(string key, Value val)
    {
      if (key == null) throw new ArgumentNullException();
      if (val == null) Delete(key);
      else root = put(root, key, val, 0);
    }

    private Node put(Node x, string key, Value val, int d)
    {
      if (x == null) x = new Node(alphabet.Radix);
      if (d == key.Length)
      {
        if (x.Value == null) N++;
        x.Value = val;
        return x;
      }
      char c = key[d];
      int idx = alphabet.ToIndex(c);
      x[idx] = put(x[idx], key, val, d + 1);
      return x;
    }

    /// <summary>
    /// Returns the number of key-value pairs in this symbol table.</summary>
    /// <returns>the number of key-value pairs in this symbol table</returns>
    ///
    public int Count
    {
      get { return N; }
    }

    /// <summary>
    /// Is this symbol table empty?</summary>
    /// <returns><c>true</c> if this symbol table is empty and <c>false</c> otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return N == 0; }
    }

    /// <summary>
    /// Returns all keys in the symbol table as an <c>Iterable</c>.
    /// To iterate over all of the keys in the symbol table named <c>st</c>,
    /// use the foreach notation: <c>foreach (Key key in st.Keys)</c>.</summary>
    /// <returns>all keys in the sybol table as an <c>Iterable</c></returns>
    ///
    public IEnumerable<string> Keys
    {
      get { return KeysWithPrefix(""); }
    }

    /// <summary>
    /// Returns all of the keys in the set that start with <c>prefix</c>.</summary>
    /// <param name="prefix">the prefix</param>
    /// <returns>all of the keys in the set that start with <c>prefix</c>
    /// as an iterable</returns>
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
      if (x.Value != null) results.Enqueue(prefix.ToString());
      for (int i = 0; i < R; i++)
      {
        prefix.Append(alphabet.ToChar(i));
        collect(x[i], prefix, results);
        prefix.Remove(prefix.Length - 1, 1);
      }
    }

    /// <summary>
    /// Returns all of the keys in the symbol table that match <c>pattern</c>,
    /// where . symbol is treated as a wildcard character.</summary>
    /// <param name="pattern">the pattern</param>
    /// <returns>all of the keys in the symbol table that match <c>pattern</c>
    /// as an iterable, where . is treated as a wildcard character.</returns>
    ///
    public IEnumerable<string> KeysThatMatch(string pattern)
    {
      LinkedQueue<string> results = new LinkedQueue<string>();
      collect(root, new StringBuilder(), pattern, results);
      return results;
    }

    private void collect(Node x, StringBuilder prefix, string pattern, LinkedQueue<string> results)
    {
      if (x == null) return;
      int d = prefix.Length;
      if (d == pattern.Length && x.Value != null)
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
    /// Returns the string in the symbol table that is the longest prefix of <c>query</c>,
    /// or <c>null</c>, if no such string.</summary>
    /// <param name="query">the query string</param>
    /// <returns>the string in the symbol table that is the longest prefix of <c>query</c>
    /// or <c>null</c> if no such string</returns>
    /// <exception cref="ArgumentNullException">if <c>query</c> is <c>null</c></exception>
    ///
    public string LongestPrefixOf(string query)
    {
      if (query == null) throw new ArgumentNullException();

      int length = LongestPrefixOf(root, query, 0, -1);
      if (length == -1) return null;
      else return query.Substring(0, length);
    }

    // returns the length of the longest string key in the subtrie
    // rooted at x that is a prefix of the query string,
    // assuming the first d character match and we have already
    // found a prefix match of given length (-1 if no such match)
    private int LongestPrefixOf(Node x, string query, int d, int length)
    {
      if (x == null) return length;
      if (x.Value != null) length = d;
      if (d == query.Length) return length;
      char c = query[d];
      return LongestPrefixOf(x[alphabet.ToIndex(c)], query, d + 1, length);
    }

    /// <summary>
    /// Removes the key from the set if the key is present.</summary>
    /// <param name="key">the key</param>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
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
        if (x.Value != null) N--;
        x.Value = null;
      }
      else
      {
        char c = key[d];
        x[alphabet.ToIndex(c)] = delete(x[alphabet.ToIndex(c)], key, d + 1);
      }

      // remove subtrie rooted at x if it is completely empty
      if (x.Value != null) return x;
      for (int i = 0; i < R; i++)
        if (x[i] != null)
          return x;
      return null;
    }

    /// <summary>
    /// Demo test the <c>TrieST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd TrieST < shellsST.txt", "Input symbols")]
    public static void MainTest(string[] args)
    {
      // build symbol table from standard input
      TrieST<int> st = new TrieST<int>();

      TextInput StdIn = new TextInput();
      for (int i = 0; !StdIn.IsEmpty; i++)
      {
        string key = StdIn.ReadString();
        st.Put(key, i);
        //Console.WriteLine("Key, value = {0}, {1} is {2}", key, st[key], st[key] == i);
      }

      // print results
      if (st.Count < 100)
      {
        Console.WriteLine("keys(\"\"):");
        foreach (string key in st.Keys)
        {
          Console.WriteLine(key + " " + st.Get(key));
        }
        Console.WriteLine();
      }

      Console.WriteLine("longestPrefixOf(\"shellsort\"):");
      Console.WriteLine(st.LongestPrefixOf("shellsort"));
      Console.WriteLine();

      Console.WriteLine("longestPrefixOf(\"quicksort\"):");
      Console.WriteLine(st.LongestPrefixOf("quicksort"));
      Console.WriteLine();

      Console.WriteLine("keysWithPrefix(\"shor\"):");
      foreach (string s in st.KeysWithPrefix("shor"))
        Console.WriteLine(s);
      Console.WriteLine();

      Console.WriteLine("keysThatMatch(\".he.l.\"):");
      foreach (string s in st.KeysThatMatch(".he.l."))
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
