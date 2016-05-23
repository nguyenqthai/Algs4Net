/******************************************************************************
 *  File name :    ResizingArrayQueue.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/13stacks/tobe.txt
 *  
 *  Queue implementation with a resizing array.
 *
 *  C:\> algscmd ResizingArrayQueue < tobe.txt 
 *  to be or not to be (2 left on queue)
 *
 ******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>ResizingArrayQueue</c> class represents a first-in-first-out (FIFO)
  /// queue of generic items.
  /// It supports the usual <c>Enqueue</c> and <c>Dequeue</c>
  /// operations, along with methods for peeking at the first item,
  /// testing if the queue is empty, and iterating through
  /// the items in FIFO order.</para><para>
  /// This implementation uses a resizing array, which double the underlying array
  /// when it is full and halves the underlying array when it is one-quarter full.
  /// The <c>Enqueue</c> and <c>Dequeue</c> operations take constant amortized time.
  /// The <c>Count</c>, <c>Peek</c>, and <c>IsEmpty</c> operations takes
  /// constant time in the worst case.</para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/13stacks">Section 1.3</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/ResizingArrayQueue.java.html">ResizingArrayQueue</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class ResizingArrayQueue<Item> : IEnumerable<Item> {
    private Item[] q;       // queue elements
    private int N;          // number of elements on queue
    private int first;      // index of first element of queue
    private int last;       // index of next available slot


    /// <summary>Initializes an empty queue.</summary>
    ///
    public ResizingArrayQueue()
    {
      q = new Item[2];
      N = 0;
      first = 0;
      last = 0;
    }

    /// <summary>
    /// Is this queue empty?</summary>
    /// <returns>true if this queue is empty; false otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return N == 0; }

    }
    /// <summary>
    /// Returns the number of items in this queue.</summary>
    /// <returns>the number of items in this queue</returns>
    ///
    public int Count
    {
      get { return N; }
    }

    // resize the underlying array
    private void resize(int max)
    {
      Debug.Assert(max >= N);
      Item[] temp = new Item[max];
      for (int i = 0; i < N; i++)
      {
        temp[i] = q[(first + i) % q.Length];
      }
      q = temp;
      first = 0;
      last = N;
    }

    /// <summary>
    /// Adds the item to this queue.</summary>
    /// <param name="item">the item to add</param>
    ///
    public void Enqueue(Item item)
    {
      // double size of array if necessary and recopy to front of array
      if (N == q.Length) resize(2 * q.Length);   // double size of array if necessary
      q[last++] = item;                        // add item
      if (last == q.Length) last = 0;          // wrap-around
      N++;
    }

    /// <summary>
    /// Removes and returns the item on this queue that was least recently added.</summary>
    /// <returns>the item on this queue that was least recently added</returns>
    /// <exception cref="InvalidOperationException">if this queue is empty</exception>
    ///
    public Item Dequeue()
    {
      if (IsEmpty) throw new InvalidOperationException("Queue underflow");
      Item item = q[first];
      q[first] = default(Item);     // to avoid loitering
      N--;
      first++;
      if (first == q.Length) first = 0;           // wrap-around
                                                  // shrink size of array if necessary
      if (N > 0 && N == q.Length / 4) resize(q.Length / 2);
      return item;
    }

    /// <summary>
    /// Returns the item least recently added to this queue.</summary>
    /// <returns>the item least recently added to this queue</returns>
    /// <exception cref="InvalidOperationException">if this queue is empty</exception>
    ///
    public Item Peek()
    {
      if (IsEmpty) throw new InvalidOperationException("Queue underflow");
      return q[first];
    }


    /// <summary>
    /// Returns an iterator that iterates over the items in this queue in FIFO order.</summary>
    /// <returns>an iterator that iterates over the items in this queue in FIFO order</returns>
    public IEnumerator<Item> GetEnumerator()
    {
      int i;
      for (i = 0; i < N; i++) yield return q[(i + first) % q.Length];
    }

    // place holder method to comply to interface implementation
    IEnumerator IEnumerable.GetEnumerator()
    {
      int i;
      for (i = 0; i < N; i++) yield return q[(i + first) % q.Length];
    }

    /// <summary>
    /// Demo test the <c>ResizingArrayQueue</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd ResizingArrayQueue < tobe.txt", "Items separated by space or new line")]
    public static void MainTest(string[] args)
    {
      ResizingArrayQueue<string> q = new ResizingArrayQueue<string>();
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
