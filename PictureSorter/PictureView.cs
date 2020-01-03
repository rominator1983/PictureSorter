using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PictureSorter
{
  public partial class PictureView : Form, IPictureView
  {
    public IKeyInputHandler KeyInputHandler { get; private set; }
    public IPictureViewController PictureViewController { get; private set; }
    public Bitmap CurrentCitmap { get; private set; }
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
      CurrentPicture.Paint += CurrentPictureOnPaint;
    }

    public void SetCurrentPicture (Picture picture)
    {
      Text = "Picture Sorter - " + picture.FileName;
      //CurrentPicture.Image = picture.Bitmap;
      CurrentCitmap = picture.Bitmap;

      CurrentPicture.Refresh ();
    }

    public void ToggleFullScreen ()
    {
      if (FormBorderStyle == FormBorderStyle.None)
        SetNonFullScreen ();
      else
        SetFullScreen ();
    }

    private void SetFullScreen ()
    {
      WindowState = FormWindowState.Normal;
      TopMost = true;
      FormBorderStyle = FormBorderStyle.None;
      WindowState = FormWindowState.Maximized;
    }

    public void SetNonFullScreen ()
    {
      FormBorderStyle = FormBorderStyle.Sizable;
      TopMost = false;
    }

    protected override bool IsInputKey (Keys keyData)
    {
      return true;
    }

    private void PictureForm_KeyDown (object sender, KeyEventArgs e)
    {
      KeyInputHandler.Handle (Handle, e);
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
      CurrentPicture.Refresh ();
    }

    public void SetZoom (double zoomFactor)
    {
      CurrentPositionX = (int)(CurrentPositionX * zoomFactor / CurrentZoomFactor);
      CurrentPositionY = (int)(CurrentPositionY * zoomFactor / CurrentZoomFactor);
      CurrentZoomFactor = zoomFactor;
      CurrentPicture.Refresh ();
    }

    private void DrawImage (double zoomFactor, Graphics graphics)
    {
      graphics.Clear (Color.Black);

      var image = CurrentCitmap;

      if (image == null)
        return;

      var grfxFactor = graphics.VisibleClipBounds.Width / (double) graphics.VisibleClipBounds.Height;
      var imageFactor = image.Width / (double) image.Height;

      double width;
      double height;
      double x;
      double y;

      if (grfxFactor > imageFactor)
      {
        // use height for scaling
        var scale = image.Height / graphics.VisibleClipBounds.Height;

        width = image.Width / scale * zoomFactor;
        height = graphics.VisibleClipBounds.Height * zoomFactor;

        x = CurrentPositionX + graphics.VisibleClipBounds.Width / 2d - (width / 2d);
        y = CurrentPositionY + graphics.VisibleClipBounds.Height / 2d - (height / 2d);
      }
      else
      {
        // use width for scaling
        var scale = image.Width / graphics.VisibleClipBounds.Width;

        width = graphics.VisibleClipBounds.Width * zoomFactor;
        height = image.Height / scale * zoomFactor;

        x = CurrentPositionX + graphics.VisibleClipBounds.Width / 2d - (width / 2d);
        y = CurrentPositionY + graphics.VisibleClipBounds.Height / 2d - (height / 2d);
      }

      while (y > 0 && (height + y) >= graphics.VisibleClipBounds.Height)
        y--;

      while (y < 0 && y + height <= graphics.VisibleClipBounds.Height)
        y++;

      while (x > 0 && (width + x) >= graphics.VisibleClipBounds.Width)
        x--;

      while (x < 0 && x + width <= graphics.VisibleClipBounds.Width)
        x++;

      graphics.DrawImage (image, (float) x, (float) y, (float) width, (float) height);

      //graphics.DrawString (string.Format ("x: {0} y: {1}", (int)x, (int)y), Font, Brushes.Red, 0, 0);
      //graphics.DrawString (string.Format ("width: {0} height: {1}", (int)width, (int)height), Font, Brushes.Red, 0, 20);
      //graphics.DrawString (string.Format ("bound-width: {0} bound-height: {1}", (int)graphics.VisibleClipBounds.Width, (int)graphics.VisibleClipBounds.Height), Font, Brushes.Red, 0, 40);
    }

    private void CurrentPicture_MouseDown (object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        MouseDownPositionX = e.X - (int)CurrentPositionX;
        MouseDownPositionY = e.Y - (int)CurrentPositionY;
      }
    }

    private void CurrentPicture_MouseMove (object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        CurrentPositionX = e.X - MouseDownPositionX;
        CurrentPositionY = e.Y - MouseDownPositionY;

        CurrentPicture.Refresh ();
      }
    }

    private void PictureView_HelpButtonClicked (object sender, CancelEventArgs e)
    {
      PictureViewController.ShowHelpScreen ();
    }

    protected override void OnDragDrop (DragEventArgs drgevent)
    {
      var data = (string[])drgevent.Data.GetData (DataFormats.FileDrop);

      if (data.Length > 0)
      {
        var droppedFile = data[0];

        if (File.Exists (droppedFile))
          PictureViewController.SetDroppedFile (droppedFile);
      }

      base.OnDragDrop (drgevent);
    }

    protected override void OnDragEnter (DragEventArgs drgevent)
    {
      if (drgevent.Data.GetDataPresent (DataFormats.FileDrop))
        drgevent.Effect = DragDropEffects.Move;

      base.OnDragEnter (drgevent);
    }

    protected override void OnDragOver (DragEventArgs drgevent)
    {
      if (drgevent.Data.GetDataPresent (DataFormats.FileDrop))
        drgevent.Effect = DragDropEffects.Move;
      
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
      if (mouseEventArgs.Delta > 0)
        PictureViewController.ZoomIn();
      else
        PictureViewController.ZoomOut();
    }
  }
}
