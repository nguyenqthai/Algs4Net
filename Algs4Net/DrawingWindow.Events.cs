/******************************************************************************
 *  File name :    DrawingWindow.Events.cs
 *  Demo test :    Use the algscmd util or Visual Studio IDE
 *            :    Enter algscmd alone for how to use the util
 *
 ******************************************************************************/
 
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Algs4Net
{
  public partial class DrawingWindow : Window
  {
    private EventHandler onRenderHandler;
    private bool frameMode = false;

    /// <summary>
    /// Allows derived class to plug in the render action
    /// </summary>
    protected EventHandler FrameUpdateHandler
    {
      set
      {
        onRenderHandler = value;
      }
      get
      {
        return onRenderHandler;
      }
    }

    /// <summary>
    /// Set the frame to run for the specificed time in milliseconds
    /// </summary>
    /// <param name="t">frame time in milliseconds</param>
    protected void ShowFrame(int t)
    {
      // TODO: improve it to be more precise
      System.Threading.Thread.Sleep(t);
    }

    private void StartClicked(object sender, RoutedEventArgs e)
    {
      if (!frameMode && onRenderHandler != null)
      {
        CompositionTarget.Rendering += onRenderHandler;
        frameMode = true;
      }
    }

    private void StopClicked(object sender, RoutedEventArgs e)
    {
      if (frameMode && onRenderHandler != null)
      {
        CompositionTarget.Rendering -= onRenderHandler;
        frameMode = false;
      }
    }

    private void ExitOnClick(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void SaveOnExecute(object sender, ExecutedRoutedEventArgs args)
    {
      string filter = "PNG (*.png)|JPEG (*.jpg)|GIF (*.gif)|TIFF (*.tif)";
      SaveFileDialog saveDialog = new SaveFileDialog();
      saveDialog.Title = Title;
      saveDialog.Filter = filter;
      saveDialog.AddExtension = true;
      //saveDialog.FileName = "Untitled"
      if ((bool)saveDialog.ShowDialog(this))
      {
        try
        {
          SaveFile(saveDialog.FileName);
        }
        catch (Exception exc)
        {
          MessageBox.Show("Error on File Save" + exc.Message, Title,
                          MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
      }
    }

    private void SaveFile(string fileName)
    {
      RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
          (int)canvas.Width, (int)canvas.Height,
          96d, 96d, System.Windows.Media.PixelFormats.Default);

      canvas.Measure(new Size((int)canvas.Width, (int)canvas.Height));
      canvas.Arrange(new Rect(new Size((int)canvas.Width, (int)canvas.Height)));

      renderBitmap.Render(canvas);
      string extenstion = System.IO.Path.GetExtension(fileName);

      // TODO: fix this repetitive code
      if (extenstion.Contains(".png"))
      {
        PngBitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

        using (System.IO.FileStream file = System.IO.File.Create(fileName))
        {
          encoder.Save(file);
        }
      }
      else if (extenstion.Contains(".gif"))
      {
        GifBitmapEncoder encoder = new GifBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

        using (System.IO.FileStream file = System.IO.File.Create(fileName))
        {
          encoder.Save(file);
        }

      }
      else if (extenstion.Contains(".tif"))
      {
        TiffBitmapEncoder encoder = new TiffBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

        using (System.IO.FileStream file = System.IO.File.Create(fileName))
        {
          encoder.Save(file);
        }

      }
      else // JPEG for everything else
      {
        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

        using (System.IO.FileStream file = System.IO.File.Create(fileName))
        {
          encoder.Save(file);
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
