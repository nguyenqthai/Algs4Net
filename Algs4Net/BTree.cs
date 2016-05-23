/******************************************************************************
 *  File name :    BTree.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  B-tree.
 *  
 *  Limitations
 *  -----------
 *   -  Assumes M is even and M >= 4
 *   -  should b be an array of children or list (it would help with
 *      casting to make it a list)
 *
 *  C:\> algscmd BTree
 *  cs.princeton.edu:  128.112.136.12
 *  hardvardsucks.com:
 *  simpsons.com:      209.052.165.60
 *  apple.com:         17.112.152.32
 *  ebay.com:          66.135.192.87
 *  dell.com:          143.166.224.230
 *  
 *  Count:    17
 *  Height:   2
 *            www.amazon.com 207.171.182.16
 *  ...
 *       (www.yahoo.com)
 *            www.yahoo.com 216.109.118.65
 *            www.yale.edu 130.132.143.21
           
 *
 ******************************************************************************/

using System;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>BTree</c> class represents an ordered symbol table of generic
  /// key-value pairs.
  /// It supports the <c>Put</c>, <c>Get</c>, <c>Indexer</c> <c>Contains</c>,
  /// <c>Count</c>, and <c>IsEmpty</c> methods.</para><para>
  /// A symbol table implements the <c>Associative array</c> abstraction:
  /// when associating a value with a key that is already in the symbol table,
  /// the convention is to replace the old value with the new value.
  /// Unlike <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>, this class uses the convention that
  /// values cannot be <c>null</c>. Setting the
  /// value associated with a key to <c>null</c> is equivalent to deleting the key
  /// from the symbol table.</para><para>
  /// This implementation uses a B-tree. It requires that
  /// the key type implements the <c>IComparable</c> interface and calls the
  /// <c>CompareTo()</c> and method to compare two keys. It does not call either
  /// <c>Equals()</c> or <c>GetHashCode()</c>. The <c>Get</c>, <c>Put</c>, 
  /// and <c>Contains</c> operations each make log<sub><c>M</c></sub>(<c>N</c>) 
  /// probes in the worst case, where <c>N</c> is the number of key-value pairs
  /// and <c>M</c> is the branching factor. The <c>Count</c>, and 
  /// <c>IsEmpty</c> operations take constant time. Construction takes constant time.
  /// </para></summary>
  /// <remarks><para>For additional documentation, see
  /// <a href="http://algs4.cs.princeton.edu/62btree">Section 6.2</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/BTree.java.html">BTree</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class BTree<Key, Value> where Key : IComparable<Key>
  {
    // max children per B-tree node = M-1
    // (must be even and greater than 2)
    private const int M = 4;

    private Node root;       // root of the B-tree
    private int height;      // height of the B-tree
    private int N;           // number of key-value pairs in the B-tree

    // helper B-tree node data type
    private class Node
    {
      public int m;                             // number of children
      public readonly Entry[] children = new Entry[M];   // the array of children

      // create a node with k children
      public Node(int k)
      {
        m = k;
      }
    }

    // internal nodes: only use key and next
    // external nodes: only use key and value
    private class Entry
    {
      public Key key;
      public object val;
      public Node next;     // helper field to iterate over array entries
      public Entry(Key key, object val, Node next)
      {
        this.key = key;
        this.val = val;
        this.next = next;
      }
    }

    /// <summary>
    /// Initializes an empty B-tree.</summary>
    ///
    public BTree()
    {
      root = new Node(0);
    }

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
      get { return N; }
    }

    /// <summary>
    /// Returns the height of this B-tree (for debugging).</summary>
    /// <returns>the height of this B-tree</returns>
    ///
    public int Height
    {
      get { return height; }
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
    /// <param name="key">the key</param>
    /// <returns>the value associated with the given key if the key is in the symbol table
    /// and <c>null</c> if the key is not in the symbol table</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public object Get(Key key)
    {
      if (key == null) throw new ArgumentNullException("key must not be null");
      return search(root, key, height);
    }

    private object search(Node x, Key key, int ht)
    {
      Entry[] children = x.children;

      // external node
      if (ht == 0)
      {
        for (int j = 0; j < x.m; j++)
        {
          if (eq(key, children[j].key)) return children[j].val;
        }
      }

      // internal node
      else
      {
        for (int j = 0; j < x.m; j++)
        {
          if (j + 1 == x.m || less(key, children[j + 1].key))
            return search(children[j].next, key, ht - 1);
        }
      }
      return null;
    }


    /// <summary>
    /// Inserts the key-value pair into the symbol table, overwriting the old value
    /// with the new value if the key is already in the symbol table.
    /// If the value is <c>null</c>, this effectively deletes the key from the symbol table.</summary>
    /// <param name="key">the key</param>
    /// <param name="val"> val the value</param>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public void Put(Key key, Value val)
    {
      if (key == null) throw new ArgumentNullException("key must not be null");
      Node u = insert(root, key, val, height);
      N++;
      if (u == null) return;

      // need to split root
      Node t = new Node(2);
      t.children[0] = new Entry(root.children[0].key, null, root);
      t.children[1] = new Entry(u.children[0].key, null, u);
      root = t;
      height++;
    }

    private Node insert(Node h, Key key, Value val, int ht)
    {
      int j;
      Entry t = new Entry(key, val, null);

      // external node
      if (ht == 0)
      {
        for (j = 0; j < h.m; j++)
        {
          if (less(key, h.children[j].key)) break;
        }
      }

      // internal node
      else
      {
        for (j = 0; j < h.m; j++)
        {
          if ((j + 1 == h.m) || less(key, h.children[j + 1].key))
          {
            Node u = insert(h.children[j++].next, key, val, ht - 1);
            if (u == null) return null;
            t.key = u.children[0].key;
            t.next = u;
            break;
          }
        }
      }

      for (int i = h.m; i > j; i--)
        h.children[i] = h.children[i - 1];
      h.children[j] = t;
      h.m++;
      if (h.m < M) return null;
      else return split(h);
    }

    // split node in half
    private Node split(Node h)
    {
      Node t = new Node(M / 2);
      h.m = M / 2;
      for (int j = 0; j < M / 2; j++)
        t.children[j] = h.children[M / 2 + j];
      return t;
    }

    /// <summary>
    /// Returns a string representation of this B-tree (for debugging).</summary>
    /// <returns>a string representation of this B-tree.</returns>
    ///
    public override string ToString()
    {
      return toString(root, height, "") + "\n";
    }

    private string toString(Node h, int ht, string indent)
    {
      StringBuilder s = new StringBuilder();
      Entry[] children = h.children;

      if (ht == 0)
      {
        for (int j = 0; j < h.m; j++)
        {
          s.Append(indent + children[j].key + " " + children[j].val + "\n");
        }
      }
      else
      {
        for (int j = 0; j < h.m; j++)
        {
          if (j > 0) s.Append(indent + "(" + children[j].key + ")\n");
          s.Append(toString(children[j].next, ht - 1, indent + "     "));
        }
      }
      return s.ToString();
    }


    private bool less(Key k1, Key k2)
    {
      return k1.CompareTo(k2) < 0;
    }

    private bool eq(Key k1, Key k2)
    {
      return k1.CompareTo(k2) == 0;
    }

    /// <summary>
    /// Demo test the <c>BTree</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd BTree")]
    public static void MainTest(string[] args)
    {
      BTree<string, string> st = new BTree<string, string>();

      st.Put("www.cs.princeton.edu", "128.112.136.12");
      st.Put("www.cs.princeton.edu", "128.112.136.11");
      st.Put("www.princeton.edu", "128.112.128.15");
      st.Put("www.yale.edu", "130.132.143.21");
      st.Put("www.simpsons.com", "209.052.165.60");
      st.Put("www.apple.com", "17.112.152.32");
      st.Put("www.amazon.com", "207.171.182.16");
      st.Put("www.ebay.com", "66.135.192.87");
      st.Put("www.cnn.com", "64.236.16.20");
      st.Put("www.google.com", "216.239.41.99");
      st.Put("www.nytimes.com", "199.239.136.200");
      st.Put("www.microsoft.com", "207.126.99.140");
      st.Put("www.dell.com", "143.166.224.230");
      st.Put("www.slashdot.org", "66.35.250.151");
      st.Put("www.espn.com", "199.181.135.201");
      st.Put("www.weather.com", "63.111.66.11");
      st.Put("www.yahoo.com", "216.109.118.65");


      Console.WriteLine("cs.princeton.edu:  " + st.Get("www.cs.princeton.edu"));
      Console.WriteLine("hardvardsucks.com: " + st.Get("www.harvardsucks.com"));
      Console.WriteLine("simpsons.com:      " + st.Get("www.simpsons.com"));
      Console.WriteLine("apple.com:         " + st.Get("www.apple.com"));
      Console.WriteLine("ebay.com:          " + st.Get("www.ebay.com"));
      Console.WriteLine("dell.com:          " + st.Get("www.dell.com"));
      Console.WriteLine();

      Console.WriteLine("Count:    " + st.Count);
      Console.WriteLine("Height:   " + st.Height);
      Console.WriteLine(st);
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
