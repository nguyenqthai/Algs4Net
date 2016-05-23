/******************************************************************************
 *  File name :    LinearRegression.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Compute least squares solution to y = beta * x + alpha.
 *  Simple linear regression.
 *  
 *  C:\> algscmd LinearRegression
 *  The model is 1.41 N + 1.41  (R^2 = 0.983)
 *  With x = 55, y could be 83.5464285714286
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>LinearRegression</c> class performs a simple linear regression
  /// on an set of <c>N</c> data points (<c>Y<sub>i</sub></c>, <c>X<sub>i</sub></c>).
  /// That is, it fits a straight line <c>Y</c> = α + β <c>X</c>,
  /// (where <c>Y</c> is the response variable, <c>X</c> is the predictor variable,
  /// α is the <c>Y-intercept</c>, and β is the <c>Slope</c>)
  /// that minimizes the sum of squared residuals of the linear regression model.
  /// It also computes associated statistics, including the coefficient of
  /// determination <c>R</c><sup>2</sup> and the standard deviation of the
  /// estimates for the slope and <c>Y</c>-intercept.</summary>
  ///
  public class LinearRegression
  {
    private readonly int N;
    private readonly double intercept, slope;
    private readonly double r2;
    private readonly double svar, svar0, svar1;

    /// <summary>
    /// Performs a linear regression on the data points <c>(y[i], x[i])</c>.</summary>
    /// <param name="x">the values of the predictor variable</param>
    /// <param name="y">the corresponding values of the response variable</param>
    /// <exception cref="ArgumentException">if the lengths of the two arrays are not equal</exception>
    ///
    public LinearRegression(double[] x, double[] y)
    {
      if (x.Length != y.Length)
      {
        throw new ArgumentException("array lengths are not equal");
      }
      N = x.Length;

      // first pass
      double sumx = 0.0, sumy = 0.0, sumx2 = 0.0;
      for (int i = 0; i < N; i++)
      {
        sumx += x[i];
        sumx2 += x[i] * x[i];
        sumy += y[i];
      }
      double xbar = sumx / N;
      double ybar = sumy / N;

      // second pass: compute summary statistics
      double xxbar = 0.0, yybar = 0.0, xybar = 0.0;
      for (int i = 0; i < N; i++)
      {
        xxbar += (x[i] - xbar) * (x[i] - xbar);
        yybar += (y[i] - ybar) * (y[i] - ybar);
        xybar += (x[i] - xbar) * (y[i] - ybar);
      }
      slope = xybar / xxbar;
      intercept = ybar - slope * xbar;

      // more statistical analysis
      double rss = 0.0;      // residual sum of squares
      double ssr = 0.0;      // regression sum of squares
      for (int i = 0; i < N; i++)
      {
        double fit = slope * x[i] + intercept;
        rss += (fit - y[i]) * (fit - y[i]);
        ssr += (fit - ybar) * (fit - ybar);
      }

      int degreesOfFreedom = N - 2;
      r2 = ssr / yybar;
      svar = rss / degreesOfFreedom;
      svar1 = svar / xxbar;
      svar0 = svar / N + xbar * xbar * svar1;
    }

    /// <summary>
    /// Returns the <c>Y</c>-intercept α of the best of the best-fit line <c>Y</c> = α + β <c>X</c>.</summary>
    /// <returns>the <c>Y</c>-intercept α of the best-fit line <c>Y = α + β x</c></returns>
    ///
    public double Intercept
    {
      get { return intercept; }
    }

    /// <summary>
    /// Returns the slope β of the best of the best-fit line <c>Y</c> = α + β <c>X</c>.</summary>
    /// <returns>the slope β of the best-fit line <c>Y</c> = α + β <c>X</c></returns>
    ///
    public double Slope
    {
      get { return slope; }
    }

    /// <summary>
    /// Returns the coefficient of determination <c>R</c><sup>2</sup>.</summary>
    /// <returns>the coefficient of determination <c>R</c><sup>2</sup>,
    ///        which is a real number between 0 and 1</returns>
    ///
    public double R2
    {
      get { return r2; }
    }

    /// <summary>
    /// Returns the standard error of the estimate for the intercept.</summary>
    /// <returns>the standard error of the estimate for the intercept</returns>
    ///
    public double InterceptStdErr
    {
      get { return Math.Sqrt(svar0); }
    }

    /// <summary>
    /// Returns the standard error of the estimate for the slope.</summary>
    /// <returns>the standard error of the estimate for the slope</returns>
    ///
    public double SlopeStdErr
    {
      get { return Math.Sqrt(svar1); }
    }

    /// <summary>
    /// Returns the expected response <c>y</c> given the value of the predictor
    /// variable <c>x</c>.</summary>
    /// <param name="x">the value of the predictor variable</param>
    /// <returns>the expected response <c>y</c> given the value of the predictor
    ///        variable <c>x</c></returns>
    ///
    public double Predict(double x)
    {
      return slope * x + intercept;
    }

    /// <summary>
    /// Returns a string representation of the simple linear regression model.</summary>
    /// <returns>a string representation of the simple linear regression model,
    ///        including the best-fit line and the coefficient of determination
    ///        <c>R</c><sup>2</sup></returns>
    ///
    public override string ToString()
    {
      string s = "";
      s += string.Format("{0:F2} N + {0:F2}", Slope, Intercept);
      return s + "  (R^2 = " + string.Format("{0:F3}", R2) + ")";
    }

    /// <summary>
    /// Demo test the <c>LinearRegression</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd LinearRegression")]
    public static void MainTest(string[] args)
    {
      double[] x = { 3, 4, 5, 6, 7, 8, 9, 10 };
      double[] y = { 9.8, 11.8, 13.3, 15.2, 16.2, 17.9, 18.2, 20 };

      LinearRegression lr = new LinearRegression(x, y);
      Console.WriteLine("The model is {0}", lr.ToString());
      Console.WriteLine("With x = {0}, y could be {1}", 55, lr.Predict(55));
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
