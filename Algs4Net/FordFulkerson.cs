/******************************************************************************
 *  File name :    FordFulkerson.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Ford-Fulkerson algorithm for computing a max flow and 
 *  a min cut using shortest augmenting path rule.
 *
 *  C:\> algscmd FordFulkerson 6 8
 *  6 8
 *  0:  0->5 0.00/99.00
 *  1:
 *  2:
 *  3:  3->2 0.00/98.00  3->1 0.00/55.00
 *  4:  4->1 0.00/81.00
 *  5:  5->2 0.00/97.00
 *  
 *  Max flow from 0 to 5
 *     0->5 99.00/99.00
 *  Min cut: 0
 *  Max flow value = 99
 *  
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>FordFulkerson</c> class represents a data type for computing a
  /// <c>Maximum st-flow</c> and <c>Minimum st-cut</c> in a flow
  /// network.</para><para>
  /// This implementation uses the <c>Ford-Fulkerson</c> algorithm with
  /// the <c>Shortest augmenting path</c> heuristic.
  /// The constructor takes time proportional to <c>E V</c> (<c>E</c> + <c>V</c>)
  /// in the worst case and extra space (not including the network)
  /// proportional to <c>V</c>, where <c>V</c> is the number of vertices
  /// and <c>E</c> is the number of edges. In practice, the algorithm will
  /// run much faster.
  /// Afterwards, the <c>inCut()</c> and <c>value()</c> methods take
  /// constant time.</para><para>
  /// If the capacities and initial flow values are all integers, then this
  /// implementation guarantees to compute an integer-valued maximum flow.
  /// If the capacities and floating-point numbers, then floating-point
  /// roundoff error can accumulate.</para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/64maxflow">Section 6.4</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/FordFulkerson.java.html">FordFulkerson</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class FordFulkerson
  {
    private const double FloatingPointEpsilon = 1E-11;

    private bool[] marked;     // marked[v] = true iff s->v path in residual graph
    private FlowEdge[] edgeTo; // edgeTo[v] = last edge on shortest residual s->v path
    private double value;      // current value of max flow

    /// <summary>
    /// Compute a maximum flow and minimum cut in the network <c>G</c>
    /// from vertex <c>s</c> to vertex <c>t</c>.</summary>
    /// <param name="G">the flow network</param>
    /// <param name="s">the source vertex</param>
    /// <param name="t">the sink vertex</param>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= s &lt; V</exception>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= t &lt; V</exception>
    /// <exception cref="ArgumentException">if s = t</exception>
    /// <exception cref="ArgumentException">if initial flow is infeasible</exception>
    ///
    public FordFulkerson(FlowNetwork G, int s, int t)
    {
      validate(s, G.V);
      validate(t, G.V);
      if (s == t) throw new ArgumentException("Source equals sink");
      if (!isFeasible(G, s, t)) throw new ArgumentException("Initial flow is infeasible");

      // while there exists an augmenting path, use it
      value = excess(G, t);
      while (hasAugmentingPath(G, s, t))
      {

        // compute bottleneck capacity
        double bottle = double.PositiveInfinity;
        for (int v = t; v != s; v = edgeTo[v].Other(v))
        {
          bottle = Math.Min(bottle, edgeTo[v].ResidualCapacityTo(v));
        }

        // augment flow
        for (int v = t; v != s; v = edgeTo[v].Other(v))
        {
          edgeTo[v].AddResidualFlowTo(v, bottle);
        }

        value += bottle;
      }

      // check optimality conditions
      Debug.Assert(check(G, s, t));
    }

    /// <summary>
    /// Returns the value of the maximum flow.</summary>
    /// <returns>the value of the maximum flow</returns>
    ///
    public double Value
    {
      get { return value; }
    }

    /// <summary>
    /// Returns true if the specified vertex is on the <c>s</c> side of the mincut.</summary>
    /// <param name="v">the vertex under consideration</param>
    /// <returns><c>true</c> if vertex <c>v</c> is on the <c>s</c> side of the micut;
    /// <c>false</c> otherwise</returns>
    /// <exception cref="IndexOutOfRangeException">unless 0 &lt;= v &lt; V</exception>
    ///
    public bool InCut(int v)
    {
      validate(v, marked.Length);
      return marked[v];
    }

    // throw an exception if v is outside prescibed range
    private void validate(int v, int V)
    {
      if (v < 0 || v >= V)
        throw new IndexOutOfRangeException("vertex " + v + " is not between 0 and " + (V - 1));
    }

    // is there an augmenting path?
    // if so, upon termination edgeTo[] will contain a parent-link representation of such a path
    // this implementation finds a shortest augmenting path (fewest number of edges),
    // which performs well both in theory and in practice
    private bool hasAugmentingPath(FlowNetwork G, int s, int t)
    {
      edgeTo = new FlowEdge[G.V];
      marked = new bool[G.V];

      // breadth-first search
      LinkedQueue<int> queue = new LinkedQueue<int>();
      queue.Enqueue(s);
      marked[s] = true;
      while (!queue.IsEmpty && !marked[t])
      {
        int v = queue.Dequeue();

        foreach (FlowEdge e in G.Adj(v))
        {
          int w = e.Other(v);

          // if residual capacity from v to w
          if (e.ResidualCapacityTo(w) > 0)
          {
            if (!marked[w])
            {
              edgeTo[w] = e;
              marked[w] = true;
              queue.Enqueue(w);
            }
          }
        }
      }
      // is there an augmenting path?
      return marked[t];
    }

    // return excess flow at vertex v
    private double excess(FlowNetwork G, int v)
    {
      double excess = 0.0;
      foreach (FlowEdge e in G.Adj(v))
      {
        if (v == e.From) excess -= e.Flow;
        else excess += e.Flow;
      }
      return excess;
    }

    // return excess flow at vertex v
    private bool isFeasible(FlowNetwork G, int s, int t)
    {

      // check that capacity constraints are satisfied
      for (int v = 0; v < G.V; v++)
      {
        foreach (FlowEdge e in G.Adj(v))
        {
          if (e.Flow < -FloatingPointEpsilon || e.Flow > e.Capacity + FloatingPointEpsilon)
          {
            Console.Error.WriteLine("Edge does not satisfy capacity constraints: " + e);
            return false;
          }
        }
      }

      // check that net flow into a vertex equals zero, except at source and sink
      if (Math.Abs(value + excess(G, s)) > FloatingPointEpsilon)
      {
        Console.Error.WriteLine("Excess at source = " + excess(G, s));
        Console.Error.WriteLine("Max flow         = " + value);
        return false;
      }
      if (Math.Abs(value - excess(G, t)) > FloatingPointEpsilon)
      {
        Console.Error.WriteLine("Excess at sink   = " + excess(G, t));
        Console.Error.WriteLine("Max flow         = " + value);
        return false;
      }
      for (int v = 0; v < G.V; v++)
      {
        if (v == s || v == t) continue;
        else if (Math.Abs(excess(G, v)) > FloatingPointEpsilon)
        {
          Console.Error.WriteLine("Net flow out of " + v + " doesn't equal zero");
          return false;
        }
      }
      return true;
    }

    // check optimality conditions
    private bool check(FlowNetwork G, int s, int t)
    {
      // check that flow is feasible
      if (!isFeasible(G, s, t))
      {
        Console.Error.WriteLine("Flow is infeasible");
        return false;
      }

      // check that s is on the source side of min cut and that t is not on source side
      if (!InCut(s))
      {
        Console.Error.WriteLine("source " + s + " is not on source side of min cut");
        return false;
      }
      if (InCut(t))
      {
        Console.Error.WriteLine("sink " + t + " is on source side of min cut");
        return false;
      }

      // check that value of min cut = value of max flow
      double mincutValue = 0.0;
      for (int v = 0; v < G.V; v++)
      {
        foreach (FlowEdge e in G.Adj(v))
        {
          if ((v == e.From) && InCut(e.From) && !InCut(e.To))
            mincutValue += e.Capacity;
        }
      }

      if (Math.Abs(mincutValue - value) > FloatingPointEpsilon)
      {
        Console.Error.WriteLine("Max flow value = " + value + ", min cut value = " + mincutValue);
        return false;
      }
      return true;
    }

    /// <summary>
    /// Demo test the <c>FordFulkerson</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd FordFulkerson V E")]
    public static void MainTest(string[] args)
    {
      //create flow network with V vertices and E edges
      int V = int.Parse(args[0]);
      int E = int.Parse(args[1]);
      int s = 0, t = V - 1;
      FlowNetwork G = new FlowNetwork(V, E);

      Console.WriteLine(G);

      // compute maximum flow and minimum cut
      FordFulkerson maxflow = new FordFulkerson(G, s, t);
      Console.WriteLine("Max flow from " + s + " to " + t);
      for (int v = 0; v < G.V; v++)
      {
        foreach (FlowEdge e in G.Adj(v))
        {
          if ((v == e.From) && e.Flow > 0)
            Console.WriteLine("   " + e);
        }
      }

      // print min-cut
      Console.Write("Min cut: ");
      for (int v = 0; v < G.V; v++)
      {
        if (maxflow.InCut(v)) Console.Write(v + " ");
      }
      Console.WriteLine();

      Console.WriteLine("Max flow value = " + maxflow.Value);
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
