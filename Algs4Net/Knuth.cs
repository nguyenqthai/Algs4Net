/******************************************************************************
 *  File name :    Knuth.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/11model/cards.txt
 *                http://algs4.cs.princeton.edu/11model/cardsUnicode.txt
 *  
 *  Reads in a list of strings and prints them in random order.
 *  The Knuth (or Fisher-Yates) shuffling algorithm guarantees
 *  to rearrange the elements in uniformly random order, under
 *  the assumption that Math.random() generates independent and
 *  uniformly distributed numbers between 0 and 1.
 *
 *  C:\> type cards.txt
 *  2C 3C 4C 5C 6C 7C 8C 9C 10C JC QC KC AC
 *  2D 3D 4D 5D 6D 7D 8D 9D 10D JD QD KD AD
 *  2H 3H 4H 5H 6H 7H 8H 9H 10H JH QH KH AH
 *  2S 3S 4S 5S 6S 7S 8S 9S 10S JS QS KS AS
 *
 *  C:\> algscmd Knuth < cards.txt
 *  6H
 *  9C
 *  8H
 *  7C
 *  JS
 *  ...
 *  KH
 *
 *  C:\> type cardsUnicode.txt (Unicode display works in a Windows PowerShell)
 *  2♣ 3♣ 4♣ 5♣ 6♣ 7♣ 8♣ 9♣ 10♣ J♣ Q♣ K♣ A♣ 
 *  2♦ 3♦ 4♦ 5♦ 6♦ 7♦ 8♦ 9♦ 10♦ J♦ Q♦ K♦ A♦ 
 *  2♥ 3♥ 4♥ 5♥ 6♥ 7♥ 8♥ 9♥ 10♥ J♥ Q♥ K♥ A♥ 
 *  2♠ 3♠ 4♠ 5♠ 6♠ 7♠ 8♠ 9♠ 10♠ J♠ Q♠ K♠ A♠ 
 * 
 *  C:\> algscmd Knuth < cardsUnicode.txt
 *  2♠
 *  K♥
 *  6♥
 *  5♣
 *  J♣
 *  ...
 *  A♦
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>Knuth</c> class provides a client for reading in a
  /// sequence of strings and <c>Shuffling</c> them using the Knuth (or Fisher-Yates)
  /// shuffling algorithm. This algorithm guarantees to rearrange the
  /// elements in uniformly random order, under
  /// the assumption that Math.random() generates independent and
  /// uniformly distributed numbers between 0 and 1.</para><para>
  /// See <seealso cref="StdRandom"/> for versions that shuffle arrays and
  /// subarrays of objects, doubles, and ints.</para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/11model">Section 1.1</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Knuth.java.html">Knuth</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Knuth
  {
    // this class should not be instantiated
    private Knuth() { }

    /// <summary>
    /// Rearranges an array of objects in uniformly random order
    /// (under the assumption that <c>Math.random()</c> generates independent
    /// and uniformly distributed numbers between 0 and 1).</summary>
    /// <param name="a">the array to be shuffled</param>
    ///
    public static void Shuffle(object[] a)
    {
      int N = a.Length;
      Random rnd = new Random(DateTime.Now.Millisecond);
      for (int i = 0; i < N; i++)
      {
        // choose index uniformly in [i, N-1]
        int r = rnd.Next(i, N);
        object swap = a[r];
        a[r] = a[i];
        a[i] = swap;
      }
    }

    /// <summary>
    /// Reads in a sequence of strings from standard input, shuffles
    /// them, and prints out the results.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Knuth < cards.txt", "A list of strings to display in random order")]
    public static void MainTest(string[] args)
    {
      // read in the data
      TextInput StdIn = new TextInput();
      string[] a = StdIn.ReadAllStrings();

      // shuffle the array
      Knuth.Shuffle(a);
      // print results.
      for (int i = 0; i < a.Length; i++)
        Console.WriteLine(a[i]);
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
