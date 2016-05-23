/******************************************************************************
 *  File name :    BasicVisual.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 ******************************************************************************/

using System.Diagnostics;
using System.Windows.Media;

namespace Algs4Net
{
  /// <summary>
  /// The base class to faciliate drawing while keeping track of the drawing
  /// visual object. See a derived class such as <see cref="Point2D"/> for
  /// an example
  /// </summary>
  public abstract class BasicVisual
  {
    private DrawingVisual _visual = null;
    private DrawingWindow _target = null;

    /// <summary>
    /// Default constructor. Use this when other members such as
    /// the <c>Display</c> property is not needed in the derived
    /// class's application.
    /// </summary>
    public BasicVisual() { }

    /// <summary>
    /// Derived classes have to call to initiaze the drawing window 
    /// </summary>
    /// <param name="target">the drawing window</param>
    public BasicVisual(DrawingWindow target)
    {
      Debug.Assert(target != null);
      _target = target;
    }

    /// <summary>
    /// Uses to set the display as needed
    /// </summary>
    public DrawingWindow Display
    {
      get { return _target; }
      set
      {
        Debug.Assert(value != null, "The drawing target is null!");
        _target = value;
      }
    }

    /// <summary>
    /// Derived class needs to implement this method
    /// </summary>
    public abstract void Draw();

    /// <summary>
    /// The <see cref="DrawingVisual"/> associated with the <c>Draw</c>
    /// operation.
    /// </summary>
    protected DrawingVisual Visual
    {
      get { return _visual; }
      set { _visual = value; }
    }

    /// <summary>
    /// Remove existing <c>Vidual</c>
    /// </summary>
    protected void Clear()
    {
      if (_visual != null)
      {
        _target.DeleteVisual(_visual);
      }
    }

    /// <summary>
    /// Dereferences components
    /// </summary>
    ~BasicVisual()
    {
      _visual = null;
      _target = null;
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
