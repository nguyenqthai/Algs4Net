/******************************************************************************
 *  File name :  UFTests.cs
 *  Purpose   :  Contains basic unit tests for the UF class
 *
 ******************************************************************************/
using Algs4Net;
using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Algs4NetUnitTests
{
  [TestClass]
  public class UFTests
  {
    private UF Union(Tuple<int, int>[] links, int N)
    {
      UF uf = new UF(N);
      foreach (var pair in links)
      {
        int p = pair.Item1;
        int q = pair.Item2;
        if (uf.Connected(p, q)) continue;
        uf.Union(pair.Item1, pair.Item2);
      }
      return uf;
    }

    [TestMethod]
    [ExpectedException(typeof(IndexOutOfRangeException))]
    public void UFTest1()
    {
      // no connected components
      Tuple<int, int>[] set1 =
      {
        new Tuple<int, int>(1, 1),
        new Tuple<int, int>(2, 2),
        new Tuple<int, int>(3, 3),
        new Tuple<int, int>(0, 0)
      };

      UF uf = Union(set1,set1.Length);
      Assert.AreEqual(uf.Count, set1.Length);

      // three connected components
      Tuple<int, int>[] set2 =
      {
        new Tuple<int, int>(0, 1),
        new Tuple<int, int>(2, 3),
        new Tuple<int, int>(4, 5),
        new Tuple<int, int>(6, 0),
        new Tuple<int, int>(7, 1)
      };
      uf = Union(set2, 8);
      Assert.AreEqual(uf.Count, 3);

      set2[0] = new Tuple<int, int>(7, 8);
      uf = Union(set2, 8); // generate exception

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

