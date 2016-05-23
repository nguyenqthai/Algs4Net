/******************************************************************************
 *  File name :    Point2D.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Immutable point data type for points in the plane.
 *  
 *  C:\> algscmd Point2D 100 200 50
 *  (a Window proram will show up) 
 *
 ******************************************************************************/

using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Point</c> class is an immutable data type to encapsulate a
  /// two-dimensional point with real-value coordinates. In order to deal with
  /// the difference behavior of double with respect to -0.0 and +0.0, the 
  /// Point2D constructor converts any coordinates that are -0.0 to +0.0.</summary>
  /// <remarks><para>For additional documentation, 
  /// see <a href="http://algs4.cs.princeton.edu/12oop">Section 1.2</a> of 
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Point2D.java.html">Point2D</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public sealed class Point2D : BasicVisual, IComparable<Point2D> {

    private readonly double x;    // x coordinate
    private readonly double y;    // y coordinate

    /// <summary>
    /// Initializes a new point (x, y).</summary>
    /// <param name="x">the x-coordinate</param>
    /// <param name="y">the y-coordinate</param>
    /// <exception cref="ArgumentException">if either <c>x</c> or <c>y</c>
    ///   is <c>double.NaN</c>, <c>double.PositiveInfinity</c> or
    ///   <c>double.NegativeInfinity</c></exception>
    ///
    public Point2D(double x, double y)
    {
      if (double.IsInfinity(x) || double.IsInfinity(y))
        throw new ArgumentException("Coordinates must be finite");
      if (double.IsNaN(x) || double.IsNaN(y))
        throw new ArgumentException("Coordinates cannot be NaN");
      if (x == 0.0) this.x = 0.0;  // convert -0.0 to +0.0
      else this.x = x;
      if (y == 0.0) this.y = 0.0;  // convert -0.0 to +0.0
      else this.y = y;
    }

    /// <summary>
    /// Returns the x-coordinate.</summary>
    /// <returns>the x-coordinate</returns>
    ///
    public double X
    {
      get { return x; }
    }

    /// <summary>
    /// Returns the y-coordinate.</summary>
    /// <returns>the y-coordinate</returns>
    ///
    public double Y
    {
      get { return y; }
    }

    /// <summary>
    /// Returns the polar radius of this point.</summary>
    /// <returns>the polar radius of this point in polar coordiantes: sqrt(x*x + y*y)</returns>
    ///
    public double R
    {
      get { return Math.Sqrt(x * x + y * y); }
    }

    /// <summary>
    /// Returns the angle of this point in polar coordinates.</summary>
    /// <returns>the angle (in radians) of this point in polar coordiantes (between -pi/2 and pi/2)</returns>
    ///
    public double Theta
    {
      get { return Math.Atan2(y, x); }
    }

    /// <summary>
    /// Returns the angle between this point and that point.</summary>
    /// <returns>the angle in radians (between -pi and pi) between this point and that point (0 if equal)</returns>
    ///
    private double angleTo(Point2D that)
    {
      double dx = that.x - x;
      double dy = that.y - y;
      return Math.Atan2(dy, dx);
    }

    /// <summary>
    /// Returns true if a->b->c is a counterclockwise turn.</summary>
    /// <param name="a">first point</param>
    /// <param name="b">second point</param>
    /// <param name="c">third point</param>
    /// <returns>{ -1, 0, +1 } if a->b->c is a { clockwise, collinear; counterclocwise } turn.</returns>
    ///
    public static int Ccw(Point2D a, Point2D b, Point2D c)
    {
      double area2 = (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);
      if (area2 < 0) return -1;
      else if (area2 > 0) return +1;
      else return 0;
    }

    /// <summary>
    /// Returns twice the signed area of the triangle a-b-c.</summary>
    /// <param name="a">first point</param>
    /// <param name="b">second point</param>
    /// <param name="c">third point</param>
    /// <returns>twice the signed area of the triangle a-b-c</returns>
    ///
    public static double Area2(Point2D a, Point2D b, Point2D c)
    {
      return (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);
    }

    /// <summary>
    /// Returns the Euclidean distance between this point and that point.</summary>
    /// <param name="that">the other point</param>
    /// <returns>the Euclidean distance between this point and that point</returns>
    ///
    public double DistanceTo(Point2D that)
    {
      double dx = x - that.x;
      double dy = y - that.y;
      return Math.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// Returns the square of the Euclidean distance between this point and that point.</summary>
    /// <param name="that">that the other point</param>
    /// <returns>the square of the Euclidean distance between this point and that point</returns>
    ///
    public double DistanceSquaredTo(Point2D that)
    {
      double dx = x - that.x;
      double dy = y - that.y;
      return dx * dx + dy * dy;
    }

    /// <summary>
    /// Compares two points by y-coordinate, breaking ties by x-coordinate.
    /// Formally, the invoking point (x0, y0) is less than the argument point (x1, y1)
    /// if and only if either y0 &lt; y1 or if y0 = y1 and x0 &lt; x1.</summary>
    /// <param name="that">the other point</param>
    /// <returns>the value <c>0</c> if this string is equal to the argument
    /// string (precisely when <c>equals()</c> returns <c>true</c>); a negative
    /// integer if this point is less than the argument point; and a positive 
    /// integer if this point is greater than the argument point</returns>
    ///
    public int CompareTo(Point2D that)
    {
      if (y < that.y) return -1;
      if (y > that.y) return +1;
      if (x < that.x) return -1;
      if (x > that.x) return +1;
      return 0;
    }

    /// <summary>Compares two points by x-coordinate.</summary>
    ///
    public static readonly Comparer<Point2D> X_ORDER = new XOrder();

    /// <summary>Compares two points by y-coordinate.</summary>
    ///
    public static readonly Comparer<Point2D> Y_ORDER = new YOrder();

    /// <summary>
    /// Compares two points by polar radius.</summary>
    ///
    public static readonly Comparer<Point2D> R_ORDER = new ROrder();

    /// <summary>
    /// Compares two points by polar angle (between 0 and 2pi) with respect to this point.</summary>
    /// <returns>the comparator</returns>
    ///
    public Comparer<Point2D> GetPolarOrder()
    {
      return new PolarOrder(this);
    }

    /// <summary>
    /// Compares two points by atan2() angle (between -pi and pi) with respect to this point.</summary>
    /// <returns>the comparator</returns>
    ///
    public Comparer<Point2D> GetAtan2Order()
    {
      return new Atan2Order(this);
    }

    /// <summary>
    /// Compares two points by distance to this point.</summary>
    /// <returns>the comparator</returns>
    ///
    public Comparer<Point2D> GetDistanceToOrder()
    {
      return new DistanceToOrder(this);
    }

    // compare points according to their x-coordinate
    private class XOrder : Comparer<Point2D>
    {
      public override int Compare(Point2D p, Point2D q)
      {
        if (p.x < q.x) return -1;
        if (p.x > q.x) return +1;
        return 0;
      }
    }

    // compare points according to their y-coordinate
    private class YOrder : Comparer<Point2D>
    {
      public override int Compare(Point2D p, Point2D q)
      {
        if (p.y < q.y) return -1;
        if (p.y > q.y) return +1;
        return 0;
      }
    }

    // compare points according to their polar radius
    private class ROrder : Comparer<Point2D>
    {
      public override int Compare(Point2D p, Point2D q)
      {
        double delta = (p.x * p.x + p.y * p.y) - (q.x * q.x + q.y * q.y);
        if (delta < 0) return -1;
        if (delta > 0) return +1;
        return 0;
      }
    }

    // compare other points relative to atan2 angle (bewteen -pi/2 and pi/2) they make with this Point
    private class Atan2Order : Comparer<Point2D>
    {
      private Point2D point;
      public Atan2Order(Point2D parent)
      {
        point = parent;
      }
      public override int Compare(Point2D q1, Point2D q2)
      {
        double angle1 = point.angleTo(q1);
        double angle2 = point.angleTo(q2);
        if (angle1 < angle2) return -1;
        else if (angle1 > angle2) return +1;
        else return 0;
      }
    }

    // compare other points relative to polar angle (between 0 and 2pi) they make with this Point
    private class PolarOrder : Comparer<Point2D>
    {
      private Point2D point;
      public PolarOrder(Point2D parent)
      {
        point = parent;
      }
      public override int Compare(Point2D q1, Point2D q2)
      {
        double dx1 = q1.x - point.x;
        double dy1 = q1.y - point.y;
        double dx2 = q2.x - point.x;
        double dy2 = q2.y - point.y;

        if (dy1 >= 0 && dy2 < 0) return -1;    // q1 above; q2 below
        else if (dy2 >= 0 && dy1 < 0) return +1;    // q1 below; q2 above
        else if (dy1 == 0 && dy2 == 0)
        {            // 3-collinear and horizontal
          if (dx1 >= 0 && dx2 < 0) return -1;
          else if (dx2 >= 0 && dx1 < 0) return +1;
          else return 0;
        }
        else return -Ccw(point, q1, q2);     // both above or below

        // Note: ccw() recomputes dx1, dy1, dx2, and dy2
      }
    }

    // compare points according to their distance to this point
    private class DistanceToOrder : Comparer<Point2D>
    {
      private Point2D point;

      public DistanceToOrder(Point2D parent)
      {
        point = parent;
      }
      public override int Compare(Point2D p, Point2D q)
      {
        double dist1 = point.DistanceSquaredTo(p);
        double dist2 = point.DistanceSquaredTo(q);
        if (dist1 < dist2) return -1;
        else if (dist1 > dist2) return +1;
        else return 0;
      }
    }

    /// <summary>
    /// Compares this point to the specified point.</summary>
    /// <param name="other">the other point</param>
    /// <returns><c>true</c> if this point equals <c>other</c>;
    ///        <c>false</c> otherwise</returns>
    ///
    public override bool Equals(object other)
    {
      if (other == this) return true;
      if (other == null) return false;
      if (other.GetType() != this.GetType()) return false;
      Point2D that = (Point2D)other;
      return x == that.x && y == that.y;
    }

    /// <summary>
    /// Return a string representation of this point.</summary>
    /// <returns>a string representation of this point in the format (x, y)</returns>
    ///
    public override string ToString()
    {
      return "(" + x + ", " + y + ")";
    }

    /// <summary>
    /// Returns an integer hash code for this point.</summary>
    /// <returns>an integer hash code for this point</returns>
    ///
    public override int GetHashCode()
    {
      int hashX = x.GetHashCode();
      int hashY = y.GetHashCode();
      return 31 * hashX + hashY;
    }

    /// <summary>
    /// Plot this point using standard draw.</summary>
    ///
    public override void Draw()
    {
      if (Display != null)
        Display.DrawPoint(x, y);
    }

    /// <summary>
    /// Plot a line from this point to that point using standard draw.</summary>
    /// <param name="that">the other point</param>
    ///
    public void DrawTo(Point2D that)
    {
      if (Display != null)
        Display.DrawLine(x, y, that.x, that.y);
    }

    class Point2DWindow : DrawingWindow
    {
      const int MAX_WIDTH = 800;
      const int MAX_HEIGHT = 800;

      private int x0;
      private int y0;
      private int N;

      Point2D[] points;
      Point2D p;
      private int currentIndex;

      public Point2DWindow(int x0, int y0, int N)
      {
        Title = "Point2D Demo";

        this.x0 = x0;
        this.y0 = y0;
        this.N = N;

        // Set up the drawing surface (canvas)
        SetCanvasSize(MAX_WIDTH, MAX_HEIGHT);
        SetPercentScale(true);

        // Initialize the domain objects with scaled coordinates
        points = new Point2D[N];
        for (int i = 0; i < N; i++)
        {
          int x = StdRandom.Uniform(MAX_WIDTH + 1);
          int y = StdRandom.Uniform(MAX_HEIGHT + 1);
          points[i] = new Point2D(x, y) { Display = this };
          points[i].Draw();
        }
        SetPenColor(Colors.Red);
        SetPenThickness(3);
        p = new Point2D(x0, y0) { Display = this };
        p.Draw();

        SetPenThickness();
        SetPenColor(Colors.Green);
        Array.Sort(points, p.GetPolarOrder());
        currentIndex = 0; // make sure we start with the first point

        FrameUpdateHandler = Update; // atache a frame update handler
      }

      // Per-frame update
      public void Update(object sender, EventArgs e)
      {
        //Console.WriteLine("Current index: " + currentIndex);
        if (currentIndex < N)
          p.DrawTo(points[currentIndex++]);
      }
    }

    /// <summary>
    /// Demo test the point data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Point2D x0 y0 N", "x0, y0 and N number of points")]
    public static void MainTest(string[] args)
    {
      //if (args.Length != 4) throw new ArgumentException("Need start point x0, y0 and number of points, N");
      int x0 = int.Parse(args[0]);
      int y0 = int.Parse(args[1]);
      int N = int.Parse(args[2]);

      Application app = new Application();
      app.Run(new Point2DWindow(x0, y0, N));
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

