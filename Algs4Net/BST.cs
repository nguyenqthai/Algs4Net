/******************************************************************************
 *  File name :    BST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/32bst/tinyST.txt
 *
 *  A symbol table implemented with a binary search tree.
 *
 *  C:\> type tinyST.txt
 *  S E A R C H E X A M P L E
 *
 *  C:\> algscmd BST < tinyST.txt
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
  /// <c>Maximum</c>, <c>Floor</c>, <c>Select</c>, <c>Ceiling</c>.
  /// It also provides a <c>Keys</c> method for iterating over all of the keys.
  /// A symbol table implements the <c>Associative array</c> abstraction:
  /// when associating a value with a key that is already in the symbol table,
  /// the convention is to replace the old value with the new value.
  /// Unlike <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>, this class uses the convention that
  /// values cannot be <c>null</c>. Setting the
  /// value associated with a key to <c>null</c> is equivalent to deleting the key
  /// from the symbol table.</para>
  /// <para>This implementation uses an (unbalanced) binary search tree. It requires that
  /// the key type implements the <c>Comparable</c> interface and calls the
  /// <c>CompareTo()</c> and method to compare two keys. It does not call either
  /// <c>Equals()</c> or <c>GetHashCode()</c>.
  /// The <c>Put</c>, <c>Contains</c>, <c>Remove</c>, <c>Minimum</c>,
  /// <c>Maximum</c>, <c>Ceiling</c>, <c>Floor</c>, <c>Select</c>, and
  /// <c>Rank</c>  operations each take
  /// linear time in the worst case, if the tree becomes unbalanced.
  /// The <c>Count</c>, and <c>IsEmpty</c> operations take constant time.
  /// Construction takes constant time.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/32bst">Section 3.2</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.</para><para>
  /// For other implementations, see <see cref="SequentialSearchST{Key, Value}"/>. This class
  /// is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BST.java.html">BST</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks>
  ///
  public class BST<Key, Value> where Key : IComparable<Key>
  {
    private Node root;             // root of BST

    private class Node
    {
      public Key key;           // sorted by key
      public Value val;         // associated data
      public Node left, right;  // left and right subtrees
      public int N;             // number of nodes in subtree

      public Node(Key key, Value val, int N)
      {
        this.key = key;
        this.val = val;
        this.N = N;
      }
    }

    /// <summary>
    /// Initializes an empty symbol table.</summary>
    ///
    public BST() { }

    /// <summary>
    /// Returns true if this symbol table is empty.</summary>
    /// <returns><c>true</c> if this symbol table is empty; <c>false</c> otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return Count == 0; }
    }

    /// <summary>
    /// Returns the number of key-value pairs in this symbol table.</summary>
    /// <returns>the number of key-value pairs in this symbol table</returns>
    ///
    public int Count
    {
      get { return size(root); }
    }

    // return number of key-value pairs in BST rooted at x
    private int size(Node x)
    {
      if (x == null) return 0;
      else return x.N;
    }

    /// <summary>
    /// Does this symbol table contain the given key?</summary>
    /// <param name="key">key the key</param>
    /// <returns><c>true</c>if this symbol table contains <c>key</c> and
    ///        <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentNullException" >if <c>key</c> is <c>null</c></exception>
    ///
    public bool Contains(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to contains() is null");
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
    /// Returns the value associated with the given key.</summary>
    /// <param name="key">key the key</param>
    /// <returns>the value associated with the given key if the key is in the symbol table
    ///        and <c>null</c> if the key is not in the symbol table.</returns>
    /// <exception cref="ArgumentNullException" >if <c>key</c> is <c>null</c></exception>
    ///
    public object Get(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to Get() is null");
      return get(root, key);
    }

    private object get(Node x, Key key)
    {
      if (x == null) return null;
      int cmp = key.CompareTo(x.key);
      if (cmp < 0) return get(x.left, key);
      else if (cmp > 0) return get(x.right, key);
      else return x.val;
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
      root = put(root, key, val);
      Debug.Assert(check());
    }

    private Node put(Node x, Key key, Value val)
    {
      if (x == null) return new Node(key, val, 1);
      int cmp = key.CompareTo(x.key);
      if (cmp < 0) x.left = put(x.left, key, val);
      else if (cmp > 0) x.right = put(x.right, key, val);
      else x.val = val;
      x.N = 1 + size(x.left) + size(x.right);
      return x;
    }

    /// <summary>
    /// Removes the smallest key and associated value from the symbol table.</summary>
    /// <exception cref="InvalidOperationException" >if the symbol table is empty</exception>
    ///
    public void DeleteMin()
    {
      if (IsEmpty) throw new InvalidOperationException("Symbol table underflow");
      root = deleteMin(root);
      Debug.Assert(check());
    }

    private Node deleteMin(Node x)
    {
      if (x.left == null) return x.right;
      x.left = deleteMin(x.left);
      x.N = size(x.left) + size(x.right) + 1;
      return x;
    }

    /// <summary>
    /// Removes the largest key and associated value from the symbol table.</summary>
    /// <exception cref="InvalidOperationException" >if the symbol table is empty</exception>
    ///
    public void DeleteMax()
    {
      if (IsEmpty) throw new InvalidOperationException("Symbol table underflow");
      root = deleteMax(root);
      Debug.Assert(check());
    }

    private Node deleteMax(Node x)
    {
      if (x.right == null) return x.left;
      x.right = deleteMax(x.right);
      x.N = size(x.left) + size(x.right) + 1;
      return x;
    }

    /// <summary>
    /// Removes the specified key and its associated value from this symbol table
    /// (if the key is in this symbol table).</summary>
    /// <param name="key">key the key</param>
    /// <exception cref="ArgumentNullException" >if <c>key</c> is <c>null</c></exception>
    ///
    public void Delete(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to delete() is null");
      if (IsEmpty) return; // indexer semantics
      // special case of one-node tree
      if (Count == 1)
      {
        root = null;
        return;
      }
      root = delete(root, key);
      Debug.Assert(check());
    }

    private Node delete(Node x, Key key)
    {
      if (x == null) return null;

      int cmp = key.CompareTo(x.key);
      if (cmp < 0) x.left = delete(x.left, key);
      else if (cmp > 0) x.right = delete(x.right, key);
      else
      {
        if (x.right == null) return x.left;
        if (x.left == null) return x.right;
        Node t = x;
        x = min(t.right);
        x.right = deleteMin(t.right);
        x.left = t.left;
      }
      x.N = size(x.left) + size(x.right) + 1;
      return x;
    }

    /// <summary>
    /// Returns the smallest key in the symbol table.</summary>
    /// <returns>the smallest key in the symbol table</returns>
    /// <exception cref="InvalidOperationException" >if the symbol table is empty</exception>
    ///
    public Key Min
    {
      get
      {
        if (IsEmpty) throw new InvalidOperationException("called Min with empty symbol table");
        return min(root).key;
      }
    }

    private Node min(Node x)
    {
      if (x.left == null) return x;
      else return min(x.left);
    }

    /// <summary>
    /// Returns the largest key in the symbol table.</summary>
    /// <returns>the largest key in the symbol table</returns>
    /// <exception cref="InvalidOperationException" >if the symbol table is empty</exception>
    ///
    public Key Max
    {
      get
      {
        if (IsEmpty) throw new InvalidOperationException("called max() with empty symbol table");
        return max(root).key;
      }
    }

    private Node max(Node x)
    {
      if (x.right == null) return x;
      else return max(x.right);
    }

    /// <summary>
    /// Returns the largest key in the symbol table less than or equal to <c>key</c>.</summary>
    /// <param name="key">key the key</param>
    /// <returns>the largest key in the symbol table less than or equal to <c>key</c></returns>
    /// <exception cref="KeyNotFoundException" >if there is no such key</exception>
    /// <exception cref="ArgumentNullException" >if <c>key</c> is <c>null</c></exception>
    ///<exception cref="InvalidOperationException">if the symbol table is empty</exception>
    ///
    public Key Floor(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to floor() is null");
      if (IsEmpty) throw new InvalidOperationException("called Floor() with empty symbol table");
      Node x = floor(root, key);
      if (x == null) throw new KeyNotFoundException("floor key does not exist");
      else return x.key;
    }

    private Node floor(Node x, Key key)
    {
      if (x == null) return null;
      int cmp = key.CompareTo(x.key);
      if (cmp == 0) return x;
      if (cmp < 0) return floor(x.left, key);
      Node t = floor(x.right, key);
      if (t != null) return t;
      else return x;
    }

    /// <summary>
    /// Returns the smallest key in the symbol table greater than or equal to <c>key</c>.</summary>
    /// <param name="key">key the key</param>
    /// <returns>the smallest key in the symbol table greater than or equal to <c>key</c></returns>
    /// <exception cref="KeyNotFoundException" >if there is no such key</exception>
    /// <exception cref="ArgumentNullException" >if <c>key</c> is <c>null</c></exception>
    ///<exception cref="InvalidOperationException">if the symbol table is empty</exception>
    ///
    public Key Ceiling(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to Ceiling() is null");
      if (IsEmpty) throw new InvalidOperationException("called Ceiling() with empty symbol table");
      Node x = ceiling(root, key);
      if (x == null) throw new KeyNotFoundException("ceiling key does not exist");
      else return x.key;
    }

    private Node ceiling(Node x, Key key)
    {
      if (x == null) return null;
      int cmp = key.CompareTo(x.key);
      if (cmp == 0) return x;
      if (cmp < 0)
      {
        Node t = ceiling(x.left, key);
        if (t != null) return t;
        else return x;
      }
      return ceiling(x.right, key);
    }

    /// <summary>
    /// Return the kth smallest key in the symbol table.</summary>
    /// <param name="k">k the order statistic</param>
    /// <returns>the kth smallest key in the symbol table</returns>
    /// <exception cref="IndexOutOfRangeException"> unless <c>k</c> is between 0 and
    ///       <c>N</c> - 1</exception>
    ///
    public Key Select(int k)
    {
      if (k < 0 || k >= Count) throw new IndexOutOfRangeException();
      Node x = select(root, k);
      return x.key;
    }

    // Return key of rank k.
    private Node select(Node x, int k)
    {
      if (x == null) return null;
      int t = size(x.left);
      if (t > k) return select(x.left, k);
      else if (t < k) return select(x.right, k - t - 1);
      else return x;
    }

    /// <summary>
    /// Return the number of keys in the symbol table strictly less than <c>key</c>.</summary>
    /// <param name="key">key the key</param>
    /// <returns>the number of keys in the symbol table strictly less than <c>key</c></returns>
    /// <exception cref="ArgumentNullException" >if <c>key</c> is <c>null</c></exception>
    ///
    public int Rank(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to rank() is null");
      return rank(key, root);
    }

    // Number of keys in the subtree less than key.
    private int rank(Key key, Node x)
    {
      if (x == null) return 0;
      int cmp = key.CompareTo(x.key);
      if (cmp < 0) return rank(key, x.left);
      else if (cmp > 0) return 1 + size(x.left) + rank(key, x.right);
      else return size(x.left);
    }

    /// <summary>
    /// Returns all keys in the symbol table as an <c>IEnumerable</c>.
    /// To iterate over all of the keys in the symbol table named <c>st</c>,
    /// use the foreach notation: <c>foreach (Key key in st.Keys())</c>.</summary>
    /// <returns>all keys in the symbol table</returns>
    ///
    public IEnumerable<Key> Keys()
    {
      return Keys(Min, Max);
    }

    /// <summary>
    /// Returns all keys in the symbol table in the given range,
    /// as an <c>IEnumerable</c>.</summary>
    /// <param name="lo">lower bound key</param>
    /// <param name="hi">upper bound key</param>
    /// <returns>all keys in the sybol table between <c>lo</c>
    ///        (inclusive) and <c>hi</c> (exclusive)</returns>
    /// <exception cref="ArgumentNullException" >if either <c>lo</c> or <c>hi</c>
    ///        is <c>null</c></exception>
    ///
    public IEnumerable<Key> Keys(Key lo, Key hi)
    {
      if (lo == null) throw new ArgumentNullException("first argument to Keys() is null");
      if (hi == null) throw new ArgumentNullException("second argument to Keys() is null");

      LinkedQueue<Key> queue = new LinkedQueue<Key>();
      keys(root, queue, lo, hi);
      return queue;
    }

    private void keys(Node x, LinkedQueue<Key> queue, Key lo, Key hi)
    {
      if (x == null) return;
      int cmplo = lo.CompareTo(x.key);
      int cmphi = hi.CompareTo(x.key);
      if (cmplo < 0) keys(x.left, queue, lo, hi);
      if (cmplo <= 0 && cmphi >= 0) queue.Enqueue(x.key);
      if (cmphi > 0) keys(x.right, queue, lo, hi);
    }

    /// <summary>
    /// Returns the number of keys in the symbol table in the given range.</summary>
    /// <returns>the number of keys in the sybol table between <c>lo</c>
    ///        (inclusive) and <c>hi</c> (exclusive)</returns>
    /// <param name="lo">lower bound key</param>
    /// <param name="hi">upper bound key</param>
    /// <exception cref="ArgumentNullException" >if either <c>lo</c> or <c>hi</c>
    ///        is <c>null</c></exception>
    ///
    public int Size(Key lo, Key hi)
    {
      if (lo == null) throw new ArgumentNullException("first argument to size() is null");
      if (hi == null) throw new ArgumentNullException("second argument to size() is null");

      if (lo.CompareTo(hi) > 0) return 0;
      if (Contains(hi)) return Rank(hi) - Rank(lo) + 1;
      else return Rank(hi) - Rank(lo);
    }

    /// <summary>
    /// Returns the height of the BST (for debugging).</summary>
    /// <returns>the height of the BST (a 1-node tree has height 0)</returns>
    ///
    public int Height()
    {
      return height(root);
    }

    private int height(Node x)
    {
      if (x == null) return -1;
      return 1 + Math.Max(height(x.left), height(x.right));
    }

    /// <summary>
    /// Returns the keys in the BST in level order (for debugging).</summary>
    /// <returns>the keys in the BST in level order traversal</returns>
    ///
    public IEnumerable<Key> LevelOrder()
    {
      LinkedQueue<Key> keys = new LinkedQueue<Key>();
      LinkedQueue<Node> queue = new LinkedQueue<Node>();
      queue.Enqueue(root);
      while (!queue.IsEmpty)
      {
        Node x = queue.Dequeue();
        if (x == null) continue;
        keys.Enqueue(x.key);
        queue.Enqueue(x.left);
        queue.Enqueue(x.right);
      }
      return keys;
    }

    /*************************************************************************
    *  Check integrity of BST data structure.
    ***************************************************************************/
    private bool check()
    {
      if (!isBST()) Console.WriteLine("Not in symmetric order");
      if (!isSizeConsistent()) Console.WriteLine("Subtree counts not consistent");
      if (!isRankConsistent()) Console.WriteLine("Ranks not consistent");
      return isBST() && isSizeConsistent() && isRankConsistent();
    }

    // does this binary tree satisfy symmetric order?
    // Note: this test also ensures that data structure is a binary tree since order is strict
    private bool isBST()
    {
      return isBST(root, null, null);
    }

    // is the tree rooted at x a BST with all keys strictly between min and max
    // (if min or max is null, treat as empty constraint)
    // Credit: Bob Dondero's elegant solution
    private bool isBST(Node x, object min, object max)
    {
      if (x == null) return true;
      if (min != null && x.key.CompareTo((Key)min) <= 0) return false;
      if (max != null && x.key.CompareTo((Key)max) >= 0) return false;
      return isBST(x.left, min, x.key) && isBST(x.right, x.key, max);
    }

    // are the size fields correct?
    private bool isSizeConsistent() { return isSizeConsistent(root); }
    private bool isSizeConsistent(Node x)
    {
      if (x == null) return true;
      if (x.N != size(x.left) + size(x.right) + 1) return false;
      return isSizeConsistent(x.left) && isSizeConsistent(x.right);
    }

    // check that ranks are consistent
    private bool isRankConsistent()
    {
      for (int i = 0; i < Count; i++)
        if (i != Rank(Select(i))) return false;
      foreach (Key key in Keys())
        if (key.CompareTo(Select(Rank(key))) != 0) return false;
      return true;
    }

    /// <summary>
    /// Demo test the <c>BST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BST < tinyST.txt", "Keys to be associated with integer values")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      BST<string, int> st = new BST<string, int>();
      for (int i = 0; !StdIn.IsEmpty; i++)
      {
        string key = StdIn.ReadString();
        st[key] = i;
      }

      foreach (string s in st.Keys())
        Console.WriteLine("{0} {1}", s, st.Get(s));

      Console.WriteLine();

      foreach (string s in st.LevelOrder())
        Console.WriteLine(s + " " + st[s]);
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
