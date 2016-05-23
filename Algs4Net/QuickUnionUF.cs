/******************************************************************************
 *  File name :    QuickUnionUF.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Quick-union algorithm.
 *
 *  C:\> algscmd QuickUnionUF < tinyUF.txt
 *  4 3
 *  3 8
 *  6 5
 *  9 4
 *  2 1
 *  5 0
 *  7 2
 *  6 1
 *  2 components
 * 
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>QuickUnionUF</c> class represents a <c>Union-find data type</c>
  /// (also known as the <c>Disjoint-sets data type</c>).
  /// It supports the <c>Union</c> and <c>Find</c> operations,
  /// along with a <c>Connected</c> operation for determining whether
  /// two sites are in the same component and a <c>Count</c> operation that
  /// returns the total number of components.
  /// </para><para>
  /// The union-find data type models connectivity among a set of <c>N</c>
  /// sites, named 0 through <c>N</c> - 1. The <c>Is-connected-to</c> relation 
  /// must be an <c>Equivalence relation</c> that partitions the sites into
  /// <c>Equivalence classes</c> (or <c>Components</c>). In this case,
  /// two sites are in the same component if and only if they are connected.
  /// Both sites and components are identified with integers between 0 and
  /// <c>N</c>- 1.</para><para> 
  /// Initially, there are <c>N</c> components, with each site in its
  /// own component.  The <c>Component identifier</c> of a component
  /// (also known as the <c>Root</c>, <c>Canonical element</c>, <c>Leader</c>,
  /// or <c>Set representative</c>) is one of the sites in the component:
  /// two sites have the same component identifier if and only if they are
  /// in the same component.</para>
  /// <ul>
  /// <li><em>union</em>(<em>p</em>, <em>q</em>) adds a
  ///        connection between the two sites <c>P</c> and <c>Q</c>.
  ///        If <c>P</c> and <c>Q</c> are in different components,
  ///        then it replaces
  ///        these two components with a new component that is the union of
  ///        the two.</li>
  /// <li><em>find</em>(<em>p</em>) returns the component
  ///        identifier of the component containing <c>P</c>.</li>
  /// <li><em>connected</em>(<em>p</em>, <em>q</em>)
  ///        returns true if both <c>P</c> and <c>Q</c>
  ///        are in the same component, and false otherwise.</li>
  /// <li><em>count</em>() returns the number of components.</li>
  /// </ul>
  /// <para>The component identifier of a component can change
  /// only when the component itself changes during a call to
  /// <c>Union</c>; it cannot change during a call
  /// to <c>Find</c>, <c>Connected</c>, or <c>Count</c>.
  /// </para><para>
  /// This implementation uses quick union.
  /// Initializing a data structure with <c>N</c> sites takes linear time.
  /// Afterwards, the <c>Union</c>, <c>Find</c>, and <c>Connected</c>
  /// operations  take linear time (in the worst case) and the
  /// <c>Count</c> operation takes constant time.</para><para>
  /// For alternate implementations of the same API, see
  /// <seealso cref="UF"/>, <seealso cref="QuickFindUF"/>, and <seealso cref="WeightedQuickUnionUF"/>.</para>
  /// </summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/15uf">Section 1.5</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/QuickUnionUF.java.html">QuickUnionUF</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class QuickUnionUF
  {
    private int[] parent;  // parent[i] = parent of i
    private int count;     // number of components

    /// <summary>Initializes an empty union-find data structure with <c>N</c> sites
    /// <c>0</c> through <c>N-1</c>. Each site is initially in its own 
    /// component.</summary>
    /// <param name="N">the number of sites</param>
    /// <exception cref="ArgumentException">if <c>N &lt; 0</c></exception>
    ///
    public QuickUnionUF(int N)
    {
      parent = new int[N];
      count = N;
      for (int i = 0; i < N; i++)
      {
        parent[i] = i;
      }
    }

    /// <summary>
    /// Returns the number of components.</summary>
    /// <returns>the number of components (between <c>1</c> and <c>N</c>)</returns>
    ///
    public int Count
    {
      get { return count; }
    }

    /// <summary>
    /// Returns the component identifier for the component containing site <c>p</c>.</summary>
    /// <param name="p">the integer representing one object</param>
    /// <returns>the component identifier for the component containing site <c>p</c></returns>
    /// <exception cref="IndexOutOfRangeException">unless <c>0 &lt;= p &lt; N</c></exception>
    ///
    public int Find(int p)
    {
      validate(p);
      while (p != parent[p])
        p = parent[p];
      return p;
    }

    // validate that p is a valid index
    private void validate(int p)
    {
      int N = parent.Length;
      if (p < 0 || p >= N)
      {
        throw new IndexOutOfRangeException("index " + p + " is not between 0 and " + (N - 1));
      }
    }

    /// <summary>
    /// Returns true if the the two sites are in the same component.</summary>
    /// <param name="p">the integer representing one site</param>
    /// <param name="q">the integer representing the other site</param>
    /// <returns><c>true</c> if the two sites <c>p</c> and <c>q</c> are in the same component;
    ///        <c>false</c> otherwise</returns>
    /// <exception cref="IndexOutOfRangeException">unless
    ///        both <c>0 &lt;= p &lt; N</c> and <c>0 &lt;= q &lt; N</c></exception>
    ///
    public bool Connected(int p, int q)
    {
      return Find(p) == Find(q);
    }

    /// <summary>
    /// Merges the component containing site <c>p</c> with the
    /// the component containing site <c>q</c>.</summary>
    /// <param name="p">the integer representing one site</param>
    /// <param name="q">the integer representing the other site</param>
    /// <exception cref="IndexOutOfRangeException">unless</exception>
    ///        both <c>0 &lt;= p &lt; N</c> and <c>0 &lt;= q &lt; N</c>
    ///
    public void Union(int p, int q)
    {
      int rootP = Find(p);
      int rootQ = Find(q);
      if (rootP == rootQ) return;
      parent[rootP] = rootQ;
      count--;
    }

    /// <summary>
    /// Reads in a sequence of pairs of integers (between 0 and N-1) from standard input,
    /// where each integer represents some object;
    /// if the sites are in different components, merge the two components
    /// and print the pair to standard output.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd QuickUnionUF < tinyUF.txt", "N followed by p q pairs")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      int N = StdIn.ReadInt();
      QuickUnionUF uf = new QuickUnionUF(N);
      while (!StdIn.IsEmpty)
      {
        int p = StdIn.ReadInt();
        int q = StdIn.ReadInt();
        if (uf.Connected(p, q)) continue;
        uf.Union(p, q);
        Console.WriteLine(p + " " + q);
      }
      Console.WriteLine(uf.Count + " components");
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
