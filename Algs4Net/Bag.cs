/******************************************************************************
 *  File name :    Bag.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/13stacks/tobe.txt
 *
 *  A generic bag or multiset, implemented using a singly-linked list.
 *
 *  C:\> type tobe.txt
 *  to be or not to - be - - that - - - is
 *
 *  C:\> algscmd Bag < tobe.txt
 *  size of bag = 14
 *  is
 *  -
 *  -
 *  -
 *  that
 *  -
 *  -
 *  be
 *  -
 *  to
 *  not
 *  or
 *  be
 *  to
 *
 ******************************************************************************/

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Algs4Net
{
    /// <summary><para>
    /// The <c>Bag</c> class represents a bag (or multiset) of
    /// generic items. It supports insertion and iterating over the
    /// items in arbitrary order.</para>
    /// <para>
    /// This implementation uses a singly-linked list with a nested, non-static
    /// class Node and hence is the same as the <c>LinkedBag</c> class in
    /// algs4.jar. The <c>Add</c>, <c>IsEmpty</c>, and <c>Count</c> operations
    /// take constant time. Iteration takes time proportional to the number of items.
    /// </para></summary>
    /// <remarks><para>
    /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/13stacks">Section 1.3</a> of
    ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
    /// <para>This class is a C# port from the original Java class 
    /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Bag.java.html">FFT</a>
    /// implementation by the respective authors.</para></remarks>
    /// 
    public class Bag<Item> : IEnumerable<Item>
  {
    private Node first;    // beginning of bag
    private int N;         // number of elements in bag

    // helper linked list class
    private class Node
    {
      public Item item;
      public Node next;
    }

    /// <summary>Initializes an empty bag.</summary>
    public Bag()
    {
      first = null;
      N = 0;
    }

    /// <summary>
    /// Returns true if this bag is empty.</summary>
    /// <returns><c>true</c> if this bag is empty;<c>false</c> otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return first == null; }
    }

    /// <summary>Returns the number of items in this bag.</summary>
    /// <returns>the number of items in this bag</returns>
    ///
    public int Count
    {
      get { return N; }
    }

    /// <summary>Adds the item to this bag.</summary>
    /// <param name="item"> item the item to add to this bag</param>
    ///
    public void Add(Item item)
    {
      Node oldFirst = first;
      first = new Node();
      first.item = item;
      first.next = oldFirst;
      N++;
    }

    /// <summary>
    /// Returns an enumerator that iterates over the items in this bag in arbitrary order.</summary>
    /// <returns>an enumerator that iterates over the items in this bag in arbitrary order</returns>
    ///
    public IEnumerator<Item> GetEnumerator()
    {
      return new ListIEnumerator(first);
    }

    /// <summary>
    /// Returns an iterator that iterates over the items in this queue in FIFO order.</summary>
    /// <returns>an iterator that iterates over the items in this queue in FIFO order</returns>
    ///
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    // an iterator, doesn't implement Displose() since it's optional
    private class ListIEnumerator : IEnumerator<Item>
    {
      private Node current = null;
      private Node first;
      private bool firstCall = true;

      public ListIEnumerator(Node first)
      {
        this.first = first;
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
          current = first;
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
    /// Returns a string representation in the format "it1 it2 it3 ... itn" (LIFO order)
    /// </summary>
    /// <returns>the format string</returns>
    public override string ToString()
    {
      if (IsEmpty) return string.Empty;

      StringBuilder sb = new StringBuilder();
      Node current = first;
      while (current != null)
      {
        sb.Append(string.Format("{0} ", current.item));
        current = current.next;
      }
      sb.Remove(sb.Length - 1, 1); // remove last space
      return sb.ToString();
    }

    /// <summary>
    /// Demo test for the <c>Bag</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText(@"algscmd Bag < tobe.txt", "Items separated by space or new line")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      Bag<string> bag = new Bag<string>();
      while (!StdIn.IsEmpty)
      {
        string item = StdIn.ReadString();
        bag.Add(item);
      }

      Console.WriteLine("Size of bag = " + bag.Count);
      foreach (string s in bag)
      {
        Console.WriteLine(s);
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
