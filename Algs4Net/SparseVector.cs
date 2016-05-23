/******************************************************************************
 *  File name :    SparseVector.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  A sparse vector, implementing using a symbol table.
 *
 *  [Not clear we need the instance variable N except for error checking.]
 *
 ******************************************************************************/

using System;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>SparseVector</c> class represents a <c>D</c>-dimensional mathematical vector.
  /// Vectors are mutable: their values can be changed after they are created.
  /// It includes methods for addition, subtraction,
  /// dot product, scalar product, unit vector, and Euclidean norm.
  /// </para><para>
  /// The implementation is a symbol table of indices and values for which the vector
  /// coordinates are nonzero. This makes it efficient when most of the vector coordindates
  /// are zero. See also <seealso cref="Vector"/> for an immutable (dense) vector data type.
  /// </para></summary>
  /// <remarks><para>For additional documentation,    
  /// see <a href="http://algs4.cs.princeton.edu/35applications">Section 3.5</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/SparseVector.java.html">SparseVector</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class SparseVector
  {
    private int d;                // dimension
    private ST<int, double> st;   // the vector, represented by index-value pairs

    /// <summary>Initializes a d-dimensional zero vector.</summary>
    /// <param name="d">the dimension of the vector</param>
    ///
    public SparseVector(int d)
    {
      this.d = d;
      this.st = new ST<int, double>();
    }

    /// <summary>
    /// Sets the ith coordinate of this vector to the specified value.</summary>
    /// <param name="i">the index</param>
    /// <param name="value">the new value</param>
    /// <exception cref="IndexOutOfRangeException">unless i is between 0 and d-1</exception>
    ///
    public void Put(int i, double value)
    {
      if (i < 0 || i >= d) throw new IndexOutOfRangeException("Illegal index");
      if (value == 0.0) st.Delete(i);
      else st.Put(i, value);
    }

    /// <summary>
    /// Returns the ith coordinate of this vector.</summary>
    /// <param name="i">the index</param>
    /// <returns>the value of the ith coordinate of this vector</returns>
    /// <exception cref="IndexOutOfRangeException">unless i is between 0 and d-1</exception>
    ///
    public double Get(int i)
    {
      if (i < 0 || i >= d) throw new IndexOutOfRangeException("Illegal index");
      if (st.Contains(i)) return st[i];
      else return 0.0;
    }

    /// <summary>
    /// Returns the number of nonzero entries in this vector.</summary>
    /// <returns>the number of nonzero entries in this vector</returns>
    ///
    public int Nnz()
    {
      return st.Count;
    }

    /// <summary>
    /// Returns the dimension of this vector.</summary>
    /// <returns>the dimension of this vector</returns>
    ///
    public int Dimension
    {
      get { return d; }
    }

    /// <summary>
    /// Returns the inner product of this vector with the specified vector.</summary>
    /// <param name="that">the other vector</param>
    /// <returns>the dot product between this vector and that vector</returns>
    /// <exception cref="ArgumentException">if the lengths of the two vectors are not equal</exception>
    ///
    public double Dot(SparseVector that)
    {
      if (this.d != that.d) throw new ArgumentException("Vector lengths disagree");
      double sum = 0.0;

      // iterate over the vector with the fewest nonzeros
      if (this.st.Count <= that.st.Count)
      {
        foreach (int i in this.st.Keys())
          if (that.st.Contains(i)) sum += this.Get(i) * that.Get(i);
      }
      else
      {
        foreach (int i in that.st.Keys())
          if (this.st.Contains(i)) sum += this.Get(i) * that.Get(i);
      }
      return sum;
    }

    /// <summary>
    /// Returns the inner product of this vector with the specified array.</summary>
    /// <param name="that"> that the array</param>
    /// <returns>the dot product between this vector and that array</returns>
    /// <exception cref="ArgumentException">if the dimensions of the vector and the array are not equal</exception>
    ///
    public double Dot(double[] that)
    {
      double sum = 0.0;
      foreach (int i in this.st.Keys())
        sum += that[i] * this.Get(i);
      return sum;
    }

    /// <summary>
    /// Returns the magnitude of this vector.
    /// This is also known as the L2 norm or the Euclidean norm.</summary>
    /// <returns>the magnitude of this vector</returns>
    ///
    public double Magnitude()
    {
      return Math.Sqrt(this.Dot(this));
    }

    /// <summary>
    /// Returns the scalar-vector product of this vector with the specified scalar.</summary>
    /// <param name="alpha">the scalar</param>
    /// <returns>the scalar-vector product of this vector with the specified scalar</returns>
    ///
    public SparseVector Scale(double alpha)
    {
      SparseVector c = new SparseVector(d);
      foreach (int i in this.st.Keys()) c.Put(i, alpha * this.Get(i));
      return c;
    }

    /// <summary>
    /// Returns the sum of this vector and the specified vector.</summary>
    /// <param name="that">the vector to add to this vector</param>
    /// <returns>the sum of this vector and that vector</returns>
    /// <exception cref="ArgumentException">if the dimensions of the two vectors are not equal</exception>
    ///
    public SparseVector Plus(SparseVector that)
    {
      if (this.d != that.d) throw new ArgumentException("Vector lengths disagree");
      SparseVector c = new SparseVector(d);
      foreach (int i in this.st.Keys()) c.Put(i, this.Get(i));                // c = this
      foreach (int i in that.st.Keys()) c.Put(i, that.Get(i) + c.Get(i));     // c = c + that
      return c;
    }

    /// <summary>
    /// Returns a string representation of this vector.</summary>
    /// <returns>a string representation of this vector, which consists of the
    /// the vector entries, separates by commas, enclosed in parentheses</returns>
    ///
    public override string ToString()
    {
      StringBuilder s = new StringBuilder();
      foreach (int i in st.Keys())
      {
        s.Append("(" + i + ", " + st.Get(i) + ") ");
      }
      return s.ToString();
    }

    /// <summary>
    /// Demo test the <c>SparseVector</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd SparseVector")]
    public static void MainTest(string[] args)
    {
      SparseVector a = new SparseVector(10);
      SparseVector b = new SparseVector(10);
      a.Put(3, 0.50);
      a.Put(9, 0.75);
      a.Put(6, 0.11);
      a.Put(6, 0.00);
      b.Put(3, 0.60);
      b.Put(4, 0.90);
      Console.WriteLine("a = " + a);
      Console.WriteLine("b = " + b);
      Console.WriteLine("a dot b = " + a.Dot(b));
      Console.WriteLine("a + b   = " + a.Plus(b));
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

