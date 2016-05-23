/******************************************************************************
 *  File name :    Multiway.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Merges together the sorted input stream given as command-line arguments
 *  into a single sorted output stream on standard output.
 *
 *  C:\> type m1.txt 
 *  A B C F G I I Z
 *
 *  C:\> type m2.txt 
 *  B D H P Q Q
 * 
 *  C:\> type m3.txt 
 *  A B E F J N
 *
 *  C:\> algscmd Multiway m1.txt m2.txt m3.txt 
 *  A A B B B C D E F F G H I I J N P Q Q Z 
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Multiway</c> class provides a client for reading in several
  /// sorted text files and merging them together into a single sorted
  /// text stream.
  /// This implementation uses a <seealso cref="IndexMinPQ{Key}"/> to perform the multiway
  /// merge.</summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/24pq">Section 2.4</a>
  /// of <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Multiway.java.html">Multiway</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Multiway
  {
    // This class should not be instantiated.
    private Multiway() { }

    /// <summary>merge together the sorted input streams and write the sorted
    /// result to standard output.</summary>
    /// <param name="streams">opened stream from user</param>
    public static void Merge(TextInput[] streams)
    {
      int N = streams.Length;
      IndexMinPQ<string> pq = new IndexMinPQ<string>(N);
      for (int i = 0; i < N; i++)
        if (!streams[i].IsEmpty)
        {
          string s = streams[i].ReadString();
          if (!s.Equals("")) pq.Insert(i, s);
        }
      // Extract and print min and read next from its stream. 
      while (!pq.IsEmpty)
      {
        Console.Write(pq.MinKey + " ");
        int i = pq.DelMin();
        if (!streams[i].IsEmpty)
        {
          string s = streams[i].ReadString();
          if (!s.Equals("")) pq.Insert(i, s);
        }
      }
      Console.WriteLine();
    }

    /// <summary>
    /// Reads sorted text files specified as command-line arguments;
    /// merges them together into a sorted output; and writes
    /// the results to standard output.
    /// Note: this client does not check that the input files are sorted.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Multiway m1.txt m2.txt m3.txt", "List of sorted files to merge")]
    public static void MainTest(string[] args)
    {
      int N = args.Length;
      TextInput[] streams = new TextInput[N];
      for (int i = 0; i < N; i++)
        streams[i] = new TextInput(args[i]);
      Multiway.Merge(streams);
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
