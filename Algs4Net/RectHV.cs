/******************************************************************************
 *  File name :    RectHV.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Immutable data type for 2D axis-aligned rectangle.
 *  
 *  C:\> algscmd RectHV
 *
 ******************************************************************************/
using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary>
  /// The <c>RectHV</c> class is an immutable data type to encapsulate a
  /// two-dimensional axis-aligned rectagle with real-value coordinates.
  /// The rectangle is <c>Closed</c>; it includes the points on the boundary.</summary>
  /// <remarks><para>For additional documentation, 
  /// see <a href="http://algs4.cs.princeton.edu/12oop">Section 1.2</a> of 
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/RectHV.java.html">RectHV</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public sealed class RectHV : BasicVisual
  {
    private readonly double xmin, ymin;   // minimum x- and y-coordinates
    private readonly double xmax, ymax;   // maximum x- and y-coordinates

    /// <summary>Initializes a new rectangle [<c>Xmin</c>, <c>Xmax</c>]
    /// x [<c>Ymin</c>, <c>Ymax</c>].</summary>
    /// <param name="xmin">the <c>X</c>-coordinate of the lower-left endpoint</param>
    /// <param name="xmax">the <c>X</c>-coordinate of the upper-right endpoint</param>
    /// <param name="ymin">the <c>Y</c>-coordinate of the lower-left endpoint</param>
    /// <param name="ymax">the <c>Y</c>-coordinate of the upper-right endpoint</param>
    /// <exception cref="ArgumentException">if any of <c>xmin</c>, <c>xmax</c>, 
    /// <c>ymin</c>, or <c>ymax</c> is <c>double.NaN</c>.</exception>
    /// <exception cref="ArgumentException">if <c>xmax</c> &lt;
    /// <c>xmin</c> or <c>ymax</c> &lt; <c>ymin</c>.</exception>
    ///
    public RectHV(double xmin, double ymin, double xmax, double ymax)
    {
      if (double.IsNaN(xmin) || double.IsNaN(xmax))
        throw new ArgumentException("x-coordinate cannot be NaN");
      if (double.IsNaN(ymin) || double.IsNaN(ymax))
        throw new ArgumentException("y-coordinates cannot be NaN");
      if (xmax < xmin || ymax < ymin)
      {
        throw new ArgumentException("Invalid rectangle");
      }
      this.xmin = xmin;
      this.ymin = ymin;
      this.xmax = xmax;
      this.ymax = ymax;
    }

    /// <summary>
    /// Returns the minimum <c>X</c>-coordinate of any point in this rectangle.</summary>
    /// <returns>the minimum <c>X</c>-coordinate of any point in this rectangle</returns>
    ///
    public double Xmin
    {
      get { return xmin; }
    }

    /// <summary>
    /// Returns the maximum <c>X</c>-coordinate of any point in this rectangle.</summary>
    /// <returns>the maximum <c>X</c>-coordinate of any point in this rectangle</returns>
    ///
    public double Xmax
    {
      get { return xmax; }
    }

    /// <summary>
    /// Returns the minimum <c>Y</c>-coordinate of any point in this rectangle.</summary>
    /// <returns>the minimum <c>Y</c>-coordinate of any point in this rectangle</returns>
    ///
    public double Ymin
    {
      get { return ymin; }
    }

    /// <summary>
    /// Returns the maximum <c>Y</c>-coordinate of any point in this rectangle.</summary>
    /// <returns>the maximum <c>Y</c>-coordinate of any point in this rectangle</returns>
    ///
    public double Ymax
    {
      get { return ymax; }
    }

    /// <summary>
    /// Returns the width of this rectangle.</summary>
    /// <returns>the width of this rectangle <c>xmax - xmin</c></returns>
    ///
    public double Width
    {
      get { return xmax - xmin; }
    }

    /// <summary>
    /// Returns the height of this rectangle.</summary>
    /// <returns>the height of this rectangle <c>ymax - ymin</c></returns>
    ///
    public double Height
    {
      get { return ymax - ymin; }
    }

    /// <summary>
    /// Returns true if the two rectangles intersect.</summary>
    /// <param name="that">the other rectangle</param>
    /// <returns><c>true</c> if this rectangle intersect the argument
    /// rectangle at one or more points, including on the boundary</returns>
    ///
    public bool Intersects(RectHV that)
    {
      return this.xmax >= that.xmin && this.ymax >= that.ymin
          && that.xmax >= this.xmin && that.ymax >= this.ymin;
    }

    /// <summary>
    /// Returns true if this rectangle contain the point.</summary>
    /// <param name="p">the point</param>
    /// <returns><c>true</c> if this rectangle contain the point <c>p</c>,
    /// possibly at the boundary; <c>false</c> otherwise</returns>
    ///
    public bool Contains(Point2D p)
    {
      return (p.X >= xmin) && (p.X <= xmax)
          && (p.Y >= ymin) && (p.Y <= ymax);
    }

    /// <summary>
    /// Returns the Euclidean distance between this rectangle and the point <c>p</c>.</summary>
    /// <param name="p">the point</param>
    /// <returns>the Euclidean distance between the point <c>p</c> and the closest point
    /// on this rectangle; 0 if the point is contained in this rectangle</returns>
    ///
    public double DistanceTo(Point2D p)
    {
      return Math.Sqrt(DistanceSquaredTo(p));
    }

    /// <summary>
    /// Returns the square of the Euclidean distance between this rectangle and the point <c>p</c>.</summary>
    /// <param name="p"> p the point</param>
    /// <returns>the square of the Euclidean distance between the point <c>p</c> and
    ///        the closest point on this rectangle; 0 if the point is contained
    ///        in this rectangle</returns>
    ///
    public double DistanceSquaredTo(Point2D p)
    {
      double dx = 0.0, dy = 0.0;
      if (p.X < xmin) dx = p.X - xmin;
      else if (p.X > xmax) dx = p.X - xmax;
      if (p.Y < ymin) dy = p.Y - ymin;
      else if (p.Y > ymax) dy = p.Y - ymax;
      return dx * dx + dy * dy;
    }

    /// <summary>
    /// Compares this rectangle to the specified rectangle.</summary>
    /// <param name="other"> other the other rectangle</param>
    /// <returns><c>true</c> if this rectangle equals <c>other</c>;
    /// <c>false</c> otherwise</returns>
    ///
    public override bool Equals(object other)
    {
      if (other == this) return true;
      if (other == null) return false;
      if (other.GetType() != this.GetType()) return false;
      RectHV that = (RectHV)other;
      if (this.xmin != that.xmin) return false;
      if (this.ymin != that.ymin) return false;
      if (this.xmax != that.xmax) return false;
      if (this.ymax != that.ymax) return false;
      return true;
    }

    /// <summary>
    /// Returns an integer hash code for this rectangle.</summary>
    /// <returns>an integer hash code for this rectangle</returns>
    ///
    public override int GetHashCode()
    {
      int hash1 = xmin.GetHashCode();
      int hash2 = ymin.GetHashCode();
      int hash3 = xmax.GetHashCode();
      int hash4 = ymax.GetHashCode();
      return 31 * (31 * (31 * hash1 + hash2) + hash3) + hash4;
    }

    /// <summary>
    /// Returns a string representation of this rectangle.</summary>
    /// <returns>a string representation of this rectangle, using the format
    ///        <c>[xmin, xmax] x [ymin, ymax]</c></returns>
    ///
    public override string ToString()
    {
      return "[" + xmin + ", " + xmax + "] x [" + ymin + ", " + ymax + "]";
    }

    /// <summary>
    /// Draws this rectangle to standard draw.</summary>
    ///
    public override void Draw()
    {
      // NOTE: If all the lines are to keep together as a single shape
      // then use AddRectangle or AddPolygon.
      Display.DrawLine(xmin, ymin, xmax, ymin);
      Display.DrawLine(xmax, ymin, xmax, ymax);
      Display.DrawLine(xmax, ymax, xmin, ymax);
      Display.DrawLine(xmin, ymax, xmin, ymin);
    }
    // TODO: Add a window class to demo the Draw method

    /// <summary>
    /// Demo test for the <c>RectHVTest</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    public static void MainTest(string[] args)
    {
      RectHV rec1 = new RectHV(50, 50, 300, 300);
      RectHV rec2 = new RectHV(150, 100, 400, 150);

      Point2D p1 = new Point2D(50, 500);
      Point2D p2 = new Point2D(400, 150);

      Console.WriteLine("Predicting rec1 intersects rec2: {0}", rec1.Intersects(rec2));
      Console.WriteLine("Predicting rec2 NOT contains p1 is {0}", !rec2.Contains(p1));
      Console.WriteLine("Predicting rec2 contains p2 is {0}", rec2.Contains(p2));

      Console.WriteLine("Distance from rectangle {0} to point {1} is {2}", rec1, p1, rec1.DistanceTo(p1));
      Console.WriteLine("Distance from rectangle {0} to point {1} is {2}", rec1, p2, rec1.DistanceTo(p2));

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
