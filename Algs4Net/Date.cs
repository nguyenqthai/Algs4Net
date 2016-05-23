/******************************************************************************
 *  File name :    Date.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  An immutable data type for dates.
 *
 *  C:\> algscmd Date
 *  2/25/2004
 *  2/26/2004
 *  ...
 *  10/25/1971
 *  10/26/1971
 *  
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// The <c>Date</c> class is an immutable data type to encapsulate a
  /// date (day, month, and year).</summary>
  /// <remarks><para>For additional documentation, 
  /// see <a href="http://algs4.cs.princeton.edu/12oop">Section 1.2</a> of 
  /// <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne. </para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/Date.java.html">Date</a>
  /// implementation by the respective authors.</para></remarks>
  ///
  public class Date : IComparable<Date>
  {
    private static readonly int[] DAYS = { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
    private readonly int month;   // month (between 1 and 12)
    private readonly int day;     // day   (between 1 and DAYS[month]
    private readonly int year;    // year

    /// <summary>Initializes a new date from the month, day, and year.</summary>
    /// <param name="month">the month (between 1 and 12)</param>
    /// <param name="day">the day (between 1 and 28-31, depending on the month)</param>
    /// <param name="year">the year</param>
    /// <exception cref="ArgumentException">if this date is invalid</exception>
    ///
    public Date(int month, int day, int year)
    {
      if (!isValid(month, day, year)) throw new ArgumentException("Invalid date");
      this.month = month;
      this.day = day;
      this.year = year;
    }

    /// <summary>
    /// Initializes new date specified as a string in form MM/DD/YYYY.</summary>
    /// <param name="date">the string representation of this date</param>
    /// <exception cref="ArgumentException">if this date is invalid</exception>
    ///
    public Date(string date)
    {
      string[] fields = date.Split(new char[] { '/' });
      if (fields.Length != 3)
      {
        throw new ArgumentException("Invalid date");
      }
      month = int.Parse(fields[0]);
      day = int.Parse(fields[1]);
      year = int.Parse(fields[2]);
      if (!isValid(month, day, year)) throw new ArgumentException("Invalid date");
    }

    /// <summary>Return the month.</summary>
    /// <returns>the month (an integer between 1 and 12)</returns>
    ///
    public int Month
    {
      get { return month; }
    }

    /// <summary>
    /// Returns the day.</summary>
    /// <returns>the day (an integer between 1 and 31)</returns>
    ///
    public int Day
    {
      get { return day; }
    }

    /// <summary>
    /// Returns the year.</summary>
    /// <returns>the year</returns>
    ///
    public int Year
    {
      get { return year; }
    }

    // is the given date valid?
    private static bool isValid(int m, int d, int y)
    {
      if (m < 1 || m > 12) return false;
      if (d < 1 || d > DAYS[m]) return false;
      if (m == 2 && d == 29 && !isLeapYear(y)) return false;
      return true;
    }

    // is y a leap year?
    private static bool isLeapYear(int y)
    {
      if (y % 400 == 0) return true;
      if (y % 100 == 0) return false;
      return y % 4 == 0;
    }

    /// <summary>
    /// Returns the next date in the calendar.</summary>
    /// <returns>a date that represents the next day after this day</returns>
    ///
    public Date Next()
    {
      if (isValid(month, day + 1, year)) return new Date(month, day + 1, year);
      else if (isValid(month + 1, 1, year)) return new Date(month + 1, 1, year);
      else return new Date(1, 1, year + 1);
    }

    /// <summary>
    /// Compares two dates chronologically.</summary>
    /// <param name="that">the other date</param>
    /// <returns><c>true</c> if this date is after that date; <c>false</c> otherwise</returns>
    ///
    public bool IsAfter(Date that)
    {
      return CompareTo(that) > 0;
    }

    /// <summary>
    /// Compares two dates chronologically.</summary>
    /// <param name="that">the other date</param>
    /// <returns><c>true</c> if this date is before that date; <c>false</c> otherwise</returns>
    ///
    public bool IsBefore(Date that)
    {
      return CompareTo(that) < 0;
    }

    /// <summary>
    /// Compares two dates chronologically.</summary>
    /// <param name="that">the other date</param>
    /// <returns>the value <c>0</c> if the argument date is equal to this date;
    ///        a negative integer if this date is chronologically less than
    ///        the argument date; and a positive ineger if this date is chronologically
    ///        after the argument date</returns>
    ///
    public int CompareTo(Date that)
    {
      if (year < that.year) return -1;
      if (year > that.year) return +1;
      if (month < that.month) return -1;
      if (month > that.month) return +1;
      if (day < that.day) return -1;
      if (day > that.day) return +1;
      return 0;
    }

    /// <summary>
    /// Returns a string representation of this date.</summary>
    /// <returns>the string representation in the format MM/DD/YYYY</returns>
    ///
    public override string ToString()
    {
      return month + "/" + day + "/" + year;
    }

    /// <summary>
    /// Compares this date to the specified date.</summary>
    /// <param name="other">the other date</param>
    /// <returns><c>true</c> if this date equals <c>other</c>; <c>false</c> otherwise</returns>
    ///
    public override bool Equals(object other)
    {
      if (other == this) return true;
      if (other == null) return false;
      if (other.GetType() != GetType()) return false;
      Date that = (Date)other;
      return (month == that.month) && (day == that.day) && (year == that.year);
    }

    /// <summary>
    /// Returns an integer hash code for this date.</summary>
    /// <returns>a hash code for this date</returns>
    ///
    public override int GetHashCode()
    {
      int hash = 17;
      hash = 31 * hash + month;
      hash = 31 * hash + day;
      hash = 31 * hash + year;
      return hash;
    }

    /// <summary>
    /// Demo test the <c>Date</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    [HelpText("algscmd Date")]
    public static void MainTest(string[] args)
    {
      Date today = new Date(2, 25, 2004);
      Console.WriteLine(today);
      for (int i = 0; i < 10; i++)
      {
        today = today.Next();
        Console.WriteLine(today);
      }

      Console.WriteLine(today.IsAfter(today.Next()));  // False
      Console.WriteLine(today.IsAfter(today));         // False
      Console.WriteLine(today.Next().IsAfter(today));  // True

      Date birthday = new Date(10, 16, 1971);
      Console.WriteLine(birthday);
      for (int i = 0; i < 10; i++)
      {
        birthday = birthday.Next();
        Console.WriteLine(birthday);
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
