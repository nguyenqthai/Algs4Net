/******************************************************************************
 *  File name :    Transaction.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Data type for commercial transactions.
 *
 *  C:\> algscmd Transaction
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Transaction</c> class is an immutable data type to encapsulate a
  /// commercial transaction with a customer name, date, and amount.</summary>
  /// <remarks>For additional documentation,
  /// see <a href="http://algs4.cs.princeton.edu/12oop">Section 1.2</a> of
  /// <em>Algorithms, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Transaction.java.html">Transaction</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.</para></remarks>
  ///
  public class Transaction : IComparable<Transaction>
  {
    private readonly string who;      // customer
    private readonly DateTime when;     // date
    private readonly double amount;   // amount

    /// <summary>
    /// Initializes a new transaction from the given arguments.</summary>
    /// <param name="who">who the person involved in this transaction</param>
    /// <param name="when">when the date of this transaction</param>
    /// <param name="amount">amount the amount of this transaction</param>
    /// <exception cref="ArgumentException">if <c>amount</c>
    ///        is <c>double.NaN</c>, <c>double.PositiveInfinity</c>,
    ///        or <c>double.NegativeInfinity</c></exception>
    ///
    public Transaction(string who, DateTime when, double amount)
    {
      if (double.IsNaN(amount) || double.IsInfinity(amount))
        throw new ArgumentException("Amount cannot be NaN or infinite");
      this.who = who;
      this.when = when;
      if (amount == 0.0) this.amount = 0.0;  // to handle -0.0
      else this.amount = amount;
    }

    /// <summary>
    /// Initializes a new transaction by parsing a string of the form NAME DATE AMOUNT.</summary>
    /// <param name="transaction"> transaction the string to parse</param>
    /// <exception cref="ArgumentException">if <c>amount</c> is <c>double.NaN</c>, 
    /// <c>double.PositiveInfinity</c>, or <c>double.NegativeInfinity</c></exception>
    ///
    public Transaction(string transaction)
    {
      Regex WhiteSpace = new Regex(@"[\s]+", RegexOptions.Compiled);
      string[] a = WhiteSpace.Split(transaction);

      who = a[0];
      when = DateTime.Parse(a[1]);
      double value = double.Parse(a[2]);
      if (value == 0.0) amount = 0.0;  // convert -0.0 0.0
      else amount = value;
      if (double.IsNaN(amount) || double.IsInfinity(amount))
        throw new ArgumentException("Amount cannot be NaN or infinite");
    }

    /// <summary>
    /// Returns the name of the customer involved in this transaction.</summary>
    /// <returns>the name of the customer involved in this transaction</returns>
    ///
    public string Who
    {
      get { return who; }
    }

    /// <summary>
    /// Returns the date of this transaction.</summary>
    /// <returns>the date of this transaction</returns>
    ///
    public DateTime When
    {
      get { return when; }
    }

    /// <summary>
    /// Returns the amount of this transaction.</summary>
    /// <returns>the amount of this transaction</returns>
    ///
    public double Amount
    {
      get { return amount; }
    }

    /// <summary>
    /// Returns a string representation of this transaction.</summary>
    /// <returns>a string representation of this transaction</returns>
    ///
    public override string ToString()
    {
      return string.Format("{0,-10} {1,10} {2,8:F2}", who, when, amount);
    }

    /// <summary>
    /// Compares two transactions by amount.</summary>
    /// <param name="that">that the other transaction</param>
    /// <returns>{a negative integer, zero, a positive integer}, depending
    ///        on whether the amount of this transaction is {less than,
    ///        equal to, or greater than} the amount of that transaction</returns>
    ///
    public int CompareTo(Transaction that)
    {
      if (amount < that.amount) return -1;
      else if (amount > that.amount) return +1;
      else return 0;
    }

    /// <summary>
    /// Compares this transaction to the specified object.</summary>
    /// <param name="other">other the other transaction</param>
    /// <returns>true if this transaction is equal to <c>other</c>; false otherwise</returns>
    ///
    public override bool Equals(object other)
    {
      if (other == this) return true;
      if (other == null) return false;
      if (other.GetType() != this.GetType()) return false;
      Transaction that = (Transaction)other;
      return (amount == that.amount) && (who.Equals(that.who))
                                          && (when.Equals(that.when));
    }

    /// <summary>
    /// Returns a hash code for this transaction.</summary>
    /// <returns>a hash code for this transaction</returns>
    /// 
    public override int GetHashCode()
    {
      int hash = 17;
      hash = 31 * hash + who.GetHashCode();
      hash = 31 * hash + when.GetHashCode();
      hash = 31 * hash + amount.GetHashCode();
      return hash;
    }

    // Custom compararors'

    /// <summary>
    /// Compares two transactions by customer name.
    /// </summary>
    public class WhoOrder : Comparer<Transaction>
    {
      /// <summary>
      /// Default constructor
      /// </summary>
      public WhoOrder() { }
      /// <summary>
      /// Compares two transactions by customer name.
      /// </summary>
      /// <param name="v">a transaction</param>
      /// <param name="w">another transaction</param>
      /// <returns>1 if v &gt; w, 0 if equal, -1 if v &lt; w</returns>
      public override int Compare(Transaction v, Transaction w)
      {
        return v.who.CompareTo(w.who);
      }
    }

    /// <summary>
    /// Compares two transactions by date.</summary>
    /// 
    public class WhenOrder : Comparer<Transaction>
    {
      /// <summary>
      /// Default constructor
      /// </summary>
      public WhenOrder() { }

      /// <summary>
      /// Compares two transactions by date.
      /// </summary>
      /// <param name="v">a transaction</param>
      /// <param name="w">another transaction</param>
      /// <returns>1 if v &gt; w, 0 if equal, -1 if v &lt; w</returns>
      public override int Compare(Transaction v, Transaction w)
      {
        return v.when.CompareTo(w.when);
      }
    }

    /// <summary>
    /// Compares two transactions by amount.</summary>
    /// 
    public class HowMuchOrder : Comparer<Transaction>
    {
      /// <summary>
      /// Default constructor
      /// </summary>
      public HowMuchOrder() {}

      /// <summary>
      /// Compares two transactions by amount.
      /// </summary>
      /// <param name="v">a transaction</param>
      /// <param name="w">another transaction</param>
      /// <returns>1 if v &gt; w, 0 if equal, -1 if v &lt; w</returns>
      public override int Compare(Transaction v, Transaction w)
      {
        if (v.amount < w.amount) return -1;
        else if (v.amount > w.amount) return +1;
        else return 0;
      }
    }

    /// <summary>
    /// Demo test the <c>Transaction</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd Transaction")]
    public static void MainTest(string[] args)
    {
      Transaction[] a = new Transaction[4];
      a[0] = new Transaction("Turing   6/17/1990  644.08");
      a[1] = new Transaction("Tarjan   3/26/2002 4121.85");
      a[2] = new Transaction("Knuth    6/14/1999  288.34");
      a[3] = new Transaction("Dijkstra 8/22/2007 2678.40");

      Console.WriteLine("Unsorted");
      for (int i = 0; i < a.Length; i++)
        Console.WriteLine(a[i]);
      Console.WriteLine();

      Console.WriteLine("Sort by date");
      Array.Sort(a, new Transaction.WhenOrder());
      for (int i = 0; i < a.Length; i++)
        Console.WriteLine(a[i]);
      Console.WriteLine();

      Console.WriteLine("Sort by customer");
      Array.Sort(a, new Transaction.WhoOrder());
      for (int i = 0; i < a.Length; i++)
        Console.WriteLine(a[i]);
      Console.WriteLine();

      Console.WriteLine("Sort by amount");
      Array.Sort(a, new Transaction.HowMuchOrder());
      for (int i = 0; i < a.Length; i++)
        Console.WriteLine(a[i]);
      Console.WriteLine();
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
