/******************************************************************************
 *  File name :    Vector.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Implementation of a vector of real numbers.
 *
 *  This class is implemented to be immutable: once the client program
 *  initialize a Vector, it cannot change any of its fields
 *  (d or data[i]) either directly or indirectly. Immutability is a
 *  very desirable feature of a data type.
 *
 *  C:\> algscmd Vector
 *  Vector
 *     x       = [ 1.000 2.000 3.000 4.000 ]
 *     y       = [ 5.000 2.000 4.000 1.000 ]
 *     z       = [ 6.000 4.000 7.000 5.000 ]
 *   10z       = [ 60.000 40.000 70.000 50.000 ]
 *    |x|      = 5.477
 *   <x, y>    = 25.000
 *  Dist(x, y) = 5.099
 *  Dir(x)     = [ 0.183 0.365 0.548 0.730 ]
 *
 *
 ******************************************************************************/

using System;
using System.Text;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Vector</c> class represents a <c>D</c>-dimensional Euclidean vector.
  /// Vectors are immutable: their values cannot be changed after they are created.
  /// It includes methods for addition, subtraction,
  /// dot product, scalar product, unit vector, Euclidean norm, and the Euclidean
  /// distance between two vectors.</summary>
  /// <remarks><para>For additional documentation, 
  /// see <a href="http://algs4.cs.princeton.edu/12oop">Section 1.2</a> of 
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Vector.java.html">Vector</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Vector
  {
    private int d;               // dimension of the vector
    private double[] data;       // array of vector's components

    /// <summary>Initializes a d-dimensional zero vector.</summary>
    /// <param name="D">the dimension of the vector</param>
    ///
    public Vector(int D)
    {
      this.d = D;
      data = new double[d];
    }

    /// <summary>
    /// Initializes a vector from either an array or a vararg list.
    /// The vararg syntax supports a constructor that takes a variable number of
    /// arugments such as Vector x = new Vector(1.0, 2.0, 3.0, 4.0).</summary>
    /// <param name="a">the array or vararg list</param>
    ///
    public Vector(params double[] a)
    {
      d = a.Length;

      // defensive copy so that client can't alter our copy of data[]
      data = new double[d];
      for (int i = 0; i < d; i++)
        data[i] = a[i];
    }

    /// <summary>
    /// Returns the length (dimenstion) of this vector. For better semantics, use <see cref="Dimension"/></summary>
    /// <returns>the length (dimension) of this vector</returns>
    ///
    public int Length
    {
      get { return d; }
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
    /// Returns the do product of this vector with the specified vector.</summary>
    /// <param name="that">the other vector</param>
    /// <returns>the dot product of this vector and that vector</returns>
    /// <exception cref="ArgumentException">if the dimensions of the two vectors are not equal</exception>
    ///
    public double Dot(Vector that)
    {
      if (d != that.d) throw new ArgumentException("Dimensions don't agree");
      double sum = 0.0;
      for (int i = 0; i < d; i++)
        sum = sum + (data[i] * that.data[i]);
      return sum;
    }

    /// <summary>
    /// Returns the magnitude of this vector.
    /// This is also known as the L2 norm or the Euclidean norm.</summary>
    /// <returns>the magnitude of this vector</returns>
    ///
    public double Magnitude()
    {
      return Math.Sqrt(Dot(this));
    }

    /// <summary>
    /// Returns the Euclidean distance between this vector and the specified vector.</summary>
    /// <param name="that">the other vector </param>
    /// <returns>the Euclidean distance between this vector and that vector</returns>
    /// <exception cref="ArgumentException">if the dimensions of the two vectors are not equal</exception>
    ///
    public double DistanceTo(Vector that)
    {
      if (d != that.d) throw new ArgumentException("Dimensions don't agree");
      return Minus(that).Magnitude();
    }

    /// <summary>
    /// Returns the sum of this vector and the specified vector.</summary>
    /// <param name="that">the vector to add to this vector</param>
    /// <returns>the vector whose value is <c>(this + that)</c></returns>
    /// <exception cref="ArgumentException">if the dimensions of the two vectors are not equal</exception>
    ///
    public Vector Plus(Vector that)
    {
      if (d != that.d) throw new ArgumentException("Dimensions don't agree");
      Vector c = new Vector(d);
      for (int i = 0; i < d; i++)
        c.data[i] = data[i] + that.data[i];
      return c;
    }

    /// <summary>
    /// Returns the difference between this vector and the specified vector.</summary>
    /// <param name="that">the vector to subtract from this vector</param>
    /// <returns>the vector whose value is <c>(this - that)</c></returns>
    /// <exception cref="ArgumentException">if the dimensions of the two vectors are not equal</exception>
    ///
    public Vector Minus(Vector that)
    {
      if (d != that.d) throw new ArgumentException("Dimensions don't agree");
      Vector c = new Vector(d);
      for (int i = 0; i < d; i++)
        c.data[i] = data[i] - that.data[i];
      return c;
    }

    /// <summary>
    /// Returns the ith cartesian coordinate.</summary>
    /// <param name="i">the coordinate index</param>
    /// <returns>the ith cartesian coordinate</returns>
    ///
    public double Cartesian(int i)
    {
      return data[i];
    }

    /// <summary>
    /// Returns the scalar-vector product of this vector and the specified scalar. 
    /// Use <see cref="Scale(double)"/> for better semantics.</summary>
    /// <param name="alpha">the scalar</param>
    /// <returns>the vector whose value is <c>(alpha * this)</c></returns>
    ///
    public Vector Times(double alpha)
    {
      Vector c = new Vector(d);
      for (int i = 0; i < d; i++)
        c.data[i] = alpha * data[i];
      return c;
    }

    /// <summary>
    /// Returns the scalar-vector product of this vector and the specified scalar</summary>
    /// <param name="alpha">the scalar</param>
    /// <returns>the vector whose value is <c>(alpha * this)</c></returns>
    ///
    public Vector Scale(double alpha)
    {
      Vector c = new Vector(d);
      for (int i = 0; i < d; i++)
        c.data[i] = alpha * data[i];
      return c;
    }

    /// <summary>
    /// Returns a unit vector in the direction of this vector.</summary>
    /// <returns>a unit vector in the direction of this vector</returns>
    /// <exception cref="ArithmeticException">if this vector is the zero vector</exception>
    ///
    public Vector Direction()
    {
      if (Magnitude() == 0.0) throw new ArithmeticException("Zero-vector has no direction");
      return Times(1.0 / Magnitude());
    }


    /// <summary>
    /// Returns a string representation of this vector.</summary>
    /// <returns>a string representation of this vector, which consists of the
    /// the vector entries, separates by single spaces</returns>
    ///
    public override string ToString()
    {
      StringBuilder s = new StringBuilder();
      s.Append("[ ");
      for (int i = 0; i < d; i++)
        s.Append(string.Format("{0:F3} ", data[i]));
      s.Append("]");
      return s.ToString();
    }

    /// <summary>
    /// Demo test the <c>Vector</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Vector")]
    public static void MainTest(string[] args)
    {
      double[] xdata = { 1.0, 2.0, 3.0, 4.0 };
      double[] ydata = { 5.0, 2.0, 4.0, 1.0 };
      Vector x = new Vector(xdata);
      Vector y = new Vector(ydata);

      Console.WriteLine("   x       = " + x);
      Console.WriteLine("   y       = " + y);

      Vector z = x.Plus(y);
      Console.WriteLine("   z       = " + z);

      z = z.Times(10.0);
      Console.WriteLine(" 10z       = " + z);

      Console.WriteLine("  |x|      = {0:F3}", x.Magnitude());
      Console.WriteLine(" <x, y>    = {0:F3}", x.Dot(y));
      Console.WriteLine("Dist(x, y) = {0:F3}", x.DistanceTo(y));
      Console.WriteLine("Dir(x)     = {0:F3}", x.Direction());
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
