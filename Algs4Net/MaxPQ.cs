/******************************************************************************
 *  File name :    MaxPQ.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Generic max priority queue implementation with a binary heap.
 *  Can be used with a comparator instead of the natural order,
 *  but the generic Key type must still be Comparable.
 *
 *  C:\> algscmd MaxPQ < tinyPQ.txt
 *  Q X P (6 left on pq)
 *
 *  We use a one-based array to simplify parent and child calculations.
 *
 *  Can be optimized by replacing full exchanges with half exchanges
 *  (ala insertion sort).
 *
 ******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>MaxPQ</c> class represents a priority queue of generic keys.
  /// It supports the usual <c>Insert</c> and <c>Delete-the-maximum</c>
  /// operations, along with methods for peeking at the maximum key,
  /// testing if the priority queue is empty, and iterating through
  /// the keys.</para>
  /// <para>This implementation uses a binary heap.
  /// The <c>Insert</c> and <c>Delete-the-maximum</c> operations take
  /// logarithmic amortized time.
  /// The <c>Max</c>, <c>Count</c>, and <c>IsEmpty</c> operations take constant time.
  /// Construction takes time proportional to the specified capacity or the number of
  /// items used to initialize the data structure.</para></summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/24pq">Section 2.4</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/MaxPQ.java.html">MaxPQ</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks>
  ///
  public class MaxPQ<Key> : IEnumerable<Key> where Key : IComparable<Key>
  {
    private Key[] pq;                    // store items at indices 1 to N
    private int N;                       // number of items on priority queue
    private Comparer<Key> comparator;  // optional comparator

    /// <summary>
    /// Initializes an empty priority queue with the given initial capacity.</summary>
    /// <param name="initCapacity">initCapacity the initial capacity of this priority queue</param>
    ///
    public MaxPQ(int initCapacity)
    {
      pq = new Key[initCapacity + 1];
      N = 0;
    }

    /// <summary>
    /// Initializes an empty priority queue.</summary>
    ///
    public MaxPQ() : this(1)
    {
    }

    /// <summary>
    /// Initializes an empty priority queue with the given initial capacity,
    /// using the given comparator.</summary>
    /// <param name="initCapacity">initCapacity the initial capacity of this priority queue</param>
    /// <param name="comparator">comparator the order to use when comparing keys</param>
    ///
    public MaxPQ(int initCapacity, Comparer<Key> comparator)
    {
      this.comparator = comparator;
      pq = new Key[initCapacity + 1];
      N = 0;
    }

    /// <summary>
    /// Initializes an empty priority queue using the given comparator.</summary>
    /// <param name="comparator">comparator the order to use when comparing keys</param>
    ///
    public MaxPQ(Comparer<Key> comparator) : this(1, comparator)
    {
    }

    /// <summary>
    /// Initializes a priority queue from the array of keys.
    /// Takes time proportional to the number of keys, using sink-based heap construction.</summary>
    /// <param name="keys">keys the array of keys</param>
    ///
    public MaxPQ(Key[] keys)
    {
      N = keys.Length;
      pq = new Key[keys.Length + 1];
      for (int i = 0; i < N; i++)
        pq[i + 1] = keys[i];
      for (int k = N / 2; k >= 1; k--)
        sink(k);
      Debug.Assert(isMaxHeap());
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
    /// Returns the number of keys on this priority queue.</summary>
    /// <returns>the number of keys on this priority queue</returns>
    ///
    public int Count
    {
      get { return N; }
    }

    /// <summary>
    /// Returns a smallest key on this priority queue.</summary>
    /// <returns>a smallest key on this priority queue</returns>
    /// <exception cref="InvalidOperationException">if this priority queue is empty</exception>
    ///
    public Key Max
    {
      get
      {
        if (IsEmpty) throw new InvalidOperationException("Priority queue underflow");
        return pq[1];
      }
    }

    // helper function to double the size of the heap array
    private void resize(int capacity)
    {
      Debug.Assert(capacity > N);
      Key[] temp = new Key[capacity];
      for (int i = 1; i <= N; i++)
      {
        temp[i] = pq[i];
      }
      pq = temp;
    }

    /// <summary>
    /// Adds a new key to this priority queue.</summary>
    /// <param name="x">x the key to add to this priority queue</param>
    ///
    public void Insert(Key x)
    {
      // double size of array if necessary
      if (N == pq.Length - 1) resize(2 * pq.Length);

      // add x, and percolate it up to maintain heap invariant
      pq[++N] = x;
      swim(N);
      Debug.Assert(isMaxHeap());
    }

    /// <summary>
    /// Removes and returns a smallest key on this priority queue.</summary>
    /// <returns>a smallest key on this priority queue</returns>
    /// <exception cref="InvalidOperationException">if this priority queue is empty</exception>
    ///
    public Key DelMax()
    {
      if (IsEmpty) throw new InvalidOperationException("Priority queue underflow");
      OrderHelper.Exch(pq, 1, N);
      Key max = pq[N--];
      sink(1);
      pq[N + 1] = default(Key); // avoid loitering and help with garbage collection
      if ((N > 0) && (N == (pq.Length - 1) / 4)) resize(pq.Length / 2);
      Debug.Assert(isMaxHeap());
      return max;
    }

    /***************************************************************************
     * Helper functions to restore the heap invariant.
     ***************************************************************************/

    private void swim(int k)
    {
      while (k > 1 && OrderHelper.Lesser<Key>(pq[k / 2], pq[k]))
      {
        OrderHelper.Exch(pq, k, k / 2);
        k = k / 2;
      }
    }

    private void sink(int k)
    {
      while (2 * k <= N)
      {
        int j = 2 * k;
        if (j < N && OrderHelper.Lesser<Key>(pq[j], pq[j + 1])) j++;
        if (!OrderHelper.Lesser<Key>(pq[k], pq[j])) break;
        OrderHelper.Exch(pq, k, j);
        k = j;
      }
    }

    /***************************************************************************
     * Helper functions for heap properties
     ***************************************************************************/

    // is pq[1..N] a min heap?
    private bool isMaxHeap()
    {
      return isMaxHeap(1);
    }

    // is subtree of pq[1..N] rooted at k a min heap?
    private bool isMaxHeap(int k)
    {
      if (k > N) return true;
      int left = 2 * k, right = 2 * k + 1;
      if (left <= N && OrderHelper.Lesser<Key>(pq[k], pq[left])) return false;
      if (right <= N && OrderHelper.Lesser<Key>(pq[k], pq[right])) return false;
      return isMaxHeap(left) && isMaxHeap(right);
    }
    /// <summary>
    /// Formatted string for the MaxPQ class
    /// </summary>
    /// <returns>returns a string in the form [ a1, a2, ... an ]</returns>
    public override string ToString()
    {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      sb.Append("Count: " + N + " [ ");
      for (int i = 1; i < pq.Length; i++)
        sb.Append(pq[i] + " ");
      sb.Append("]");
      return sb.ToString();

    }
    /// <summary>
    /// Returns an iterator that iterates over the keys on this priority queue
    /// in ascending order.</summary>
    /// <returns>an iterator that iterates over the keys in ascending order</returns>
    public IEnumerator<Key> GetEnumerator()
    {
      return new HeapIEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    private class HeapIEnumerator : IEnumerator<Key>
    {
      // create a new pq
      private MaxPQ<Key> copy;
      private MaxPQ<Key> innerPQ;

      // add all items to copy of heap
      // takes linear time since already in heap order so no keys move
      public HeapIEnumerator(MaxPQ<Key> pq)
      {
        if (pq.comparator == null) innerPQ = new MaxPQ<Key>(pq.Count);
        else innerPQ = new MaxPQ<Key>(pq.Count, pq.comparator);
        for (int i = 1; i <= pq.N; i++)
          innerPQ.Insert(pq.pq[i]);
        copy = innerPQ;
      }

      private void Init()
      {
      }

      public Key Current
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
    /// Demo test the <c>MaxPQ</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd MaxPQ < tinyPQ.txt", "Input of comparable items")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      MaxPQ<string> pq = new MaxPQ<string>();
      while (!StdIn.IsEmpty)
      {
        string item = StdIn.ReadString();
        if (!item.Equals("-")) pq.Insert(item);
        else if (!pq.IsEmpty) Console.Write(pq.DelMax() + " ");
      }
      Console.WriteLine("(" + pq.Count + " left on pq)");
    }

    // additional test
    static void BottomInts()
    {
      int[] allInts = { 12, 11, 8, 7, 9, 5, 4, 3, 2, 29, 23, 1, 24, 30, 9, 4, 88, 5, 100, 29, 23, 5, 99, 87, 22, 111 };
      MaxPQ<int> pq0 = new MaxPQ<int>(allInts);
      int M = allInts.Length / 3;
      MaxPQ<int> pq = new MaxPQ<int>(M + 1);
      Console.WriteLine("Top {0} is ", M);
      foreach (var n in allInts)
      {
        pq.Insert(n);
        Console.WriteLine("Max is {0}", pq.Max);
        // remove maximum if M+1 entries on the PQ
        if (pq.Count > M)
          pq.DelMax();
      }
      // print entries on PQ in reverse order
      LinkedStack<int> stack = new LinkedStack<int>();
      foreach (int n in pq)
        stack.Push(n);
      foreach (int n in stack)
        Console.WriteLine(n);
      Console.WriteLine("These are the bottom elements");
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
