/******************************************************************************
 *  File name :    OrderHelper.cs
 *  
 *  A helper class for sorting
 *
 ******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Algs4Net
{
  static class OrderHelper
  {
    // TODO: Factor more comparison methods into this class

    /***************************************************************************
     *  Helper sorting functions.
     ***************************************************************************/

    // is v == v ?
    internal static bool Eq(IComparable v, IComparable w)
    {
      return v.CompareTo(w) == 0;
    }

    internal static bool Eq<T>(T v, T w, Comparer<T> comparator)
    {
      return comparator.Compare(v, w) == 0;
    }

    internal static bool Eq(object v, object w, Comparer comparator)
    {
      return comparator.Compare(v, w) == 0;
    }

    // is v < w ?
    internal static bool Less(IComparable v, IComparable w)
    {
      return v.CompareTo(w) < 0;
    }

    internal static bool Less<T>(T v, T w, Comparer<T> comparator)
    {
      return comparator.Compare(v, w) < 0;
    }

    internal static bool Less(object v, object w, Comparer comparator)
    {
      return comparator.Compare(v, w) < 0;
    }

    // is v < w ?

    internal static bool Lesser<T>(T v, T w) where T : IComparable<T>
    {
      return v.CompareTo(w) < 0;
    }

    // is v < w ?
    internal static bool Lesser<T>(T v, T w, Comparer<T> comparator)
    {
      return comparator.Compare(v, w) < 0;
    }


    // is v > w ?

    internal static bool Greater<T>(T v, T w) where T : IComparable<T>
    {
      return v.CompareTo(w) > 0;
    }

    // is v > w ?
    internal static bool Greater<T>(T v, T w, Comparer<T> comparator)
    {
      return comparator.Compare(v, w) > 0;
    }

    // exchange a[i] and a[j]
    internal static void Exch(object[] a, int i, int j)
    {
      object swap = a[i];
      a[i] = a[j];
      a[j] = swap;
    }

    // exchange a[i] and a[j] 
    internal static void Exch<T>(T[] a, int i, int j)
    {
      T swap = a[i];
      a[i] = a[j];
      a[j] = swap;
    }

    /***************************************************************************
     *  Check if array is sorted - useful for debugging.
     ***************************************************************************/
    internal static bool IsSorted(IComparable[] a)
    {
      return IsSorted(a, 0, a.Length - 1);
    }

    // is the array sorted from a[lo] to a[hi]
    internal static bool IsSorted(IComparable[] a, int lo, int hi)
    {
      for (int i = lo + 1; i <= hi; i++)
        if (Less(a[i], a[i - 1])) return false;
      return true;
    }

    internal static bool IsSorted<T>(T[] a, Comparer<T> comparator)
    {
      return IsSorted(a, 0, a.Length - 1, comparator);
    }

    // is the array sorted from a[lo] to a[hi]
    internal static bool IsSorted<T>(T[] a, int lo, int hi, Comparer<T> comparator)
    {
      for (int i = lo + 1; i <= hi; i++)
        if (Less(a[i], a[i - 1], comparator)) return false;
      return true;
    }

    internal static bool IsSorted(object[] a, Comparer comparator)
    {
      return IsSorted(a, 0, a.Length - 1, comparator);
    }

    // is the array sorted from a[lo] to a[hi]
    internal static bool IsSorted(object[] a, int lo, int hi, Comparer comparator)
    {
      for (int i = lo + 1; i <= hi; i++)
        if (Less(a[i], a[i - 1], comparator)) return false;
      return true;
    }

    // print array to standard output
    internal static void Show(IComparable[] a)
    {
      for (int i = 0; i < a.Length; i++)
      {
        Console.WriteLine(a[i]);
      }
    }

    internal static void Show<Item>(Item[] a)
    {
      for (int i = 0; i < a.Length; i++)
      {
        Console.WriteLine(a[i]);
      }
    }

    internal static T[] RemoveDuplicates<T>(T[] a)
    {
      SortedSet<T> set = new SortedSet<T>();
      foreach (T item in a)
      {
        if (!set.Contains(item)) set.Add(item);
      }
      return set.ToArray();
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
