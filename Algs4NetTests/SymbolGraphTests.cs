/******************************************************************************
 *  File name :  SymbolGraphTests.cs
 *  Purpose   :  Contains basic unit tests for the SymbolGraph class
 *
 ******************************************************************************/
using System;
using Algs4Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Algs4NetUnitTests
{
  [TestClass]
  public class SymbolGraphTests
  {

    [TestMethod]
    public void SymbolGraphTest1()
    {
      // NOTE: the file has to be in the current working directory for unit tests
      string usastates = "us-state-neighbors.txt";
      SymbolGraph sg;
      Graph G;
      try
      {
        sg = new SymbolGraph(usastates, " ");
      }
      catch (Exception ex)
      {
        Assert.Fail(string.Format("Symbol graph construction error: {0}", ex.Message));
        return;
      }
      G = sg.G;
      string state = "HI";
      Assert.IsTrue(sg.Contains(state));
      string[] neighbors = new string[] { "CA" };
      SET<string> expected = new SET<string>(neighbors);
      SET<string> actual = TestHelpers.getAdjacents(sg, state);
      Assert.AreEqual(actual, expected);

      state = "CA";
      neighbors = new string[] { "OR", "NV", "AZ", "HI" };
      expected = new SET<string>(neighbors);
      actual = TestHelpers.getAdjacents(sg, state);
      Assert.AreEqual(actual, expected);

      state = "IA";
      neighbors = new string[] { "MN", "WI", "MO", "NE", "SD", "IL" };
      expected = new SET<string>(neighbors);
      actual = TestHelpers.getAdjacents(sg, state);
      Assert.AreEqual(actual, expected);

      // more ...

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
