/******************************************************************************
 *  File name :    ResizingArrayBag.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Bag implementation with a resizing array.
 *  
 *  C:\> algscmd ResizingArrayBag < tobe.txt
 *
 ******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>ResizingArrayBag</c> class represents a bag (or multiset) of
  /// generic items. It supports insertion and iterating over the 
  /// items in arbitrary order.</para><para>
  /// This implementation uses a resizing array.
  /// See <seealso cref="Bag{Item}"/> for a version that uses a singly-linked list.
  /// The <c>Add</c> operation takes constant amortized time; the
  /// <c>IsEmpty</c>, and <c>Count</c> operations
  /// take constant time. Iteration takes time proportional to the number of items.
  /// </para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/13stacks">Section 1.3</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/ResizingArrayBag.java.html">ResizingArrayBag</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class ResizingArrayBag<Item> : IEnumerable<Item>
  {
    private Item[] a;         // array of items
    private int N;            // number of elements on stack

    /// <summary>Initializes an empty bag.</summary>
    ///
    public ResizingArrayBag()
    {
      a = new Item[2];
      N = 0;
    }

    /// <summary>
    /// Is this bag empty?</summary>
    /// <returns>true if this bag is empty; false otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return N == 0; }
    }

    /// <summary>
    /// Returns the number of items in this bag.</summary>
    /// <returns>the number of items in this bag</returns>
    ///
    public int Count
    {
      get { return N; }
    }

    // resize the underlying array holding the elements
    private void resize(int capacity)
    {
      Debug.Assert(capacity >= N);
      Item[] temp = new Item[capacity];
      for (int i = 0; i < N; i++)
        temp[i] = a[i];
      a = temp;
    }

    /// <summary>
    /// Adds the item to this bag.</summary>
    /// <param name="item">item the item to add to this bag</param>
    ///
    public void Add(Item item)
    {
      if (N == a.Length) resize(2 * a.Length);  // double size of array if necessary
      a[N++] = item;                            // add item
    }


    /// <summary>
    /// Returns an iterator that iterates over the items in the bag in arbitrary order.</summary>
    /// <returns>an iterator that iterates over the items in the bag in arbitrary order</returns>
    ///
    public IEnumerator<Item> GetEnumerator()
    {
      int i;
      for (i = 0; i < N; i++) yield return a[i];
    }

    // place holder method to comply to interface implementation
    IEnumerator IEnumerable.GetEnumerator()
    {
      int i;
      for (i = 0; i < N; i++) yield return a[i];
    }

    /// <summary>
    /// Demo test the <c>ResizingArrayBag</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd ResizingArrayBag < tobe.txt", "Items separated by space or new line")]
    public static void MainTest(string[] args)
    {
      ResizingArrayBag<string> bag = new ResizingArrayBag<string>();
      TextInput StdIn = new TextInput();
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
