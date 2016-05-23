/******************************************************************************
 *  File name :    ResizingArrayStack.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/13stacks/tobe.txt
 *
 *  Stack implementation with a resizing array.
 *
 *  C:\> type tobe.txt
 *  to be or not to - be - - that - - - is
 *
 *  C:\> algscmd ResizingArrayStack < tobe.txt
 *  to be not that or be (2 left on stack)
 *
 ******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Algs4Net 
{
  /// <summary>
  /// <para>The <c>ResizingArrayStack</c> class represents a (LIFO)
  /// stack of generic items. It supports the usual <c>Push</c> and <c>Pop</c>
  /// operations, along with methods for peeking at the top item, testing if 
  /// the stack is empty, and iterating through the items in LIFO order.</para>
  /// <para>This implementation uses a resizing array, which double the  
  /// underlying array when it is full and halves the underlying array when it
  /// is one-quarter full. The <c>Push</c> and <c>Pop</c> operations take 
  /// constant amortized time, whereas he <c>Count</c>, <c>Peek</c>, and 
  /// <c>IsEmpty</c> operations takes constant time in the worst case.</para>
  /// </summary>
  /// <remarks>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/13stacks">Section 1.3</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/ResizingArrayStack.java.html">ResizingArrayStack</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks> 
  /// 
  public class ResizingArrayStack<Item> : IEnumerable<Item>
  {

    private Item[] a;         // array of items
    private int N;            // number of elements on stack

    /// <summary>
    /// Initializes an empty stack.
    /// </summary>
    public ResizingArrayStack()
    {
      a = new Item[2];
      N = 0;
    }

    /// <summary>
    /// Is this stack empty? 
    /// </summary>
    /// <returns>true if this stack is empty; false otherwise</returns>
    public bool IsEmpty
    {
      get { return N == 0; }
    }

    /// <summary>
    /// Returns the number of items in the stack.
    /// </summary>
    public int Count
    {
      get { return N; }
    }

    /// <summary>
    /// Adds the item to this stack.
    /// </summary>
    /// <param name="item">the item to add</param>
    public void Push(Item item)
    {
      if (N == a.Length) Resize(2 * a.Length);  // double size of array if necessary
      a[N++] = item;                            // add item
    }

    /// <summary>
    /// Removes and returns the item most recently added to this stack. 
    /// </summary>
    /// <returns>the item most recently added</returns>
    /// <exception cref="InvalidOperationException">if the stack is empty</exception>
    public Item Pop()
    {
      if (IsEmpty) throw new InvalidOperationException("Stack underflow");
      Item item = a[N - 1];
      // to avoid loitering for reference types
      a[N - 1] = default(Item);
      N--;
      // shrink size of array if necessary
      if (N > 0 && N == a.Length / 4) Resize(a.Length / 2);
      return item;
    }

    /// <summary>
    /// Returns (but does not remove) the item most recently added to this stack.
    /// </summary>
    /// <returns>the item most recently added</returns>
    /// <exception cref="InvalidOperationException">if the stack is empty</exception>
    public Item Peek()
    {
      if (IsEmpty) throw new InvalidOperationException("Stack underflow");
      return a[N - 1];
    }


    // resize the underlying array holding the elements
    private void Resize(int capacity)
    {
      System.Diagnostics.Debug.Assert(capacity >= N);

      Item[] temp = new Item[capacity];
      for (int i = 0; i < N; i++)
      {
        temp[i] = a[i];
      }
      a = temp;
    }

    /// <summary>
    /// Returns an iterator to this stack that iterates through the items in LIFO order.
    /// </summary>
    /// <returns>the iterator in LIFO order</returns>
    public IEnumerator<Item> GetEnumerator()
    {
      int i;
      for (i = N - 1; i >= 0; i--) yield return a[i];
    }

    // place holder method to comply to interface implementation
    IEnumerator IEnumerable.GetEnumerator()
    {
      int i;
      for (i = N - 1; i >= 0; i--) yield return a[i];
    }

    /// <summary>
    /// Demo test for the <c>ResizingArrayStack</c> data type.
    /// </summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd ResizingArrayStack < tobe.txt", "Items separated by space or new line")]
    public static void MainTest(string[] args)
    {
      ResizingArrayStack<string> s = new ResizingArrayStack<string>();
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
