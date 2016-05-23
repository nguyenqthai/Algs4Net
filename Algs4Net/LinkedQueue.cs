/******************************************************************************
 *  File name :    LinkedQueue.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/13stacks/tobe.txt
 *
 *  A generic queue, implemented using a singly-linked list.
 *
 *  C:\> algscmd LinkedQueue < tobe.txt
 *  to be or not to be (2 left on queue)
 *
 ******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Algs4Net
{
  /// <summary>
  /// <para>The <c>LinkedQueue</c> class represents a first-in-first-out (FIFO)
  /// queue of generic items. So named to avoid conflict with the .NET framwork 
  /// <see cref="Queue{T}"/> class. Since C# does not allow an inner static class with
  /// instances, the implementation is effectively the same as the 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Queue.java.html">Queue</a>
  /// class implementation.</para><para>
  /// It supports the usual <c>Enqueue</c> and <c>Dequeue</c>
  /// operations, along with methods for peeking at the first item,
  /// testing if the queue is empty, and iterating through
  /// the items in FIFO order.
  /// </para><para>
  /// This implementation uses a singly-linked list with a nested, non-static
  /// class Node and hence is the same as the <c>Queue</c> class in algs4.jar.
  /// The <c>Enqueue</c>, <c>Dequeue</c>, <c>Peek</c>, <c>Count</c>, and <c>IsEmpty</c>
  /// operations all take constant time in the worst case.
  /// </para></summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/13stacks">Section 1.3</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/LinkedQueue.java.html">LinkedQueue</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks>
  ///
  public class LinkedQueue<Item> : IEnumerable<Item>
  {
    private int N;         // number of elements on queue
    private Node first;    // beginning of queue
    private Node last;     // end of queue

    // helper linked list class
    private class Node
    {
      public Item item;
      public Node next;
    }

    /// <summary>
    /// Initializes an empty queue.</summary>
    ///
    public LinkedQueue()
    {
      first = null;
      last = null;
      N = 0;
    }

    /// <summary>Is this queue empty?</summary>
    /// <returns>true if this queue is empty; false otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return first == null; }
    }

    /// <summary>Returns the number of items in this queue.</summary>
    /// <returns>the number of items in this queue</returns>
    ///
    public int Count
    {
      get { return N; }
    }

    /// <summary>Returns the item least recently added to this queue.</summary>
    /// <returns>the item least recently added to this queue</returns>
    /// <exception cref="InvalidOperationException">if this queue is empty</exception>
    ///
    public Item Peek()
    {
      if (IsEmpty) throw new InvalidOperationException("LinkedQueue underflow");
      return first.item;
    }

    /// <summary>Adds the item to this queue.</summary>
    /// <param name="item">item the item to add</param>
    ///
    public void Enqueue(Item item)
    {
      Node oldlast = last;
      last = new Node();
      last.item = item;
      last.next = null;
      if (IsEmpty) first = last;
      else oldlast.next = last;
      N++;
      Debug.Assert(check());
    }

    /// <summary>
    /// Removes and returns the item on this queue that was least recently added.</summary>
    /// <returns>the item on this queue that was least recently added</returns>
    /// <exception cref="InvalidOperationException">if this queue is empty</exception>
    ///
    public Item Dequeue()
    {
      if (IsEmpty) throw new InvalidOperationException("LinkedQueue underflow");
      Item item = first.item;
      first = first.next;
      N--;
      if (IsEmpty) last = null;   // to avoid loitering
      Debug.Assert(check());
      return item;
    }

    /// <summary>
    /// Returns a string representation of this queue.</summary>
    /// <returns>the sequence of items in FIFO order, separated by spaces</returns>
    ///
    public  override string ToString()
    {
      StringBuilder s = new StringBuilder();
      foreach (Item item in this)
        s.Append(item + " ");
      return s.ToString();
    }

    // check internal invariants
    private bool check()
    {
      if (N < 0)
      {
        return false;
      }
      else if (N == 0)
      {
        if (first != null) return false;
        if (last != null) return false;
      }
      else if (N == 1)
      {
        if (first == null || last == null) return false;
        if (first != last) return false;
        if (first.next != null) return false;
      }
      else {
        if (first == null || last == null) return false;
        if (first == last) return false;
        if (first.next == null) return false;
        if (last.next != null) return false;

        // check internal consistency of instance variable N
        int numberOfNodes = 0;
        for (Node x = first; x != null && numberOfNodes <= N; x = x.next)
        {
          numberOfNodes++;
        }
        if (numberOfNodes != N) return false;

        // check internal consistency of instance variable last
        Node lastNode = first;
        while (lastNode.next != null)
        {
          lastNode = lastNode.next;
        }
        if (last != lastNode) return false;
      }

      return true;
    }


    /// <summary>
    /// Returns an iterator that iterates over the items in this queue in FIFO order.</summary>
    /// <returns>an iterator that iterates over the items in this queue in FIFO order</returns>
    ///
    public IEnumerator<Item> GetEnumerator()
    {
      return new ListIEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }


    // an iterator, doesn't implement remove() since it's optional
    private class ListIEnumerator : IEnumerator<Item>
    {
      private Node current = null;
      private LinkedQueue<Item> queue = null;
      private bool firstCall = true;

      public ListIEnumerator(LinkedQueue<Item> collection)
      {
        queue = collection;
      }

      public Item Current
      {
        get
        {
          if (current == null)
            throw new InvalidOperationException("Past end of collection!");
          return current.item;
        }
      }

      object IEnumerator.Current
      {
        get { return Current as object; }
      }

      public bool MoveNext()
      {
        if (firstCall)
        {
          current = queue.first;
          firstCall = false;
          return current != null;
        }
        if (current != null)
        {
          current = current.next;
          return current != null;
        }
        return false;
      }

      public void Reset()
      {
        current = null;
        firstCall = true;
      }

      public void Dispose() { }
    }

    /// <summary>
    /// Demo test the <c>LinkedQueue</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    ///
    [HelpText("algscmd LinkedQueue < tobe.txt", "Items separated by space or new line")]
    public static void MainTest(string[] args)
    {
      LinkedQueue<string> q = new LinkedQueue<string>();
      TextInput StdIn = new TextInput();
      while (!StdIn.IsEmpty)
      {
        string item = StdIn.ReadString();
        if (!item.Equals("-")) q.Enqueue(item);
        else if (!q.IsEmpty) Console.Write(q.Dequeue() + " ");
      }
      Console.WriteLine("(" + q.Count + " left on queue)");
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
