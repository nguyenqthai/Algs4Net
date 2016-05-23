/******************************************************************************
 *  File name :  BreadthFirstPathsTests.cs
 *  Purpose   :  Contains basic unit tests for the BreadthFirstPaths class
 *
 ******************************************************************************/
using Algs4Net;
using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Algs4NetUnitTests
{
  [TestClass]
  public class BreadthFirstPathsTests
  {
    [TestMethod]
    public void BreadthFirstPathsTest1()
    {
      Graph g = TestHelpers.buildGraph();
      BreadthFirstPaths bfps;
      bfps = new BreadthFirstPaths(g, 0);

      Assert.AreEqual(0, bfps.DistTo(0));
      Assert.AreEqual(1, bfps.DistTo(1));
      Assert.AreEqual(3, bfps.DistTo(4));

      Assert.AreEqual(int.MaxValue, bfps.DistTo(10));
      Assert.AreEqual(int.MaxValue, bfps.DistTo(12));
      Assert.AreEqual(int.MaxValue, bfps.DistTo(5));

      bfps = new BreadthFirstPaths(g, 10);
      Assert.IsTrue(bfps.HasPathTo(11));
      Assert.AreEqual(1, bfps.DistTo(11));

      bfps = new BreadthFirstPaths(g, 12);
      Assert.AreEqual(2, bfps.DistTo(15));

      Assert.IsFalse(bfps.HasPathTo(11));
      Assert.IsFalse(bfps.HasPathTo(3));
      Assert.IsFalse(bfps.HasPathTo(9));
    }

  }
}
