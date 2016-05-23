/******************************************************************************
 *  File name :  IndexMaxPQTests.cs
 *  Purpose   :  Contains basic unit tests for the IndexMaxPQ class
 *
 ******************************************************************************/
using Algs4Net;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Algs4NetUnitTests
{
  [TestClass]
  public class IndexMaxPQTests
  {
    [TestMethod]
    public void IndexMaxPQTest1()
    {
      const int MaxSize = 8;
      const double MinValue = 3.9;
      const double MaxValue = MinValue * MaxSize + 32;
      int index;
      // MaxValue index == 3, MinValue index == 4
      double[] items = { MinValue * 2, MinValue * 3, MinValue * 4, MaxValue, MinValue, MinValue * 5, MinValue * 6, MinValue * 7 };
      StdRandom.Seed = 101;

      IndexMaxPQ<double> pq = new IndexMaxPQ<double>(MaxSize);
      index = StdRandom.Uniform(items.Length);
      Assert.IsFalse(pq.Contains(index));
      Assert.IsTrue(pq.IsEmpty);
      Assert.AreEqual(0, pq.Count);

      try
      {
        index = pq.DelMax();
        Assert.Fail("Failed to catch exception");
      }
      catch (InvalidOperationException) { }

      for (int i = 0; i < items.Length; i++)
      {
        pq.Insert(i, items[i]);
      }
      Assert.AreEqual(items.Length, pq.Count);
      Assert.AreEqual(MaxValue, pq.MaxKey);
      Assert.AreEqual(3, pq.MaxIndex);
      Assert.AreEqual(MinValue, pq.KeyOf(4));

      index = StdRandom.Uniform(items.Length);
      Assert.AreEqual(items[index], pq.KeyOf(index));

      pq.ChangeKey(1, pq.MaxKey * 1.9); // make it the largest item
      Assert.AreEqual(1, pq.MaxIndex);

      pq.IncreaseKey(3, pq.MaxKey * 1.87);
      Assert.AreEqual(3, pq.MaxIndex);

      pq.Delete(3);
      Assert.AreNotEqual(3, pq.MaxIndex);

      Assert.AreEqual(1, pq.DelMax());

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

