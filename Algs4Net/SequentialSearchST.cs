/******************************************************************************
 *  File name :    SequentialSearchST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/31elementary/tinyST.txt
 *
 *  Symbol table implementation with sequential search in an
 *  unordered linked list of key-value pairs.
 *
 *  C:\> type tinyST.txt
 *  S E A R C H E X A M P L E
 *
 *  C:\> algscmd SequentialSearchST < tiny.txt
 *  L 11
 *  P 10
 *  M 9
 *  X 7
 *  H 5
 *  C 4
 *  R 3
 *  A 8
 *  E 12
 *  S 0
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>SequentialSearchST</c> class represents an (unordered)
  /// symbol table of generic key-value pairs.
  /// It supports the usual <c>Put</c>, <c>Get</c>, <c>Indexer</c>, <c>Contains</c>,
  /// <c>Delete</c>, <c>Count</c>, and <c>IsEmpty</c> methods.
  /// It also provides a <c>Keys</c> method for iterating over all of the keys.
  /// A symbol table implements the <c>Associative array</c> abstraction:
  /// when associating a value with a key that is already in the symbol table,
  /// the convention is to replace the old value with the new value.
  /// The class also uses the convention that values cannot be <c>null</c>. Setting the
  /// value associated with a key to <c>null</c> is equivalent to deleting the key
  /// from the symbol table.
  /// </para><para>
  /// This implementation uses a singly-linked list and sequential search.
  /// It relies on the <c>Equals()</c> method to test whether two keys
  /// are equal. It does not call either the <c>CompareTo()</c> or
  /// <c>GetHashCode()</c> method.
  /// The <c>Put</c> and <c>Delete</c> operations take linear time; the
  /// <c>Get</c> and <c>Contains</c> operations takes linear time in the worst case.
  /// The <c>Count</c>, and <c>IsEmpty</c> operations take constant time.
  /// Construction takes constant time.</para></summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/31elementary">Section 3.1</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/SequentialSearchST.java.html">SequentialSearchST</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks>
  /// 
  public class SequentialSearchST<Key, Value>
  {
    private int N;           // number of key-value pairs
    private Node first;      // the linked list of key-value pairs

    // a helper linked list data type
    private class Node
    {
      internal Key key;
      internal object val;
      internal Node next;

      public Node(Key key, Value val, Node next)
      {
        this.key = key;
        this.val = val;
        this.next = next;
      }
    }

    /// <summary>
    /// Initializes an empty symbol table.</summary>
    ///
    public SequentialSearchST()
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
    /// Returns true if this symbol table is empty.</summary>
    ///
    /// <returns><c>true</c> if this symbol table is empty;
    ///        <c>false</c> otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return N == 0; }
    }

    /// <summary>
    /// Returns true if this symbol table contains the specified key.</summary>
    /// <param name="key">key the key</param>
    /// <returns><c>true</c> if this symbol table contains <c>key</c>;
    ///        <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public bool Contains(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to Contains() is null");
      return Get(key) != null;
    }

    /// <summary>
    /// Indexer wrapping <c>Get</c> and <c>Put</c> for convenient syntax
    /// </summary>
    /// <param name="key">key the key </param>
    /// <returns>value associated with the key</returns>
    /// <exception cref="NullReferenceException">null reference being used for value type</exception>
    public Value this[Key key]
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
    /// Returns the value associated with the given key in this symbol table.</summary>
    /// <param name="key">key the key</param>
    /// <returns>the (boxed) value associated with the given key if the key is in the symbol table
    ///    and <c>null</c> if the key is not in the symbol table</returns>
    /// <exception cref="ArgumentNullException" >if <c>key</c> is <c>null</c></exception>
    ///
    public object Get(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to Get() is null");
      for (Node x = first; x != null; x = x.next)
      {
        if (key.Equals(x.key))
          return x.val;
      }
      return null;
    }

    /// <summary>
    /// Inserts the specified key-value pair into the symbol table, overwriting the old
    /// value with the new value if the symbol table already contains the specified key.
    /// Deletes the specified key (and its associated value) from this symbol table
    /// if the specified value is <c>null</c>.</summary>
    /// <param name="key">key the key</param>
    /// <param name="val">val the value</param>
    /// <exception cref="ArgumentNullException" >if <c>key</c> is <c>null</c></exception>
    ///
    public void Put(Key key, Value val)
    {
      if (key == null) throw new ArgumentNullException("first argument to put() is null");
      if (val == null)
      {
        Delete(key);
        return;
      }

      for (Node x = first; x != null; x = x.next)
      {
        if (key.Equals(x.key))
        {
          x.val = val;
          return;
        }
      }
      first = new Node(key, val, first);
      N++;
    }

    /// <summary>
    /// Removes the specified key and its associated value from this symbol table
    /// (if the key is in this symbol table).</summary>
    /// <param name="key">key the key</param>
    /// <exception cref="ArgumentNullException" >if <c>key</c> is <c>null</c></exception>
    ///
    public void Delete(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to Delete() is null");
      first = Delete(first, key);
    }

    // delete key in linked list beginning at Node x
    // warning: function call stack too large if table is large
    private Node Delete(Node x, Key key)
    {
      if (x == null) return null;
      if (key.Equals(x.key))
      {
        N--;
        return x.next;
      }
      x.next = Delete(x.next, key);
      return x;
    }


    /// <summary>
    /// Returns all keys in the symbol table as an <c>Iterable</c>.
    /// To iterate over all of the keys in the symbol table named <c>st</c>,
    /// use the foreach notation: <c>foreach (Key key in st.Keys())</c>.</summary>
    /// <returns>all keys in the sybol table</returns>
    ///
    public IEnumerable<Key> Keys()
    {
      LinkedQueue<Key> queue = new LinkedQueue<Key>();
      for (Node x = first; x != null; x = x.next)
        queue.Enqueue(x.key);
      return queue;
    }

    /// <summary>
    /// Demo test the <c>SequentialSearchST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd SequentialSearchST < tiny.txt", "Keys to be associated with integer values")]
    public static void MainTest(string[] args)
    {
      SequentialSearchST<string, int> st = new SequentialSearchST<string, int>();
      TextInput StdIn = new TextInput();
      string[] keys = StdIn.ReadAllStrings();

      for (int i = 0; i < keys.Length; i++)
      {
        st.Put(keys[i], i);
      }
      foreach (string s in st.Keys())
        Console.WriteLine("{0} {1}", s, st.Get(s));
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
