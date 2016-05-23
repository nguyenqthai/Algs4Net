/******************************************************************************
 *  File name :  BagTests.cs
 *  Purpose   :  Contains basic unit tests for the Bag class
 *
 ******************************************************************************/
using Algs4Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Algs4NetUnitTests
{
  [TestClass]
  public class BagTests
  {
    [TestMethod]
    public void BagTest1()
    {
      string expected = "";
      Bag<int> bag = new Bag<int>();
      StringBuilder sb = new StringBuilder();
      foreach (var s in bag)
      {
        sb.Append(string.Format("{0} ", s));
      }
      Assert.IsTrue(bag.IsEmpty);
      Assert.AreEqual(expected, bag.ToString());
      Assert.AreEqual(expected, sb.ToString());

      bag.Add(3);
      expected = "3";
      Assert.AreEqual(expected, bag.ToString());

      bag.Add(2);
      bag.Add(3);
      Assert.IsFalse(bag.IsEmpty);
      Assert.AreEqual(3, bag.Count);
      expected = "3 2 3";
      Assert.AreEqual(expected, bag.ToString());

      Bag<int> bag2 = new Bag<int>();
      for (int i=0; i<10; i++)
      {
        bag.Add(i);
      }
      expected = "9 8 7 6 5 4 3 2 1 0 3 2 3";
      Assert.AreEqual(expected, bag.ToString());

      sb = new StringBuilder();
      sb.Append("[ ");
      foreach (var s in bag)
      {
        sb.Append(string.Format("{0} ", s));
      }
      sb.Append("]");
      Assert.AreEqual("[ "+expected+" ]", sb.ToString());

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
