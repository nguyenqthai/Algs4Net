/******************************************************************************
 *  File name :    NFA.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  C:\> algscmd NFA "(A*B|AC)D" AAAABD
 *  True
 *
 *  C:\> algscmd NFA "(A*B|AC)D" AAAAC
 *  False
 *
 *  C:\> algscmd NFA "(a|(bc)*d)*" abcbcd
 *  True
 *
 *  C:\> algscmd NFA "(a|(bc)*d)*" abcbcbcdaaaabcbcdaaaddd
 *  True
 *
 *  Remarks
 *  -----------
 *  The following features are not supported:
 *    - The + operator
 *    - Multiway or
 *    - Metacharacters in the text
 *    - Character classes.
 *
 ******************************************************************************/

using System;
using System.Diagnostics;

namespace Algs4Net
{
  /// <summary><para>The <c>NFA</c> class provides a data type for creating a
  /// <c>Nondeterministic finite state automaton</c> (NFA) from a regular
  /// expression and testing whether a given string is matched by that regular
  /// expression.
  /// It supports the following operations: <c>Concatenation</c>,
  /// <c>Closure</c>, <c>Binary or</c>, and <c>Parentheses</c>.
  /// It does not support <c>Mutiway or</c>, <c>Character classes</c>,
  /// <c>Metacharacters</c> (either in the text or pattern),
  /// <c>Capturing capabilities</c>, <c>Greedy</c> or <c>Relucantant</c>
  /// modifiers, and other features in industrial-strength implementations
  /// such as <see cref="System.Text.RegularExpressions.Regex"/>and <see cref="System.Text.RegularExpressions.Match"/>.
  /// </para><para>This implementation builds the NFA using a digraph and a stack
  /// and simulates the NFA using digraph search (see the textbook for details).
  /// The constructor takes time proportional to <c>M</c>, where <c>M</c>
  /// is the number of characters in the regular expression.
  /// The <c>Recognizes</c> method takes time proportional to <c>M N</c>,
  /// where <c>N</c> is the number of characters in the text.</para></summary>
  /// <remarks><para>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/54regexp">Section 5.4</a> of
  ///  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/NFA.java.html">NFA</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class NFA
  {
    private Digraph G;         // digraph of epsilon transitions
    private string regexp;     // regular expression
    private int M;             // number of characters in regular expression

    /// <summary>Initializes the NFA from the specified regular expression.</summary>
    /// <param name="regexp">the regular expression</param>
    /// <exception cref="ArgumentException">if the regular expression is invalid</exception>
    ///
    public NFA(string regexp)
    {
      this.regexp = regexp;
      M = regexp.Length;
      LinkedStack<int> ops = new LinkedStack<int>();
      G = new Digraph(M + 1);
      for (int i = 0; i < M; i++)
      {
        int lp = i;
        if (regexp[i] == '(' || regexp[i] == '|')
          ops.Push(i);
        else if (regexp[i] == ')')
        {
          int or = ops.Pop();

          // 2-way or operator
          if (regexp[or] == '|')
          {
            lp = ops.Pop();
            G.AddEdge(lp, or + 1);
            G.AddEdge(or, i);
          }
          else if (regexp[or] == '(')
            lp = or;
          else Debug.Assert(false);
        }

        // closure operator (uses 1-character lookahead)
        if (i < M - 1 && regexp[i + 1] == '*')
        {
          G.AddEdge(lp, i + 1);
          G.AddEdge(i + 1, lp);
        }
        if (regexp[i] == '(' || regexp[i] == '*' || regexp[i] == ')')
          G.AddEdge(i, i + 1);
      }
      if (ops.Count != 0)
        throw new ArgumentException("Invalid regular expression");
    }

    /// <summary>
    /// Returns true if the text is matched by the regular expression.</summary>
    /// <param name="txt"> txt the text</param>
    /// <returns><c>true</c> if the text is matched by the regular expression,
    /// <c>false</c> otherwise</returns>
    ///
    public bool Recognizes(string txt)
    {
      DirectedDFS dfs = new DirectedDFS(G, 0);
      Bag<int> pc = new Bag<int>();
      for (int v = 0; v < G.V; v++)
        if (dfs.Marked(v)) pc.Add(v);

      // Compute possible NFA states for txt[i+1]
      for (int i = 0; i < txt.Length; i++)
      {
        if (txt[i] == '*' || txt[i] == '|' || txt[i] == '(' || txt[i] == ')')
          throw new ArgumentException("text contains the metacharacter '" + txt[i] + "'");

        Bag<int> match = new Bag<int>();
        foreach (int v in pc)
        {
          if (v == M) continue;
          if ((regexp[v] == txt[i]) || regexp[v] == '.')
            match.Add(v + 1);
        }
        dfs = new DirectedDFS(G, match);
        pc = new Bag<int>();
        for (int v = 0; v < G.V; v++)
          if (dfs.Marked(v)) pc.Add(v);

        // optimization if no states reachable
        if (pc.Count == 0) return false;
      }

      // check for accept state
      foreach (int v in pc)
        if (v == M) return true;
      return false;
    }

    /// <summary>
    /// Demo test the <c>NFA</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd NFA \"pattern\" input_text")]
    public static void MainTest(string[] args)
    {
      string regexp = "(" + args[0] + ")";
      string txt = args[1];
      NFA nfa = new NFA(regexp);
      Console.WriteLine(nfa.Recognizes(txt));
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
