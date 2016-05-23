/******************************************************************************
 *  File name :    FlowEdge.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Capacitated edge with a flow in a flow network.
 *
 * C:\> algscmd FlowEdge
 * 12->23 0.00/3.14
 * 
 ******************************************************************************/
using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>FlowEdge</c> class represents a capacitated edge with a
  /// flow in a <seealso cref="FlowNetwork"/>. Each edge consists of two integers
  /// (naming the two vertices), a real-valued capacity, and a real-valued
  /// flow. The data type provides methods for accessing the two endpoints
  /// of the directed edge and the weight. It also provides methods for
  /// changing the amount of flow on the edge and determining the residual
  /// capacity of the edge.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/64maxflow">Section 6.4</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/FlowEdge.java.html">FlowEdge</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class FlowEdge
  {
    private readonly int v;             // from
    private readonly int w;             // to 
    private readonly double capacity;   // capacity
    private double flow;                // flow

    /// <summary>
    /// Initializes an edge from vertex <c>v</c> to vertex <c>w</c> with
    /// the given <c>capacity</c> and zero flow.</summary>
    /// <param name="v">he tail vertex</param>
    /// <param name="w">whe head vertex</param>
    /// <param name="capacity">the capacity of the edge</param>
    /// <exception cref="IndexOutOfRangeException">if either <c>v</c> or <c>w</c></exception>
    ///   is a negative integer
    /// <exception cref="ArgumentException">if <c>capacity</c> is negative</exception>
    ///
    public FlowEdge(int v, int w, double capacity)
    {
      if (v < 0) throw new IndexOutOfRangeException("Vertex name must be a nonnegative integer");
      if (w < 0) throw new IndexOutOfRangeException("Vertex name must be a nonnegative integer");
      if (!(capacity >= 0.0)) throw new ArgumentException("Edge capacity must be nonnegaitve");
      this.v = v;
      this.w = w;
      this.capacity = capacity;
      this.flow = 0.0;
    }

    /// <summary>
    /// Initializes an edge from vertex <c>v</c> to vertex <c>w</c> with
    /// the given <c>capacity</c> and <c>flow</c>.</summary>
    /// <param name="v">the tail vertex</param>
    /// <param name="w">the head vertex</param>
    /// <param name="capacity">the capacity of the edge</param>
    /// <param name="flow">the flow on the edge</param>
    /// <exception cref="IndexOutOfRangeException">if either <c>v</c> or <c>w</c>
    /// is a negative integer</exception>
    /// <exception cref="ArgumentException">if <c>capacity</c> is negative</exception>
    /// <exception cref="ArgumentException">unless <c>flow</c> is between 
    /// <c>0.0</c> and <c>capacity</c>.</exception>
    ///
    public FlowEdge(int v, int w, double capacity, double flow)
    {
      if (v < 0) throw new IndexOutOfRangeException("Vertex name must be a nonnegative integer");
      if (w < 0) throw new IndexOutOfRangeException("Vertex name must be a nonnegative integer");
      if (!(capacity >= 0.0)) throw new ArgumentException("Edge capacity must be nonnegaitve");
      if (!(flow <= capacity)) throw new ArgumentException("Flow exceeds capacity");
      if (!(flow >= 0.0)) throw new ArgumentException("Flow must be nonnnegative");
      this.v = v;
      this.w = w;
      this.capacity = capacity;
      this.flow = flow;
    }

    /// <summary>
    /// Initializes a flow edge from another flow edge.</summary>
    /// <param name="e">the edge to copy</param>
    ///
    public FlowEdge(FlowEdge e)
    {
      v = e.v;
      w = e.w;
      capacity = e.capacity;
      flow = e.flow;
    }

    /// <summary>
    /// Returns the tail vertex of the edge.</summary>
    /// <returns>the tail vertex of the edge</returns>
    ///
    public int From
    {
      get { return v; }
    }

    /// <summary>
    /// Returns the head vertex of the edge.</summary>
    /// <returns>the head vertex of the edge</returns>
    ///
    public int To
    {
      get { return w; }
    }

    /// <summary>
    /// Returns the capacity of the edge.</summary>
    /// <returns>the capacity of the edge</returns>
    ///
    public double Capacity
    {
      get { return capacity; }
    }

    /// <summary>
    /// Returns the flow on the edge.</summary>
    /// <returns>the flow on the edge</returns>
    ///
    public double Flow
    {
      get { return flow; }
    }

    /// <summary>
    /// Returns the endpoint of the edge that is different from the given vertex
    /// (unless the edge represents a self-loop in which case it returns the same vertex).</summary>
    /// <param name="vertex">one endpoint of the edge</param>
    /// <returns>the endpoint of the edge that is different from the given vertex
    /// (unless the edge represents a self-loop in which case it returns the same vertex)</returns>
    /// <exception cref="ArgumentException">if <c>vertex</c> is not one of the endpoints</exception>
    ///  of the edge
    ///
    public int Other(int vertex)
    {
      if (vertex == v) return w;
      else if (vertex == w) return v;
      else throw new ArgumentException("Illegal endpoint");
    }

    /// <summary>
    /// Returns the residual capacity of the edge in the direction
    /// to the given <c>vertex</c>.</summary>
    /// <param name="vertex">one endpoint of the edge</param>
    /// <returns>the residual capacity of the edge in the direction to the given vertex;
    /// If <c>vertex</c> is the tail vertex, the residual capacity equals
    /// <c>Capacity - Flow</c>; if <c>vertex</c> is the head vertex, the
    ///  residual capacity equals <c>Flow</c>.</returns>
    /// <exception cref="ArgumentException">if <c>vertex</c> is not one of the endpoints
    ///  of the edge</exception>
    ///
    public double ResidualCapacityTo(int vertex)
    {
      if (vertex == v) return flow;                   // backward edge
      else if (vertex == w) return capacity - flow;   // forward edge
      else throw new ArgumentException("Illegal endpoint");
    }

    /// <summary>
    /// Increases the flow on the edge in the direction to the given vertex.
    ///  If <c>vertex</c> is the tail vertex, this increases the flow on the edge by <c>delta</c>;
    ///  if <c>vertex</c> is the head vertex, this decreases the flow on the edge by <c>delta</c>.</summary>
    /// <param name="vertex">one endpoint of the edge</param>
    /// <param name="delta">the residual flow</param>
    /// <exception cref="ArgumentException">if <c>vertex</c> is not one of the endpoints
    /// of the edge</exception>
    /// <exception cref="ArgumentException">if <c>delta</c> makes the flow on
    ///  on the edge either negative or larger than its capacity</exception>
    /// <exception cref="ArgumentException">if <c>delta</c> is <c>NaN</c></exception>
    ///
    public void AddResidualFlowTo(int vertex, double delta)
    {
      if (vertex == v) flow -= delta;               // backward edge
      else if (vertex == w) flow += delta;          // forward edge
      else throw new ArgumentException("Illegal endpoint");
      if (double.IsNaN(delta)) throw new ArgumentException("Change in flow = NaN");
      if (!(flow >= 0.0)) throw new ArgumentException("Flow is negative");
      if (!(flow <= capacity)) throw new ArgumentException("Flow exceeds capacity");
    }

    /// <summary>
    /// Returns a string representation of the edge.</summary>
    /// <returns>a string representation of the edge</returns>
    ///
    public override string ToString()
    {
      return string.Format("{0}->{1} {2:F}/{3:F}", v, w, flow, capacity);
    }

    /// <summary>
    /// Demo test the <c>FlowEdge</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    public static void MainTest(string[] args)
    {
      FlowEdge e = new FlowEdge(12, 23, 3.14);
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
