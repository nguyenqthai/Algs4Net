/******************************************************************************
 *  File name :    BinarySearchST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/31elementary/tinyST.txt
 *
 *  Symbol table implementation with binary search in an ordered array.
 *
 *  C:\> type tinyST.txt
 *  S E A R C H E X A M P L E
 *
 *  C:\> algscmd BinarySearchST < tinyST.txt
 *  A 8
 *  C 4
 *  E 12
 *  H 5
 *  L 11
 *  M 9
 *  P 10
 *  R 3
 *  S 0
 *  X 7
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>BST</c> class represents an ordered symbol table of generic
  /// key-value pairs.
  /// It supports the usual <c>Put</c>, <c>Get</c>, <c>Indexer</c>, <c>Contains</c>,
  /// <c>Delete</c>, <c>Count</c>, and <c>IsEmpty</c> methods.
  /// It also provides ordered methods for finding the <c>Minimum</c>,
  /// <c>Maximum</c>, <c>Floor</c>, and <c>Ceiling</c>.
  /// It also provides a <c>Keys</c> method for iterating over all of the keys.
  /// A symbol table implements the <c>Associative array</c> abstraction:
  /// when associating a value with a key that is already in the symbol table,
  /// the convention is to replace the old value with the new value.
  /// Unlike <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>, this class uses the convention that
  /// values cannot be <c>null</c>. Setting the value associated with a key 
  /// to <c>null</c> is equivalent to deleting the key from the symbol table.</para>
  /// <para>This implementation uses a sorted array. It requires that
  /// the key type implements the <c>IComparable</c> interface and calls the
  /// <c>CompareTo()</c> method to compare two keys. It does not call either
  /// <c>Equals()</c> or <c>GetHashCode()</c>.
  /// The <c>Put</c> and <c>Remove</c> operations each take linear time in
  /// the worst case; the <c>Contains</c>, <c>Ceiling</c>, <c>Floor</c>,
  /// and <c>Rank</c> operations take logarithmic time; the <c>Count</c>,
  /// <c>IsEmpty</c>, <c>Minimum</c>, <c>Maximum</c>, and <c>indexer</c>
  /// operations take constant time. Construction takes constant time.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/31elementary">Section 3.1</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.</para><para>
  /// For other implementations, see <see cref="SequentialSearchST{Key, Value}"/>. This class
  /// is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BinarySearchST.java.html">BinarySearchST</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks>
  ///
  public class BinarySearchST<Key, Value> where Key : IComparable<Key>
  {
    private static readonly int INIT_CAPACITY = 2;
    private Key[] keys;
    private Value[] vals;
    private int N = 0;

    /// <summary>
    /// Initializes an empty symbol table.</summary>
    ///
    public BinarySearchST() : this(INIT_CAPACITY)
    {
    }

    /// <summary>
    /// Initializes an empty symbol table with the specified initial capacity.</summary>
    /// <param name="capacity">initial space for the collection</param>
    ///
    public BinarySearchST(int capacity)
    {
      keys = new Key[capacity];
      vals = new Value[capacity];
    }

    // resize the underlying arrays
    private void resize(int capacity)
    {
      Debug.Assert(capacity >= N);
      Key[] tempk = new Key[capacity];
      Value[] tempv = new Value[capacity];
      for (int i = 0; i < N; i++)
      {
        tempk[i] = keys[i];
        tempv[i] = vals[i];
      }
      vals = tempv;
      keys = tempk;
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
    /// Does this symbol table contain the given key?</summary>
    /// <param name="key">key the key</param>
    /// <returns><c>true</c> if this symbol table contains <c>key</c> and
    ///        <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentNullException"> if <c>key</c> is <c>null</c></exception>
    ///
    public bool Contains(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to Contains() is null");
      return Get(key) != null;
    }

    /// <summary>
    /// Returns the value associated with the given key in this symbol table.</summary>
    /// <param name="key"> key the key</param>
    /// <returns>the value associated with the given key if the key is in the symbol table
    ///        and <c>null</c> if the key is not in the symbol table</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public object Get(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to Get() is null");
      if (IsEmpty) return null;
      int i = Rank(key);
      if (i < N && keys[i].CompareTo(key) == 0) return vals[i];
      return null;
    }

    /// <summary>
    /// Returns the number of keys in this symbol table strictly less than <c>key</c>.</summary>
    /// <param name="key">key the key</param>
    /// <returns>the number of keys in the symbol table strictly less than <c>key</c></returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public int Rank(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to Rank() is null");

      int lo = 0, hi = N - 1;
      while (lo <= hi)
      {
        int mid = lo + (hi - lo) / 2;
        int cmp = key.CompareTo(keys[mid]);
        if (cmp < 0) hi = mid - 1;
        else if (cmp > 0) lo = mid + 1;
        else return mid;
      }
      return lo;
    }

    /// <summary>
    /// Removes the specified key and its associated value from this symbol table
    /// (if the key is in this symbol table).</summary>
    /// <param name="key">key the key</param>
    /// <param name="val">val the value</param>
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

      int i = Rank(key);

      // key is already in table
      if (i < N && keys[i].CompareTo(key) == 0)
      {
        vals[i] = val;
        return;
      }

      // insert new key-value pair
      if (N == keys.Length) resize(2 * keys.Length);

      for (int j = N; j > i; j--)
      {
        keys[j] = keys[j - 1];
        vals[j] = vals[j - 1];
      }
      keys[i] = key;
      vals[i] = val;
      N++;

      Debug.Assert(check());
    }

    /// <summary>
    /// Removes the specified key and associated value from this symbol table
    /// (if the key is in the symbol table).</summary>
    /// <param name="key">key the key</param>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public void Delete(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to delete() is null");
      if (IsEmpty) return; // indexer semantics

      // compute rank
      int i = Rank(key);

      // key not in table
      if (i == N || keys[i].CompareTo(key) != 0)
      {
        return;
      }

      for (int j = i; j < N - 1; j++)
      {
        keys[j] = keys[j + 1];
        vals[j] = vals[j + 1];
      }

      N--;
      // to avoid loitering for reference types
      keys[N] = default(Key);  
      vals[N] = default(Value);

      // resize if 1/4 full
      if (N > 0 && N == keys.Length / 4) resize(keys.Length / 2);

      Debug.Assert(check());
    }

    /// <summary>
    /// Removes the smallest key and associated value from this symbol table.</summary>
    /// <exception cref="InvalidOperationException">if the symbol table is empty</exception>
    ///
    public void DeleteMin()
    {
      if (IsEmpty) throw new InvalidOperationException("Symbol table underflow error");
      Delete(Min);
    }

    /// <summary>
    /// Removes the largest key and associated value from this symbol table.</summary>
    ///
    /// <exception cref="InvalidOperationException">if the symbol table is empty</exception>
    ///
    public void DeleteMax()
    {
      if (IsEmpty) throw new InvalidOperationException("Symbol table underflow error");
      Delete(Max);
    }

    /***************************************************************************
     *  Ordered symbol table methods.
     ***************************************************************************/

    /// <summary>
    /// Returns the smallest key in this symbol table.</summary>
    /// <returns>the smallest key in this symbol table</returns>
    /// <exception cref="InvalidOperationException">if this symbol table is empty</exception>
    ///
    public Key Min
    {
      get
      {
        if (IsEmpty) new InvalidOperationException("Symbol table underflow error");
        return keys[0];
      }
    }

    /// <summary>
    /// Returns the largest key in this symbol table.</summary>
    /// <returns>the largest key in this symbol table</returns>
    /// <exception cref="InvalidOperationException">if this symbol table is empty</exception>
    ///
    public Key Max
    {
      get
      {
        if (IsEmpty) new InvalidOperationException("Symbol table underflow error");
        return keys[N - 1];
      }
    }

    /// <summary>
    /// Return the kth smallest key in this symbol table.</summary>
    /// <param name="k">k the order statistic</param>
    /// <returns>the kth smallest key in this symbol table</returns>
    /// <exception cref="IndexOutOfRangeException">unless <c>k</c> is between 0 and
    ///       <c>N-1</c></exception>
    ///
    public Key Select(int k)
    {
      if (k < 0 || k >= N) throw new IndexOutOfRangeException("Refering to out of range index");
      return keys[k];
    }

    /// <summary>
    /// Returns the largest key in this symbol table less than or equal to <c>key</c>.</summary>
    /// <param name="key">key the key</param>
    /// <returns>the largest key in this symbol table less than or equal to <c>key</c></returns>
    /// <exception cref="KeyNotFoundException">if there is no such key</exception>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public Key Floor(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to Floor() is null");
      int i = Rank(key);
      if (i < N && key.CompareTo(keys[i]) == 0) return keys[i];
      if (i == 0) throw new KeyNotFoundException("floor key does not exist");
      else return keys[i - 1];
    }

    /// <summary>
    /// Returns the smallest key in this symbol table greater than or equal to <c>key</c>.</summary>
    /// <param name="key">key the key</param>
    /// <returns>the smallest key in this symbol table greater than or equal to <c>key</c></returns>
    /// <exception cref="KeyNotFoundException">if there is no such key</exception>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public Key Ceiling(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to ceiling() is null");
      int i = Rank(key);
      if (i == 0) throw new KeyNotFoundException("ceiling key does not exist");
      else return keys[i];
    }

    /// <summary>
    /// Returns the number of keys in this symbol table in the specified range.</summary>
    /// <param name="lo">lower bound key</param>
    /// <param name="hi">upper bound key</param>
    /// <returns>the number of keys in this sybol table between <c>lo</c>
    ///        (inclusive) and <c>hi</c> (exclusive)</returns>
    /// <exception cref="ArgumentNullException">if either <c>lo</c> or <c>hi</c>
    ///        is <c>null</c></exception>
    ///
    public int Size(Key lo, Key hi)
    {
      if (lo == null) throw new ArgumentNullException("first argument to Count is null");
      if (hi == null) throw new ArgumentNullException("second argument to Count is null");

      if (lo.CompareTo(hi) > 0) return 0;
      if (Contains(hi)) return Rank(hi) - Rank(lo) + 1;
      else return Rank(hi) - Rank(lo);
    }

    /// <summary>
    /// Returns all keys in this symbol table as an <c>IEnumerable</c>.
    /// To iterate over all of the keys in the symbol table named <c>st</c>,
    /// use the foreach notation: <c>foreach (Key key in st.Keys())</c>.</summary>
    /// <returns>all keys in this symbol table</returns>
    ///
    public IEnumerable<Key> Keys()
    {
      return Keys(Min, Max);
    }

    /// <summary>
    /// Returns all keys in this symbol table in the given range,
    /// as an <c>IEnumerable</c>.</summary>
    /// <returns>all keys in this symbol table between <c>lo</c>
    ///        (inclusive) and <c>hi</c> (exclusive)</returns>
    /// <param name="lo">lower bound key</param>
    /// <param name="hi">upper bound key</param>
    /// <exception cref="ArgumentNullException">if either <c>lo</c> or <c>hi</c>
    ///        is <c>null</c></exception>
    ///
    public IEnumerable<Key> Keys(Key lo, Key hi)
    {
      if (lo == null) throw new ArgumentNullException("first argument to Count is null");
      if (hi == null) throw new ArgumentNullException("second argument to Count is null");

      LinkedQueue<Key> queue = new LinkedQueue<Key>();
      if (lo == null) throw new ArgumentNullException("lo is null in keys()");
      if (hi == null) throw new ArgumentNullException("hi is null in keys()");
      if (lo.CompareTo(hi) > 0) return queue;
      for (int i = Rank(lo); i < Rank(hi); i++)
        queue.Enqueue(keys[i]);
      if (Contains(hi)) queue.Enqueue(keys[Rank(hi)]);
      return queue;
    }


    /***************************************************************************
     *  Check internal invariants.
     ***************************************************************************/

    private bool check()
    {
      return isSorted() && rankCheck();
    }

    // are the items in the array in ascending order?
    private bool isSorted()
    {
      for (int i = 1; i < Count; i++)
        if (keys[i].CompareTo(keys[i - 1]) < 0) return false;
      return true;
    }

    // check that rank(select(i)) = i
    private bool rankCheck()
    {
      for (int i = 0; i < Count; i++)
        if (i != Rank(Select(i))) return false;
      for (int i = 0; i < Count; i++)
        if (keys[i].CompareTo(Select(Rank(keys[i]))) != 0) return false;
      return true;
    }

    /// <summary>
    /// Demo test the <c>BinarySearchST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BinarySearchST < tiny.txt", "Keys to be associated with integer values")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();

      BinarySearchST<string, int> st = new BinarySearchST<string, int>();
      for (int i = 0; !StdIn.IsEmpty; i++)
      {
        string key = StdIn.ReadString();
        st[key] = i;
      }
      foreach (string s in st.Keys())
        Console.WriteLine("{0} {1}", s, st[s]);
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
