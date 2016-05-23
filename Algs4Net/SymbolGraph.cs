/******************************************************************************
 *  File name :    SymbolGraph.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *  Data files:   http://algs4.cs.princeton.edu/41graph/routes.txt
 *                http://algs4.cs.princeton.edu/41graph/movies.txt
 *                http://algs4.cs.princeton.edu/41graph/moviestiny.txt
 *                http://algs4.cs.princeton.edu/41graph/moviesG.txt
 *                http://algs4.cs.princeton.edu/41graph/moviestopGrossing.txt
 *  
 *  C:\> algscmd SymbolGraph routes.txt " "
 *  JFK
 *     MCO
 *     ATL
 *     ORD
 *  LAX
 *     PHX
 *     LAS
 *
 *  C:\> algscmd SymbolGraph movies.txt "/"
 *  Tin Men (1987)
 *     Hershey, Barbara
 *     Geppi, Cindy
 *     Jones, Kathy (II)
 *     Herr, Marcia
 *     ...
 *     Blumenfeld, Alan
 *     DeBoy, David
 *  Bacon, Kevin
 *     Woodsman, The (2004)
 *     Wild Things (1998)
 *     Where the Truth Lies (2005)
 *     Tremors (1990)
 *     ...
 *     Apollo 13 (1995)
 *     Animal House (1978)
 *
 * 
 *  Assumes that input file is encoded using UTF-8.
 *  % iconv -f ISO-8859-1 -t UTF-8 movies-iso8859.txt > movies.txt
 *
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>SymbolGraph</c> class represents an undirected graph, where the
  /// vertex names are arbitrary strings.
  /// By providing mappings between string vertex names and integers,
  /// it serves as a wrapper around the
  /// <seealso cref="Graph"/> data type, which assumes the vertex names are integers
  /// between 0 and <c>V</c> - 1.
  /// It also supports initializing a symbol graph from a file.
  /// </para><para>
  /// This implementation uses an <seealso cref="ST{Key, Value}"/> to map from strings to integers,
  /// an array to map from integers to strings, and a <seealso cref="Graph"/> to store
  /// the underlying graph.
  /// The <c>Index</c> and <c>Contains</c> operations take time 
  /// proportional to log <c>V</c>, where <c>V</c> is the number of vertices.
  /// The <c>Name</c> operation takes constant time.
  /// </para></summary>
  /// <remarks><para>
  /// For additional documentation, see <a href="http://algs4.cs.princeton.edu/41graph">Section 4.1</a> of
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/SymbolGraph.java.html">SymbolGraph</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class SymbolGraph
  {
    private ST<string, int> st;   // string -> index
    private string[] keys;        // index  -> string
    private Graph innerGraph;

    /// <summary>
    /// Initializes a graph from a file using the specified delimiter.
    /// Each line in the file contains
    /// the name of a vertex, followed by a list of the names
    /// of the vertices adjacent to that vertex, separated by the delimiter.</summary>
    /// <param name="filename">the name of the file</param>
    /// <param name="delimiter">the delimiter between fields</param>
    ///
    public SymbolGraph(string filename, string delimiter)
    {
      st = new ST<string, int>();

      // First pass builds the index by reading strings to associate
      // distinct strings with an index
      TextInput input = new TextInput(filename);
      // while (in.hasNextLine()) {
      while (!input.IsEmpty) {
        string[] a = input.ReadLine().Split(delimiter.ToCharArray());
        for (int i = 0; i < a.Length; i++)
        {
          if (!st.Contains(a[i]))
            st[a[i]] = st.Count;
        }
      }
      Console.WriteLine("Done reading " + filename);

      // inverted index to get string keys in an aray
      keys = new string[st.Count];
      foreach (string name in st.Keys())
      {
        keys[st[name]] = name;
      }

      // second pass builds the graph by connecting first vertex on each
      // line to all others
      innerGraph = new Graph(st.Count);
      input = new TextInput(filename);
      while (input.HasNextLine()) {
        string[] a = input.ReadLine().Split(delimiter.ToCharArray());
        int v = st[a[0]];
        for (int i = 1; i < a.Length; i++)
        {
          int w = st[a[i]];
          innerGraph.AddEdge(v, w);
        }
      }
    }

    /// <summary>
    /// Does the graph contain the vertex named <c>s</c>?</summary>
    /// <param name="s">the name of a vertex</param>
    /// <returns><c>true</c> if <c>s</c> is the name of a vertex, and <c>false</c> otherwise</returns>
    ///
    public bool Contains(string s)
    {
      return st.Contains(s);
    }

    /// <summary>
    /// Returns the integer associated with the vertex named <c>s</c>.</summary>
    /// <param name="s">the name of a vertex</param>
    /// <returns>the integer (between 0 and <c>V</c> - 1) associated with the vertex named <c>s</c></returns>
    ///
    public int Index(string s)
    {
      return st[s];
    }

    /// <summary>
    /// Returns the name of the vertex associated with the integer <c>v</c>.</summary>
    /// <param name="v">the integer corresponding to a vertex (between 0 and <c>V</c> - 1) </param>
    /// <returns>the name of the vertex associated with the integer <c>v</c></returns>
    ///
    public string Name(int v)
    {
      return keys[v];
    }

    /// <summary>
    /// Returns the graph assoicated with the symbol graph. It is the client's responsibility
    /// not to mutate the graph.</summary>
    /// <returns>the graph associated with the symbol graph</returns>
    ///
    public Graph G
    {
      get { return innerGraph; }
    }

    /// <summary>
    /// Demo test the <c>SymbolGraph</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd SymbolGraph routes.txt \" \"", "File format with symbols and theirs adjacents, separated by a delimiter")]
    public static void MainTest(string[] args)
    {
      TextInput StdIn = new TextInput();

      string filename = args[0];
      string delimiter = args[1];
      SymbolGraph sg = new SymbolGraph(filename, delimiter);
      Graph G = sg.G;
      while (StdIn.HasNextLine())
      {
        string source = StdIn.ReadLine();
        if (sg.Contains(source))
        {
          int s = sg.Index(source);
          foreach (int v in G.Adj(s))
          {
            Console.WriteLine("   " + sg.Name(v));
          }
        }
        else
        {
          Console.WriteLine("input not contain '" + source + "'");
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
