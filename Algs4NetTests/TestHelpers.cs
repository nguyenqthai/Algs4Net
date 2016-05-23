/******************************************************************************
 *  File name :  TestHelpers.cs
 *  Purpose   :  Contains unit test helpers Algs4Net
 *
 ******************************************************************************/
using Algs4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Algs4NetUnitTests
{
  internal class TestHelpers
  {
    internal static SET<string> getAdjacents(SymbolGraph sg, string vertice)
    {
      SET<string> set = new SET<string>();
      Graph g = sg.G;
      int s = sg.Index(vertice);
      foreach (int v in g.Adj(s))
      {
        set.Add(sg.Name(v));
      }
      return set;
    }

    internal static Graph buildGraph()
    {
      Graph g = new Graph(16);
      Edge[] edges =
      {
        // 1st component
        new Edge(0, 1), new Edge(1, 3), new Edge(3, 2), new Edge(2, 1), new Edge(2, 4),
        new Edge(4, 4), // self-loop
        // 2nd component
        new Edge(5, 6), new Edge(5, 7), new Edge(5, 8), new Edge(8, 6), new Edge(8, 7), new Edge(8, 9),
        // 3rd component
        new Edge(10, 11), new Edge(11, 10), // parallel edge
        // 4th component
        new Edge(12, 13), new Edge(12, 14), new Edge(13, 15), new Edge(13, 14), new Edge(14, 15)
      };
      foreach (var e in edges) g.AddEdge(e);
      return g;
    }

    internal static Digraph buildDigraph(bool isDAG)
    {
      Digraph g = new Digraph(11);
      Edge[] edges =
      {
        new Edge(0, 1), new Edge(0, 2), new Edge(0, 3), new Edge(2, 4), new Edge(2, 5),
        new Edge(4, 7), new Edge(5, 7), new Edge(7, 8), new Edge(7, 9),
        new Edge(3, 6), new Edge(6, 8), new Edge(8, 10), new Edge(9, 10)
      };
      foreach (var e in edges) g.AddEdge(e);
      if (isDAG)
        return g;
      // add a few more edges to make it a cycle
      g.AddEdge(new Edge(10, 2));
      return g;

    }
  }
}
