/******************************************************************************
 *  File name :  QuickTests.cs
 *  Purpose   :  Contains basic unit tests for the Quick class
 *
 ******************************************************************************/
using Algs4Net;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Algs4NetUnitTests
{
  [TestClass]
  public class QuickTests
  {
    [TestMethod]
    [ExpectedException(typeof(IndexOutOfRangeException))]
    public void QuickTest1()
    {
      string[] a = new string[] { "aba" };
      string s;

      Quick.Sort(a);
      s = (string)Quick.Select(a, 0);
      Assert.AreEqual(s, a[0]);

      a = new string[] { "zoo", "able", "after", "cury", "aba", "bed", "bug", "boy", "bing", " " };
      s = (string)Quick.Select(a, a.Length - 1);
      Assert.AreEqual(s, "zoo");

      Quick.Sort(a);
      Assert.AreEqual("aba", a[1]);

      Quick.Select(a, a.Length); // generate exception
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

