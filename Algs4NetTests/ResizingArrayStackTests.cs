/******************************************************************************
 *  File name :  ResizingArrayStackTests.cs
 *  Purpose   :  Contains basic unit tests for the ResizingArrayStack class
 *
 ******************************************************************************/
using Algs4Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Algs4NetUnitTests
{
  [TestClass]
  public class ResizingArrayStackTests
  {
    private string IteratorToString(ResizingArrayStack<int> input)
    {
      StringBuilder sb = new StringBuilder();
      foreach (int i in input) sb.Append(i + " ");
      return sb.ToString();
    }

    [TestMethod]
    public void ResizingArrayStackTest1()
    {
      ResizingArrayStack<int> stack = new ResizingArrayStack<int>();

      Assert.AreEqual(0, stack.Count, "New stack has zero size");
      Assert.AreEqual(true, stack.IsEmpty, "New stack is empty");
      Assert.AreEqual("", IteratorToString(stack));
    }

    [TestMethod]
    public void ResizingArrayStackTest2()
    {
      int[] input = { 3, 2, 4, 6, 9 };
      ResizingArrayStack<int> stack = new ResizingArrayStack<int>();

      stack.Push(3);
      Assert.AreEqual("3 ", IteratorToString(stack));
      stack.Push(4);

      Assert.AreEqual("4 3 ", IteratorToString(stack));
      Assert.AreEqual(4, stack.Pop());
      Assert.AreEqual(3, stack.Pop());
      Assert.AreEqual(0, stack.Count, "New stack has zero size");

      foreach (int i in input) stack.Push(i);
      Assert.AreEqual(input.Length, stack.Count);
      Assert.AreEqual("9 6 4 2 3 ", IteratorToString(stack));
    }
  }

}


/******************************************************************************
 *  Copyright 2016, Thai Nguyen.
 *  Copyright 2002-2015, Robert Sedgewick and Kevin Wayne.
 *
 *  This file is part of a unit test module for Algs4Net.dll, a .NET library 
 *  that ports algs4.jar, which accompanies the textbook
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
