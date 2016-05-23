/******************************************************************************
 *  File name :    LinearProbingHashST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Symbol table implementation with linear probing hash table.
 *
 *  C:\> algscmd LinearProbingHashST < tiny.txt
 *  M 9
 *  L 11
 *  H 5
 *  S 0
 *  R 3
 *  P 10
 *  X 7
 *  E 12
 *  C 4
 *  A 8
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>LinearProbingHashST</c> class represents a symbol table of generic
  /// key-value pairs.
  /// It supports the usual <c>Put</c>, <c>Get</c>, <c>Indexer</c>, <c>Contains</c>,
  /// <c>Delete</c>, <c>Count</c>, and <c>IsEmpty</c> methods.
  /// It also provides a <c>Keys</c> method for iterating over all of the keys.
  /// A symbol table implements the <c>Associative array</c> abstraction:
  /// when associating a value with a key that is already in the symbol table,
  /// the convention is to replace the old value with the new value.
  /// Unlike <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>, this class uses the convention that
  /// values cannot be <c>null</c>. Setting the
  /// value associated with a key to <c>null</c> is equivalent to deleting the key
  /// from the symbol table.</para><para>
  /// This implementation uses a linear probing hash table. It requires that
  /// the key type overrides the <c>Equals()</c> and <c>GetHashCode()</c> methods.
  /// The expected time per <c>Put</c>, <c>Contains</c>, or <c>Remove</c>
  /// operation is constant, subject to the uniform hashing assumption.
  /// The <c>Count</c>, and <c>IsEmpty</c> operations take constant time.
  /// Construction takes constant time.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/34hash">Section 3.4</a>
  /// of <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>For other implementations, see <see cref="SeparateChainingHashST{Key, Value}"/>. This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/LinearProbingHashST.java.html">LinearProbingHashST</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class LinearProbingHashST<Key, Value>
  {
    private static readonly int INIT_CAPACITY = 4;

    private int N;           // number of key-value pairs in the symbol table
    private int M;           // size of linear probing table
    private object[] keys;      // the keys
    private object[] vals;    // the values

    /// <summary>
    /// Initializes an empty symbol table.</summary>
    ///
    public LinearProbingHashST() : this(INIT_CAPACITY)
    {
    }

    /// <summary>
    /// Initializes an empty symbol table with the specified initial capacity.</summary>
    /// <param name="capacity">the initial capacity</param>
    ///
    public LinearProbingHashST(int capacity)
    {
      M = capacity;
      keys = new object[M];
      vals = new object[M];
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
      if (key == null) throw new ArgumentNullException("argument to Contains() is null");
      return Get(key) != null;
    }

    // hash function for keys - returns value between 0 and M-1
    private int hash(Key key)
    {
      return (key.GetHashCode() & 0x7fffffff) % M;
    }

    // resizes the hash table to the given capacity by re-hashing all of the keys
    private void resize(int capacity)
    {
      LinearProbingHashST<Key, Value> temp = new LinearProbingHashST<Key, Value>(capacity);
      for (int i = 0; i < M; i++)
      {
        if (keys[i] != null)
        {
          temp.Put((Key)keys[i], (Value)vals[i]);
        }
      }
      keys = temp.keys;
      vals = temp.vals;
      M = temp.M;
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

      // double table size if 50% full
      if (N >= M / 2) resize(2 * M);

      int i;
      for (i = hash(key); keys[i] != null; i = (i + 1) % M)
      {
        if (keys[i].Equals(key))
        {
          vals[i] = val;
          return;
        }
      }
      keys[i] = key;
      vals[i] = val;
      N++;
    }

    /// <summary>
    /// Returns the value associated with the specified key.</summary>
    /// <param name="key">the key</param>
    /// <returns>the value associated with <c>key</c>;
    ///        <c>null</c> if no such value</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public object Get(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to get() is null");
      for (int i = hash(key); keys[i] != null; i = (i + 1) % M)
        if (keys[i].Equals(key))
          return vals[i];
      return null;
    }

    /// <summary>
    /// Removes the specified key and its associated value from this symbol table
    /// (if the key is in this symbol table).</summary>
    /// <param name="key"> key the key</param>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public void Delete(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to delete() is null");
      if (!Contains(key)) return;

      // find position i of key
      int i = hash(key);
      while (!key.Equals(keys[i]))
      {
        i = (i + 1) % M;
      }

      // delete key and associated value
      keys[i] = null;
      vals[i] = null;

      // rehash all keys in same cluster
      i = (i + 1) % M;
      while (keys[i] != null)
      {
        // delete keys[i] an vals[i] and reinsert
        Key keyToRehash = (Key)keys[i];
        object valToRehash = vals[i];
        keys[i] = null;
        vals[i] = null;
        N--;
        Put(keyToRehash, (Value)valToRehash);
        i = (i + 1) % M;
      }

      N--;

      // halves size of array if it's 12.5% full or less
      if (N > 0 && N <= M / 8) resize(M / 2);

      Debug.Assert(check());
    }

    /// <summary>
    /// Returns all keys in this symbol table as an <c>Iterable</c>.
    /// To iterate over all of the keys in the symbol table named <c>st</c>,
    /// use the foreach notation: <c>foreach (Key key in st.Keys())</c>.</summary>
    /// <returns>all keys in this sybol table</returns>
    ///
    public IEnumerable<Key> Keys()
    {
      LinkedQueue<Key> queue = new LinkedQueue<Key>();
      for (int i = 0; i < M; i++)
        if (keys[i] != null) queue.Enqueue((Key)keys[i]);
      return queue;
    }

    // integrity check - don't check after each put() because
    // integrity not maintained during a delete()
    private bool check()
    {

      // check that hash table is at most 50% full
      if (M < 2 * N)
      {
        Console.Error.WriteLine("Hash table size M = " + M + "; array size N = " + N);
        return false;
      }

      // check that each key in table can be found by get()
      for (int i = 0; i < M; i++)
      {
        if (keys[i] == null) continue;
        else if (Get((Key)keys[i]) != vals[i])
        {
          Console.Error.WriteLine("get[" + keys[i] + "] = " + Get((Key)keys[i]) + "; vals[i] = " + vals[i]);
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Demo test the <c>LinearProbingHashST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd LinearProbingHashST < tinyST.txt", "Keys to be associated with integer values")]
    public static void MainTest(string[] args)
    {
      LinearProbingHashST<string, int> st = new LinearProbingHashST<string, int>();
      TextInput StdIn = new TextInput();
      string[] keys = StdIn.ReadAllStrings();

      for (int i = 0; i < keys.Length; i++)
      {
        st[keys[i]] = i;
      }
      foreach (string s in st.Keys())
        Console.WriteLine("{0} {1}", s, st[s]);
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

