/******************************************************************************
 *  File name :    DegreesOfSeparation.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/41graph/routes.txt
 *                http://algs4.cs.princeton.edu/41graph/movies.txt
 *  
 *  
 *  C:\> algscmd DegreesOfSeparation routes.txt " " "JFK"
 *  LAS
 *     JFK
 *     ORD
 *     DEN
 *     LAS
 *  DFW
 *     JFK
 *     ORD
 *     DFW
 *  EWR
 *     Not in database.
 *
 *  C:\> algscmd DegreesOfSeparation movies.txt "/" "Bacon, Kevin"
 *  Kidman, Nicole
 *     Bacon, Kevin
 *     Woodsman, The (2004)
 *     Grier, David Alan
 *     Bewitched (2005)
 *     Kidman, Nicole
 *  Grant, Cary
 *     Bacon, Kevin
 *     Planes, Trains & Automobiles (1987)
 *     Martin, Steve (I)
 *     Dead Men Don't Wear Plaid (1982)
 *     Grant, Cary
 *
 *  C:\> algscmd DegreesOfSeparation movies.txt "/" "Animal House (1978)"
 *  Titanic (1997)
 *     Animal House (1978)
 *     Allen, Karen (I)
 *     Raiders of the Lost Ark (1981)
 *     Taylor, Rocky (I)
 *     Titanic (1997)
 *  To Catch a Thief (1955)
 *     Animal House (1978)
 *     Vernon, John (I)
 *     Topaz (1969)
 *     Hitchcock, Alfred (I)
 *     To Catch a Thief (1955)
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>DegreesOfSeparation</c> class provides a client for finding
  /// the degree of separation between one distinguished individual and
  /// every other individual in a social network.
  /// As an example, if the social network consists of actors in which
  /// two actors are connected by a link if they appeared in the same movie,
  /// and Kevin Bacon is the distinguished individual, then the client
  /// computes the Kevin Bacon number of every actor in the network.
  /// </para><para>
  /// The running time is proportional to the number of individuals and
  /// connections in the network. If the connections are given implicitly,
  /// as in the movie network example (where every two actors are connected
  /// if they appear in the same movie), the efficiency of the algorithm
  /// is improved by allowing both movie and actor vertices and connecting
  /// each movie to all of the actors that appear in that movie.
  /// </para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/DegreesOfSeparation.java.html">DegreesOfSeparation</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class DegreesOfSeparation
  {
    // an empty class for future development
    // this class cannot be instantiated
    private DegreesOfSeparation() { }

    /// <summary>Reads in a social network from a file, and then repeatedly reads in
    /// individuals from standard input and prints out their degrees of
    /// separation.
    /// Takes three command-line arguments: the name of a file,
    /// a delimiter, and the name of the distinguished individual.
    /// Each line in the file contains the name of a vertex, followed by a
    /// list of the names of the vertices adjacent to that vertex,
    /// separated by the delimiter.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd DegreesOfSeparation routes.txt \" \" \"JFK\"",
      "File format with symbols and their adjacents, separated by a delimiter and a star symbol")]
    public static void MainTest(string[] args)
    {
      string filename = args[0];
      string delimiter = args[1];
      string source = args[2];

      SymbolGraph sg = new SymbolGraph(filename, delimiter);
      Graph G = sg.G;

      if (!sg.Contains(source))
      {
        Console.WriteLine(source + " not in database.");
        return;
      }

      int s = sg.Index(source);
      BreadthFirstPaths bfs = new BreadthFirstPaths(G, s);

      TextInput StdIn = new TextInput();
      while (!StdIn.IsEmpty)
      {
        string sink = StdIn.ReadLine();
        if (sg.Contains(sink))
        {
          int t = sg.Index(sink);
          if (bfs.HasPathTo(t))
          {
            foreach (int v in bfs.PathTo(t))
            {
              Console.WriteLine("   " + sg.Name(v));
            }
          }
          else
          {
            Console.WriteLine("Not connected");
          }
        }
        else
        {
          Console.WriteLine("   Not in database.");
        }
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
