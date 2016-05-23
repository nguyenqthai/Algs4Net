/******************************************************************************
 *  File name :    LinkedStack.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A generic stack, implemented using a linked list. Each stack
 *  element is of type Item.
 *
 *  C:\> type tobe.txt
 *  to be or not to - be - - that - - - is
 *
 *  C:\> algscmd LinkedStack < tobe.txt
 *  to be not that or be (2 left on stack)
 *
 ******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>LinkedStack</c> class represents a last-in-first-out (LIFO) stack of
  /// generic items. So named to avoid conflict with the .NET framwork 
  /// <see cref="Stack{T}"/> class. Since C# strictly does not allow static class with
  /// instances, the implementation is effectively the same as the
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Stack.java.html">Stack</a>
  /// class implementation.</para><para>
  /// It supports the usual <c>Push</c> and <c>Pop</c> operations, along with methods
  /// for peeking at the top item, testing if the stack is empty, and iterating through
  /// the items in LIFO order.
  /// </para><para>
  /// This implementation uses a singly-linked list with a nested, non-static
  /// class Node and hence is the same as the <c>Stack</c> class in algs4.jar.
  /// The <c>Push</c>, <c>Pop</c>, <c>Peek</c>, <c>Count</c>, and <c>IsEmpty</c>
  /// operations all take constant time in the worst case.
  /// </para></summary>
  /// <remarks>
  /// For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/13stacks">Section 1.3</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/LinkedStack.java.html">LinkedStack</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks>
  ///
  public class LinkedStack<Item> : IEnumerable<Item>
  {
    private int N;          // size of the stack
    private Node first;     // top of stack

    // helper linked list class
    private class Node
    {
      public Item item;
      public Node next;
    }

    /// <summary>
    /// Initializes an empty stack.</summary>
    ///
    public LinkedStack()
    {
      first = null;
      N = 0;
      Debug.Assert(check());
    }

    /// <summary>
    /// Is this stack empty?</summary>
    /// <returns>true if this queue is empty; false otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return first == null; }
    }

    /// <summary>
    /// Returns the number of items in the stack.</summary>
    /// <returns>the number of items in the stack</returns>
    ///
    public int Count
    {
      get { return N; }
    }

    /// <summary>
    /// Adds the item to this stack.</summary>
    /// <param name="item">item the item to add</param>
    ///
    public void Push(Item item)
    {
      Node oldfirst = first;
      first = new Node();
      first.item = item;
      first.next = oldfirst;
      N++;
      Debug.Assert(check());
    }

    /// <summary>
    /// Removes and returns the item most recently added to this stack.</summary>
    /// <returns>the item most recently added</returns>
    /// <exception cref="InvalidOperationException">if this stack is empty</exception>
    ///
    public Item Pop()
    {
      if (IsEmpty) throw new InvalidOperationException("LinkedStack underflow");
      Item item = first.item;
      first = first.next;
      N--;
      Debug.Assert(check());
      return item;
    }

    /// <summary>
    /// Returns (but does not remove) the item most recently added to this stack.</summary>
    /// <returns>the item most recently added to this stack</returns>
    /// <exception cref="InvalidOperationException">if this stack is empty</exception>
    ///
    public Item Peek()
    {
      if (IsEmpty) throw new InvalidOperationException("LinkedStack underflow");
      return first.item;
    }

    /// <summary>
    /// Returns a string representation of this stack.</summary>
    /// <returns>the sequence of items in the stack in LIFO order, separated by spaces</returns>
    ///
    public override string ToString()
    {
      StringBuilder s = new StringBuilder();
      foreach (Item item in this)
        s.Append(item + " ");
      return s.ToString();
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
      return this.GetEnumerator();
    }


    // an iterator, doesn't implement remove() since it's optional
    private class ListIEnumerator : IEnumerator<Item>
    {
      private Node current = null;
      private LinkedStack<Item> stack = null;
      private bool firstCall = true;

      public ListIEnumerator(LinkedStack<Item> collection)
      {
        stack = collection;
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
          current = stack.first;
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

    // check internal invariants
    private bool check()
    {

      // check a few properties of instance variable 'first'
      if (N < 0)
      {
        return false;
      }
      if (N == 0)
      {
        if (first != null) return false;
      }
      else if (N == 1)
      {
        if (first == null) return false;
        if (first.next != null) return false;
      }
      else {
        if (first == null) return false;
        if (first.next == null) return false;
      }

      // check internal consistency of instance variable N
      int numberOfNodes = 0;
      for (Node x = first; x != null && numberOfNodes <= N; x = x.next)
      {
        numberOfNodes++;
      }
      if (numberOfNodes != N) return false;

      return true;
    }

    /// <summary>
    /// Demo test the <c>LinkedStack</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    ///
    [HelpText("algscmd LinkedStack < tobe.txt", "Items separated by space or new line")]
    public static void MainTest(string[] args)
    {
      LinkedStack<string> s = new LinkedStack<string>();

      TextInput StdIn = new TextInput();
      while (!StdIn.IsEmpty)
      {
        string item = StdIn.ReadString();
        if (!item.Equals("-")) s.Push(item);
        else if (!s.IsEmpty) Console.Write(s.Pop() + " ");
      }
      Console.WriteLine("(" + s.Count + " left on stack)");
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
