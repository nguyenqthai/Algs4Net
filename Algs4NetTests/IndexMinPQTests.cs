/******************************************************************************
 *  File name :  IndexMinPQTests.cs
 *  Purpose   :  Contains basic unit tests for the IndexMinPQ class
 *
 ******************************************************************************/
using Algs4Net;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Algs4NetUnitTests
{
  [TestClass]
  public class IndexMinPQTests
  {
    [TestMethod]
    public void IndexMinPQTest1()
    {
      const int MaxSize = 8;
      const double MinValue = 3.9;
      const double MaxValue = MinValue * MaxSize + 32;
      int index;
      // MinValue index == 3, MaxValue index == 4
      double[] items = { MinValue * 2, MinValue * 3, MinValue * 4, MinValue, MaxValue, MinValue * 5, MinValue * 6, MinValue * 7 };
      StdRandom.Seed = 101;

      IndexMinPQ<double> pq = new IndexMinPQ<double>(MaxSize);
      index = StdRandom.Uniform(items.Length);
      Assert.IsFalse(pq.Contains(index));
      Assert.IsTrue(pq.IsEmpty);
      Assert.AreEqual(0, pq.Count);

      try
      {
        index = pq.DelMin();
        Assert.Fail("Failed to catch exception");
      }
      catch (InvalidOperationException) { }

      for (int i=0; i<items.Length; i++)
      {
        pq.Insert(i, items[i]);
      }
      Assert.AreEqual(items.Length, pq.Count);
      Assert.AreEqual(MinValue, pq.MinKey);
      Assert.AreEqual(3, pq.MinIndex);
      Assert.AreEqual(MaxValue, pq.KeyOf(4));

      index = StdRandom.Uniform(items.Length);
      Assert.AreEqual(items[index], pq.KeyOf(index));

      pq.ChangeKey(1, pq.MinKey * 0.9); // make it the smallest item
      Assert.AreEqual(1, pq.MinIndex);

      pq.DecreaseKey(3, pq.MinKey * 0.87);
      Assert.AreEqual(3, pq.MinIndex);

      pq.Delete(3);
      Assert.AreNotEqual(3, pq.MinIndex);

      Assert.AreEqual(1, pq.DelMin());
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
