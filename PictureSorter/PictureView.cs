using System;
using System.ComponentModel;
using Eto.Drawing;
using System.IO;
using Eto.Forms;

namespace PictureSorter
{
  public partial class PictureView : Form, IPictureView
  {
    public IKeyInputHandler KeyInputHandler { get; private set; }
    public IPictureViewController PictureViewController { get; private set; }
    public Bitmap CurrentBitmap { get; private set; }
    public double CurrentZoomFactor { get; private set; }
    public float CurrentPositionX { get; private set; }
    public float CurrentPositionY { get; private set; }

    public int MouseDownPositionX { get; private set; }
    public int MouseDownPositionY { get; private set; }

    public PictureView (PictureViewController pictureViewController, KeyInputHandler keyInputHandler)
    {
      InitializeComponent ();

      CurrentZoomFactor = 1.0;
      PictureViewController = pictureViewController;
      KeyInputHandler = keyInputHandler;

      PictureViewController.SetPictureForm (this);

      MouseWheel += OnMouseWheel;
      //CurrentPicture.Paint += CurrentPictureOnPaint;
    }

    public void SetCurrentPicture (Picture picture)
    {
      Title = "Picture Sorter - " + picture.FileName;
      //CurrentPicture.Image = picture.Bitmap;
      CurrentBitmap = picture.Bitmap;
      CurrentPicture.Image = picture.Bitmap;

      DrawImage (CurrentZoomFactor, new Graphics(CurrentBitmap));
    }

    public void ToggleFullScreen ()
    {
      if (WindowState == WindowState.Maximized)
        SetNonFullScreen ();
      else
        SetFullScreen ();
    }

    private void SetFullScreen ()
    {
      WindowState = WindowState.Maximized;
    }

    public void SetNonFullScreen ()
    {
      WindowState = WindowState.Normal;
    }

    // protected override bool IsInputKey (Keys keyData)
    // {
    //   return true;
    // }

    private void PictureForm_KeyDown (object sender, KeyEventArgs e)
    {
      KeyInputHandler.Handle(e);
    }

    private void CurrentPictureOnPaint (object sender, PaintEventArgs paintEventArgs)
    {
      DrawImage (CurrentZoomFactor, paintEventArgs.Graphics);
    }

    public void ResetZoomAndPosition ()
    {
      CurrentZoomFactor = 1.0;
      CurrentPositionX = 0;
      CurrentPositionY = 0;
     // CurrentPicture.Refresh ();
    }

    public void SetZoom (double zoomFactor)
    {
      CurrentPositionX = (int)(CurrentPositionX * zoomFactor / CurrentZoomFactor);
      CurrentPositionY = (int)(CurrentPositionY * zoomFactor / CurrentZoomFactor);
      CurrentZoomFactor = zoomFactor;
      // CurrentPicture.Refresh ();
    }

    private void DrawImage (double zoomFactor, Graphics graphics)
    {
      graphics.Clear (Color.FromRgb(0));

      var image = CurrentBitmap;

      if (image == null)
        return;

      var grfxFactor = graphics.ClipBounds.Width / (double) graphics.ClipBounds.Height;
      var imageFactor = image.Width / (double) image.Height;

      double width;
      double height;
      double x;
      double y;

      if (grfxFactor > imageFactor)
      {
        // use height for scaling
        var scale = image.Height / graphics.ClipBounds.Height;

        width = image.Width / scale * zoomFactor;
        height = graphics.ClipBounds.Height * zoomFactor;

        x = CurrentPositionX + graphics.ClipBounds.Width / 2d - (width / 2d);
        y = CurrentPositionY + graphics.ClipBounds.Height / 2d - (height / 2d);
      }
      else
      {
        // use width for scaling
        var scale = image.Width / graphics.ClipBounds.Width;

        width = graphics.ClipBounds.Width * zoomFactor;
        height = image.Height / scale * zoomFactor;

        x = CurrentPositionX + graphics.ClipBounds.Width / 2d - (width / 2d);
        y = CurrentPositionY + graphics.ClipBounds.Height / 2d - (height / 2d);
      }

      while (y > 0 && (height + y) >= graphics.ClipBounds.Height)
        y--;

      while (y < 0 && y + height <= graphics.ClipBounds.Height)
        y++;

      while (x > 0 && (width + x) >= graphics.ClipBounds.Width)
        x--;

      while (x < 0 && x + width <= graphics.ClipBounds.Width)
        x++;

      graphics.DrawImage (image, (float) x, (float) y, (float) width, (float) height);

      //graphics.DrawText (string.Format ("x: {0} y: {1}", (int)x, (int)y), Font, Brushes.Red, 0, 0);
      //graphics.DrawText (string.Format ("width: {0} height: {1}", (int)width, (int)height), Font, Brushes.Red, 0, 20);
      //graphics.DrawText (string.Format ("bound-width: {0} bound-height: {1}", (int)graphics.ClipBounds.Width, (int)graphics.ClipBounds.Height), Font, Brushes.Red, 0, 40);
    }

    private void CurrentPicture_MouseDown (object sender, MouseEventArgs e)
    {
      if (e.Buttons == MouseButtons.Primary)
      {
        MouseDownPositionX = (int)e.Location.X - (int)CurrentPositionX;
        MouseDownPositionY = (int)e.Location.Y - (int)CurrentPositionY;
      }
    }

    private void CurrentPicture_MouseMove (object sender, MouseEventArgs e)
    {
      if (e.Buttons == MouseButtons.Primary)
      {
        CurrentPositionX = (int)e.Location.X - MouseDownPositionX;
        CurrentPositionY = (int)e.Location.Y - MouseDownPositionY;

        //CurrentPicture.Refresh ();
      }
    }

    private void PictureView_HelpButtonClicked (object sender, CancelEventArgs e)
    {
      PictureViewController.ShowHelpScreen ();
    }

    protected override void OnDragDrop (DragEventArgs drgevent)
    {
      // TODO: fix dragging
      //var data = (string[])drgevent.Data.GetData (DataFormats.FileDrop);
//
      //if (data.Length > 0)
      //{
      //  var droppedFile = data[0];
//
      //  if (File.Exists (droppedFile))
      //    PictureViewController.SetDroppedFile (droppedFile);
      //}

      base.OnDragDrop (drgevent);
    }

    protected override void OnDragEnter (DragEventArgs drgevent)
    {
      // TODO: fix dragging
      //if (drgevent.Data.GetDataPresent (DataFormats.FileDrop))
      //  drgevent.Effect = DragDropEffects.Move;
//
      base.OnDragEnter (drgevent);
    }

    protected override void OnDragOver (DragEventArgs drgevent)
    {
      // TODO: fix dragging
      //if (drgevent.Data.GetDataPresent (DataFormats.FileDrop))
      //  drgevent.Effect = DragDropEffects.Move;
      
      base.OnDragOver (drgevent);
    }

    private void PictureView_MouseDoubleClick (object sender, MouseEventArgs e)
    {
      PictureViewController.ToggleFullScreen ();
    }

    private void CurrentPicture_DoubleClick (object sender, EventArgs e)
    {
      PictureViewController.ToggleFullScreen ();
    }

    private void OnMouseWheel (object sender, MouseEventArgs mouseEventArgs)
    {
      if (mouseEventArgs.Delta.Height > 0)
        PictureViewController.ZoomIn();
      else
        PictureViewController.ZoomOut();
    }
  }
}
