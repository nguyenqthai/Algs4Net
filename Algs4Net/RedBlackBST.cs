/******************************************************************************
 *  File name :    RedBlackBST.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/33balanced/tinyST.txt
 *
 *  A symbol table implemented using a left-leaning red-black BST.
 *  This is the 2-3 version.
 *
 *  C:\> type tinyST.txt
 *  S E A R C H E X A M P L E
 *
 *  C:\> algscmd RedBlackBST < tinyST.txt
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
  /// values cannot be <c>null</c>. Setting the
  /// value associated with a key to <c>null</c> is equivalent to deleting the key
  /// from the symbol table.</para>
  /// <para>This implementation uses a left-leaning red-black BST. It requires that
  /// the key type implements the <c>Comparable</c> interface and calls the
  /// <c>compareTo()</c> and method to compare two keys. It does not call either
  /// <c>equals()</c> or <c>GetHashCode()</c>.
  /// The <c>Put</c>, <c>Contains</c>, <c>Remove</c>, <c>Minimum</c>,
  /// <c>Maximum</c>, <c>Ceiling</c>, and <c>Floor</c> operations each take
  /// logarithmic time in the worst case, if the tree becomes unbalanced.
  /// The <c>Count</c>, and <c>IsEmpty</c> operations take constant time.
  /// Construction takes constant time.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/33balanced">Section 3.3</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.</para><para>
  /// For other implementations, see <see cref="BST{Key, Value}"/>. This class
  /// is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/RedBlackBST.java.html">BST</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks>
  /// 
  public class RedBlackBST<Key, Value> where Key : IComparable<Key>
  {
    private static readonly bool RED = true;
    private static readonly bool BLACK = false;

    private Node root;     // root of the BST

    // BST helper node data type
    private class Node
    {
      public Key key;           // key
      public Value val;         // associated data
      public Node left, right;  // links to left and right subtrees
      public bool color;     // color of parent link
      public int N;             // subtree count

      public Node(Key key, Value val, bool color, int N)
      {
        this.key = key;
        this.val = val;
        this.color = color;
        this.N = N;
      }
    }

    /// <summary>
    /// Initializes an empty symbol table.</summary>
    ///
    public RedBlackBST() { }

    /***************************************************************************
     *  Node helper methods.
     ***************************************************************************/
    // is node x red; false if x is null ?
    private bool isRed(Node x)
    {
      if (x == null) return false;
      return x.color == RED;
    }

    // number of node in subtree rooted at x; 0 if x is null
    private int size(Node x)
    {
      if (x == null) return 0;
      return x.N;
    }


    /// <summary>
    /// Returns the number of key-value pairs in this symbol table.</summary>
    /// <returns>the number of key-value pairs in this symbol table</returns>
    ///
    public int Count
    {
      get { return size(root); }
    }

    /// <summary>
    /// Is this symbol table empty?</summary>
    /// <returns><c>true</c> if this symbol table is empty and <c>false</c> otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return root == null; }
    }

    /***************************************************************************
     *  Standard BST search.
     ***************************************************************************/

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
    ///    and <c>null</c> if the key is not in the symbol table</returns>
    /// <exception cref="ArgumentNullException"> if <c>key</c> is <c>null</c></exception>
    ///
    public object Get(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to get() is null");
      return get(root, key);
    }

    // value associated with the given key in subtree rooted at x; null if no such key
    private object get(Node x, Key key)
    {
      while (x != null)
      {
        int cmp = key.CompareTo(x.key);
        if (cmp < 0) x = x.left;
        else if (cmp > 0) x = x.right;
        else return x.val;
      }
      return null;
    }

    /// <summary>
    /// Does this symbol table contain the given key?</summary>
    /// <param name="key">key the key</param>
    /// <returns><c>true</c> if this symbol table contains <c>key</c> and
    ///    <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public bool Contains(Key key)
    {
      return Get(key) != null;
    }

    /***************************************************************************
     *  Red-black tree insertion.
     ***************************************************************************/

    /// <summary>
    /// Inserts the specified key-value pair into the symbol table, overwriting the old
    /// value with the new value if the symbol table already contains the specified key.
    /// Deletes the specified key (and its associated value) from this symbol table
    /// if the specified value is <c>null</c>.</summary>
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

      root = put(root, key, val);
      root.color = BLACK;
      Debug.Assert(check());
    }

    // insert the key-value pair in the subtree rooted at h
    private Node put(Node h, Key key, Value val)
    {
      if (h == null) return new Node(key, val, RED, 1);

      int cmp = key.CompareTo(h.key);
      if (cmp < 0) h.left = put(h.left, key, val);
      else if (cmp > 0) h.right = put(h.right, key, val);
      else h.val = val;

      // fix-up any right-leaning links
      if (isRed(h.right) && !isRed(h.left)) h = rotateLeft(h);
      if (isRed(h.left) && isRed(h.left.left)) h = rotateRight(h);
      if (isRed(h.left) && isRed(h.right)) flipColors(h);
      h.N = size(h.left) + size(h.right) + 1;

      return h;
    }

    /***************************************************************************
     *  Red-black tree deletion.
     ***************************************************************************/

    /// <summary>
    /// Removes the smallest key and associated value from the symbol table.</summary>
    /// <exception cref="InvalidOperationException">if the symbol table is empty</exception>
    ///
    public void DeleteMin()
    {
      if (IsEmpty) throw new InvalidOperationException("BST underflow");

      // if both children of root are black, set root to red
      if (!isRed(root.left) && !isRed(root.right))
        root.color = RED;

      root = deleteMin(root);
      if (!IsEmpty) root.color = BLACK;
      Debug.Assert(check());
    }

    // delete the key-value pair with the minimum key rooted at h
    private Node deleteMin(Node h)
    {
      if (h.left == null)
        return null;

      if (!isRed(h.left) && !isRed(h.left.left))
        h = moveRedLeft(h);

      h.left = deleteMin(h.left);
      return balance(h);
    }


    /// <summary>
    /// Removes the largest key and associated value from the symbol table.</summary>
    /// <exception cref="InvalidOperationException">if the symbol table is empty</exception>
    ///
    public void DeleteMax()
    {
      if (IsEmpty) throw new InvalidOperationException("BST underflow");

      // if both children of root are black, set root to red
      if (!isRed(root.left) && !isRed(root.right))
        root.color = RED;

      root = deleteMax(root);
      if (!IsEmpty) root.color = BLACK;
      Debug.Assert(check());
    }

    // delete the key-value pair with the maximum key rooted at h
    private Node deleteMax(Node h)
    {
      if (isRed(h.left))
        h = rotateRight(h);

      if (h.right == null)
        return null;

      if (!isRed(h.right) && !isRed(h.right.left))
        h = moveRedRight(h);

      h.right = deleteMax(h.right);

      return balance(h);
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

      // if both children of root are black, set root to red
      if (!isRed(root.left) && !isRed(root.right))
        root.color = RED;

      root = delete(root, key);
      if (!IsEmpty) root.color = BLACK;
      Debug.Assert(check());
    }

    // delete the key-value pair with the given key rooted at h
    private Node delete(Node h, Key key)
    {
      Debug.Assert(get(h, key) != null);

      if (key.CompareTo(h.key) < 0)
      {
        if (!isRed(h.left) && !isRed(h.left.left))
          h = moveRedLeft(h);
        h.left = delete(h.left, key);
      }
      else {
        if (isRed(h.left))
          h = rotateRight(h);
        if (key.CompareTo(h.key) == 0 && (h.right == null))
          return null;
        if (!isRed(h.right) && !isRed(h.right.left))
          h = moveRedRight(h);
        if (key.CompareTo(h.key) == 0)
        {
          Node x = min(h.right);
          h.key = x.key;
          h.val = x.val;
          // h.val = get(h.right, min(h.right).key);
          // h.key = min(h.right).key;
          h.right = deleteMin(h.right);
        }
        else h.right = delete(h.right, key);
      }
      return balance(h);
    }

    /***************************************************************************
     *  Red-black tree helper functions.
     ***************************************************************************/

    // make a left-leaning link lean to the right
    private Node rotateRight(Node h)
    {
      Debug.Assert((h != null) && isRed(h.left));
      Node x = h.left;
      h.left = x.right;
      x.right = h;
      x.color = x.right.color;
      x.right.color = RED;
      x.N = h.N;
      h.N = size(h.left) + size(h.right) + 1;
      return x;
    }

    // make a right-leaning link lean to the left
    private Node rotateLeft(Node h)
    {
      Debug.Assert((h != null) && isRed(h.right));
      Node x = h.right;
      h.right = x.left;
      x.left = h;
      x.color = x.left.color;
      x.left.color = RED;
      x.N = h.N;
      h.N = size(h.left) + size(h.right) + 1;
      return x;
    }

    // flip the colors of a node and its two children
    private void flipColors(Node h)
    {
      // h must have opposite color of its two children
      Debug.Assert((h != null) && (h.left != null) && (h.right != null));
      Debug.Assert((!isRed(h) &&  isRed(h.left) &&  isRed(h.right))
                || (isRed(h)  && !isRed(h.left) && !isRed(h.right)));
      h.color = !h.color;
      h.left.color = !h.left.color;
      h.right.color = !h.right.color;
    }

    // Assuming that h is red and both h.left and h.left.left
    // are black, make h.left or one of its children red.
    private Node moveRedLeft(Node h)
    {
      Debug.Assert((h != null));
      Debug.Assert(isRed(h) && !isRed(h.left) && !isRed(h.left.left));

      flipColors(h);
      if (isRed(h.right.left))
      {
        h.right = rotateRight(h.right);
        h = rotateLeft(h);
        flipColors(h);
      }
      return h;
    }

    // Assuming that h is red and both h.right and h.right.left
    // are black, make h.right or one of its children red.
    private Node moveRedRight(Node h)
    {
      Debug.Assert((h != null));
      Debug.Assert(isRed(h) && !isRed(h.right) && !isRed(h.right.left));
      flipColors(h);
      if (isRed(h.left.left))
      {
        h = rotateRight(h);
        flipColors(h);
      }
      return h;
    }

    // restore red-black tree invariant
    private Node balance(Node h)
    {
      Debug.Assert((h != null));

      if (isRed(h.right)) h = rotateLeft(h);
      if (isRed(h.left) && isRed(h.left.left)) h = rotateRight(h);
      if (isRed(h.left) && isRed(h.right)) flipColors(h);

      h.N = size(h.left) + size(h.right) + 1;
      return h;
    }

    /***************************************************************************
     *  Utility functions.
     ***************************************************************************/

    /// <summary>
    /// Returns the height of the BST (for debugging).</summary>
    /// <returns>the height of the BST (a 1-node tree has height 0)</returns>
    ///
    public int Height
    {
      get { return height(root); }
    }

    private int height(Node x)
    {
      if (x == null) return -1;
      return 1 + Math.Max(height(x.left), height(x.right));
    }

    /***************************************************************************
     *  Ordered symbol table methods.
     ***************************************************************************/

    /// <summary>
    /// Returns the smallest key in the symbol table.</summary>
    /// <returns>the smallest key in the symbol table</returns>
    /// <exception cref="InvalidOperationException">if the symbol table is empty</exception>
    ///
    public Key Min
    {
      get
      {
        if (IsEmpty) throw new InvalidOperationException("called min() with empty symbol table");
        return min(root).key;
      }
    }

    // the smallest key in subtree rooted at x; null if no such key
    private Node min(Node x)
    {
      Debug.Assert(x != null);
      if (x.left == null) return x;
      else return min(x.left);
    }

    /// <summary>
    /// Returns the largest key in the symbol table.</summary>
    /// <returns>the largest key in the symbol table</returns>
    /// <exception cref="InvalidOperationException">if the symbol table is empty</exception>
    ///
    public Key Max
    {
      get {
        if (IsEmpty) throw new InvalidOperationException("called max() with empty symbol table");
        return max(root).key;
      }
    }

    // the largest key in the subtree rooted at x; null if no such key
    private Node max(Node x)
    {
      Debug.Assert(x != null);
      if (x.right == null) return x;
      else return max(x.right);
    }


    /// <summary>
    /// Returns the largest key in the symbol table less than or equal to <c>key</c>.</summary>
    /// <param name="key">key the key</param>
    /// <returns>the largest key in the symbol table less than or equal to <c>key</c></returns>
    /// <exception cref="KeyNotFoundException">if there is no such key</exception>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    /// <exception cref="InvalidOperationException">if the symbol table is empty</exception>
    ///
    public Key Floor(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to floor() is null");
      if (IsEmpty) throw new InvalidOperationException("called floor() with empty symbol table");
      Node x = floor(root, key);
      if (x == null) throw new KeyNotFoundException("floor key does not exist");
      else return x.key;
    }

    // the largest key in the subtree rooted at x less than or equal to the given key
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
    /// <exception cref="KeyNotFoundException">if there is no such key</exception>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    /// <exception cref="InvalidOperationException">if the symbol table is empty</exception>
    ///
    public Key Ceiling(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to ceiling() is null");
      if (IsEmpty) throw new InvalidOperationException("called ceiling() with empty symbol table");
      Node x = ceiling(root, key);
      if (x == null) throw new KeyNotFoundException("ceiling key does not exist");
      else return x.key;
    }

    // the smallest key in the subtree rooted at x greater than or equal to the given key
    private Node ceiling(Node x, Key key)
    {
      if (x == null) return null;
      int cmp = key.CompareTo(x.key);
      if (cmp == 0) return x;
      if (cmp > 0) return ceiling(x.right, key);
      Node t = ceiling(x.left, key);
      if (t != null) return t;
      else return x;
    }

    /// <summary>
    /// Return the kth smallest key in the symbol table.</summary>
    /// <param name="k">k the order statistic</param>
    /// <returns>the kth smallest key in the symbol table</returns>
    /// <exception cref="IndexOutOfRangeException">unless <c>k</c> is between 0 and
    ///    <c>N</c> - 1</exception>
    ///
    public Key Select(int k)
    {
      if (k < 0 || k >= Count) throw new IndexOutOfRangeException();
      Node x = select(root, k);
      return x.key;
    }

    // the key of Rank k in the subtree rooted at x
    private Node select(Node x, int k)
    {
      Debug.Assert(x != null);
      Debug.Assert(k >= 0 && k < size(x));
      int t = size(x.left);
      if (t > k) return select(x.left, k);
      else if (t < k) return select(x.right, k - t - 1);
      else return x;
    }

    /// <summary>
    /// Return the number of keys in the symbol table strictly less than <c>key</c>.</summary>
    /// <param name="key">key the key</param>
    /// <returns>the number of keys in the symbol table strictly less than <c>key</c></returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public int Rank(Key key)
    {
      if (key == null) throw new ArgumentNullException("argument to Rank() is null");
      return Rank(key, root);
    }

    // number of keys less than key in the subtree rooted at x
    private int Rank(Key key, Node x)
    {
      if (x == null) return 0;
      int cmp = key.CompareTo(x.key);
      if (cmp < 0) return Rank(key, x.left);
      else if (cmp > 0) return 1 + size(x.left) + Rank(key, x.right);
      else return size(x.left);
    }

    /***************************************************************************
     *  Range count and range search.
     ***************************************************************************/

    /// <summary>
    /// Returns all keys in the symbol table as an <c>IEnumerable</c>.
    /// To iterate over all of the keys in the symbol table named <c>st</c>,
    /// use the foreach notation: <c>foreach (Key key in st.Keys())</c>.</summary>
    /// <returns>all keys in the sybol table as an <c>IEnumerable</c></returns>
    ///
    public IEnumerable<Key> Keys()
    {
      if (IsEmpty) return new LinkedQueue<Key>();
      return Keys(Min, Max);
    }

    /// <summary>
    /// Returns all keys in the symbol table in the given range,
    /// as an <c>IEnumerable</c>.</summary>
    /// <param name="lo">lower bound key</param>
    /// <param name="hi">upper bound key</param>
    /// <returns>all keys in the sybol table between <c>lo</c></returns>
    ///   (inclusive) and <c>hi</c> (exclusive) as an <c>IEnumerable</c>
    /// <exception cref="ArgumentNullException">if either <c>lo</c> or <c>hi</c>
    ///   is <c>null</c></exception>
    ///
    public IEnumerable<Key> Keys(Key lo, Key hi)
    {
      if (lo == null) throw new ArgumentNullException("first argument to keys() is null");
      if (hi == null) throw new ArgumentNullException("second argument to keys() is null");

      LinkedQueue<Key> queue = new LinkedQueue<Key>();
      // if (IsEmpty || lo.CompareTo(hi) > 0) return queue;
      keys(root, queue, lo, hi);
      return queue;
    }

    // add the keys between lo and hi in the subtree rooted at x
    // to the queue
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
    /// <param name="lo">lower bound key</param>
    /// <param name="hi">upper bound key</param>
    /// <returns>the number of keys in the sybol table between <c>lo</c>
    ///   (inclusive) and <c>hi</c> (exclusive)</returns>
    /// <exception cref="ArgumentNullException">if either <c>lo</c> or <c>hi</c>
    ///   is <c>null</c></exception>
    ///
    public int Size(Key lo, Key hi)
    {
      if (lo == null) throw new ArgumentNullException("first argument to size() is null");
      if (hi == null) throw new ArgumentNullException("second argument to size() is null");

      if (lo.CompareTo(hi) > 0) return 0;
      if (Contains(hi)) return Rank(hi) - Rank(lo) + 1;
      else return Rank(hi) - Rank(lo);
    }


    /***************************************************************************
     *  Check integrity of red-black tree data structure.
     ***************************************************************************/
    private bool check()
    {
      if (!isBST()) Console.WriteLine("Not in symmetric order");
      if (!isSizeConsistent()) Console.WriteLine("Subtree counts not consistent");
      if (!isRankConsistent()) Console.WriteLine("Ranks not consistent");
      if (!is23()) Console.WriteLine("Not a 2-3 tree");
      if (!isBalanced()) Console.WriteLine("Not balanced");
      return isBST() && isSizeConsistent() && isRankConsistent() && is23() && isBalanced();
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

    // Does the tree have no red right links, and at most one (left)
    // red links in a row on any path?
    private bool is23() { return is23(root); }
    private bool is23(Node x)
    {
      if (x == null) return true;
      if (isRed(x.right)) return false;
      if (x != root && isRed(x) && isRed(x.left))
        return false;
      return is23(x.left) && is23(x.right);
    }

    // do all paths from root to leaf have same number of black edges?
    private bool isBalanced()
    {
      int black = 0;     // number of black links on path from root to min
      Node x = root;
      while (x != null)
      {
        if (!isRed(x)) black++;
        x = x.left;
      }
      return isBalanced(root, black);
    }

    // does every path from the root to a leaf have the given number of black links?
    private bool isBalanced(Node x, int black)
    {
      if (x == null) return black == 0;
      if (!isRed(x)) black--;
      return isBalanced(x.left, black) && isBalanced(x.right, black);
    }

    /// <summary>
    /// Demo test the <c>RedBlackBST</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd RedBlackBST < tinyST.txt")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      RedBlackBST<string, int> st = new RedBlackBST<string, int>();
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
