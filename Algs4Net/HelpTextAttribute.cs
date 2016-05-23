/******************************************************************************
 *  File name :    HelpTextAttribute.cs
 *
 *  Provides help text for a method
 *  
 ******************************************************************************/

using System;

namespace Algs4Net
{
  /// <summary>
  /// Provides help text for a method, mainly used for decorating the demo tests
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public sealed class HelpTextAttribute : Attribute
  {
    /// <summary>
    /// Usage of a test method
    /// </summary>
    public readonly string Usage;

    /// <summary>
    /// Additional details about the <c>Usage</c>
    /// </summary>
    public readonly string Details;

    private HelpTextAttribute() { }

    /// <summary>
    /// Construct a help text with a usage and optional details texts
    /// </summary>
    /// <param name="usage">The command line format</param>
    /// <param name="details">Description of command line arguments, if any</param>
    public HelpTextAttribute(string usage, string details = "")
    {
      Usage = usage;
      Details = details;
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
