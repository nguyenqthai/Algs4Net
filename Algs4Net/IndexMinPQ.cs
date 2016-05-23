/******************************************************************************
 *  File name :    IndexMinPQ.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Minimum-oriented indexed PQ implementation using a binary heap.
 *
 *  C:\> algscmd IndexMinPQ
 *
 ******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>IndexMinPQ</c> class represents an indexed priority queue of generic keys.
  /// It supports the usual <c>Insert</c> and <c>Delete-the-minimum</c>
  /// operations, along with <c>Delete</c> and <c>Change-the-key</c>
  /// methods. In order to let the client refer to keys on the priority queue,
  /// an integer between 0 and maxN-1 is associated with each key-the client
  /// uses this integer to specify which key to delete or change.
  /// It also supports methods for peeking at the minimum key,
  /// testing if the priority queue is empty, and iterating through
  /// the keys.</para>
  /// <para>
  /// This implementation uses a binary heap along with an array to associate
  /// keys with integers in the given range.
  /// The <c>Insert</c>, <c>Delete-the-minimum</c>, <c>Delete</c>,
  /// <c>Change-key</c>, <c>Decrease-key</c>, and <c>Increase-key</c>
  /// operations take logarithmic time.
  /// The <c>Is-empty</c>, <c>Count</c>, <c>Min-index</c>, <c>Min-key</c>, and <c>Key-of</c>
  /// operations take constant time.
  /// Construction takes time proportional to the specified capacity.
  /// </para></summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/24pq">Section 2.4</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/IndexMinPQ.java.html">IndexMinPQ</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.
  /// </para></remarks>
  ///
  public class IndexMinPQ<Key> : IEnumerable<int> where Key : IComparable<Key> 
  {
    private int maxN;        // maximum number of elements on PQ
    private int N;           // number of elements on PQ
    private int[] pq;        // binary heap using 1-based indexing
    private int[] qp;        // inverse of pq - qp[pq[i]] = pq[qp[i]] = i
    private Key[] keys;      // keys[i] = priority of i

    /// <summary>Initializes an empty indexed priority queue with indices between <c>0</c>
    /// and <c>maxN - 1</c>.</summary>
    /// <param name="maxN">the keys on this priority queue are index from <c>0</c>
    ///        <c>maxN - 1</c></param>
    /// <exception cref="ArgumentException">if <c>maxN</c> &lt; <c>0</c></exception>
    ///
    public IndexMinPQ(int maxN)
    {
      if (maxN < 0) throw new ArgumentException("Negative collection size");
      this.maxN = maxN;
      keys = new Key[maxN + 1];         // make this of length maxN??
      pq = new int[maxN + 1];
      qp = new int[maxN + 1];           // make this of length maxN??
      for (int i = 0; i <= maxN; i++)
        qp[i] = -1;
    }

    /// <summary>
    /// Returns true if this priority queue is empty.</summary>
    /// <returns><c>true</c> if this priority queue is empty;
    ///        <c>false</c> otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return N == 0; }
    }

    /// <summary>
    /// Is <c>i</c> an index on this priority queue?</summary>
    /// <param name="i"> i an index</param>
    /// <returns><c>true</c> if <c>i</c> is an index on this priority queue;
    ///        <c>false</c> otherwise</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    ///
    public bool Contains(int i)
    {
      if (i < 0 || i >= maxN) throw new IndexOutOfRangeException();
      return qp[i] != -1;
    }

    /// <summary>
    /// Returns the number of keys on this priority queue.</summary>
    /// <returns>the number of keys on this priority queue</returns>
    ///
    public int Count
    {
      get { return N; }
    }

    /// <summary>Associates key with index <c>i</c>.</summary>
    ///
    /// <param name="i">i an index</param>
    /// <param name="key">key the key to associate with index <c>i</c></param>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    /// <exception cref="ArgumentException">if there already is an item associated
    ///        with index <c>i</c></exception>
    ///
    public void Insert(int i, Key key)
    {
      if (i < 0 || i >= maxN) throw new IndexOutOfRangeException();
      if (Contains(i)) throw new ArgumentException("index is already in the priority queue");
      N++;
      qp[i] = N;
      pq[N] = i;
      keys[i] = key;
      swim(N);
    }

    /// <summary>
    /// Returns an index associated with a minimum key.</summary>
    /// <returns>an index associated with a minimum key</returns>
    /// <exception cref="InvalidOperationException">if this priority queue is empty</exception>
    ///
    public int MinIndex
    {
      get
      {
        if (IsEmpty) throw new InvalidOperationException("Priority queue underflow");
        return pq[1];
      }
    }

    /// <summary>Returns the current minimum key.</summary>
    /// <returns>a minimum key</returns>
    /// <exception cref="InvalidOperationException">if this priority queue is empty</exception>
    ///
    public Key MinKey
    {
      get
      {
        if (IsEmpty) throw new InvalidOperationException("Priority queue underflow");
        return keys[pq[1]];
      }
    }

    /// <summary>
    /// Removes a minimum key and returns its associated index.</summary>
    /// <returns>an index associated with a minimum key</returns>
    /// <exception cref="InvalidOperationException">if this priority queue is empty</exception>
    ///
    public int DelMin()
    {
      if (IsEmpty) throw new InvalidOperationException("Priority queue underflow");
      int min = pq[1];
      exch(1, N--);
      sink(1);
      Debug.Assert(min == pq[N + 1]);
      qp[min] = -1;        // delete
      keys[min] = default(Key);    // to help with garbage collection
      pq[N + 1] = -1;              // not needed
      return min;
    }

    /// <summary>
    /// Returns the key associated with index <c>i</c>.</summary>
    /// <param name="i">i the index of the key to return</param>
    /// <returns>the key associated with index <c>i</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    /// <exception cref="InvalidOperationException">no key is associated with index <c>i</c></exception>
    ///
    public Key KeyOf(int i)
    {
      if (i < 0 || i >= maxN) throw new IndexOutOfRangeException();
      if (!Contains(i)) throw new InvalidOperationException("index is not in the priority queue");
      else return keys[i];
    }

    /// <summary>
    /// Change the key associated with index <c>i</c> to the specified value.</summary>
    /// <param name="i">i the index of the key to change</param>
    /// <param name="key">key change the key associated with index <c>i</c> to this key</param>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    /// <exception cref="InvalidOperationException">no key is associated with index <c>i</c></exception>
    ///
    public void ChangeKey(int i, Key key)
    {
      if (i < 0 || i >= maxN) throw new IndexOutOfRangeException();
      if (!Contains(i)) throw new InvalidOperationException("index is not in the priority queue");
      keys[i] = key;
      swim(qp[i]);
      sink(qp[i]);
    }

    /// <summary>
    /// Change the key associated with index <c>i</c> to the specified value.</summary>
    /// <param name="i">i the index of the key to change</param>
    /// <param name="key">key change the key associated with index <c>i</c> to this key</param>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    /// <remarks>Use <see cref="ChangeKey(int, Key)"/>.</remarks>
    ///
    public void Change(int i, Key key)
    {
      ChangeKey(i, key);
    }

    /// <summary>
    /// Decrease the key associated with index <c>i</c> to the specified value.</summary>
    /// <param name="i">i the index of the key to decrease</param>
    /// <param name="key">key decrease the key associated with index <c>i</c> to this key</param>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    /// <exception cref="ArgumentException">if key &gt;= key associated with index <c>i</c></exception>
    /// <exception cref="InvalidOperationException">no key is associated with index <c>i</c></exception>
    ///
    public void DecreaseKey(int i, Key key)
    {
      if (i < 0 || i >= maxN) throw new IndexOutOfRangeException();
      if (!Contains(i)) throw new InvalidOperationException("index is not in the priority queue");
      if (keys[i].CompareTo(key) <= 0)
        throw new ArgumentException("Calling decreaseKey() with given argument would not strictly decrease the key");
      keys[i] = key;
      swim(qp[i]);
    }

    /// <summary>
    /// Increase the key associated with index <c>i</c> to the specified value.</summary>
    /// <param name="i">i the index of the key to increase</param>
    /// <param name="key">key increase the key associated with index <c>i</c> to this key</param>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    /// <exception cref="ArgumentException">if key &lt;= key associated with index <c>i</c></exception>
    /// <exception cref="ArgumentException">no key is associated with index <c>i</c></exception>
    ///
    public void IncreaseKey(int i, Key key)
    {
      if (i < 0 || i >= maxN) throw new IndexOutOfRangeException();
      if (!Contains(i)) throw new ArgumentException("index is not in the priority queue");
      if (keys[i].CompareTo(key) >= 0)
        throw new ArgumentException("Calling increaseKey() with given argument would not strictly increase the key");
      keys[i] = key;
      sink(qp[i]);
    }


    /// <summary>
    /// Remove the key associated with index <c>i</c>.</summary>
    /// <param name="i">i the index of the key to remove</param>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    /// <exception cref="ArgumentException">no key is associated with index <c>i</c></exception>
    ///
    public void Delete(int i)
    {
      if (i < 0 || i >= maxN) throw new IndexOutOfRangeException();
      if (!Contains(i)) throw new ArgumentException("index is not in the priority queue");
      int index = qp[i];
      exch(index, N--);
      swim(index);
      sink(index);
      keys[i] = default(Key);
      qp[i] = -1;
    }

    /***************************************************************************
     * General helper functions.
     ***************************************************************************/
    private bool greater(int i, int j)
    {
      return keys[pq[i]].CompareTo(keys[pq[j]]) > 0;
    }

    private void exch(int i, int j)
    {
      int swap = pq[i];
      pq[i] = pq[j];
      pq[j] = swap;
      qp[pq[i]] = i;
      qp[pq[j]] = j;
    }

    /***************************************************************************
     * Heap helper functions.
     ***************************************************************************/
    private void swim(int k)
    {
      while (k > 1 && greater(k / 2, k))
      {
        exch(k, k / 2);
        k = k / 2;
      }
    }

    private void sink(int k)
    {
      while (2 * k <= N)
      {
        int j = 2 * k;
        if (j < N && greater(j, j + 1)) j++;
        if (!greater(k, j)) break;
        exch(k, j);
        k = j;
      }
    }

    /// <summary>
    /// Formatted string for the IndexMinPQ class
    /// </summary>
    /// <returns>returns a string in the form [ a1, a2, ... an ]</returns>
    public override string ToString()
    {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      sb.Append("Count: " + N + " [ ");
      for (int i = 1; i < pq.Length; i++)
        sb.Append(string.Format("({0}, {1}) ", pq[i], keys[pq[i]]));
      sb.Append("]");
      return sb.ToString();

    }
    /// <summary>
    /// Returns an iterator that iterates over the keys on this priority queue
    /// in ascending order.</summary>
    /// <returns>an iterator that iterates over the keys in ascending order</returns>
    public IEnumerator<int> GetEnumerator()
    {
      return new HeapIEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    private class HeapIEnumerator : IEnumerator<int>
    {
      // create a new pq
      private IndexMinPQ<Key> copy;
      private IndexMinPQ<Key> innerPQ;

      // add all items to copy of heap
      // takes linear time since already in heap order so no keys move
      public HeapIEnumerator(IndexMinPQ<Key> minpq)
      {
        innerPQ = new IndexMinPQ<Key>(minpq.Count);
        for (int i = 1; i <= minpq.N; i++)
          innerPQ.Insert(minpq.pq[i], minpq.keys[minpq.pq[i]]);
        copy = innerPQ;
      }

      private void Init()
      {
      }

      public int Current
      {
        get
        {
          if (innerPQ.IsEmpty) throw new InvalidOperationException("Priority queue underflow");
          return innerPQ.DelMin();
        }
      }

      object IEnumerator.Current
      {
        get
        {
          return Current as object;
        }
      }

      public bool MoveNext()
      {
        if (innerPQ.IsEmpty) return false;
        else return true;
      }

      public void Reset()
      {
        innerPQ = copy;
      }

      public void Dispose() { }
    }

    /// <summary>
    /// Demo test the <c>IndexMinPQ</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd IndexMinPQ")]
    public static void MainTest(string[] args)
    {
      // insert a bunch of strings
      string[] strings = { "it", "was", "the", "best", "of", "times", "it", "was", "the", "worst" };

      IndexMinPQ<string> pq = new IndexMinPQ<string>(strings.Length);
      for (int i = 0; i < strings.Length; i++)
      {
        pq.Insert(i, strings[i]);
      }

      // delete and print each key
      while (!pq.IsEmpty)
      {
        int i = pq.DelMin();
        Console.WriteLine(i + " " + strings[i]);
      }
      Console.WriteLine();

      // reinsert the same strings
      for (int i = 0; i < strings.Length; i++)
      {
        pq.Insert(i, strings[i]);
      }

      // print each key using the iterator
      foreach (int i in pq)
      {
        Console.WriteLine(i + " " + strings[i]);
      }
      while (!pq.IsEmpty)
      {
        Console.WriteLine("Min k={0} at {1}", pq.MinKey, pq.MinIndex);
        Console.WriteLine("Removed {0}", pq.DelMin());
      }
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
