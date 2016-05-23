/******************************************************************************
 *  File name :    DrawingWindow.Init.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Algs4Net
{
  /// <summary>
  /// A bare bone WPF client to support drawing and simple frame animation
  /// </summary>
  public partial class DrawingWindow : Window
  {
    // main window structure
    private Grid grid;
    private StackPanel panel;
    private Viewbox view;
    private Menu menu;
    private DrawingCanvas canvas;

    // sizes for the default window in DPI
    private const int DefaultSize = 500;

    /// <summary>
    /// Creates the WPF client
    /// </summary>
    public DrawingWindow()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      // Create the content with a menu and a canvas
      panel = new StackPanel();
      Content = panel;

      // Add menu
      InitMenu();

      grid = new Grid();
      grid.Margin = new Thickness(5);
      panel.Children.Add(grid);

      // Set row definitions.
      RowDefinition rowdef = new RowDefinition();
      rowdef.Height = GridLength.Auto;
      grid.RowDefinitions.Add(rowdef);

      rowdef = new RowDefinition();
      rowdef.Height = GridLength.Auto;
      grid.RowDefinitions.Add(rowdef);

      // Add a button row on top
      StackPanel buttons = new StackPanel() { Orientation = Orientation.Horizontal, Background = Brushes.CornflowerBlue, Height = 30 };
      Button aButton = new Button();
      //aButton.Name = "start";
      aButton.Content = "Start";
      aButton.Margin = new Thickness(5);
      aButton.Click += StartClicked;
      buttons.Children.Add(aButton);

      aButton = new Button();
      //aButton.Name = "stop";
      aButton.Content = "Stop";
      aButton.Margin = new Thickness(5);
      aButton.Click += StopClicked;
      buttons.Children.Add(aButton);
      grid.Children.Add(buttons);
      Grid.SetRow(buttons, 0);

      // Add a view box at the bottom
      view = new Viewbox();
      view.HorizontalAlignment = HorizontalAlignment.Left;
      grid.Children.Add(view);
      Grid.SetRow(view, 1);

      canvas = new DrawingCanvas() { Background = Brushes.White };
      view.Child = canvas;
      SetCanvasSize(DefaultSize, DefaultSize);

      // Set up other properties
      Title = "Minimal Window";
      Top = 60;
      Left = 60;

      SizeToContent = SizeToContent.WidthAndHeight;
    }

    private void InitMenu()
    {
      // Create the minimalist menu
      menu = new Menu() { Height = 25 };
      panel.Children.Add(menu);
      DockPanel.SetDock(menu, Dock.Top);

      // Create File menu.
      MenuItem fileItem = new MenuItem();
      fileItem.Header = "File";
      menu.Items.Add(fileItem);

      MenuItem saveItem = new MenuItem();
      saveItem.Header = "Save";
      saveItem.Command = ApplicationCommands.Save;
      CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, SaveOnExecute));
      fileItem.Items.Add(saveItem);

      fileItem.Items.Add(new Separator());

      MenuItem exitItem = new MenuItem();
      exitItem.Header = "Exit";
      exitItem.Click += ExitOnClick;
      fileItem.Items.Add(exitItem);
    }

    /// <summary>
    /// Returns the current canvas size 
    /// </summary>
    /// <returns>Returns the current canvas size </returns>
    public Size GetCanvasSize()
    {
      return new Size() { Width = canvas.Width, Height = canvas.Height };
    }

    /// <summary>
    /// Resets the canvas size to default size
    /// </summary>
    public void SetCanvasSize()
    {
      SetCanvasSize(DefaultSize, DefaultSize);
    }

    /// <summary>
    /// Sets the canvas (drawing area) to be <c>Width</c>-by-<c>Height</c> pixels.
    /// This also erases the current drawing and resets the coordinate system,
    /// pen radius, pen color, and font back to their default values.
    /// Ordinarly, this method is called once, at the very beginning
    /// of a program.</summary>
    /// <param name="canvasWidth">the width as a number of pixels</param>
    /// <param name="canvasHeight">the height as a number of pixels</param>
    /// <exception cref="ArgumentException">unless both Width and Height
    /// are positive</exception>
    ///
    public void SetCanvasSize(int canvasWidth, int canvasHeight)
    {
      if (canvasWidth <= 0 || canvasHeight <= 0) throw new ArgumentException("width and height must be positive");
      canvas.Width = canvasWidth;
      canvas.Height = canvasHeight;
      view.Height = canvasHeight;
    }

    #region DrawingCanvas class
    private class DrawingCanvas : Canvas
    {
      private List<Visual> visuals = new List<Visual>();

      protected override Visual GetVisualChild(int index)
      {
        return visuals[index];
      }

      protected override int VisualChildrenCount
      {
        get
        {
          return visuals.Count;
        }
      }

      internal void AddVisual(Visual visual)
      {
        visuals.Add(visual);

        base.AddVisualChild(visual);
        base.AddLogicalChild(visual);
      }

      internal void DeleteVisual(Visual visual)
      {
        Debug.Assert(visuals.Contains(visual));
        visuals.Remove(visual);

        base.RemoveVisualChild(visual);
        base.RemoveLogicalChild(visual);
      }

      internal void Clear()
      {
        foreach (var v in visuals)
        {
          base.RemoveVisualChild(v);
          base.RemoveLogicalChild(v);
        }
        visuals.Clear();
      }
    }

    #endregion
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
