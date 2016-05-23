/******************************************************************************
 *  File name :    Arbitrage.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *                BellmanFordSP.java
 *  Data file:    http://algs4.cs.princeton.edu/44sp/rates.txt
 *
 *  Arbitrage detection.
 *
 *  C:\> type rates.txt
 *  5
 *  USD 1      0.741  0.657  1.061  1.005
 *  EUR 1.349  1      0.888  1.433  1.366
 *  GBP 1.521  1.126  1      1.614  1.538
 *  CHF 0.942  0.698  0.619  1      0.953
 *  CAD 0.995  0.732  0.650  1.049  1
 *
 *  C:\> algscmd Arbitrage < rates.txt
 *  1000.00000 USD =  741.00000 EUR
 *   741.00000 EUR = 1012.20600 CAD
 *  1012.20600 CAD = 1007.14497 USD
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>Arbitrage</c> class provides a client that finds an arbitrage
  /// opportunity in a currency exchange table by constructing a
  /// complete-digraph representation of the exchange table and then finding
  /// a negative cycle in the digraph.
  /// </para><para>
  /// This implementation uses the Bellman-Ford algorithm to find a
  /// negative cycle in the complete digraph.
  /// The running time is proportional to <c>V</c><sup>3</sup> in the
  /// worst case, where <c>V</c> is the number of currencies.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/44sp">Section 4.4</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Arbitrage.java.html">Arbitrage</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Arbitrage
  {

    // this class cannot be instantiated
    private Arbitrage() { }

    /// <summary>
    /// Reads the currency exchange table from standard input and
    /// prints an arbitrage opportunity to standard output (if one exists).
    /// </summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Arbitrage < rates.txt", "File with the format for the application")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();
      // V currencies
      int V = StdIn.ReadInt();
      string[] name = new string[V];

      // create complete network
      EdgeWeightedDigraph G = new EdgeWeightedDigraph(V);
      for (int v = 0; v < V; v++)
      {
        name[v] = StdIn.ReadString();
        for (int w = 0; w < V; w++)
        {
          double rate = StdIn.ReadDouble();
          DirectedEdge e = new DirectedEdge(v, w, -Math.Log(rate));
          G.AddEdge(e);
        }
      }

      // find negative cycle
      BellmanFordSP spt = new BellmanFordSP(G, 0);
      if (spt.HasNegativeCycle)
      {
        double stake = 1000.0;
        foreach (DirectedEdge e in spt.GetNegativeCycle())
        {
          Console.Write("{0,10:F5} {1} ", stake, name[e.From]);
          stake *= Math.Exp(-e.Weight);
          Console.Write("= {0,10:F5} {1}\n", stake, name[e.To]);
        }
      }
      else
      {
        Console.WriteLine("No arbitrage opportunity");
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
