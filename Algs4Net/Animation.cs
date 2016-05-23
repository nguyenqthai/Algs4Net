/******************************************************************************
 *  File name :    DrawingWindow.Init.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 ******************************************************************************/

// Common references
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary>
  /// Demonstrates basic drawing and animation capabilities using the current
  /// version of <see cref="DrawingWindow"/>. The class could be marked 
  /// internal as it has no use outside the 
  /// <see cref="Animation.MainTest(string[])"/> method.
  /// </summary>
  public class Animation : DrawingWindow
  {
    const int CanvasWidth = 500;
    const int CanvasHeight = 500;
    const int MAX_BALLS = 15;
    const int BALL_RADIUS = 8;

    Random rnd = new Random();
    private List<Ball> balls = new List<Ball>();

    // an example use of the BasicVisual class
    private class Ball : BasicVisual
    {
      public double X { get; set; }
      public double Y { get; set; }

      // a derived member/method/property
      public double Radius { get; set; }

      // the method derived classes have to override
      public override void Draw()
      {
        Debug.Assert(Display != null, "Remember to set the drawing target");
        if (Visual != null)
          Display.DrawFilledCircle(Visual, X, Y, Radius);
        else
          Visual = Display.DrawFilledCircle(X, Y, Radius);
      }
    }

    /// <summary>
    /// Sets up the window, draws a background and attach a handler for frame-based animation
    /// </summary>
    public Animation()
    {
      Title = "Animation Demo";

      // Set up the drawing surface (canvas)
      SetCanvasSize(CanvasWidth, CanvasHeight);

      // Draw the background
      SetPercentScale(true);
      SetPenColor(Colors.Orange);
      DrawRectangle(0.64, 0.30, 0.34, 0.25);
      DrawFilledRectangle(0.42, 0.28, 0.10, 0.2);
      DrawSquare(0.75, 0.28, 0.2);
      SetPenColor(Colors.Green);
      DrawFilledSquare(0.68, 0.15, 0.05);

      SetPenColor(Colors.Cyan);
      DrawCircle(0.1, 0.1, 0.1);
      DrawFilledCircle(0.1, 0.1, 0.05);

      double[] px = { 0.15, 0.30, 0.25, 0.05, 0.0 };
      double[] py = { 0.65, 0.70, 0.80, 0.80, 0.70 };
      SetPenColor(Colors.Orange);
      DrawFilledPolygon(px, py);
      SetPenColor(Colors.Gold);
      DrawFilledEllipse(0.15, 0.73, 0.10, 0.05);

      SetFont(new FontFamily("Arial"));
      SetPenColor(Colors.Red);
      DrawText(0.42, 0.85, "Introduction to Algorithms", 18, true);
      DrawText(0.42, 0.85, "Introduction to Algorithms", 18, -45);

      SetPenColor(Colors.LightSeaGreen);
      DrawFilledSquare(0.8, 0.76, 0.05);
      SetPenColor(Colors.DarkGreen);
      SetPenThickness(7);
      DrawCircle(0.8, 0.76, 0.1);

      SetPenThickness();
      SetPercentScale(false);
      
      // Initialize the domain objects with unscaled coordinates
      SetPenColor(Colors.DarkBlue);
      for (int i = 0; i < MAX_BALLS; i++)
      {
        double x = rnd.Next(0, (int)CanvasWidth);
        double y = rnd.Next(0, (int)CanvasHeight);
        balls.Add(new Ball() { X = x, Y = y, Radius = BALL_RADIUS, Display = this });
        balls[i].Draw();
      }

      FrameUpdateHandler = Update; // atache a frame update handler
    }

    /// <summary>
    /// The event handler to support frame-based animation
    /// </summary>
    /// <param name="sender">the window host</param>
    /// <param name="e">event argument, usually ignored</param>
    public void Update(object sender, EventArgs e)
    {
      ShowFrame(20);
      // domain update per frame
      for (int i = 0; i < MAX_BALLS; i++)
      {
        double newX = balls[i].X + (int)rnd.Next(-2, 2);
        double newY = balls[i].Y + (int)rnd.Next(-2, 2);
        Size sz = GetCanvasSize();
        if (newX < 0) newX = sz.Width + (int)rnd.Next(-2, 0);
        if (newY < 0) newY = sz.Height + (int)rnd.Next(-2, 0);
        balls[i].X = newX;
        balls[i].Y = newY;

        balls[i].Draw(); // render new position
      }
    }

    /// <summary>
    /// Demo test the Animation data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    public static void MainTest(string[] args)
    {
      Application app = new Application();
      app.Run(new Animation());
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
