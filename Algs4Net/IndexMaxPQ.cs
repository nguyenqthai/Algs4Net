/******************************************************************************
 *  File name :    IndexMaxPQ.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Maximum-oriented indexed PQ implementation using a binary heap.
 *  
 *  C:\> algscmd IndexMaxPQ
 *
 ******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>IndexMaxPQ</c> class represents an indexed priority queue of generic keys.
  /// It supports the usual <c>Insert</c> and <c>Delete-the-maximum</c>
  /// operations, along with <c>Delete</c> and <c>Change-the-key</c> 
  /// methods. In order to let the client refer to items on the priority queue,
  /// an integer between 0 and maxN-1 is associated with each key. The client
  /// uses this integer to specify which key to delete or change.
  /// It also supports methods for peeking at a maximum key,
  /// testing if the priority queue is empty, and iterating through
  /// the keys.</para><para>
  /// This implementation uses a binary heap along with an array to associate
  /// keys with integers in the given range.
  /// The <c>Insert</c>, <c>Delete-the-maximum</c>, <c>Delete</c>,
  /// <c>Change-key</c>, <c>Decrease-key</c>, and <c>Increase-key</c>
  /// operations take logarithmic time.
  /// The <c>IsEmpty</c>, <c>Count</c>, <c>Max-index</c>, <c>Max-key</c>, and <c>Key-of</c>
  /// operations take constant time.
  /// Construction takes time proportional to the specified capacity.
  /// </para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/24pq">Section 2.4</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/IndexMaxPQ.java.html">IndexMaxPQ</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class IndexMaxPQ<Key> : IEnumerable<int> where Key : IComparable<Key>
  {
    private int N;           // number of elements on PQ
    private int[] pq;        // binary heap using 1-based indexing
    private int[] qp;        // inverse of pq - qp[pq[i]] = pq[qp[i]] = i
    private Key[] keys;      // keys[i] = priority of i

    /// <summary>
    /// Initializes an empty indexed priority queue with indices between <c>0</c>
    /// and <c>maxN - 1</c>.</summary>
    /// <param name="maxN">the keys on this priority queue are index from <c>0</c> to <c>maxN - 1</c></param>
    /// <exception cref="ArgumentException">if maxN &lt; 0</exception>
    ///
    public IndexMaxPQ(int maxN)
    {
      keys = new Key[maxN + 1];    // make this of length maxN??
      pq = new int[maxN + 1];
      qp = new int[maxN + 1];                   // make this of length maxN??
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
    /// <param name="i">an index</param>
    /// <returns><c>true</c> if <c>i</c> is an index on this priority queue;</returns>
    ///        <c>false</c> otherwise
    /// <exception cref="IndexOutOfRangeException">unless (0 &lt;= i &lt; maxN)</exception>
    ///
    public bool Contains(int i)
    {
      return qp[i] != -1;
    }

    /// <summary>
    /// Returns the number of keys on this priority queue.</summary>
    /// <returns>the number of keys on this priority queue </returns>
    ///
    public int Count
    {
      get { return N; }
    }

    /// <summary>
    /// Associate key with index i.</summary>
    /// <param name="i">an index</param>
    /// <param name="key">the key to associate with index <c>i</c></param>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    /// <exception cref="ArgumentException">if there already is an item
    ///        associated with index <c>i</c></exception>
    ///
    public void Insert(int i, Key key)
    {
      if (Contains(i)) throw new ArgumentException("index is already in the priority queue");
      N++;
      qp[i] = N;
      pq[N] = i;
      keys[i] = key;
      swim(N);
    }

    /// <summary>
    /// Returns an index associated with a maximum key.</summary>
    /// <returns>an index associated with a maximum key</returns>
    /// <exception cref="InvalidOperationException">if this priority queue is empty</exception>
    ///
    public int MaxIndex
    {
      get
      {
        if (N == 0) throw new InvalidOperationException("Priority queue underflow");
        return pq[1];
      }
    }

    /// <summary>
    /// Returns a maximum key.</summary>
    /// <returns>a maximum key</returns>
    /// <exception cref="InvalidOperationException">if this priority queue is empty</exception>
    ///
    public Key MaxKey
    {
      get
      {
        if (N == 0) throw new InvalidOperationException("Priority queue underflow");
        return keys[pq[1]];
      }
    }
    /// <summary>
    /// Removes a maximum key and returns its associated index.</summary>
    /// <returns>an index associated with a maximum key</returns>
    /// <exception cref="InvalidOperationException">if this priority queue is empty</exception>
    ///
    public int DelMax()
    {
      if (N == 0) throw new InvalidOperationException("Priority queue underflow");
      int max = pq[1];
      exch(1, N--);
      sink(1);

      Debug.Assert(pq[N + 1] == max);
      qp[max] = -1;        // delete
      keys[max] = default(Key);    // to help with garbage collection
      pq[N + 1] = -1;        // not needed
      return max;
    }

    /// <summary>
    /// Returns the key associated with index <c>i</c>.</summary>
    /// <param name="i">the index of the key to return</param>
    /// <returns>the key associated with index <c>i</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    /// <exception cref="InvalidOperationException">no key is associated with index <c>i</c></exception>
    ///
    public Key KeyOf(int i)
    {
      if (!Contains(i)) throw new InvalidOperationException("index is not in the priority queue");
      else return keys[i];
    }

    /// <summary>
    /// Change the key associated with index <c>i</c> to the specified value.</summary>
    /// <param name="i">the index of the key to change</param>
    /// <param name="key">change the key associated with index <c>i</c> to this key</param>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= i &lt; maxN</exception>
    ///
    public void ChangeKey(int i, Key key)
    {
      if (!Contains(i)) throw new InvalidOperationException("index is not in the priority queue");
      keys[i] = key;
      swim(qp[i]);
      sink(qp[i]);
    }

    /// <summary>
    /// Increase the key associated with index <c>i</c> to the specified value.</summary>
    /// <param name="i">the index of the key to increase</param>
    /// <param name="key">increase the key associated with index <c>i</c> to this key</param>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    /// <exception cref="ArgumentException">if key &lt;= key associated with index <c>i</c></exception>
    /// <exception cref="InvalidOperationException">no key is associated with index <c>i</c></exception>
    ///
    public void IncreaseKey(int i, Key key)
    {
      if (!Contains(i)) throw new InvalidOperationException("index is not in the priority queue");
      if (keys[i].CompareTo(key) >= 0)
        throw new ArgumentException("Calling increaseKey() with given argument would not strictly increase the key");

      keys[i] = key;
      swim(qp[i]);
    }

    /// <summary>
    /// Decrease the key associated with index <c>i</c> to the specified value.</summary>
    /// <param name="i">the index of the key to decrease</param>
    /// <param name="key"> key decrease the key associated with index <c>i</c> to this key</param>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    /// <exception cref="ArgumentException">if key &gt;=e; key associated with index <c>i</c></exception>
    /// <exception cref="InvalidOperationException">no key is associated with index <c>i</c></exception>
    ///
    public void DecreaseKey(int i, Key key)
    {
      if (!Contains(i)) throw new InvalidOperationException("index is not in the priority queue");
      if (keys[i].CompareTo(key) <= 0)
        throw new ArgumentException("Calling decreaseKey() with given argument would not strictly decrease the key");

      keys[i] = key;
      sink(qp[i]);
    }

    /// <summary>
    /// Remove the key on the priority queue associated with index <c>i</c>.</summary>
    /// <param name="i">the index of the key to remove</param>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= <c>i</c> &lt; <c>maxN</c></exception>
    /// <exception cref="InvalidOperationException">no key is associated with index <c>i</c></exception>
    ///
    public void Delete(int i)
    {
      if (!Contains(i)) throw new InvalidOperationException("index is not in the priority queue");
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
    private bool less(int i, int j)
    {
      return keys[pq[i]].CompareTo(keys[pq[j]]) < 0;
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
      while (k > 1 && less(k / 2, k))
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
        if (j < N && less(j, j + 1)) j++;
        if (!less(k, j)) break;
        exch(k, j);
        k = j;
      }
    }

    /// <summary>
    /// Returns an iterator that iterates over the keys on the
    /// priority queue in descending order.</summary>
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
      private IndexMaxPQ<Key> copy;
      private IndexMaxPQ<Key> innerPQ;

      // add all elements to copy of heap
      // takes linear time since already in heap order so no keys move
      public HeapIEnumerator(IndexMaxPQ<Key> maxpq)
      {
        innerPQ = new IndexMaxPQ<Key>(maxpq.Count);
        for (int i = 1; i <= maxpq.N; i++)
          innerPQ.Insert(maxpq.pq[i], maxpq.keys[maxpq.pq[i]]);
        copy = innerPQ;
      }

      public int Current
      {
        get
        {
          if (innerPQ.IsEmpty) throw new InvalidOperationException("Priority queue underflow");
          return innerPQ.DelMax();
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
    /// Demo test the <c>IndexMaxPQ</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd IndexMaxPQ")]
    public static void MainTest(string[] args)
    {
      // insert a bunch of strings
      string[] strings = { "it", "was", "the", "best", "of", "times", "it", "was", "the", "worst" };

      IndexMaxPQ<string> pq = new IndexMaxPQ<string>(strings.Length);
      for (int i = 0; i < strings.Length; i++)
      {
        pq.Insert(i, strings[i]);
      }

      // print each key using the iterator
      foreach (int i in pq)
      {
        Console.WriteLine(i + " " + strings[i]);
      }

      Console.WriteLine();

      // increase or decrease the key
      for (int i = 0; i < strings.Length; i++)
      {
        if (StdRandom.Uniform() < 0.5)
          pq.IncreaseKey(i, strings[i] + strings[i]);
        else
          pq.DecreaseKey(i, strings[i].Substring(0, 1));
      }

      // delete and print each key
      while (!pq.IsEmpty)
      {
        string key = pq.MaxKey;
        int i = pq.DelMax();
        Console.WriteLine(i + " " + key);
      }
      Console.WriteLine();

      // reinsert the same strings
      for (int i = 0; i < strings.Length; i++)
      {
        pq.Insert(i, strings[i]);
      }
      Console.WriteLine("Deleting in randome order");
      // delete them in random order
      int[] perm = new int[strings.Length];
      for (int i = 0; i < strings.Length; i++)
        perm[i] = i;
      StdRandom.Shuffle(perm);
      for (int i = 0; i < perm.Length; i++)
      {
        string key = pq.KeyOf(perm[i]);
        pq.Delete(perm[i]);
        Console.WriteLine(perm[i] + " " + key);
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