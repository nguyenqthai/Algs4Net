/******************************************************************************
 *  File name :    Average.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Reads in a sequence of real numbers, and computes their average.
 *  
 *  C:\> algscmd Average < tinyW.txt
 *  Average is 42.875
 *  
 *  C:\> algscmd Average
 *  10 5 6
 *  3 7 32
 *  ^Z
 *  Average is 10.5
 *
 *  Note [Ctrl-d] signifies the end of file on Unix.
 *  On windows use [Ctrl-z].
 *
 ******************************************************************************/
using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Average</c> class provides a client for reading in a sequence
  /// of real numbers and printing out their average.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/11model">Section 1.1</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Average.java.html">Average</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Average
  {
    // this class should not be instantiated
    private Average() { }

    /// <summary>Reads in a sequence of real numbers from standard input and prints
    /// out their average to standard output.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Average < tinyW.txt", "Numbers separated by space or new line")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();

      int count = 0;       // number input values
      double sum = 0.0;    // sum of input values
      // read data and compute statistics
      while (!StdIn.IsEmpty)
      {
        double value = StdIn.ReadDouble();
        sum += value;
        count++;
      }
      // compute the average
      double average = sum / count;

      // print results
      Console.WriteLine("Average is " + average);
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
