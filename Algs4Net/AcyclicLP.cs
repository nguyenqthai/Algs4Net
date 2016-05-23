/******************************************************************************
 *  File name :    AcyclicLP.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/44sp/tinyEWDAG.txt
 *  
 *  Computes longeset paths in an edge-weighted acyclic digraph.
 *
 *  Remark: should probably check that graph is a DAG before running
 *
 *  C:\> algscmd AcyclicLP tinyEWDAG.txt 5
 *  5 to 0 (2.44)  5->1  0.32   1->3  0.29   3->6  0.52   6->4  0.93   4->0  0.38   
 *  5 to 1 (0.32)  5->1  0.32   
 *  5 to 2 (2.77)  5->1  0.32   1->3  0.29   3->6  0.52   6->4  0.93   4->7  0.37   7->2  0.34   
 *  5 to 3 (0.61)  5->1  0.32   1->3  0.29   
 *  5 to 4 (2.06)  5->1  0.32   1->3  0.29   3->6  0.52   6->4  0.93   
 *  5 to 5 (0.00)  
 *  5 to 6 (1.13)  5->1  0.32   1->3  0.29   3->6  0.52   
 *  5 to 7 (2.43)  5->1  0.32   1->3  0.29   3->6  0.52   6->4  0.93   4->7  0.37   
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>AcyclicLP</c> class represents a data type for solving the
  /// single-source longest paths problem in edge-weighted directed
  /// acyclic graphs (DAGs). The edge weights can be positive, negative, or zero.
  /// </para><para>
  /// This implementation uses a topological-sort based algorithm.
  /// The constructor takes time proportional to <c>V</c> + <c>E</c>,
  /// where <c>V</c> is the number of vertices and <c>E</c> is the number of edges.
  /// Afterwards, the <c>DistTo()</c> and <c>HasPathTo()</c> methods take
  /// constant time and the <c>PathTo()</c> method takes time proportional to the
  /// number of edges in the longest path returned.
  /// </para></summary>
  /// <remarks><para>For additional documentation,   
  /// see <a href="http://algs4.cs.princeton.edu/44sp">Section 4.4</a> of   
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/AcyclicLP.java.html">AcyclicLP</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class AcyclicLP
  {
    private double[] distTo;          // distTo[v] = distance  of longest s->v path
    private DirectedEdge[] edgeTo;    // edgeTo[v] = last edge on longest s->v path

    /// <summary>
    /// Computes a longest paths tree from <c>s</c> to every other vertex in
    /// the directed acyclic graph <c>G</c>.</summary>
    /// <param name="G">the acyclic digraph</param>
    /// <param name="s">the source vertex</param>
    /// <exception cref="ArgumentException">if the digraph is not acyclic</exception>
    /// <exception cref="ArgumentException">unless 0 &lt;= <c>s</c> &lt;= <c>V</c> - 1</exception>
    ///
    public AcyclicLP(EdgeWeightedDigraph G, int s)
    {
      distTo = new double[G.V];
      edgeTo = new DirectedEdge[G.V];
      for (int v = 0; v < G.V; v++)
        distTo[v] = double.NegativeInfinity;
      distTo[s] = 0.0;

      // relax vertices in toplogical order
      Topological topological = new Topological(G);
      if (!topological.HasOrder)
        throw new ArgumentException("Digraph is not acyclic.");
      foreach (int v in topological.Order())
      {
        foreach (DirectedEdge e in G.Adj(v))
          relax(e);
      }
    }

    // relax edge e, but update if you find a *longer* path
    private void relax(DirectedEdge e)
    {
      int v = e.From, w = e.To;
      if (distTo[w] < distTo[v] + e.Weight)
      {
        distTo[w] = distTo[v] + e.Weight;
        edgeTo[w] = e;
      }
    }

    /// <summary>
    /// Returns the length of a longest path from the source vertex <c>s</c> to vertex <c>v</c>.</summary>
    /// <param name="v">the destination vertex</param>
    /// <returns>the length of a longest path from the source vertex <c>s</c> to vertex <c>v</c>;</returns>
    ///   <c>double.PositiveInfinity</c> if no such path
    ///
    public double DistTo(int v)
    {
      return distTo[v];
    }

    /// <summary>
    /// Is there a path from the source vertex <c>s</c> to vertex <c>v</c>?</summary>
    /// <param name="v">the destination vertex</param>
    /// <returns><c>true</c> if there is a path from the source vertex
    ///   <c>s</c> to vertex <c>v</c>, and <c>false</c> otherwise</returns>
    ///
    public bool HasPathTo(int v)
    {
      return distTo[v] > double.NegativeInfinity;
    }

    /// <summary>
    /// Returns a longest path from the source vertex <c>s</c> to vertex <c>v</c>.</summary>
    /// <param name="v">the destination vertex</param>
    /// <returns>a longest path from the source vertex <c>s</c> to vertex <c>v</c></returns>
    ///   as an iterable of edges, and <c>null</c> if no such path
    ///
    public IEnumerable<DirectedEdge> PathTo(int v)
    {
      if (!HasPathTo(v)) return null;
      LinkedStack<DirectedEdge> path = new LinkedStack<DirectedEdge>();
      for (DirectedEdge e = edgeTo[v]; e != null; e = edgeTo[e.From])
      {
        path.Push(e);
      }
      return path;
    }
    /// <summary>
    /// Demo test the <c>AcyclicLP</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd AcyclicLP tinyEWDAG.txt 5", "File with the pre-defined format for directed, weighted graph")]
    public static void MainTest(string[] args)
    {
      EdgeWeightedDigraph G = new EdgeWeightedDigraph(new TextInput(args[0]));

      int s = int.Parse(args[1]);
      AcyclicLP lp = new AcyclicLP(G, s);

      for (int v = 0; v < G.V; v++)
      {
        if (lp.HasPathTo(v))
        {
          Console.Write("{0} to {1} ({2:F2})  ", s, v, lp.DistTo(v));
          foreach (DirectedEdge e in lp.PathTo(v))
          {
            Console.Write(e + "   ");
          }
          Console.WriteLine();
        }
        else
        {
          Console.Write("{0} to {0}         no path\n", s, v);
        }
      }
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
