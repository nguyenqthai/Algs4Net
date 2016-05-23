/******************************************************************************
 *  File name :    Edge.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Immutable weighted edge.
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Edge</c> class represents a weighted edge in an
  /// <seealso cref="EdgeWeightedGraph"/>. Each edge consists of two integers
  /// (naming the two vertices) and a real-value weight. The data type
  /// provides methods for accessing the two endpoints of the edge and
  /// the weight. The natural order for this data type is by
  /// ascending order of weight.</summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/43mst">Section 4.3</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Edge.java.html">Edge</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.
  /// </para></remarks>
  ///
  public class Edge : IComparable<Edge>
  {

    private readonly int v;
    private readonly int w;
    private readonly double weight;

    /// <summary>
    /// Initializes an edge between vertices <c>v</c> and <c>w</c> of an
    /// undirected graph when weight is immaterial.</summary>
    /// <param name="v"> v one vertex</param>
    /// <param name="w"> w the other vertex</param>
    /// <exception cref="IndexOutOfRangeException">if either <c>v</c> or <c>w</c>
    ///        is a negative integer</exception>
    /// <exception cref="ArgumentException">if <c>weight</c> is <c>NaN</c></exception>
    ///
    public Edge(int v, int w)
    {
      validateVertex(v);
      validateVertex(w);
      this.v = v;
      this.w = w;
      this.weight = double.NaN; // dummy
    }

    /// <summary>
    /// Initializes an edge between vertices <c>v</c> and <c>w</c> of an undirected
    /// graph with the given <c>weight</c>.</summary>
    /// <param name="v"> v one vertex</param>
    /// <param name="w"> w the other vertex</param>
    /// <param name="weight"> weight the weight of this edge</param>
    /// <exception cref="IndexOutOfRangeException">if either <c>v</c> or <c>w</c>
    /// is a negative integer</exception>
    /// <exception cref="ArgumentException">if <c>weight</c> is <c>NaN</c></exception>
    ///
    public Edge(int v, int w, double weight)
    {
      validateVertex(v);
      validateVertex(w);
      if (double.IsNaN(weight)) throw new ArgumentException("Weight is NaN");
      this.v = v;
      this.w = w;
      this.weight = weight;
    }

    // throw an IndexOutOfRangeException unless 0 <= v < V
    private void validateVertex(int v)
    {
      if (v < 0)
        throw new IndexOutOfRangeException("Vertex name must be a nonnegative integer");
    }

    /// <summary>Returns the weight of this edge.</summary>
    /// <returns>the weight of this edge</returns>
    ///
    public double Weight
    {
      get { return weight; }
    }

    /// <summary>Returns either endpoint of this edge.</summary> 
    /// <returns>either endpoint of this edge</returns>
    ///
    public int Either
    {
      get { return v; }
    }

    /// <summary>
    /// Returns the endpoint of this edge that is different from the given vertex.</summary>
    /// <param name="vertex"> vertex one endpoint of this edge</param>
    /// <returns>the other endpoint of this edge</returns>
    /// <exception cref="ArgumentException">if the vertex is not one of the
    ///        endpoints of this edge</exception>
    ///
    public int Other(int vertex)
    {
      if (vertex == v) return w;
      else if (vertex == w) return v;
      else throw new ArgumentException("Illegal endpoint");
    }

    /// <summary>
    /// Compares two edges by weight.
    /// Note that <c>CompareTo()</c> is not consistent with <c>Equals()</c>,
    /// which uses the reference equality implementation inherited from <c>object</c>.</summary>
    /// <param name="that"> that the other edge</param>
    /// <returns>a negative integer, zero, or positive integer depending on whether</returns>
    ///        the weight of this is less than, equal to, or greater than the
    ///        argument edge
    ///
    public int CompareTo(Edge that)
    {
      if (this.Weight < that.Weight) return -1;
      else if (this.Weight > that.Weight) return +1;
      else return 0;
    }

    /// <summary>Returns a string representation of this edge.</summary>
    /// <returns>a string representation of this edge</returns>
    ///
    public override string ToString()
    {
      if (double.IsNaN(weight))
        return string.Format("{0}-{1}", v, w);
      else
        return string.Format("{0}-{1} {2:F5}", v, w, weight);
    }

    /// <summary>Demo test the <c>Edge</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    public static void MainTest(string[] args)
    {
      Edge e = new Edge(12, 34, 5.67);
      Console.WriteLine("Edge with weight {0}", e);
      e = new Edge(32, 24);
      Console.WriteLine("Edge without weight {0}", e);
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
