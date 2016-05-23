/******************************************************************************
 *  File name :    DrawingWindow.Draw.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 ******************************************************************************/

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DrawingWindow</c> class provides a basic capability for
  /// creating drawings in a .NET environment using Windows Presentation
  /// Foundation (WPF) classes. It allows you to create drawings consisting of 
  /// points, lines, squares, circles, and other geometric shapes in a 
  /// window to save the drawings to a file. The class also includes
  /// facilities for text, color, pictures, and simple animation.</para>
  /// <para>The coordinate system follows Windows convention, meaning that
  /// an x,y coordinate starts from the top-left corner, with the y axis
  /// oriented downward.</para>
  /// <para>The API is modeled after the 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/StdDraw.java.html">StdDraw</a>
  /// class with necessary adaptation for the Windows environment.</para>
  /// </summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://introcs.cs.princeton.edu/15inout">Section 1.5</a> of
  /// <c>Introduction to Programming in Java: An Interdisciplinary Approach</c>
  /// by Robert Sedgewick and Kevin Wayne.</para></remarks>
  ///
  public partial class DrawingWindow : Window
  {

    /***************************************************************************
    *  Drawing geometric shapes.
    ***************************************************************************/

    /// <summary>
    /// Draws a line segment between (<c>X</c><sub>0</sub>, <c>Y</c><sub>0</sub>) and
    /// (<c>X</c><sub>1</sub>, <c>Y</c><sub>1</sub>).</summary>
    /// <param name="x0">the <c>X</c>-coordinate of one endpoint</param>
    /// <param name="y0">the <c>Y</c>-coordinate of one endpoint</param>
    /// <param name="x1">the <c>X</c>-coordinate of the other endpoint</param>
    /// <param name="y1">the <c>Y</c>-coordinate of the other endpoint</param>
    /// <returns>A drawing visual as handle to the drawing</returns>
    ///
    public DrawingVisual DrawLine(double x0, double y0, double x1, double y1)
    {
      DrawingVisual v = new DrawingVisual();
      DrawLine(v, x0, y0, x1, y1);
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a line segment between (<c>X</c><sub>0</sub>, <c>Y</c><sub>0</sub>) and
    /// (<c>X</c><sub>1</sub>, <c>Y</c><sub>1</sub>).</summary>
    /// <param name="v">The rendered visual</param>
    /// <param name="x0">the <c>X</c>-coordinate of one endpoint</param>
    /// <param name="y0">the <c>Y</c>-coordinate of one endpoint</param>
    /// <param name="x1">the <c>X</c>-coordinate of the other endpoint</param>
    /// <param name="y1">the <c>Y</c>-coordinate of the other endpoint</param>
    ///
    public void DrawLine(DrawingVisual v, double x0, double y0, double x1, double y1)
    {
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        dc.DrawLine(new Pen(brush, penThickness), new Point(scaleX(x0), scaleY(y0)), new Point(scaleX(x1), scaleY(y1)));
      }
    }

    /// <summary>
    /// Draws a point centered at (<c>X</c>, <c>Y</c>).
    /// The point is a filled circle whose radius is equal to half the pen thickness.
    /// </summary>
    /// <param name="x">the <c>X</c>-coordinate of the point</param>
    /// <param name="y">the <c>Y</c>-coordinate of the point</param>
    /// <returns>A drawing visual as handle to the drawing</returns>
    ///
    public DrawingVisual DrawPoint(double x, double y)
    {
      DrawingVisual v = new DrawingVisual();
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        dc.DrawEllipse(brush, new Pen(brush, penThickness), new Point(scaleX(x), scaleY(y)), penThickness / 2, penThickness / 2);
      }
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a circle of the specified radius, centered at (<c>X</c>, <c>Y</c>).
    /// </summary>
    /// <param name="x">the <c>X</c>-coordinate of the center of the circle</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the circle</param>
    /// <param name="radius">the radius of the circle</param>
    /// <exception cref="ArgumentException">if {@code radius} is negative</exception>
    /// <returns>A drawing visual as handle to the drawing</returns>
    ///
    public DrawingVisual DrawCircle(double x, double y, double radius)
    {
      DrawingVisual v = new DrawingVisual();
      DrawCirle(v, x, y, radius);
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a circle of the specified radius, centered at (<c>X</c>, <c>Y</c>).
    /// </summary>
    /// <param name="v">The rendered visual</param>
    /// <param name="x">the <c>X</c>-coordinate of the center of the circle</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the circle</param>
    /// <param name="radius">the radius of the circle</param>
    /// <exception cref="ArgumentException">if {@code radius} is negative</exception>
    ///
    public void DrawCirle(DrawingVisual v, double x, double y, double radius)
    {
      if (!(radius >= 0)) throw new ArgumentException("radius must be nonnegative");

      double xs = scaleX(x);
      double ys = scaleY(y);
      double rs = factorX(radius);

      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        dc.DrawEllipse(null, new Pen(brush, penThickness), new Point(xs, ys), rs, rs);
      }
    }

    /// <summary>
    /// Draws a filled circle of the specified radius, centered at (<c>X</c>, <c>Y</c>).
    /// </summary>
    /// <param name="x">the <c>X</c>-coordinate of the center of the circle</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the circle</param>
    /// <param name="radius">the radius of the circle</param>
    /// <exception cref="ArgumentException">if {@code radius} is negative</exception>
    /// <returns>A drawing visual as handle to the drawing</returns>
    ///
    public DrawingVisual DrawFilledCircle(double x, double y, double radius)
    {
      DrawingVisual v = new DrawingVisual();
      DrawFilledCircle(v, x, y, radius);
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a filled circle of the specified radius, centered at (<c>X</c>, <c>Y</c>).
    /// </summary>
    /// <param name="v">The rendered visual</param>
    /// <param name="x">the <c>X</c>-coordinate of the center of the circle</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the circle</param>
    /// <param name="radius">the radius of the circle</param>
    /// <exception cref="ArgumentException">if radius is negative</exception>
    ///
    public void DrawFilledCircle(DrawingVisual v, double x, double y, double radius)
    {
      if (!(radius >= 0)) throw new ArgumentException("radius must be nonnegative");

      double xs = scaleX(x);
      double ys = scaleY(y);
      double rs = factorX(radius);

      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        dc.DrawEllipse(brush, new Pen(brush, penThickness), new Point(xs, ys), rs, rs);
      }
    }

    /// <summary>
    /// Draws an ellipse with the specified semimajor and semiminor axes,
    /// centered at (<c>X</c>, <c>Y</c>).</summary>
    /// <param name="x">the <c>X</c>-coordinate of the center of the ellipse</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the ellipse</param>
    /// <param name="semiMajorAxis">is the semimajor axis of the ellipse</param>
    /// <param name="semiMinorAxis">is the semiminor axis of the ellipse</param>
    /// <exception cref="ArgumentException">if either axis is negative</exception>
    /// <returns>A drawing visual as handle to the drawing</returns>
    ///
    public DrawingVisual DrawEllipse(double x, double y, double semiMajorAxis, double semiMinorAxis)
    {
      DrawingVisual v = new DrawingVisual();
      DrawEllipse(v, x, y, semiMajorAxis, semiMinorAxis);
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws an ellipse with the specified semimajor and semiminor axes,
    /// centered at (<c>X</c>, <c>Y</c>).</summary>
    /// <param name="v">The rendered visual</param>
    /// <param name="x">the <c>X</c>-coordinate of the center of the ellipse</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the ellipse</param>
    /// <param name="semiMajorAxis">is the semimajor axis of the ellipse</param>
    /// <param name="semiMinorAxis">is the semiminor axis of the ellipse</param>
    /// <exception cref="ArgumentException">if either axis is negative</exception>
    ///
    public void DrawEllipse(DrawingVisual v, double x, double y, double semiMajorAxis, double semiMinorAxis)
    {
      if (!(semiMajorAxis >= 0)) throw new ArgumentException("ellipse semimajor axis must be nonnegative");
      if (!(semiMinorAxis >= 0)) throw new ArgumentException("ellipse semiminor axis must be nonnegative");
      double xs = scaleX(x);
      double ys = scaleY(y);
      double ws = factorX(semiMajorAxis);
      double hs = factorY(semiMinorAxis);

      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        dc.DrawEllipse(null, new Pen(brush, penThickness), new Point(xs, ys), ws, hs);
      }
    }

    /// <summary>
    /// Draws a filled ellipse with the specified semimajor and semiminor axes,
    /// centered at (<c>X</c>, <c>Y</c>).</summary>
    /// <param name="x">the <c>X</c>-coordinate of the center of the ellipse</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the ellipse</param>
    /// <param name="semiMajorAxis">is the semimajor axis of the ellipse</param>
    /// <param name="semiMinorAxis">is the semiminor axis of the ellipse</param>
    /// <exception cref="ArgumentException">if either axis is negative</exception>
    /// <returns>A drawing visual as handle to the drawing</returns>
    ///
    public DrawingVisual DrawFilledEllipse(double x, double y, double semiMajorAxis, double semiMinorAxis)
    {
      DrawingVisual v = new DrawingVisual();
      DrawFilledEllipse(v, x, y, semiMajorAxis, semiMinorAxis);
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a filled ellipse with the specified semimajor and semiminor axes,
    /// centered at (<c>X</c>, <c>Y</c>).</summary>
    /// <param name="v">The rendered visual</param>
    /// <param name="x">the <c>X</c>-coordinate of the center of the ellipse</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the ellipse</param>
    /// <param name="semiMajorAxis">is the semimajor axis of the ellipse</param>
    /// <param name="semiMinorAxis">is the semiminor axis of the ellipse</param>
    /// <exception cref="ArgumentException">if either axis is negative</exception>
    ///
    public void DrawFilledEllipse(DrawingVisual v, double x, double y, double semiMajorAxis, double semiMinorAxis)
    {
      if (!(semiMajorAxis >= 0)) throw new ArgumentException("ellipse semimajor axis must be nonnegative");
      if (!(semiMinorAxis >= 0)) throw new ArgumentException("ellipse semiminor axis must be nonnegative");
      double xs = scaleX(x);
      double ys = scaleY(y);
      double ws = factorX(semiMajorAxis);
      double hs = factorY(semiMinorAxis);

      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        dc.DrawEllipse(brush, new Pen(brush, penThickness), new Point(xs, ys), ws, hs);
      }
    }

    /// <summary>
    /// Draws a circular arc of the specified radius,
    /// centered at (<c>X</c>, <c>Y</c>), from angle1 to angle2 (in degrees) 
    /// <em>clockwise</em>. An angle of 0 would means an arc start at 3 o'clock
    /// </summary>
    /// <param name="x">the <c>X</c>-coordinate of the center of the circle</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the circle</param>
    /// <param name="radius">the radius of the circle</param>
    /// <param name="angle1">the starting angle. 0 would mean an arc beginning at 3 o'clock.</param>
    /// <param name="angle2">the angle at the end of the arc. For example, if
    ///  you want a 90 degree arc, then angle2 should be angle1 + 90.</param>
    /// <exception cref="ArgumentException">if {@code radius} is negative</exception>
    /// <returns>A drawing visual as handle to the drawing</returns>
    ///
    public DrawingVisual DrawArc(double x, double y, double radius, double angle1, double angle2)
    {
      if (radius < 0) throw new ArgumentException("arc radius must be nonnegative");
      while (angle2 < angle1) angle2 += 360;
      double rX = factorX(radius);
      double rY = factorY(radius);

      double x1 = scaleX(x) + rX * Math.Cos((angle1 * Math.PI) / 180);
      double y1 = scaleY(y) + rY * Math.Sin((angle1 * Math.PI) / 180);
      double x2 = scaleX(x) + rX * Math.Cos((angle2 * Math.PI) / 180);
      double y2 = scaleY(y) + rY * Math.Sin((angle2 * Math.PI) / 180);

      PathFigure drawing = new PathFigure() { IsClosed = false, StartPoint = new Point(x1, y1) };

      ArcSegment arc = new ArcSegment()
      {
        Point = new Point(x2, y2),
        Size = new Size(rX, rY),
        SweepDirection = SweepDirection.Clockwise
      };
      drawing.Segments.Add(arc);

      PathGeometry geomery = new PathGeometry();
      geomery.Figures.Add(drawing);

      DrawingVisual v = new DrawingVisual();
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        dc.DrawGeometry(null, new Pen(brush, penThickness), geomery);
      }
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a polygon with the vertices
    /// (<c>X</c><sub>0</sub>, <c>Y</c><sub>0</sub>),
    /// (<c>X</c><sub>1</sub>, <c>Y</c><sub>1</sub>), ...,
    /// (<c>X</c><sub><c>N</c>-1</sub>, <c>Y</c><sub><c>N</c>-1</sub>).</summary>
    /// <param name="x">an array of all the <c>X</c>-coordinates of the polygon</param>
    /// <param name="y">an array of all the <c>Y</c>-coordinates of the polygon</param>
    /// <exception cref="ArgumentException">unless x[] and y[] are of 
    /// the same length</exception>
    /// <returns>A drawing visual as handle to the drawing</returns>
    /// 
    public DrawingVisual DrawPolygon(double[] x, double[] y)
    {
      if (x == null) throw new ArgumentNullException("x-coordinate array is null");
      if (y == null) throw new ArgumentNullException("y-coordinate array is null");
      if (x.Length != y.Length) throw new ArgumentException("arrays must be of the same length");

      PathGeometry geomery = CreatePolylinePath(x, y, false);

      DrawingVisual v = new DrawingVisual();
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        dc.DrawGeometry(null, new Pen(brush, penThickness), geomery);
      }
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a filled polygon with the vertices
    /// (<c>X</c><sub>0</sub>, <c>Y</c><sub>0</sub>),
    /// (<c>X</c><sub>1</sub>, <c>Y</c><sub>1</sub>), ...,
    /// (<c>X</c><sub><c>N</c>-1</sub>, <c>Y</c><sub><c>N</c>-1</sub>).</summary>
    /// <param name="x">an array of all the <c>X</c>-coordinates of the polygon</param>
    /// <param name="y">an array of all the <c>Y</c>-coordinates of the polygon</param>
    /// <exception cref="ArgumentException">unless x[] and y[] are of 
    /// the same length</exception>
    /// <returns>A drawing visual as handle to the drawing</returns>
    ///
    public DrawingVisual DrawFilledPolygon(double[] x, double[] y)
    {
      if (x == null) throw new ArgumentNullException("x-coordinate array is null");
      if (y == null) throw new ArgumentNullException("y-coordinate array is null");
      if (x.Length != y.Length) throw new ArgumentException("arrays must be of the same length");

      PathGeometry geomery = CreatePolylinePath(x, y, true);

      DrawingVisual v = new DrawingVisual();
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        dc.DrawGeometry(brush, new Pen(brush, penThickness), geomery);
      }
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a rectangle of the specified size, centered at (<c>X</c>, <c>Y</c>).</summary>
    /// <param name="x">the <c>X</c>-coordinate of the center of the rectangle</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the rectangle</param>
    /// <param name="halfWidth">one half the width of the rectangle</param>
    /// <param name="halfHeight">one half the height of the rectangle</param>
    /// <exception cref="ArgumentException">if either halfWidth or halfHeight is negative</exception>
    /// <returns>A drawing visual as handle to the drawing</returns>
    ///
    public DrawingVisual DrawRectangle(double x, double y, double halfWidth, double halfHeight)
    {
      if (!(halfWidth >= 0)) throw new ArgumentException("half width must be nonnegative");
      if (!(halfHeight >= 0)) throw new ArgumentException("half height must be nonnegative");

      double xs = scaleX(x);
      double ys = scaleY(y);
      double ws = factorX(halfWidth * 2);
      double hs = factorY(halfHeight * 2);

      DrawingVisual v = new DrawingVisual();
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        Rect rect = new Rect(new Point(xs - ws / 2, ys - hs / 2), new Size(ws, hs));
        dc.DrawRectangle(null, new Pen(brush, penThickness), rect);
      }
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a filled rectangle of the specified size, centered at (<c>X</c>, <c>Y</c>).</summary>
    /// <param name="x">the <c>X</c>-coordinate of the center of the rectangle</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the rectangle</param>
    /// <param name="halfWidth">one half the width of the rectangle</param>
    /// <param name="halfHeight">one half the height of the rectangle</param>
    /// <exception cref="ArgumentException">if either halfWidth or halfHeight is negative</exception>
    /// <returns>A drawing visual as handle to the drawing</returns>
    ///
    public DrawingVisual DrawFilledRectangle(double x, double y, double halfWidth, double halfHeight)
    {
      if (!(halfWidth >= 0)) throw new ArgumentException("half width must be nonnegative");
      if (!(halfHeight >= 0)) throw new ArgumentException("half height must be nonnegative");

      double xs = scaleX(x);
      double ys = scaleY(y);
      double ws = factorX(halfWidth * 2);
      double hs = factorY(halfHeight * 2);

      DrawingVisual v = new DrawingVisual();
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        Rect rect = new Rect(new Point(xs - ws / 2, ys - hs / 2), new Size(ws, hs));
        dc.DrawRectangle(brush, new Pen(brush, penThickness), rect);
      }
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a square of the specified size, centered at (<c>X</c>, <c>Y</c>).</summary>
    /// <param name="x">the <c>X</c>-coordinate of the center of the square</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the square</param>
    /// <param name="halfLength">one half the side of the square</param>
    /// <exception cref="ArgumentException">if halfLength is negative</exception>
    /// <returns>A drawing visual as handle to the drawing</returns>
    ///
    public DrawingVisual DrawSquare(double x, double y, double halfLength)
    {
      if (!(halfLength >= 0)) throw new ArgumentException("half length must be nonnegative");
      double xs = scaleX(x);
      double ys = scaleY(y);
      double ws = factorX(halfLength * 2);

      DrawingVisual v = new DrawingVisual();
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        Rect rect = new Rect(new Point(xs - ws / 2, ys - ws / 2), new Size(ws, ws));
        dc.DrawRectangle(null, new Pen(brush, penThickness), rect);
      }
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a filled square of the specified size, centered at (<c>X</c>, <c>Y</c>).</summary>
    /// <param name="x">the <c>X</c>-coordinate of the center of the square</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the square</param>
    /// <param name="halfLength">one half the side of the square</param>
    /// <exception cref="ArgumentException">if halfLength is negative</exception>
    /// <returns>A drawing visual as handle to the drawing</returns>
    /// 
    public DrawingVisual DrawFilledSquare(double x, double y, double halfLength)
    {
      if (!(halfLength >= 0)) throw new ArgumentException("half length must be nonnegative");
      double xs = scaleX(x);
      double ys = scaleY(y);
      double ws = factorX(halfLength * 2);

      DrawingVisual v = new DrawingVisual();
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        Rect rect = new Rect(new Point(xs - ws / 2, ys - ws / 2), new Size(ws, ws));
        dc.DrawRectangle(brush, new Pen(brush, penThickness), rect);
      }
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Loads and draws a picture centered at (<c>X</c>, <c>Y</c>) within 
    /// the specified rectangle with sizes in <c>desiredWidth</c> and 
    /// <c>desiredHeight</c>. The picture will be scaled to fit the rectangle
    /// </summary>
    /// <param name="x">the <c>X</c>-coordinate of the center of the picture</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the picture</param>
    /// <param name="filename">the file name, relative or absolute path or an URL</param>
    /// <param name="desiredWidth">the width of the rendered picture</param>
    /// <param name="desiredHeight">the height of the rendered picture</param>
    /// <returns>A drawing visual as handle to the drawing</returns>
    public DrawingVisual DrawPicture(double x, double y, string filename, double desiredWidth, double desiredHeight)
    {
      BitmapImage image = GetImage(filename);
      if (image == null) return null;

      double ws = factorX(desiredWidth);
      double hs = factorY(desiredHeight);
      double xs = scaleX(x);
      double ys = scaleY(y);

      DrawingVisual v = new DrawingVisual();
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        Rect rect = new Rect(new Point(xs - ws / 2, ys - hs / 2), new Size(ws, ws));
        dc.DrawImage(image, rect);
      }
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Loads and draws a picture centered at (<c>X</c>, <c>Y</c>) within 
    /// the specified rectangle with sizes in <c>desiredWidth</c> and 
    /// <c>desiredHeight</c>. The picture will be scaled to fit the rectangle
    /// </summary>
    /// <param name="x">the <c>X</c>-coordinate of the center of the picture</param>
    /// <param name="y">the <c>Y</c>-coordinate of the center of the picture</param>
    /// <param name="filename">the file name, relative or absolute path or an URL</param>
    /// <param name="desiredWidth">the width of the rendered picture</param>
    /// <param name="desiredHeight">the height of the rendered picture</param>
    /// <param name="degrees">the degree to rotate around the center</param>
    /// <returns>A drawing visual as handle to the drawing</returns>
    /// 
    public DrawingVisual DrawPicture(double x, double y, string filename, double desiredWidth, double desiredHeight, double degrees)
    {
      BitmapImage image = GetImage(filename);
      if (image == null) return null;

      double ws = factorX(desiredWidth);
      double hs = factorY(desiredHeight);
      double xs = scaleX(x);
      double ys = scaleY(y);

      DrawingVisual v = new DrawingVisual();      
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        Rect rect = new Rect(new Point(xs - ws / 2, ys - hs /2), new Size(ws, ws));
        dc.DrawImage(image, rect);
      }
      v.Transform = new RotateTransform(degrees, xs, ys);
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a text using the current font starting at (<c>X</c>, <c>Y</c>) 
    /// given the desired font size. The text is defaulted to flow from left
    /// to right can be specified to flow from right to left.
    /// </summary>
    /// <param name="x">the starting <c>X</c>-coordinate of the text</param>
    /// <param name="y">the starting <c>Y</c>-coordinate of the text</param>
    /// <param name="text">the text to draw</param>
    /// <param name="fontSize">font size as used in a word processor</param>
    /// <param name="isRightToLeft">default to left to right flow</param>
    /// <returns>A drawing visual as handle to the drawing</returns>
    public DrawingVisual DrawText(double x, double y, string text, double fontSize, bool isRightToLeft = false)
    {
      double xs = scaleX(x);
      double ys = scaleY(y);

      DrawingVisual v = new DrawingVisual();
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        FlowDirection dir = (isRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight);
        FormattedText fmtText = GetFormattedText(text, fontSize, dir);
        dc.DrawText(fmtText, new Point(xs, ys));
      }
      canvas.AddVisual(v);
      return v;
    }

    /// <summary>
    /// Draws a text using the current font starting at (<c>X</c>, <c>Y</c>) 
    /// given the desired font size. The text is defaulted to flow from left
    /// to right can be specified to flow from right to left.
    /// </summary>
    /// <param name="x">the starting <c>X</c>-coordinate of the text</param>
    /// <param name="y">the starting <c>Y</c>-coordinate of the text</param>
    /// <param name="text">the text to draw</param>
    /// <param name="fontSize">font size as used in a word processor</param>
    /// <param name="degrees">the degree to rotate around the starting point</param>
    /// <param name="isRightToLeft">default to left to right flow</param>
    /// <returns>A drawing visual as handle to the drawing</returns>
    public DrawingVisual DrawText(double x, double y, string text, double fontSize, double degrees, bool isRightToLeft = false)
    {
      double xs = scaleX(x);
      double ys = scaleY(y);

      DrawingVisual v = new DrawingVisual();
      using (DrawingContext dc = v.RenderOpen())
      {
        Brush brush = new SolidColorBrush(penColor);
        FlowDirection dir = (isRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight);
        FormattedText fmtText = GetFormattedText(text, fontSize, dir);
        dc.DrawText(fmtText, new Point(xs, ys));
      }
      v.Transform = new RotateTransform(degrees, xs, ys);
      canvas.AddVisual(v);
      return v;
    }

    /***************************************************************************
     *  Managing visuals
     ***************************************************************************/

    /// <summary>
    /// Removes a visual from the canvas
    /// </summary>
    /// <param name="visual">The visual to remove</param>
    public void DeleteVisual(DrawingVisual visual)
    {
      canvas.DeleteVisual(visual);
    }

    /***************************************************************************
     *  Helper methods
     ***************************************************************************/
    private PathGeometry CreatePolylinePath(double[] x, double[] y, bool isFilled)
    {
      int n = x.Length;

      PathFigure drawing = new PathFigure() {
        IsClosed = true, IsFilled = isFilled, StartPoint = new Point(scaleX(x[0]), scaleY(y[0]))
      };

      for (int i = 1; i < n; i++)
      {
        Point p = new Point(scaleX(x[i]), scaleY(y[i]));
        LineSegment line = new LineSegment() { Point = p };
        drawing.Segments.Add(line);
      }

      PathGeometry geomery = new PathGeometry();
      geomery.Figures.Add(drawing);

      return geomery;
    }

    // get an image from the given fileName
    private static BitmapImage GetImage(string fileName)
    {
      if (fileName == null) throw new ArgumentNullException();

      BitmapImage bitMap = null;
      try
      {
        // Create source.
        Uri uri = new Uri(fileName, UriKind.RelativeOrAbsolute);
        bitMap = new BitmapImage(uri);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(string.Format("Loading error: {0}", ex.Message));
      }

      return bitMap;
    }

    // Private method to create FormattedText object.
    private FormattedText GetFormattedText(string str, double em, FlowDirection direction)
    {
      //FormattedText fmtText = new FormattedText()
      Brush brush = new SolidColorBrush(penColor);
      Typeface face = new Typeface("");
      return new FormattedText(str, CultureInfo.CurrentUICulture,
                      direction, face, em, brush);
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
