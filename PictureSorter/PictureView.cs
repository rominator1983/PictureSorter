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

    public PictureView(PictureViewController pictureViewController, KeyInputHandler keyInputHandler)
    {
      InitializeComponent();

      CurrentZoomFactor = 1.0;
      PictureViewController = pictureViewController;
      KeyInputHandler = keyInputHandler;

      PictureViewController.SetPictureForm(this);

      MouseWheel += OnMouseWheel;
      //CurrentPicture.Paint += CurrentPictureOnPaint;
    }

    public void SetCurrentPicture(Picture picture)
    {
      Title = "Picture Sorter - " + picture.FileName;
      //CurrentPicture.Image = picture.Bitmap;
      CurrentBitmap = picture.Bitmap;
      //CurrentPicture.Image = picture.Bitmap;
    }

    private void Update()
    {
      Console.WriteLine("Update");
      DrawImage(CurrentZoomFactor, CurrentPicture.CreateGraphics());
    }

    public void ToggleFullScreen()
    {
      if (WindowState == WindowState.Maximized)
        SetNonFullScreen();
      else
        SetFullScreen();
    }

    private void SetFullScreen()
    {
      WindowState = WindowState.Maximized;
    }

    public void SetNonFullScreen()
    {
      WindowState = WindowState.Normal;
    }

    private void PictureForm_KeyDown(object sender, KeyEventArgs e)
    {
      KeyInputHandler.Handle(e);
    }

    private void CurrentPictureOnPaint(object sender, PaintEventArgs paintEventArgs)
    {
      DrawImage(CurrentZoomFactor, paintEventArgs.Graphics);
      Update();
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

    private void DrawImage(double zoomFactor, Graphics graphics)
    {
      graphics.Clear(Color.FromRgb(0));

      var image = CurrentBitmap;

      if (image == null)
        return;

      var grfxFactor = graphics.ClipBounds.Width / (double)graphics.ClipBounds.Height;
      var imageFactor = image.Width / (double)image.Height;

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

      graphics.DrawImage(image, (float)x, (float)y, (float)width, (float)height);
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

    protected override void OnDragDrop(DragEventArgs drgevent)
    {
      // IMPROVE: implement dragging into form. Currently, the form is not accepting any files.
      //var data = (string[])drgevent.Data.GetData (DataFormats.FileDrop);
      //
      //if (data.Length > 0)
      //{
      //  var droppedFile = data[0];
      //
      //  if (File.Exists (droppedFile))
      //    PictureViewController.SetDroppedFile (droppedFile);
      //}

      base.OnDragDrop(drgevent);
    }

    protected override void OnDragEnter(DragEventArgs drgevent)
    {
      // IMPROVE: implement dragging into form. Currently, the form is not accepting any files.
      //if (drgevent.Data.GetDataPresent (DataFormats.FileDrop))
      //  drgevent.Effect = DragDropEffects.Move;
      //
      base.OnDragEnter(drgevent);
    }

    protected override void OnDragOver(DragEventArgs drgevent)
    {
      // IMPROVE: implement dragging into form. Currently, the form is not accepting any files.
      //if (drgevent.Data.GetDataPresent (DataFormats.FileDrop))
      //  drgevent.Effect = DragDropEffects.Move;

      base.OnDragOver(drgevent);
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
      if (mouseEventArgs.Delta.Height > 0)
        PictureViewController.ZoomIn();
      else
        PictureViewController.ZoomOut();
    }
  }
}
