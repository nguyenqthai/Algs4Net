/******************************************************************************
 *  File name :    DijkstraAllPairsSP.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Dijkstra's algorithm run from each vertex. 
 *  Takes time proportional to E V log V and space proportional to EV.
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DijkstraAllPairsSP</c> class represents a data type for solving the
  /// all-pairs shortest paths problem in edge-weighted digraphs
  /// where the edge weights are nonnegative.
  /// </para><para>
  /// This implementation runs Dijkstra's algorithm from each vertex.
  /// The constructor takes time proportional to <c>V</c> (<c>E</c> log <c>V</c>)
  /// and uses space proprtional to <c>V</c><sup>2</sup>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>Dist()</c> and <c>hasPath()</c> methods take
  /// constant time and the <c>Path()</c> method takes time proportional to the
  /// number of edges in the shortest path returned.</para></summary>
  /// <remarks><para>For additional documentation,    
  /// see <a href="http://algs4.cs.princeton.edu/44sp">Section 4.4</a> of    
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DijkstraAllPairsSP.java.html">DijkstraAllPairsSP</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DijkstraAllPairsSP
  {
    private DijkstraSP[] all;

    /// <summary>
    /// Computes a shortest paths tree from each vertex to to every other vertex in
    /// the edge-weighted digraph <c>G</c>.</summary>
    /// <param name="G">the edge-weighted digraph</param>
    /// <exception cref="ArgumentException">if an edge weight is negative</exception>
    /// <exception cref="ArgumentException">unless 0 &lt;= <c>s</c> &lt;= <c>V</c> - 1</exception>
    ///
    public DijkstraAllPairsSP(EdgeWeightedDigraph G)
    {
      all = new DijkstraSP[G.V];
      for (int v = 0; v < G.V; v++)
        all[v] = new DijkstraSP(G, v);
    }

    /// <summary>
    /// Returns a shortest path from vertex <c>s</c> to vertex <c>t</c>.</summary>
    /// <param name="s">the source vertex</param>
    /// <param name="t">the destination vertex</param>
    /// <returns>a shortest path from vertex <c>s</c> to vertex <c>t</c>
    ///   as an iterable of edges, and <c>null</c> if no such path</returns>
    ///
    public IEnumerable<DirectedEdge> Path(int s, int t)
    {
      return all[s].PathTo(t);
    }

    /// <summary>
    /// Is there a path from the vertex <c>s</c> to vertex <c>t</c>?</summary>
    /// <param name="s">the source vertex</param>
    /// <param name="t">the destination vertex</param>
    /// <returns><c>true</c> if there is a path from vertex <c>s</c> </returns>
    ///   to vertex <c>t</c>, and <c>false</c> otherwise
    ///
    public bool HasPath(int s, int t)
    {
      return Dist(s, t) < double.PositiveInfinity;
    }

    /// <summary>
    /// Returns the length of a shortest path from vertex <c>s</c> to vertex <c>t</c>.</summary>
    /// <param name="s">the source vertex</param>
    /// <param name="t">the destination vertex</param>
    /// <returns>the length of a shortest path from vertex <c>s</c> to vertex <c>t</c>;
    ///   <c>double.PositiveInfinity</c> if no such path</returns>
    ///
    public double Dist(int s, int t)
    {
      return all[s].DistTo(t);
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
