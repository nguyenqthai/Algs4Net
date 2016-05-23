/******************************************************************************
 *  File name :    Counter.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A mutable data type for an integer counter.
 *
 *  The test clients create N counters and performs T increment
 *  operations on random counters.
 *
 *  C:\> algscmd Counter 5 100
 *  15 counter0
 *  17 counter1
 *  17 counter2
 *  24 counter3
 *  27 counter4
 *
 ******************************************************************************/
using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Counter</c> class is a mutable data type to encapsulate a counter.</summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/12oop">Section 1.2</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Counter.java.html">Counter</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Counter : IComparable<Counter>
  {
    private readonly string name; // counter name
    private int count = 0;        // current value

    /// <summary>Initializes a new counter starting at 0, with the given id.</summary>
    /// <param name="id">id the name of the counter</param>
    ///
    public Counter(string id)
    {
      name = id;
    }

    /// <summary>
    /// Increments the counter by 1.</summary>
    ///
    public void Increment()
    {
      count++;
    }

    /// <summary>
    /// Returns the current value of this counter.</summary>
    /// <returns>the current value of this counter</returns>
    ///
    public int Tally
    {
      get { return count; }
    }

    /// <summary>
    /// Returns a string representation of this counter.</summary>
    /// <returns>a string representation of this counter</returns>
    ///
    public override string ToString()
    {
      return count + " " + name;
    }

    /// <summary>
    /// Compares this counter to the specified counter.</summary>
    /// <param name="that"> that the other counter</param>
    /// <returns><c>0</c> if the value of this counter equals
    /// the value of that counter; a negative integer if the
    /// value of this counter is less than the value of that
    /// counter; and a positive integer if the value of this
    /// counter is greater than the value of that counter</returns>
    ///
    public int CompareTo(Counter that)
    {
      if (count < that.count) return -1;
      else if (count > that.count) return +1;
      else return 0;
    }

    /// <summary>
    /// Reads two command-line integers N and T; creates N counters;
    /// increments T counters at random; and prints results.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Counter N T",
      "N counters, T number of increments on randomly selected counters")]
    public static void MainTest(string[] args)
    {
      int N = int.Parse(args[0]);
      int T = int.Parse(args[1]);

      // create N counters
      Counter[] hits = new Counter[N];
      for (int i = 0; i < N; i++)
      {
        hits[i] = new Counter("counter" + i);
      }

      // increment T counters at random
      for (int t = 0; t < T; t++)
      {
        hits[StdRandom.Uniform(N)].Increment();
      }

      // print results
      for (int i = 0; i < N; i++)
      {
        Console.WriteLine(hits[i]);
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
