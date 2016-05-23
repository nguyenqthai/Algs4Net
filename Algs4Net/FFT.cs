/******************************************************************************
 *  File name :    FFT.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Compute the FFT and inverse FFT of a length N complex sequence.
 *  Bare bones implementation that runs in O(N log N) time. Our goal
 *  is to optimize the clarity of the code, rather than performance.
 *
 *  Limitations
 *  -----------
 *   -  assumes N is a power of 2
 *
 *   -  not the most memory efficient algorithm (because it uses
 *      an object type for representing complex numbers and because
 *      it re-allocates memory for the subarray, instead of doing
 *      in-place or reusing a single temporary array)
 *  
 *
 *  C:\> algscmd FFT 4
 *  x
 *  -------------------
 *  -0.03480425839330703
 *  0.07910192950176387
 *  0.7233322451735928
 *  0.1659819820667019
 *
 *  y = fft(x)
 *  -------------------
 *  0.9336118983487516
 *  -0.7581365035668999 + 0.08688005256493803i
 *  0.44344407521182005
 *  -0.7581365035668999 - 0.08688005256493803i
 *
 *  z = ifft(y)
 *  -------------------
 *  -0.03480425839330703
 *  0.07910192950176387 + 2.6599344570851287E-18i
 *  0.7233322451735928
 *  0.1659819820667019 - 2.6599344570851287E-18i
 *
 *  c = cconvolve(x, x)
 *  -------------------
 *  0.5506798633981853
 *  0.23461407150576394 - 4.033186818023279E-18i
 *  -0.016542951108772352
 *  0.10288019294318276 + 4.033186818023279E-18i
 *
 *  d = convolve(x, x)
 *  -------------------
 *  0.001211336402308083 - 3.122502256758253E-17i
 *  -0.005506167987577068 - 5.058885073636224E-17i
 *  -0.044092969479563274 + 2.1934338938072244E-18i
 *  0.10288019294318276 - 3.6147323062478115E-17i
 *  0.5494685269958772 + 3.122502256758253E-17i
 *  0.240120239493341 + 4.655566391833896E-17i
 *  0.02755001837079092 - 2.1934338938072244E-18i
 *  4.01805098805014E-17i
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>FFT</c> class provides methods for computing the
  /// FFT (Fast-Fourier Transform), inverse FFT, linear convolution,
  /// and circular convolution of a complex array.
  /// </para><para>
  /// It is a bare-bones implementation that runs in <c>N</c> log <c>N</c> time,
  /// where <c>N</c> is the length of the complex array. For simplicity,
  /// <c>N</c> must be a power of 2.</para><para>
  /// Our goal is to optimize the clarity of the code, rather than performance.
  /// It is not the most memory efficient implementation because it uses
  /// objects to represents complex numbers and it it re-allocates memory
  /// for the subarray, instead of doing in-place or reusing a single temporary array.
  /// </para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/99scientific">Section 9.9</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/FFT.java.html">FFT</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class FFT
  {

    private static readonly Complex ZERO = new Complex(0, 0);

    // Do not instantiate.
    private FFT() { }

    /// <summary>Returns the FFT of the specified complex array.</summary>
    /// <param name="x">the complex array</param>
    /// <returns>the FFT of the complex array <c>x</c></returns>
    /// <exception cref="ArgumentException">if the length of <c>x</c> is not a power of 2</exception>
    ///
    public static Complex[] Fft(Complex[] x)
    {
      int N = x.Length;

      // base case
      if (N == 1) return new Complex[] { x[0] };

      // radix 2 Cooley-Tukey FFT
      if (N % 2 != 0)
      {
        throw new ArgumentException("N is not a power of 2");
      }

      // fft of even terms
      Complex[] even = new Complex[N / 2];
      for (int k = 0; k < N / 2; k++)
      {
        even[k] = x[2 * k];
      }
      Complex[] q = Fft(even);

      // fft of odd terms
      Complex[] odd = even;  // reuse the array
      for (int k = 0; k < N / 2; k++)
      {
        odd[k] = x[2 * k + 1];
      }
      Complex[] r = Fft(odd);

      // combine
      Complex[] y = new Complex[N];
      for (int k = 0; k < N / 2; k++)
      {
        double kth = -2 * k * Math.PI / N;
        Complex wk = new Complex(Math.Cos(kth), Math.Sin(kth));
        y[k] = q[k] + (wk * r[k]);
        y[k + N / 2] = q[k] - (wk * r[k]);
      }
      return y;
    }


    /// <summary>
    /// Returns the inverse FFT of the specified complex array.</summary>
    /// <param name="x">the complex array</param>
    /// <returns>the inverse FFT of the complex array <c>x</c></returns>
    /// <exception cref="ArgumentException">if the length of <c>x</c> is not a power of 2</exception>
    ///
    public static Complex[] Ifft(Complex[] x)
    {
      int N = x.Length;
      Complex[] y = new Complex[N];

      // take conjugate
      for (int i = 0; i < N; i++)
      {
        y[i] = x[i].Conjugate();
      }

      // compute forward FFT
      y = Fft(y);

      // take conjugate again
      for (int i = 0; i < N; i++)
      {
        y[i] = y[i].Conjugate();
      }

      // divide by N
      for (int i = 0; i < N; i++)
      {
        y[i] = y[i].Scale(1.0 / N);
      }

      return y;

    }

    /// <summary>
    /// Returns the circular convolution of the two specified complex arrays.</summary>
    /// <param name="x">one complex array</param>
    /// <param name="y">the other complex array</param>
    /// <returns>the circular convolution of <c>x</c> and <c>y</c></returns>
    /// <exception cref="ArgumentException">if the length of <c>x</c> does not equal
    /// the length of <c>y</c> or if the length is not a power of 2</exception>
    ///
    public static Complex[] Cconvolve(Complex[] x, Complex[] y)
    {

      // should probably pad x and y with 0s so that they have same length
      // and are powers of 2
      if (x.Length != y.Length)
      {
        throw new ArgumentException("Dimensions don't agree");
      }

      int N = x.Length;

      // compute FFT of each sequence
      Complex[] a = Fft(x);
      Complex[] b = Fft(y);

      // point-wise multiply
      Complex[] c = new Complex[N];
      for (int i = 0; i < N; i++)
      {
        c[i] = a[i] * b[i];
      }

      // compute inverse FFT
      return Ifft(c);
    }

    /// <summary>
    /// Returns the linear convolution of the two specified complex arrays.</summary>
    /// <param name="x">one complex array</param>
    /// <param name="y">the other complex array</param>
    /// <returns>the linear convolution of <c>x</c> and <c>y</c></returns>
    /// <exception cref="ArgumentException">if the length of <c>x</c> does not equal
    /// the length of <c>y</c> or if the length is not a power of 2</exception>
    ///
    public static Complex[] Convolve(Complex[] x, Complex[] y)
    {
      Complex[] a = new Complex[2 * x.Length];
      for (int i = 0; i < x.Length; i++)
        a[i] = x[i];
      for (int i = x.Length; i < 2 * x.Length; i++)
        a[i] = ZERO;

      Complex[] b = new Complex[2 * y.Length];
      for (int i = 0; i < y.Length; i++)
        b[i] = y[i];
      for (int i = y.Length; i < 2 * y.Length; i++)
        b[i] = ZERO;

      return Cconvolve(a, b);
    }

    // display an array of Complex numbers to standard output
    internal static void Show(Complex[] x, string title)
    {
      Console.WriteLine(title);
      Console.WriteLine("-------------------");
      for (int i = 0; i < x.Length; i++)
      {
        Console.WriteLine(x[i]);
      }
      Console.WriteLine();
    }

    /// <summary>
    /// Demo test the <c>FFT</c> class.</summary>
    /// <param name="args">Place holder for user arguments</param>
    [HelpText("algscmd FFT N", "Where N is power of two")]
    public static void MainTest(string[] args)
    {
      int N = int.Parse(args[0]);
      Complex[] x = new Complex[N];

      // original data
      Random rnd = new Random();
      for (int i = 0; i < N; i++)
      {
        x[i] = new Complex(i, 0);
        x[i] = new Complex(-2 * StdRandom.Uniform() + 1, 0);
      }
      FFT.Show(x, "x");

      // FFT of original data
      Complex[] y = FFT.Fft(x);
      FFT.Show(y, "y = fft(x)");

      // take inverse FFT
      Complex[] z = FFT.Ifft(y);
      FFT.Show(z, "z = ifft(y)");

      // circular convolution of x with itself
      Complex[] c = FFT.Cconvolve(x, x);
      FFT.Show(c, "c = cconvolve(x, x)");

      // linear convolution of x with itself
      Complex[] d = FFT.Convolve(x, x);
      FFT.Show(d, "d = convolve(x, x)");
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
