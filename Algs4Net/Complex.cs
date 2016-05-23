/******************************************************************************
 *  File name :    Complex.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Data type for complex numbers.
 *
 *  The data type is "immutable" so once you create and initialize
 *  a Complex object, you cannot change it. The "final" keyword
 *  when declaring re and im enforces this rule, making it a
 *  compile-time error to change the .re or .im fields after
 *  they've been initialized.
 *
 *  C:\> algscmd Complex
 *  a            = 5.0 + 6.0i
 *  b            = -3.0 + 4.0i
 *  Re(a)        = 5.0
 *  Im(a)        = 6.0
 *  b + a        = 2.0 + 10.0i
 *  a - b        = 8.0 + 2.0i
 *  a * b        = -39.0 + 2.0i
 *  b * a        = -39.0 + 2.0i
 *  a / b        = 0.36 - 1.52i
 *  (a / b) * b  = 5.0 + 6.0i
 *  conj(a)      = 5.0 - 6.0i
 *  |a|          = 7.810249675906654
 *  tan(a)       = -6.685231390246571E-6 + 1.0000103108981198i
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>The <c>Complex</c> class represents a complex number.
  /// Complex numbers are immutable: their values cannot be changed after they
  /// are created. It includes methods for addition, subtraction, multiplication, 
  /// division, conjugation, and other common functions on complex numbers.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/99scientific">Section 9.9</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Complex.java.html">Complex</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Complex
  {
    private readonly double re;   // the real part
    private readonly double im;   // the imaginary part

    /// <summary>
    /// Initializes a complex number from the specified real and imaginary parts.</summary>
    /// <param name="real">the real part</param>
    /// <param name="imag">the imaginary part</param>
    ///
    public Complex(double real, double imag)
    {
      re = real;
      im = imag;
    }

    /// <summary>
    /// Returns the real part of this complex number.</summary>
    /// <returns>the real part of this complex number</returns>
    ///
    public double Re
    {
      get { return re; }
    }

    /// <summary>
    /// Returns the imaginary part of this complex number.</summary>
    /// <returns>the imaginary part of this complex number</returns>
    ///
    public double Im
    {
      get { return im; }
    }

    /// <summary>
    /// Returns a string representation of this complex number.</summary>
    /// <returns>a string representation of this complex number,
    ///        of the form 34 - 56i.</returns>
    ///
    public override string ToString()
    {
      if (im == 0) return re + "";
      if (re == 0) return im + "i";
      if (im < 0) return re + " - " + (-im) + "i";
      return re + " + " + im + "i";
    }

    // standard arithmetic operators

    /// <summary>
    /// Returns the sum of this complex number and the specified complex number.</summary>
    /// <param name="opand1">the first complex number</param>
    /// <param name="opand2">the second complex number</param>
    /// <returns>the complex number whose value is <c>(opand1 + opand2)</c></returns>
    ///
    public static Complex operator +(Complex opand1, Complex opand2)
    {
      double real = opand1.re + opand2.re;
      double imag = opand1.im + opand2.im;
      return new Complex(real, imag);
    }

    /// <summary>
    /// Returns the result of subtracting the specified complex number from
    /// this complex number.</summary>
    /// <param name="opand1">the first complex number</param>
    /// <param name="opand2">the second complex number</param>
    /// <returns>the complex number whose value is <c>(opand1 - opand2)</c></returns>
    ///
    public static Complex operator -(Complex opand1, Complex opand2)
    {
      double real = opand1.re - opand2.re;
      double imag = opand1.im - opand2.im;
      return new Complex(real, imag);
    }

    /// <summary>
    /// Returns the product of this complex number and the specified complex number.</summary>
    /// <param name="opand1">the first complex number</param>
    /// <param name="opand2">the second complex number</param>
    /// <returns>the complex number whose value is <c>(opand1 * opand2)</c></returns>
    ///
    public static Complex operator *(Complex opand1, Complex opand2)
    {
      double real = opand1.re * opand2.re - opand1.im * opand2.im;
      double imag = opand1.re * opand2.im + opand1.im * opand2.re;
      return new Complex(real, imag);
    }

    /// <summary>
    /// Returns the result of dividing the specified complex number into
    /// this complex number.</summary>
    /// <param name="opand1">the first complex number</param>
    /// <param name="opand2">the second complex number</param>
    /// <returns>the complex number whose value is <c>(opand1 / opand2)</c></returns>
    ///
    public static Complex operator /(Complex opand1, Complex opand2)
    {
      return opand1 * (opand2.Reciprocal());
    }
    
    // typical standard math methods

    /// <summary>Returns the absolute value of this complex number, or angle/phase/argument.
    /// This quantity is also known as the <c>Modulus</c> or <c>Magnitude</c>.</summary>
    /// <returns>the absolute value of this complex number</returns>
    ///
    public double Abs()
    {
      return Math.Sqrt(re*re + im*im);
    }

    /// <summary>Returns the phase of this complex number.
    /// This quantity is also known as the <c>Ange</c> or <c>Argument</c>.</summary>
    /// <returns>the phase of this complex number, a real number between -pi and pi</returns>
    ///
    public double Phase()
    {
      return Math.Atan2(im, re);
    }

    /// <summary>
    /// Returns the product of this complex number and the specified scalar.</summary>
    /// <param name="alpha">the scalar</param>
    /// <returns>the complex number whose value is <c>(alpha*this)</c></returns>
    ///
    public Complex Scale(double alpha)
    {
      return new Complex(alpha * re, alpha * im);
    }

    /// <summary>
    /// Returns the complex conjugate of this complex number.</summary>
    /// <returns>the complex conjugate of this complex number</returns>
    ///
    public Complex Conjugate()
    {
      return new Complex(re, -im);
    }

    /// <summary>
    /// Returns the reciprocal of this complex number.</summary>
    /// <returns>the complex number whose value is <c>(1 / this)</c></returns>
    ///
    public Complex Reciprocal()
    {
      double scale = re * re + im * im;
      return new Complex(re / scale, -im / scale);
    }

    /// <summary>
    /// Returns the complex exponential of this complex number.</summary>
    /// <returns>the complex exponential of this complex number</returns>
    ///
    public Complex Exp()
    {
      return new Complex(Math.Exp(re) * Math.Cos(im), Math.Exp(re) * Math.Sin(im));
    }

    /// <summary>
    /// Returns the complex sine of this complex number.</summary>
    /// <returns>the complex sine of this complex number</returns>
    ///
    public Complex Sin()
    {
      return new Complex(Math.Sin(re) * Math.Cosh(im), Math.Cos(re) * Math.Sinh(im));
    }

    /// <summary>
    /// Returns the complex cosine of this complex number.</summary>
    /// <returns>the complex cosine of this complex number</returns>
    ///
    public Complex Cos()
    {
      return new Complex(Math.Cos(re) * Math.Cosh(im), -Math.Sin(re) * Math.Sinh(im));
    }

    /// <summary>
    /// Returns the complex tangent of this complex number.</summary>
    /// <returns>the complex tangent of this complex number</returns>
    ///
    public Complex Tan()
    {
      return Sin()/(Cos());
    }

    /// <summary>
    /// Demo test the <c>Complex</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Complex")]
    public static void MainTest(string[] args)
    {
      Complex a = new Complex(5.0, 6.0);
      Complex b = new Complex(-3.0, 4.0);

      Console.WriteLine("a            = " + a);
      Console.WriteLine("b            = " + b);
      Console.WriteLine("Re(a)        = " + a.Re);
      Console.WriteLine("Im(a)        = " + a.Im);
      Console.WriteLine("b + a        = " + (b + a));
      Console.WriteLine("a - b        = " + (a - b));
      Console.WriteLine("a * b        = " + (a * b));
      Console.WriteLine("b * a        = " + (b * a));
      Console.WriteLine("a / b        = " + (a / b));
      Console.WriteLine("(a / b) * b  = " + ((a / b) * b));
      Console.WriteLine("conj(a)      = " + a.Conjugate());
      Console.WriteLine("|a|          = " + a.Abs());
      Console.WriteLine("tan(a)       = " + a.Tan());
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
