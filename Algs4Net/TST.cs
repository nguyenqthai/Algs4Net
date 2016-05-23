/******************************************************************************
 *  File name :    TST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Symbol table with string keys, implemented using a ternary search
 *  trie (TST).
 *
 *
 *  C:\> algscmd TST < shellsST.txt
 *  keys(""):
 *  by 4
 *  sea 6
 *  sells 1
 *  she 0
 *  shells 3
 *  shore 7
 *  the 5
 *
 *  longestPrefixOf("shellsort"):
 *  shells
 *
 *  keysWithPrefix("shor"):
 *  shore
 *
 *  keysThatMatch(".he.l."):
 *  shells
 *
 *  C:\> algscmd TST
 *  theory the now is the time for all good men
 *
 *  Remarks
 *  --------
 *    - can't use a key that is the empty string ""
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>The <c>TST</c> class represents an symbol table of key-value
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
  /// values cannot be <c>null</c>. Setting the
  /// value associated with a key to <c>null</c> is equivalent to deleting the key
  /// from the symbol table.</para><para>
  /// This implementation uses a ternary search trie.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/52trie">Section 5.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/TST.java.html">TST</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class TST<Value>
  {
    private int N;              // size
    private Node root;   // root of TST

    private class Node
    {
      public char c;                 // character
      public Node left, mid, right;  // left, middle, and right subtries
      public object val;              // value associated with string
    }

    /// <summary>
    /// Initializes an empty string symbol table.</summary>
    ///
    public TST()
    {
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
    /// Does this symbol table contain the given key?</summary>
    /// <param name="key">the key</param>
    /// <returns><c>true</c> if this symbol table contains <c>key</c> and
    /// <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public bool Contains(string key)
    {
      return Get(key) != null;
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
    /// <returns>the value associated with the given key if the key is in the symbol table
    /// and <c>null</c> if the key is not in the symbol table</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public object Get(string key)
    {
      if (key == null) throw new ArgumentNullException();
      if (key.Length == 0) throw new ArgumentException("key must have length >= 1");
      Node x = get(root, key, 0);
      if (x == null) return null;
      return x.val;
    }

    // return subtrie corresponding to given key
    private Node get(Node x, string key, int d)
    {
      if (key == null) throw new ArgumentNullException();
      if (key.Length == 0) throw new ArgumentException("key must have length >= 1");
      if (x == null) return null;
      char c = key[d];
      if (c < x.c) return get(x.left, key, d);
      else if (c > x.c) return get(x.right, key, d);
      else if (d < key.Length - 1) return get(x.mid, key, d + 1);
      else return x;
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
      if (!Contains(key)) N++;
      root = put(root, key, val, 0);
    }

    private Node put(Node x, string key, Value val, int d)
    {
      char c = key[d];
      if (x == null)
      {
        x = new Node();
        x.c = c;
      }
      if (c < x.c) x.left = put(x.left, key, val, d);
      else if (c > x.c) x.right = put(x.right, key, val, d);
      else if (d < key.Length - 1) x.mid = put(x.mid, key, val, d + 1);
      else x.val = val;
      return x;
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
      if (query == null || query.Length == 0) return null;
      int length = 0;
      Node x = root;
      int i = 0;
      while (x != null && i < query.Length)
      {
        char c = query[i];
        if (c < x.c) x = x.left;
        else if (c > x.c) x = x.right;
        else
        {
          i++;
          if (x.val != null) length = i;
          x = x.mid;
        }
      }
      return query.Substring(0, length);
    }

    /// <summary>
    /// Returns all keys in the symbol table as an <c>Iterable</c>.
    /// To iterate over all of the keys in the symbol table named <c>st</c>,
    /// use the foreach notation: <c>foreach (Key key in st.Keys)</c>.</summary>
    /// <returns>all keys in the sybol table as an <c>Iterable</c></returns>
    ///
    public IEnumerable<string> Keys
    {
      get
      {
        LinkedQueue<string> queue = new LinkedQueue<string>();
        collect(root, new StringBuilder(), queue);
        return queue;
      }
    }

    /// <summary>
    /// Returns all of the keys in the set that start with <c>prefix</c>.</summary>
    /// <param name="prefix">the prefix</param>
    /// <returns>all of the keys in the set that start with <c>prefix</c>
    /// as an iterable</returns>
    ///
    public IEnumerable<string> KeysWithPrefix(string prefix)
    {
      LinkedQueue<string> queue = new LinkedQueue<string>();
      Node x = get(root, prefix, 0);
      if (x == null) return queue;
      if (x.val != null) queue.Enqueue(prefix);
      collect(x.mid, new StringBuilder(prefix), queue);
      return queue;
    }

    // all keys in subtrie rooted at x with given prefix
    private void collect(Node x, StringBuilder prefix, LinkedQueue<string> queue)
    {
      if (x == null) return;
      collect(x.left, prefix, queue);
      if (x.val != null) queue.Enqueue(prefix.ToString() + x.c);
      collect(x.mid, prefix.Append(x.c), queue);
      prefix.Remove(prefix.Length - 1, 1);
      collect(x.right, prefix, queue);
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
      LinkedQueue<string> queue = new LinkedQueue<string>();
      collect(root, new StringBuilder(), 0, pattern, queue);
      return queue;
    }

    private void collect(Node x, StringBuilder prefix, int i, string pattern, LinkedQueue<string> queue)
    {
      if (x == null) return;
      char c = pattern[i];
      if (c == '.' || c < x.c) collect(x.left, prefix, i, pattern, queue);
      if (c == '.' || c == x.c)
      {
        if (i == pattern.Length - 1 && x.val != null) queue.Enqueue(prefix.ToString() + x.c);
        if (i < pattern.Length - 1)
        {
          collect(x.mid, prefix.Append(x.c), i + 1, pattern, queue);
          prefix.Remove(prefix.Length - 1, 1);
        }
      }
      if (c == '.' || c > x.c) collect(x.right, prefix, i, pattern, queue);
    }

    /// <summary>
    /// Demo test the <c>TST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd TST < shellsST.txt", "Input symbols, duplicates ignored")]
    public static void MainTest(string[] args)
    {
      // build symbol table from standard input
      TST<int> st = new TST<int>();
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
          Console.WriteLine(key + " " + st[key]);
        }
        Console.WriteLine();
      }

      Console.WriteLine("LongestPrefixOf(\"shellsort\"):");
      Console.WriteLine(st.LongestPrefixOf("shellsort"));
      Console.WriteLine();

      Console.WriteLine("LongestPrefixOf(\"quicksort\"):");
      Console.WriteLine(st.LongestPrefixOf("quicksort"));
      Console.WriteLine();

      Console.WriteLine("KeysWithPrefix(\"shor\"):");
      foreach (string s in st.KeysWithPrefix("shor"))
        Console.WriteLine(s);
      Console.WriteLine();

      Console.WriteLine("KeysThatMatch(\".he.l.\"):");
      foreach (string s in st.KeysThatMatch(".he.l."))
        Console.WriteLine(s);
      Console.WriteLine();
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
