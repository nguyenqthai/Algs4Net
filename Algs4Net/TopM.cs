/******************************************************************************
 *  File name :    TopM.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/24pq/tinyBatch.txt
 *
 *  Given an integer M from the command line and an input stream where
 *  each line contains a string and a long value, this MinPQ client
 *  prints the M lines whose numbers are the highest.
 *
 *  C:\> algscmd TopM 5 < tinyBatch.txt
 *  Thompson   2/27/2000 12:00:00 AM  4747.08
 *  vonNeumann 2/12/1994 12:00:00 AM  4732.35
 *  vonNeumann 1/11/1999 12:00:00 AM  4409.74
 *  Hoare      8/18/1992 12:00:00 AM  4381.21
 *  vonNeumann 3/26/2002 12:00:00 AM  4121.85
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>TopM</c> class provides a client that reads a sequence of
  /// transactions from standard input and prints the <c>M</c> largest ones
  /// to standard output. This implementation uses a <see cref="MinPQ{Key}"/> of size
  /// at most <c>M</c> + 1 to identify the <c>M</c> largest transactions
  /// and a <see cref="LinkedStack{Item}"/> to output them in the proper order.</summary>
  /// <remarks>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/24pq">Section 2.4</a>
  /// of <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/TopM.java.html">TopM</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks>
  ///
  ///
  public class TopM
  {
    // This class should not be instantiated.
    private TopM() { }

    /// <summary>
    /// Reads a sequence of transactions from standard input; takes a
    /// command-line integer M; prints to standard output the M largest
    /// transactions in descending order.</summary>
    ///
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd TopM M < tinyBatch.txt", "The top M transactions from input")]
    public static void MainTest(string[] args)
    {
      int M = int.Parse(args[0]);
      MinPQ<Transaction> pq = new MinPQ<Transaction>(M + 1);

      TextInput StdIn = new TextInput();
      while (!StdIn.IsEmpty)
      {
        // Create an entry from the next line and put on the PQ.
        string line = StdIn.ReadLine();
        Transaction transaction = new Transaction(line);
        pq.Insert(transaction);

        // remove minimum if M+1 entries on the PQ
        if (pq.Count > M)
          pq.DelMin();
      }   // top M entries are on the PQ

      // print entries on PQ in reverse order
      LinkedStack<Transaction> stack = new LinkedStack<Transaction>();
      foreach (Transaction transaction in pq)
        stack.Push(transaction);
      foreach (Transaction transaction in stack)
        Console.WriteLine(transaction);
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
