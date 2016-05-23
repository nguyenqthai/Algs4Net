/******************************************************************************
 *  File name :    GrahamScan.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Create points from standard input and compute the convex hull using
 *  Graham scan algorithm.
 *
 *  NOTE: May be floating-point issues if x- and y-coordinates are not integers.
 *  
 *  C:\> algscmd GrahamScan
 *  6
 *  1 2
 *  3 4
 *  5 6
 *  8 9
 *  9 9
 *  10 1
 *  (10, 1)
 *  (9, 9)
 *  (8, 9)
 *  (1, 2)
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>GrahamScan</c> data type provides methods for computing the
  /// convex hull of a set of <c>N</c> points in the plane.
  /// </para><para>
  /// The implementation uses the Graham-Scan convex hull algorithm.
  /// It runs in O(<c>N</c> log <c>N</c>) time in the worst case
  /// and uses O(<c>N</c>) extra memory.
  /// </para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/99scientific">Section 9.9</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/GrahamScan.java.html">GrahamScan</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class GrahamScan
  {
    private LinkedStack<Point2D> hull = new LinkedStack<Point2D>();

    /// <summary>
    /// Computes the convex hull of the specified array of points.
    /// </summary>
    /// <param name="pts">the array of points</param>
    /// <exception cref="NullReferenceException">if <c>points</c> is <c>null</c> or if any
    /// entry in <c>points[]</c> is <c>null</c></exception>
    ///
    public GrahamScan(Point2D[] pts)
    {

      // defensive copy
      int N = pts.Length;
      Point2D[] points = new Point2D[N];
      for (int i = 0; i < N; i++)
        points[i] = pts[i];

      // preprocess so that points[0] has lowest y-coordinate; break ties by x-coordinate
      // points[0] is an extreme point of the convex hull
      // (alternatively, could do easily in linear time)
      Array.Sort(points);

      // sort by polar angle with respect to base point points[0],
      // breaking ties by distance to points[0]
      Array.Sort(points, 1, N - 1, points[0].GetPolarOrder());

      hull.Push(points[0]);       // p[0] is first extreme point

      // find index k1 of first point not equal to points[0]
      int k1;
      for (k1 = 1; k1 < N; k1++)
        if (!points[0].Equals(points[k1])) break;
      if (k1 == N) return;        // all points equal

      // find index k2 of first point not collinear with points[0] and points[k1]
      int k2;
      for (k2 = k1 + 1; k2 < N; k2++)
        if (Point2D.Ccw(points[0], points[k1], points[k2]) != 0) break;
      hull.Push(points[k2 - 1]);    // points[k2-1] is second extreme point

      // Graham scan; note that points[N-1] is extreme point different from points[0]
      for (int i = k2; i < N; i++)
      {
        Point2D top = hull.Pop();
        while (Point2D.Ccw(hull.Peek(), top, points[i]) <= 0)
        {
          top = hull.Pop();
        }
        hull.Push(top);
        hull.Push(points[i]);
      }

      Debug.Assert(isConvex());
    }

    /// <summary>
    /// Returns the extreme points on the convex hull in counterclockwise order.</summary>
    /// <returns>the extreme points on the convex hull in counterclockwise order</returns>
    ///
    public IEnumerable<Point2D> Hull()
    {
      LinkedStack<Point2D> s = new LinkedStack<Point2D>();
      foreach (Point2D p in hull) s.Push(p);
      return s;
    }

    // check that boundary of hull is strictly convex
    private bool isConvex()
    {
      int N = hull.Count;
      if (N <= 2) return true;

      Point2D[] points = new Point2D[N];
      int n = 0;
      foreach (Point2D p in Hull())
      {
        points[n++] = p;
      }

      for (int i = 0; i < N; i++)
      {
        if (Point2D.Ccw(points[i], points[(i + 1) % N], points[(i + 2) % N]) <= 0)
        {
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Demo test the <c>ClosestPair</c> data type.
    /// Reads in an integer <c>N</c> and <c>N</c> points (specified by
    /// their <c>X</c>- and <c>Y</c>-coordinates) from standard input;
    /// computes their convex hull; and prints out the points on the
    /// convex hull to standard output.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd GrahamScan < rs1423.txt", "The number of points followed by x y coordinates")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      int N = StdIn.ReadInt();
      Point2D[] points = new Point2D[N];
      for (int i = 0; i < N; i++)
      {
        int x = StdIn.ReadInt();
        int y = StdIn.ReadInt();
        points[i] = new Point2D(x, y);
      }

      GrahamScan graham = new GrahamScan(points);
      foreach (Point2D p in graham.Hull())
        Console.WriteLine(p);
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
