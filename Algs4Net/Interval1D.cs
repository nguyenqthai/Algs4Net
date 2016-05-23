/******************************************************************************
 *  File name :    Interval1D.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  1-dimensional interval data type.
 *
 *  C:\> algscmd Interval1D
 * 
 ******************************************************************************/

using System;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Interval1D</c> class represents a one-dimensional interval.
  /// The interval is <c>Closed</c>, which contains both endpoints.
  /// Intervals are immutable: their values cannot be changed after they are created.
  /// The class <c>Interval1D</c> includes methods for checking whether
  /// an interval contains a point and determining whether two intervals intersect.</summary>
  /// <remarks><para>For additional documentation, 
  /// see <a href="http://algs4.cs.princeton.edu/12oop">Section 1.2</a> of 
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Interval1D.java.html">Interval1D</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Interval1D
  {
    /// <summary>
    /// Compares two intervals by min endpoint.</summary>
    ///
    public static readonly Comparer<Interval1D> MIN_ENDPOINT_ORDER = new MinEndpointComparer();

    /// <summary>
    /// Compares two intervals by max endpoint.</summary>
    ///
    public static readonly Comparer<Interval1D> MAX_ENDPOINT_ORDER = new MaxEndpointComparer();

    /// <summary>
    /// Compares two intervals by length.</summary>
    ///
    public static readonly Comparer<Interval1D> LENGTH_ORDER = new LengthComparer();

    private readonly double min;
    private readonly double max;

    /// <summary>
    /// Initializes a closed interval [min, max].</summary>
    /// <param name="min">the smaller endpoint</param>
    /// <param name="max">the larger endpoint</param>
    /// <exception cref="ArgumentException">if the min endpoint is greater than the max endpoint</exception>
    /// <exception cref="ArgumentException">if either <c>min</c> or <c>max</c>
    ///        is <c>double.NaN</c>, <c>double.POSITIVE_INFINITY</c> or
    ///        <c>double.NEGATIVE_INFINITY</c></exception>
    ///
    public Interval1D(double min, double max)
    {
      if (double.IsInfinity(min) || double.IsInfinity(max))
        throw new ArgumentException("Endpoints must be finite");
      if (double.IsNaN(min) || double.IsNaN(max))
        throw new ArgumentException("Endpoints cannot be NaN");

      // convert -0.0 to +0.0
      if (min == 0.0) min = 0.0;
      if (max == 0.0) max = 0.0;

      if (min <= max)
      {
        this.min = min;
        this.max = max;
      }
      else throw new ArgumentException("Illegal interval");
    }

    /// <summary>
    /// Returns the min endpoint of this interval.</summary>
    /// <returns>the min endpoint of this interval</returns>
    ///
    public double Min
    {
      get { return min; }
    }

    /// <summary>
    /// Returns the max endpoint of this interval.</summary>
    /// <returns>the max endpoint of this interval</returns>
    ///
    public double Max
    {
      get { return max; }
    }

    /// <summary>
    /// Returns true if this interval intersects the specified interval.</summary>
    /// <param name="that">the other interval</param>
    /// <returns><c>true</c> if this interval intersects the argument interval;
    ///        <c>false</c> otherwise</returns>
    ///
    public bool Intersects(Interval1D that)
    {
      if (this.max < that.min) return false;
      if (that.max < this.min) return false;
      return true;
    }

    /// <summary>
    /// Returns true if this interval contains the specified value.</summary>
    /// <param name="x">the value</param>
    /// <returns><c>true</c> if this interval contains the value <c>x</c>;
    ///        <c>false</c> otherwise</returns>
    ///
    public bool Contains(double x)
    {
      return (min <= x) && (x <= max);
    }

    /// <summary>
    /// Returns the length of this interval.</summary>
    /// <returns>the length of this interval (max - min)</returns>
    ///
    public double Length
    {
      get { return max - min; }
    }

    /// <summary>
    /// Returns a string representation of this interval.</summary>
    /// <returns>a string representation of this interval in the form [min, max]</returns>
    ///
    public override string ToString()
    {
      return "[" + min + ", " + max + "]";
    }

    /// <summary>
    /// Compares this transaction to the specified object.</summary>
    /// <param name="other">the other interval</param>
    /// <returns><c>true</c> if this interval equals the other interval;
    ///        <c>false</c> otherwise</returns>
    ///
    public override bool Equals(object other)
    {
      if (other == this) return true;
      if (other == null) return false;
      if (other.GetType() != this.GetType()) return false;
      Interval1D that = (Interval1D)other;
      return this.min == that.min && this.max == that.max;
    }

    /// <summary>
    /// Returns an integer hash code for this interval.</summary>
    /// <returns>an integer hash code for this interval</returns>
    ///
    public override int GetHashCode()
    {
      int hash1 = min.GetHashCode();
      int hash2 = max.GetHashCode();
      return 31 * hash1 + hash2;
    }

    // ascending order of min endpoint, breaking ties by max endpoint
    private class MinEndpointComparer : Comparer<Interval1D>
    {
      public override int Compare(Interval1D a, Interval1D b)
      {
        if (a.min < b.min) return -1;
        else if (a.min > b.min) return +1;
        else if (a.max < b.max) return -1;
        else if (a.max > b.max) return +1;
        else return 0;
      }
    }

    // ascending order of max endpoint, breaking ties by min endpoint
    private class MaxEndpointComparer : Comparer<Interval1D>
    {
      public override int Compare(Interval1D a, Interval1D b)
      {
        if (a.min < b.max) return -1;
        else if (a.min > b.max) return +1;
        else if (a.min < b.min) return -1;
        else if (a.min > b.min) return +1;
        else return 0;
      }
    }

    // ascending order of length
    private class LengthComparer : Comparer<Interval1D>
    {
      public override int Compare(Interval1D a, Interval1D b)
      {
        double alen = a.Length;
        double blen = b.Length;
        if (alen < blen) return -1;
        else if (alen > blen) return +1;
        else return 0;
      }
    }

    /// <summary>
    /// Demo test the <c>Interval1D</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Interval1D")]
    public static void MainTest(string[] args)
    {
      Interval1D[] intervals = new Interval1D[4];
      intervals[0] = new Interval1D(15.0, 33.0);
      intervals[1] = new Interval1D(45.0, 60.0);
      intervals[2] = new Interval1D(20.0, 70.0);
      intervals[3] = new Interval1D(46.0, 55.0);

      Console.WriteLine("Unsorted");
      for (int i = 0; i < intervals.Length; i++)
        Console.WriteLine(intervals[i]);
      Console.WriteLine();

      Console.WriteLine("Sort by min endpoint");
      Array.Sort(intervals, Interval1D.MIN_ENDPOINT_ORDER);
      for (int i = 0; i < intervals.Length; i++)
        Console.WriteLine(intervals[i]);
      Console.WriteLine();

      Console.WriteLine("Sort by max endpoint");
      Array.Sort(intervals, Interval1D.MAX_ENDPOINT_ORDER);
      for (int i = 0; i < intervals.Length; i++)
        Console.WriteLine(intervals[i]);
      Console.WriteLine();

      Console.WriteLine("Sort by length");
      Array.Sort(intervals, Interval1D.LENGTH_ORDER);
      for (int i = 0; i < intervals.Length; i++)
        Console.WriteLine(intervals[i]);
      Console.WriteLine();
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
