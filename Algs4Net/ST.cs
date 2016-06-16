/******************************************************************************
 *  File name :    ST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Sorted symbol table implementation using .NET's SortedDictionary
 *  Does not allow duplicates.
 *
 *  C:\> algscmd ST
 * ab hb
 * ab 0
 * ac 4
 * cd 1
 * cf 5
 * cz 8
 * eb 6
 * ef 2
 * gh 3
 * ha 9
 * hb 10
 *
 ******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>ST</c> class represents an ordered symbol table of generic
  /// key-value pairs.
  /// It supports the usual <c>Put</c>, <c>Get</c>, <c>Contains</c>, <c>Indexer</c>,
  /// <c>Delete</c>, <c>Count</c>, and <c>IsEmpty</c> methods.
  /// It also provides ordered methods for finding the <c>Minimum</c>,
  /// <c>Maximum</c>, <c>Floor</c>, and <c>Ceiling</c>.
  /// It also provides a <c>Keys</c> method for iterating over all of the keys.
  /// A symbol table implements the <c>Associative array</c> abstraction:
  /// when associating a value with a key that is already in the symbol table,
  /// the convention is to replace the old value with the new value.
  /// Unlike <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>, this class uses the convention that
  /// values cannot be <c>null</c>. Setting the
  /// value associated with a key to <c>null</c> is equivalent to deleting the key
  /// from the symbol table.</para><para>
  /// This implementation uses a balanced binary search tree. It requires that
  /// the key type implements the <c>IComparable</c> interface and calls the
  /// <c>CompareTo()</c> and method to compare two keys. It does not call either
  /// <c>Equals()</c> or <c>GetHashCode()</c>.
  /// The <c>Put</c>, <c>Contains</c>, <c>Remove</c>, <c>Minimum</c>,
  /// <c>Maximum</c>, <c>Ceiling</c>, and <c>Floor</c> operations each take
  /// logarithmic time in the worst case.
  /// The <c>Count</c>, and <c>IsEmpty</c> operations take constant time.
  /// Construction takes constant time.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/35applications">Section 3.5</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/ST.java.html">ST</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class ST<Key, Value> : IEnumerable<KeyValuePair<Key, Value>> where Key : IComparable<Key>
  {
    private SortedDictionary<Key, Value> st;

    /// <summary>Initializes an empty symbol table.</summary>
    ///
    public ST()
    {
      st = new SortedDictionary<Key, Value>();
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
    /// <param name="key">the key</param>
    /// <returns>the value associated with the given key if the key is in this symbol table;
    ///        <c>null</c> if the key is not in this symbol table</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public object Get(Key key)
    {
      if (key == null) throw new  ArgumentNullException("called get() with null key");
      if (st.ContainsKey(key)) return st[key];
      return null;
    }

    /// <summary>
    /// Inserts the specified key-value pair into the symbol table, overwriting the old
    /// value with the new value if the symbol table already contains the specified key.
    /// Deletes the specified key (and its associated value) from this symbol table
    /// if the specified value is <c>null</c>.</summary>
    /// <param name="key">the key</param>
    /// <param name="val">val the value</param>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public void Put(Key key, Value val)
    {
      if (key == null) throw new ArgumentNullException("called Put() with null key");
      if (val == null) st.Remove(key);
      else
      {
        st.Remove(key);
        st.Add(key, val);
      }
    }

    /// <summary>
    /// Removes the specified key and its associated value from this symbol table
    /// (if the key is in this symbol table).</summary>
    /// <param name="key">the key</param>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public void Delete(Key key)
    {
      if (key == null) throw new ArgumentNullException("called Delete() with null key");
      st.Remove(key);
    }

    /// <summary>
    /// Returns true if this symbol table contain the given key.</summary>
    /// <param name="key">the key</param>
    /// <returns><c>true</c> if this symbol table contains <c>key</c> and
    ///        <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public bool Contains(Key key)
    {
      if (key == null) throw new ArgumentNullException("called contains() with null key");
      return st.ContainsKey(key);
    }

    /// <summary>
    /// Returns the number of key-value pairs in this symbol table.</summary>
    /// <returns>the number of key-value pairs in this symbol table</returns>
    ///
    public int Count
    {
      get { return st.Count; }
    }

    /// <summary>
    /// Returns true if this symbol table is empty.</summary>
    /// <returns><c>true</c> if this symbol table is empty and <c>false</c> otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return Count == 0; }
    }

      /// <summary>
      /// Returns all keys in this symbol table.
      /// To iterate over all of the keys in the symbol table named <c>st</c>,
      /// use the foreach notation: <c>foreach (Key key n st.Keys())</c>.</summary>
      ///
      /// <returns>all keys in this symbol table</returns>
      ///
      public IEnumerable<Key> Keys()
      {
        return st.Keys;
      }

      /// <summary>
      /// Returns the smallest key in this symbol table.</summary>
      /// <returns>the smallest key in this symbol table</returns>
      /// <exception cref="InvalidOperationException">if this symbol table is empty</exception>
      ///
      public Key Min
      {
        get
        {
          if (IsEmpty) throw new InvalidOperationException("called Min with empty symbol table");
          return st.Keys.Min();
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
          if (IsEmpty) throw new InvalidOperationException("called max() with empty symbol table");
          return st.Keys.Max();
        }
      }

      /// <summary>
      /// Returns the smallest key in this symbol table greater than or equal to <c>key</c>.</summary>
      /// <param name="key">the key</param>
      /// <returns>the smallest key in this symbol table greater than or equal to <c>key</c></returns>
      /// <exception cref="InvalidOperationException">if there is no such key</exception>
      /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
      ///
      public Key Ceiling(Key key)
      {
        if (key == null) throw new ArgumentNullException("called ceiling() with null key");
        Key k = st.Keys.First(s => s.CompareTo(key) >= 0);
        if (k == null) throw new InvalidOperationException("all keys are less than " + key);
        return k;
      }

      /// <summary>
      /// Returns the largest key in this symbol table less than or equal to <c>key</c>.</summary>
      /// <param name="key"> key the key</param>
      /// <returns>the largest key in this symbol table less than or equal to <c>key</c></returns>
      /// <exception cref="InvalidOperationException">if there is no such key</exception>
      /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
      ///
      public Key Floor(Key key)
      {
        if (key == null) throw new ArgumentNullException("called floor() with null key");
        Key k = st.Keys.First(s => s.CompareTo(key) <= 0);
        if (k == null) throw new InvalidOperationException("all keys are greater than " + key);
        return k;
      }

    /// <summary>
    /// Returns all of the key-value pairs in this symbol table.
    /// To iterate over all of the keys in a symbol table named <c>st</c>, use the
    /// foreach notation: <c>foreach (KeyValuePair pairs in st)</c>.
    /// This method is provided for compatibility with the inner dictionary class. Use the method 
    /// <see cref="Keys"/> for all-key iteration</summary>
    /// <returns>an iterator to all of the key-value pairs in this symbol table</returns>
    ///
    IEnumerator<KeyValuePair<Key, Value>> IEnumerable<KeyValuePair<Key, Value>>.GetEnumerator()
    {
      return ((IEnumerable<KeyValuePair<Key, Value>>)st).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable<KeyValuePair<Key, Value>>)st).GetEnumerator();
    }

    /// <summary>
    /// Demo test the <c>ST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd ST")]
    public static void MainTest(string[] args)
    {
      ST<string, int> st = new ST<string, int>();
      string[] keys = { "ab", "cd", "ef", "gh", "ac", "cf", "eb", "ga", "cz", "ha", "hb" };

      for (int i = 0; i < keys.Length; i++)
      {
        st.Put(keys[i], i);
      }
      string ga = "ga";
      st.Delete(ga);
      if (st.Contains(ga)) throw new ApplicationException("delete key did not work");
      Array.Sort(keys);
      if (st.Min.Equals(keys[0]) && st.Max.Equals(keys[keys.Length - 1]))
        Console.WriteLine("{0} {1}", keys[0], keys[keys.Length - 1]);
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
