/******************************************************************************
 *  File name :  BinarySearchTests.cs
 *  Purpose   :  Contains basic unit tests for the BinarySearch class
 *
 ******************************************************************************/
using Algs4Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Algs4NetUnitTests
{
  [TestClass]
  public class BinarySearchTests
  {
    [TestMethod]
    public void BinarySearchTest1()
    {
      int[] a = { 2, 4, 6, 8, 10, 12, 14, 16, 18, 22 };

      Assert.AreEqual(-1, BinarySearch.IndexOf(a, 1));
      Assert.AreEqual(-1, BinarySearch.IndexOf(a, 20));
      Assert.AreEqual(-1, BinarySearch.IndexOf(a, 25));
      Assert.AreEqual(1, BinarySearch.IndexOf(a, 4));
      Assert.AreEqual(5, BinarySearch.IndexOf(a, 12));
      Assert.AreEqual(9, BinarySearch.IndexOf(a, 22));
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
