/******************************************************************************
 *  File name :    DirectedEdge.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Immutable weighted directed edge.
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>DirectedEdge</c> class represents a weighted edge in an
  /// <seealso cref="EdgeWeightedDigraph"/>. Each edge consists of two integers
  /// (naming the two vertices) and a real-value weight. The data type
  /// provides methods for accessing the two endpoints of the directed edge and
  /// the weight.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/44sp">Section 4.4</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DirectedEdge.java.html">DirectedEdge</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DirectedEdge
  {
    private readonly int v;
    private readonly int w;
    private readonly double weight;

    /// <summary>Initializes a directed edge from vertex <c>v</c> to vertex <c>w</c> with
    /// the given <c>weight</c>.</summary>
    /// <param name="v">the tail vertex</param>
    /// <param name="w">the head vertex</param>
    /// <param name="weight">the weight of the directed edge</param>
    /// <exception cref="IndexOutOfRangeException">if either <c>v</c> or <c>w</c>
    /// is a negative integer</exception>
    /// <exception cref="ArgumentException">if <c>weight</c> is <c>NaN</c></exception>
    ///
    public DirectedEdge(int v, int w, double weight)
    {
      if (v < 0) throw new IndexOutOfRangeException("Vertex names must be nonnegative integers");
      if (w < 0) throw new IndexOutOfRangeException("Vertex names must be nonnegative integers");
      if (double.IsNaN(weight)) throw new ArgumentException("Weight is NaN");
      this.v = v;
      this.w = w;
      this.weight = weight;
    }

    /// <summary>
    /// Returns the tail vertex of the directed edge.</summary>
    /// <returns>the tail vertex of the directed edge</returns>
    ///
    public int From
    {
      get { return v; }
    }

    /// <summary>
    /// Returns the head vertex of the directed edge.</summary>
    /// <returns>the head vertex of the directed edge</returns>
    ///
    public int To
    {
      get { return w; }
    }

    /// <summary>
    /// Returns the weight of the directed edge.</summary>
    /// <returns>the weight of the directed edge</returns>
    ///
    public double Weight
    {
      get { return weight; }
    }

    /// <summary>
    /// Returns a string representation of the directed edge.</summary>
    /// <returns>a string representation of the directed edge</returns>
    ///
    public override string ToString()
    {
      return v + "->" + w + " " + string.Format("{0:F2}", weight);
    }

    /// <summary>
    /// Demo test the <c>DirectedEdge</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    public static void MainTest(string[] args)
    {
      DirectedEdge e = new DirectedEdge(12, 34, 5.67);
      Console.WriteLine(e);
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
