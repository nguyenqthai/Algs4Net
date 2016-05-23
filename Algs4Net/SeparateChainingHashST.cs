/******************************************************************************
 *  File name :    SeparateChainingHashST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A symbol table implemented with a separate-chaining hash table.
 *  C:\> type tinyST.txt
 *  S E A R C H E X A M P L E
 *
 * C:\> algscmd SeparateChainingHashST < tinyST.txt
 * L 11
 * P 10
 * X 7
 * H 5
 * C 4
 * S 0
 * R 3
 * M 9
 * A 8
 * E 12
 * 
 ******************************************************************************/

using System;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>SeparateChainingHashST</c> class represents a symbol table of generic
  /// key-value pairs.
  /// It supports the usual <c>Put</c>, <c>Get</c>, <c>Contains</c>,
  /// <c>Delete</c>, <c>Count</c>, and <c>IsEmpty</c> methods.
  /// It also provides a <c>Keys</c> method for iterating over all of the keys.
  /// A symbol table implements the <c>Associative array</c> abstraction:
  /// when associating a value with a key that is already in the symbol table,
  /// the convention is to replace the old value with the new value.
  /// Unlike <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>, this class uses the convention that
  /// values cannot be <c>null</c>. Setting the
  /// value associated with a key to <c>null</c> is equivalent to deleting the key
  /// from the symbol table.</para><para>
  /// This implementation uses a separate chaining hash table. It requires that
  /// the key type overrides the <c>Equals()</c> and <c>GetHashCode()</c> methods.
  /// The expected time per <c>Put</c>, <c>Contains</c>, or <c>Remove</c>
  /// operation is constant, subject to the uniform hashing assumption.
  /// The <c>Count</c>, and <c>IsEmpty</c> operations take constant time.
  /// Construction takes constant time.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/34hash">Section 3.4</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>For other implementations, see <see cref="LinearProbingHashST{Key, Value}"/>. This class
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/SeparateChainingHashST.java.html">SeparateChainingHashST</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class SeparateChainingHashST<Key, Value>
  {
    private static readonly int INIT_CAPACITY = 4;

    // largest prime <= 2^i for i = 3 to 31
    // not currently used for doubling and shrinking
    // private static readonly int[] PRIMES = {
    //    7, 13, 31, 61, 127, 251, 509, 1021, 2039, 4093, 8191, 16381,
    //    32749, 65521, 131071, 262139, 524287, 1048573, 2097143, 4194301,
    //    8388593, 16777213, 33554393, 67108859, 134217689, 268435399,
    //    536870909, 1073741789, 2147483647
    // };

    private int N;                                // number of key-value pairs
    private int M;                                // hash table size
    private SequentialSearchST<Key, Value>[] st;  // array of linked-list symbol tables


    /// <summary>
    /// Initializes an empty symbol table.</summary>
    ///
    public SeparateChainingHashST() : this(INIT_CAPACITY)
    {
    }

    /// <summary>
    /// Initializes an empty symbol table with <c>M</c> chains.</summary>
    /// <param name="M">the initial number of chains</param>
    ///
    public SeparateChainingHashST(int M)
    {
      this.M = M;
      st = new SequentialSearchST<Key, Value>[M];
      for (int i = 0; i < M; i++)
        st[i] = new SequentialSearchST<Key, Value>();
    }

    // resize the hash table to have the given number of chains b rehashing all of the keys
    private void resize(int chains)
    {
      SeparateChainingHashST<Key, Value> temp = new SeparateChainingHashST<Key, Value>(chains);
      for (int i = 0; i < M; i++)
      {
        foreach (Key key in st[i].Keys())
        {
          temp.Put(key, (Value)st[i].Get(key));
        }
      }
      this.M = temp.M;
      this.N = temp.N;
      this.st = temp.st;
    }

    // hash value between 0 and M-1
    private int hash(Key key)
    {
      return (key.GetHashCode() & 0x7fffffff) % M;
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
    /// <returns><c>true</c> if this symbol table is empty;
    ///        <c>false</c> otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return Count == 0; }
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
    /// Returns true if this symbol table contains the specified key.</summary>
    /// <param name="key">the key</param>
    /// <returns><c>true</c> if this symbol table contains <c>key</c>;
    ///        <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    ///
    public bool Contains(Key key)
    {
      if (key == null) throw new ArgumentNullException ("argument to contains() is null");
      return Get(key) != null;
    }

    /// <summary>
    /// Returns the value associated with the specified key in this symbol table.</summary>
    /// <param name="key">the key</param>
    /// <returns>the value associated with <c>key</c> in the symbol table;
    ///        <c>null</c> if no such value</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public object Get(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to Get() is null");
      int i = hash(key);
      return st[i].Get(key);
    }

    /// <summary>
    /// Inserts the specified key-value pair into the symbol table, overwriting the old
    /// value with the new value if the symbol table already contains the specified key.
    /// Deletes the specified key (and its associated value) from this symbol table
    /// if the specified value is <c>null</c>.</summary>
    /// <param name="key">the key</param>
    /// <param name="val">the value</param>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public void Put(Key key, Value val)
    {
      if (key == null) throw new ArgumentNullException("first argument to Put() is null");
      if (val == null)
      {
        Delete(key);
        return;
      }

      // double table size if average length of list >= 10
      if (N >= 10 * M) resize(2 * M);

      int i = hash(key);
      if (!st[i].Contains(key)) N++;
      st[i].Put(key, val);
    }

    /// <summary>
    /// Removes the specified key and its associated value from this symbol table
    /// (if the key is in this symbol table).</summary>
    /// <param name="key">the key</param>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public void Delete(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to Delete() is null");

      int i = hash(key);
      if (st[i].Contains(key)) N--;
      st[i].Delete(key);

      // halve table size if average length of list <= 2
      if (M > INIT_CAPACITY && N <= 2 * M) resize(M / 2);
    }

    /// <summary>
    /// Returns all keys in the symbol table as an <c>IEnumerable</c>.
    /// To iterate over all of the keys in the symbol table named <c>st</c>,
    /// use the foreach notation: <c>foreach (Key key in st.Keys())</c>.</summary>
    /// <returns>all keys in the sybol table as an <c>IEnumerable</c></returns>
    ///
    public IEnumerable<Key> Keys()
    {
      LinkedQueue<Key> queue = new LinkedQueue<Key>();
      for (int i = 0; i < M; i++)
      {
        foreach (Key key in st[i].Keys())
          queue.Enqueue(key);
      }
      return queue;
    }

    /// <summary>
    /// Demo test the <c>SeparateChainingHashST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd SeparateChainingHashST < tinyST.txt", "Keys to be associated with integer values")]
    public static void MainTest(string[] args)
    {
      SeparateChainingHashST<string, int> st = new SeparateChainingHashST<string, int>();
      TextInput StdIn = new TextInput();
      string[] keys = StdIn.ReadAllStrings();

      for (int i = 0; i < keys.Length; i++)
      {
        st.Put(keys[i], i);
      }
      foreach (string s in st.Keys())
        Console.WriteLine("{0} {1}", s, st.Get(s));
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
