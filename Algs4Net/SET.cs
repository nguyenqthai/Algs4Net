/******************************************************************************
 *  File name :    SET.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 *  Set implementation using .NET's SortedSet<T> class.
 *  Does not allow duplicates.
 *
 *  C:\> algscmd SET
 *  True
 *  ...
 *  www.whitehouse.gov
 *  www.yale.edu
 *  
 *  Set equality: True
 *  ToString() equality: True
 *
 ******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algs4Net
{
  /// <summary><para>
  /// The <c>SET</c> class represents an ordered set of comparable keys.
  /// It supports the usual <c>Add</c>, <c>Contains</c>, and <c>Delete</c>
  /// methods. It also provides ordered methods for finding the <c>Minimum</c>,
  /// <c>Maximum</c>, <c>Floor</c>, and <c>Ceiling</c> and set methods
  /// for <c>Union</c>, <c>Intersection</c>, and <c>Equality</c>.
  /// </para><para>
  /// Even though this implementation include the method <c>Equals()</c>, it
  /// does not support the method <c>GetHashCode()</c> because sets are mutable.
  /// </para><para>
  /// This implementation requires that
  /// the key type implements the <c>Comparable</c> interface and calls the
  /// <c>CompareTo()</c> and method to compare two keys. It does not call either
  /// <c>Equals()</c> or <c>GetHashCode()</c>.
  /// The <c>Add</c>, <c>Contains</c>, <c>Delete</c>, <c>Minimum</c>,
  /// <c>Maximum</c>, <c>Ceiling</c>, and <c>Floor</c> methods take
  /// logarithmic time in the worst case.
  /// The <c>Count</c>, and <c>IsEmpty</c> operations take constant time.
  /// Construction takes constant time.
  /// </para></summary>
  /// <remarks><para>
  /// For additional documentation, see
  /// <a href="http://algs4.cs.princeton.edu/35applications">Section 3.5</a> of
  /// <em>Algorithms in Java, 4th Edition</em> by Robert Sedgewick and Kevin Wayne.</para>
  /// <para>This class is a C# port from the original Java class 
  /// <a href="http://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/SET.java.html">Bag</a> implementation by
  /// Robert Sedgewick and Kevin Wayne.
  /// </para></remarks>
  ///
  public class SET<Key> : IEquatable<SET<Key>>, IEnumerable<Key> where Key : IComparable<Key>
  {
    private SortedSet<Key> set;

    /// <summary>
    /// Initializes an empty set.</summary>
    ///
    public SET()
    {
      set = new SortedSet<Key>();
    }

    /// <summary>
    /// Initializes a set with an initial list of items.</summary>
    /// <param name="items">initial elements to add the set</param>
    ///
    public SET(IEnumerable<Key> items)
    {
      set = new SortedSet<Key>(items);
    }

    /// <summary>
    /// Initializes a new set that is an independent copy of the specified set.</summary>
    /// <param name="x">the set to copy to this set</param>
    public SET(SET<Key> x)
    {
      set = new SortedSet<Key>(x.set);
    }

    /// <summary>
    /// Adds the key to this set (if it is not already present).</summary>
    /// <param name="key"> key the key to add</param>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public void Add(Key key)
    {
      if (key == null) throw new ArgumentNullException("called Add() with a null key");
      set.Add(key);
    }

    /// <summary>
    /// Returns true if this set contains the given key.</summary>
    /// <param name="key"> key the key</param>
    /// <returns><c>true</c> if this set contains <c>key</c>;
    ///        <c>false</c> otherwise</returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public bool Contains(Key key)
    {
      if (key == null) throw new ArgumentNullException("called contains() with a null key");
      return set.Contains(key);
    }

    /// <summary>
    /// Removes the specified key from this set (if the set contains the specified key).</summary>
    ///
    /// <param name="key"> key the key</param>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    ///
    public void Delete(Key key)
    {
      if (key == null) throw new ArgumentNullException("called delete() with a null key");
      set.Remove(key);
    }

    /// <summary>
    /// Returns the number of keys in this set.</summary>
    /// <returns>the number of keys in this set</returns>
    ///
    public int Count
    {
      get { return set.Count; }
    }

    /// <summary>
    /// Returns true if this set is empty.</summary>
    ///
    /// <returns><c>true</c> if this set is empty;
    ///        <c>false</c> otherwise</returns>
    ///
    public bool IsEmpty
    {
      get { return Count == 0; }
    }

    /// <summary>
    /// Returns all of the keys in this set, as an iterator.
    /// To iterate over all of the keys in a set named <c>set</c>, use the
    /// foreach notation: <c>foreach (Key key in set)</c>.</summary>
    /// <returns>an iterator to all of the keys in this set</returns>
    ///
    public IEnumerator<Key> GetEnumerator()
    {
      return set.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    /// <summary>
    /// Returns the largest key in this set.</summary>
    /// <returns>the largest key in this set</returns>
    /// <exception cref="InvalidOperationException">if this set is empty</exception>
    ///
    public Key Max
    {
      get
      {
        if (IsEmpty) throw new InvalidOperationException("called max() with empty set");
        return set.Max;
      }
    }

    /// <summary>
    /// Returns the smallest key in this set.</summary>
    /// <returns>the smallest key in this set</returns>
    /// <exception cref="InvalidOperationException">if this set is empty</exception>
    ///
    public Key Min
    {
      get {
        if (IsEmpty) throw new InvalidOperationException("called min() with empty set");
        return set.Min;
      }
    }

    /// <summary>
    /// Returns the smallest key in this set greater than or equal to <c>key</c>.</summary>
    ///
    /// <param name="key"> key the key</param>
    /// <returns>the smallest key in this set greater than or equal to <c>key</c></returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    /// <exception cref="InvalidOperationException">if there is no such key</exception>
    ///
    public Key Ceiling(Key key)
    {
      if (key == null) throw new ArgumentNullException("called ceiling() with a null key");
      Key k = set.First(aKey => (aKey.CompareTo(key) >= 0));
      if (k == null) throw new InvalidOperationException("all keys are less than " + key);
      return k;
    }

    /// <summary>
    /// Returns the largest key in this set less than or equal to <c>key</c>.</summary>
    /// <param name="key">the key</param>
    /// <returns>the largest key in this set table less than or equal to <c>key</c></returns>
    /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
    /// <exception cref="InvalidOperationException">if there is no such key</exception>
    ///
    public Key Floor(Key key)
    {
      if (key == null) throw new ArgumentNullException("called floor() with a null key");
      Key k = set.Last(aKey => (aKey.CompareTo(key) <= 0));
      if (k == null) throw new InvalidOperationException("all keys are greater than " + key);
      return k;
    }

    /// <summary>
    /// Returns the union of this set and that set.</summary>
    ///
    /// <param name="that"> that the other set</param>
    /// <returns>the union of this set and that set</returns>
    /// <exception cref="ArgumentNullException">if <c>that</c> is <c>null</c></exception>
    ///
    public SET<Key> Union(SET<Key> that)
    {
      if (that == null) throw new ArgumentNullException("called union() with a null argument");
      SET<Key> c = new SET<Key>();
      foreach (Key x in this)
      {
        c.Add(x);
      }
      foreach (Key x in that)
      {
        c.Add(x);
      }
      return c;
    }

    /// <summary>
    /// Returns the intersection of this set and that set.</summary>
    /// <param name="that"> that the other set</param>
    /// <returns>the intersection of this set and that set</returns>
    /// <exception cref="ArgumentNullException">if <c>that</c> is <c>null</c></exception>
    ///
    public SET<Key> Intersects(SET<Key> that)
    {
      if (that == null) throw new ArgumentNullException("called intersects() with a null argument");
      SET<Key> c = new SET<Key>();
      if (this.Count < that.Count)
      {
        foreach (Key x in this)
        {
          if (that.Contains(x)) c.Add(x);
        }
      }
      else
      {
        foreach (Key x in that)
        {
          if (this.Contains(x)) c.Add(x);
        }
      }
      return c;
    }

    /// <summary>
    /// Compares this set to the specified set.</summary>
    /// <param name="other">the other set</param>
    /// <returns><c>true</c> if this set equals <c>other</c>;
    /// <c>false</c> otherwise</returns>
    ///
    public override bool Equals(object other)
    {
      if (other.GetType() != this.GetType()) return false;
      SET<Key> that = other as SET<Key>;
      return Equals(that);
    }

    /// <summary>
    /// Compares this set to the other set using set element comparison. This is
    /// an override of the default rererence equality comparison.
    /// </summary>
    /// <param name="other">the other set</param>
    /// <returns><c>true</c> if this set equals <c>other</c>; <c>false</c> otherwise</returns>
    /// 
    public bool Equals(SET<Key> other)
    {
      if (other == this) return true;
      if (other == null) return false;

      Key[] keys1 = set.ToArray();
      Key[] keys2 = other.set.ToArray();
      if (keys1.Length != keys2.Length) return false;
      for (int i=0; i<keys1.Length; i++)
      {
        if (keys1[i].CompareTo(keys2[i]) != 0) return false;
      }
      return true;
    }

    /// <summary>
    /// This operation is not supported because sets are mutable.</summary>
    /// <returns>does not return a value</returns>
    /// <exception cref="InvalidOperationException">if called</exception>
    ///
    public override int GetHashCode()
    {
      throw new InvalidOperationException("hashCode() is not supported because sets are mutable");
    }

    /// <summary>
    /// Returns a string representation of this set.</summary>
    /// <returns>a string representation of this set, with the keys separated
    ///        by single spaces</returns>
    ///
    public override string ToString()
    {
      StringBuilder s = new StringBuilder();
      foreach (Key key in this)
        s.Append(key + " ");
      return s.ToString();
    }

    /// <summary>
    /// Demo test the <c>SET</c> data type.</summary>
    /// <param name="args">Place holder for user arguments</param>
    /// 
    [HelpText("algscmd SET")]
    public static void MainTest(string[] args)
    {
      SET<string> set = new SET<string>();

      // insert some keys
      set.Add("www.cs.princeton.edu");
      set.Add("www.cs.princeton.edu");    // overwrite old value
      set.Add("www.princeton.edu");
      set.Add("www.math.princeton.edu");
      set.Add("www.yale.edu");
      set.Add("www.amazon.com");
      set.Add("www.simpsons.com");
      set.Add("www.stanford.edu");
      set.Add("www.google.com");
      set.Add("www.ibm.com");
      set.Add("www.apple.com");
      set.Add("www.slashdot.com");
      set.Add("www.whitehouse.gov");
      set.Add("www.espn.com");
      set.Add("www.snopes.com");
      set.Add("www.movies.com");
      set.Add("www.cnn.com");
      set.Add("www.iitb.ac.in");


      Console.WriteLine(set.Contains("www.cs.princeton.edu"));
      Console.WriteLine(!set.Contains("www.harvardsucks.com"));
      Console.WriteLine(set.Contains("www.simpsons.com"));
      Console.WriteLine();

      Console.WriteLine("ceiling(www.simpsonr.com) = " + set.Ceiling("www.simpsonr.com"));
      Console.WriteLine("ceiling(www.simpsons.com) = " + set.Ceiling("www.simpsons.com"));
      Console.WriteLine("ceiling(www.simpsont.com) = " + set.Ceiling("www.simpsont.com"));
      Console.WriteLine("floor(www.simpsonr.com)   = " + set.Floor("www.simpsonr.com"));
      Console.WriteLine("floor(www.simpsons.com)   = " + set.Floor("www.simpsons.com"));
      Console.WriteLine("floor(www.simpsont.com)   = " + set.Floor("www.simpsont.com"));
      Console.WriteLine();


      // print out all keys in this set in lexicographic order
      foreach (string s in set)
      {
        Console.WriteLine(s);
      }

      Console.WriteLine();
      SET<string> set2 = new SET<string>(set);
      Console.WriteLine("Set equality: {0}", set.Equals(set2));
      Console.WriteLine("ToString() equality: {0}", set2.ToString().Equals(set.ToString()));
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
