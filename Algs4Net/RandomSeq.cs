/******************************************************************************
 *  File name :    RandomSeq.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Prints N numbers between lo and hi.
 *
 *  C:\> algscmd RandomSeq 5 100.0 200.0
 *  123.43
 *  153.13
 *  144.38
 *  155.18
 *  104.02
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>RandomSeq</c> class is a client that prints out a pseudorandom
  /// sequence of real numbers in a given range.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/11model">Section 1.1</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/RandomSeq.java.html">RandomSeq</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class RandomSeq
  {
    // this class should not be instantiated
    private RandomSeq() { }

    /// <summary>
    /// Reads in two command-line arguments lo and hi and prints N uniformly
    /// random real numbers in [lo, hi) to standard output.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd RandomSeq N lo hi",
      "Number of randoms N, optionally followed by a range [lo, hi)")]
    public static void MainTest(string[] args)
    {

      // command-line arguments
      int N = int.Parse(args[0]);

      // for backward compatibility with Intro to Programming in Java version of RandomSeq
      if (args.Length == 1)
      {
        // generate and print N numbers between 0.0 and 1.0
        for (int i = 0; i < N; i++)
        {
          double x = StdRandom.Uniform();
          Console.WriteLine(x);
        }
      }
      else if (args.Length == 3)
      {
        double lo = double.Parse(args[1]);
        double hi = double.Parse(args[2]);

        // generate and print N numbers between lo and hi
        for (int i = 0; i < N; i++)
        {
          double x = StdRandom.Uniform(lo, hi);
          Console.Write("{0:F2}\n", x);
        }
      }

      else
      {
        throw new ArgumentException("Invalid number of arguments");
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
