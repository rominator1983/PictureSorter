using System;
using System.ComponentModel;
using Eto.Forms;
using SkiaSharp;

namespace PictureSorter
{
  public partial class PictureView : Form, IPictureView
  {
    public IKeyInputHandler KeyInputHandler { get; private set; }
    public IPictureViewController PictureViewController { get; private set; }
    public SKBitmap CurrentBitmap { get; private set; }
    public double CurrentZoomFactor { get; private set; }
    public float CurrentPositionX { get; private set; }
    public float CurrentPositionY { get; private set; }

    public int MouseDownPositionX { get; private set; }
    public int MouseDownPositionY { get; private set; }

    public PictureView(IPictureViewController pictureViewController, KeyInputHandler keyInputHandler)
    {
      InitializeComponent();

      CurrentZoomFactor = 1.0;
      PictureViewController = pictureViewController;
      KeyInputHandler = keyInputHandler;

      PictureViewController.SetPictureForm(this);

      MouseWheel += OnMouseWheel;
    }

    public void SetCurrentPicture(Picture picture)
    {
      Title = "Picture Sorter - " + picture.FileName;
      CurrentBitmap = picture.Bitmap;
      Update();
    }

    private void Update()
    {
      //Console.WriteLine("Update");
      CurrentPicture.Invalidate();
    }

    public void ToggleFullScreen()
    {
      if (WindowState == Eto.Forms.WindowState.Maximized)
        SetNonFullScreen();
      else
        SetFullScreen();
    }

    private void SetFullScreen()
    {
      WindowState = Eto.Forms.WindowState.Maximized;
    }

    public void SetNonFullScreen()
    {
      WindowState = Eto.Forms.WindowState.Normal;
    }

    private void PictureForm_KeyDown(object sender, KeyEventArgs e)
    {
      KeyInputHandler.Handle(e);
    }

    // NOTE: fix for GTK not forwarding key down events for arrow keys
    private void PictureForm_GtkKeyPress(object sender, Gtk.KeyReleaseEventArgs e)
    {
      if (e.Event.Key == Gdk.Key.Left)
        KeyInputHandler.Handle(new KeyEventArgs(Keys.Left, KeyEventType.KeyDown));

      if (e.Event.Key == Gdk.Key.Right)
        KeyInputHandler.Handle(new KeyEventArgs(Keys.Right, KeyEventType.KeyDown));
    }

    private void CurrentPictureOnPaint(SKSurface sKSurface)
    {
      DrawImage(CurrentZoomFactor, sKSurface.Canvas);
    }

    public void ResetZoomAndPosition()
    {
      CurrentZoomFactor = 1.0;
      CurrentPositionX = 0;
      CurrentPositionY = 0;
      Update();
    }

    public void SetZoom(double zoomFactor)
    {
      CurrentPositionX = (int)(CurrentPositionX * zoomFactor / CurrentZoomFactor);
      CurrentPositionY = (int)(CurrentPositionY * zoomFactor / CurrentZoomFactor);
      CurrentZoomFactor = zoomFactor;
      Update();
    }

    private void DrawImage(double zoomFactor, SKCanvas graphics)
    {
      graphics.Clear(new SKColor(40, 40, 40));

      var image = CurrentBitmap;

      if (image == null)
        return;

      var grfxFactor = graphics.LocalClipBounds.Width / (double)graphics.LocalClipBounds.Height;
      var imageFactor = image.Width / (double)image.Height;

      double width;
      double height;
      double x;
      double y;

      if (grfxFactor > imageFactor)
      {
        // graphics is more wide than image. use height for scaling
        var scale = image.Height / graphics.LocalClipBounds.Height;

        width = image.Width / scale * zoomFactor;
        height = graphics.LocalClipBounds.Height * zoomFactor;

        x = CurrentPositionX + graphics.LocalClipBounds.Width / 2d - (width / 2d);
        y = CurrentPositionY + graphics.LocalClipBounds.Height / 2d - (height / 2d);
      }
      else
      {
        // graphics is more high than image. use width for scaling
        var scale = image.Width / graphics.LocalClipBounds.Width;

        width = graphics.LocalClipBounds.Width * zoomFactor;
        height = image.Height / scale * zoomFactor;

        x = CurrentPositionX + graphics.LocalClipBounds.Width / 2d - (width / 2d);
        y = CurrentPositionY + graphics.LocalClipBounds.Height / 2d - (height / 2d);
      }

      while (y > 0 && (height + y) >= graphics.LocalClipBounds.Height)
        y--;

      while (y < 0 && y + height <= graphics.LocalClipBounds.Height)
        y++;

      while (x > 0 && (width + x) >= graphics.LocalClipBounds.Width)
        x--;

      while (x < 0 && x + width <= graphics.LocalClipBounds.Width)
        x++;

      graphics.DrawBitmap(CurrentBitmap, new SKRect((float)x, (float)y, (float)x + (float)width, (float)y + (float)height));
    }

    private void CurrentPicture_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Buttons == MouseButtons.Primary)
      {
        MouseDownPositionX = (int)e.Location.X - (int)CurrentPositionX;
        MouseDownPositionY = (int)e.Location.Y - (int)CurrentPositionY;
      }
    }

    private void CurrentPicture_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.Buttons == MouseButtons.Primary)
      {
        CurrentPositionX = (int)e.Location.X - MouseDownPositionX;
        CurrentPositionY = (int)e.Location.Y - MouseDownPositionY;

        Update();
      }
    }

    private void PictureView_HelpButtonClicked(object sender, CancelEventArgs e)
    {
      PictureViewController.ShowHelpScreen();
    }

    private void PictureView_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      PictureViewController.ToggleFullScreen();
    }

    private void CurrentPicture_DoubleClick(object sender, EventArgs e)
    {
      PictureViewController.ToggleFullScreen();
    }

    private void OnMouseWheel(object sender, MouseEventArgs mouseEventArgs)
    {
      if (mouseEventArgs.Delta.Height > 1)
        PictureViewController.ZoomIn();
      else if (mouseEventArgs.Delta.Height < -1)
        PictureViewController.ZoomOut();
    }
  }
}
