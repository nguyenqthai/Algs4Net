/******************************************************************************
 *  File name :    Interval2D.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  2-dimensional interval data type.
 *  
 *  C:\> algscmd Interval2D 0.1 0.8 0.2 0.7 20
 *  Interval2D 0.1 0.8 0.2 0.7 20
 *  5 Hits
 *  Box area = 0.35
 *
 ******************************************************************************/
using System;
using System.Windows;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Interval2D</c> class represents a closed two-dimensional interval,
  /// which represents all points (x, y) with both xmin &lt;= x &lt;= xmax and
  /// ymin &lt;= y &lt;= ymax.
  /// Two-dimensional intervals are immutable: their values cannot be changed
  /// after they are created.
  /// The class <c>Interval2D</c> includes methods for checking whether
  /// a two-dimensional interval contains a point and determining whether
  /// two two-dimensional intervals intersect.</summary>
  /// <remarks><para>For additional documentation, 
  /// see <a href="http://algs4.cs.princeton.edu/12oop">Section 1.2</a> of 
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Interval2D.java.html">Interval2D</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Interval2D : BasicVisual
  {
    private readonly Interval1D x;
    private readonly Interval1D y;

    /// <summary>
    /// Initializes a two-dimensional interval.</summary>
    /// <param name="x">the one-dimensional interval of x-coordinates</param>
    /// <param name="y">the one-dimensional interval of y-coordinates</param>
    ///
    public Interval2D(Interval1D x, Interval1D y)
    {
      this.x = x;
      this.y = y;
    }

    /// <summary>
    /// Does this two-dimensional interval intersect that two-dimensional interval?</summary>
    /// <param name="that">the other two-dimensional interval</param>
    /// <returns>true if this two-dimensional interval intersects
    ///   that two-dimensional interval; false otherwise</returns>
    ///
    public bool Intersects(Interval2D that)
    {
      if (!this.x.Intersects(that.x)) return false;
      if (!this.y.Intersects(that.y)) return false;
      return true;
    }

    /// <summary>
    /// Does this two-dimensional interval contain the point p?</summary>
    /// <param name="p">the two-dimensional point</param>
    /// <returns>true if this two-dimensional interval contains the point p; false otherwise</returns>
    ///
    public bool Contains(Point2D p)
    {
      return x.Contains(p.X) && y.Contains(p.Y);
    }

    /// <summary>
    /// Returns the area of this two-dimensional interval.</summary>
    /// <returns>the area of this two-dimensional interval</returns>
    ///
    public double Area()
    {
      return x.Length * y.Length;
    }

    /// <summary>
    /// Returns a string representation of this two-dimensional interval.</summary>
    /// <returns>a string representation of this two-dimensional interval
    ///   in the form [xmin, xmax] x [ymin, ymax]</returns>
    ///
    public override string ToString()
    {
      return x + " x " + y;
    }

    /// <summary>
    /// Does this interval equal the other interval?</summary>
    /// <param name="other">the other interval</param>
    /// <returns>true if this interval equals the other interval; false otherwise</returns>
    ///
    public override bool Equals(object other)
    {
      if (other == this) return true;
      if (other == null) return false;
      if (other.GetType() != this.GetType()) return false;
      Interval2D that = (Interval2D)other;
      return this.x.Equals(that.x) && this.y.Equals(that.y);
    }

    /// <summary>
    /// Returns an integer hash code for this interval.</summary>
    /// <returns>an integer hash code for this interval </returns>
    ///
    public override int GetHashCode()
    {
      int hash1 = x.GetHashCode();
      int hash2 = y.GetHashCode();
      return 31 * hash1 + hash2;
    }

    /// <summary>
    /// Draws this two-dimensional interval to the display.</summary>
    ///
    public override void Draw()
    {
      if (Display != null)
      {
        double xc = (x.Min + x.Max) / 2.0;
        double yc = (y.Min + y.Max) / 2.0;
        Display.DrawRectangle(xc, yc, x.Length / 2.0, y.Length / 2.0);
      }
    }

    class Interval2DWindow : DrawingWindow
    {
      public Interval2DWindow(double xlo, double xhi, double ylo, double yhi, int T)
      {
        SetPercentScale(true);

        Interval1D xinterval = new Interval1D(xlo, xhi);
        Interval1D yinterval = new Interval1D(ylo, yhi);
        Interval2D box = new Interval2D(xinterval, yinterval) { Display = this };
        box.Draw();

        Counter counter = new Counter("Hits");
        for (int t = 0; t < T; t++)
        {
          double x = StdRandom.Uniform(0.0, 1.0);
          double y = StdRandom.Uniform(0.0, 1.0);
          Point2D p = new Point2D(x, y) { Display = this };

          if (box.Contains(p)) counter.Increment();
          else p.Draw();
        }

        Console.WriteLine(counter);
        Console.WriteLine("Box area = {0:F2}", box.Area());
      }
    }

    /// <summary>
    /// Demo test the <c>Interval2D</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd xlo xhi ylo yhi", "x, y coordinates in the range of [0, 1)")]
    public static void MainTest(string[] args)
    {
      double xlo = double.Parse(args[0]);
      double xhi = double.Parse(args[1]);
      double ylo = double.Parse(args[2]);
      double yhi = double.Parse(args[3]);

      if ((xlo > 1.0) || (xhi > 1.0) || (ylo > 1.0) || (yhi > 1.0))
      {
        Console.WriteLine("Info: coordinates need to be in percent. Enter your data again");
        return;
      }
      int T = int.Parse(args[4]);
      Application app = new Application();
      app.Run(new Interval2DWindow(xlo, xhi, ylo, yhi, T));
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
