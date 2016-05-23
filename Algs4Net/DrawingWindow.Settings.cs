/******************************************************************************
 *  File name :    DrawingWindow.Settings.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 ******************************************************************************/

using System;
using System.Windows;
using System.Windows.Media;

namespace Algs4Net
{
  public partial class DrawingWindow : Window
  {

    /***************************************************************************
     *  Default drawing settings.
     ***************************************************************************/
    private static readonly Color DefaultPenColor   = Colors.Black;
    private static readonly Color DefaultClearColor = Colors.White;
    private static readonly FontFamily DefaultFont = new FontFamily("Comic Sans MS, Verdana");
    private static readonly double DefaultPenThickness = 2;

    /***************************************************************************
     *  Drawing settings.
     ***************************************************************************/

    private FontFamily font = DefaultFont;
    private Color penColor = DefaultPenColor;
    private double penThickness = DefaultPenThickness; // equivalent to pen diameter
    private bool isPercentScaled = false;

    /***************************************************************************
     *  User and screen coordinate systems.
     ***************************************************************************/
    /// <summary>
    /// Returns the curent font family
    /// </summary>
    /// <returns>the curent font family</returns>
    public FontFamily GetFont()
    {
      return font;
    }

    /// <summary>
    /// Resets the text font to the default font
    /// </summary>
    public void SetFont()
    {
      SetFont(DefaultFont);
    }

    /// <summary>
    /// Changes the text font to the desired font family
    /// </summary>
    /// <param name="font">the desired font family</param>
    public void SetFont(FontFamily font)
    {
      if (font == null) throw new ArgumentNullException("Font is null");
      this.font = font;
    }

    /// <summary>
    /// Returns the current pen/solid brush color
    /// </summary>
    /// <returns>the current pen color</returns>
    public Color GetPenColor()
    {
      return penColor;
    }

    /// <summary>
    /// Resets the the current pen/solid brush color to default (black)
    /// </summary>
    public void SetPenColor()
    {
      SetPenColor(DefaultPenColor);
    }

    /// <summary>
    /// Changes the the current pen/solid brush color to desired color
    /// using the <see cref="System.Windows.Media.Color"/> type.
    /// </summary>
    /// <param name="color">the desired color</param>
    public void SetPenColor(Color color)
    {
      penColor = color;
    }

    /// <summary>
    /// Changes the the current pen/solid brush color to desired color in RGB from
    /// </summary>
    /// <param name="red">the red byte</param>
    /// <param name="green">the green byte</param>
    /// <param name="blue">the blue byte</param>
    public void SetPenColor(int red, int green, int blue)
    {
      penColor = Color.FromRgb((byte)red, (byte)green, (byte)blue);
    }

    /// <summary>
    /// Returns the current pen thickness (2 * radius), or the line width</summary>
    /// <returns>the current value of the pen thickness</returns>
    ///
    public double GetPenThickness()
    {
      return penThickness;
    }

    /// <summary>
    /// Sets the pen size to the default size (2 Dpi).
    /// The pen is circular, so that lines have rounded ends. When you
    /// draw a point, you get a circle.</summary>
    ///
    public void SetPenThickness()
    {
      SetPenThickness(DefaultPenThickness);
    }

    /// <summary>
    /// Sets the pen size or line width to the desired size.
    /// The pen is circular, so that lines have rounded ends. When you
    /// draw a point, you get a circle.</summary>
    /// <param name="thickness">The desired line width</param>
    public void SetPenThickness(double thickness)
    {
      penThickness = thickness;
    }

    /// <summary>
    /// Clears the screen to the default color (white).</summary>
    ///
    public void Clear()
    {
      Clear(DefaultClearColor);
    }

    /// <summary>
    /// Clears the screen to the specified color.</summary>
    /// <param name="color">the color to make the background</param>
    ///
    public void Clear(Color color)
    {
      canvas.Clear();
      canvas.Background = new SolidColorBrush(color);
    }

    /// <summary>
    /// If set, the input coordinate will be in the range <c>0.0 - 1.0</c>. If
    /// an input coordinate is greater than 1, this setting will not take effect
    /// </summary>
    /// <param name="useScale">if true, use the [0.0, 1.9) scale; if false, use the raw coordinate</param>
    public void SetPercentScale(bool useScale)
    {
      isPercentScaled = useScale;
    }

    // helper functions that scale from user coordinates to screen coordinates and back
    private bool usePercent(double x)
    {
      if (!isPercentScaled || Math.Abs(x) > 1.0) return false;
      return true;
    }
    private double scaleX(double x) { return (usePercent(x) ? canvas.Width * x : x); }
    private double scaleY(double y) { return (usePercent(y) ? canvas.Height * y : y); }
    private double factorX(double w) { return (usePercent(w) ? w * canvas.Width : w); }
    private double factorY(double h) { return (usePercent(h) ? h * canvas.Height : h); }
    private double userX(double x) { return (usePercent(x) ? x / canvas.Width : x); }
    private double userY(double y) { return (usePercent(y) ? y / canvas.Height : y); }

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
