using System;
using Eto.Drawing;
using Eto.Forms;
using Gtk;

namespace PictureSorter
{
  public partial class PictureForm : Form, IPictureForm
  {
    public IKeyInputHandler KeyInputHandler { get; private set; }
    public IPictureFormController PictureFormController { get; private set; }
    public Bitmap CurrentBitmap { get; private set; }
    public double CurrentZoomFactor { get; private set; }
    public float CurrentPositionX { get; private set; }
    public float CurrentPositionY { get; private set; }

    public int MouseDownPositionX { get; private set; }
    public int MouseDownPositionY { get; private set; }

    public PictureForm (PictureFormController pictureFormController, KeyInputHandler keyInputHandler)
    {
      InitializeComponent ();

      CurrentZoomFactor = 1.0;
      PictureFormController = pictureFormController;
      KeyInputHandler = keyInputHandler;

      PictureFormController.SetPictureForm (this);

      //CurrentPicture.Paint += CurrentPictureOnPaint;
    }

    public void SetCurrentPicture (Picture picture)
    {
      Title = "Picture Sorter - " + picture.FileName; 
      //Text = "Picture Sorter - " + picture.FileName;
      //CurrentPicture.Image = picture.Bitmap;
      CurrentBitmap = picture.Bitmap;

      CurrentPicture.Image = picture.Bitmap;
      //CurrentPicture.Refresh ();
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
      WindowState = WindowState.Normal;
      //TopMost = true;
      //BorderStyle = FormBorderStyle.None;
      //FormBorderStyle = FormBorderStyle.None;
      WindowState = WindowState.Maximized;

    }

    public void SetNonFullScreen ()
    {
      WindowState = WindowState.Normal;
      //FormBorderStyle = FormBorderStyle.Sizable;
      //TopMost = false;
    }

    // protected override bool IsInputKey (Keys keyData)
    // {
    //   return true;
    // }

    private void PictureForm_KeyDown (object sender, KeyEventArgs e)
    {
      //var window = System.Windows.Window.GetWindow(this);
      //var wih = new System.Windows.Interop.WindowInteropHelper(window);
      //IntPtr hWnd = wih.Handle;

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
      //CurrentPicture.Refresh ();
    }

    public void SetZoom (double zoomFactor)
    {
      CurrentPositionX = (int)(CurrentPositionX * zoomFactor / CurrentZoomFactor);
      CurrentPositionY = (int)(CurrentPositionY * zoomFactor / CurrentZoomFactor);
      CurrentZoomFactor = zoomFactor;
      //CurrentPicture.Refresh ();
    }

    private void DrawImage (double zoomFactor, Graphics graphics)
    {
      graphics.Clear (Color.FromRgb(0));

      var image = CurrentBitmap;

      var grfxFactor = graphics.ClipBounds.Width / (double) graphics.ClipBounds.Height;
      var imageFactor = image.Width / (double) image.Height;

      if (grfxFactor > imageFactor)
      {
        // use height for scaling
        var scale = image.Height / graphics.ClipBounds.Height;

        var width = image.Width / scale * zoomFactor;
        var height = graphics.ClipBounds.Height * zoomFactor;

        var x = CurrentPositionX + graphics.ClipBounds.Width / 2d - (width / 2d);
        var y = CurrentPositionY + graphics.ClipBounds.Height / 2d - (height / 2d);

        graphics.DrawImage (image, (float) x, (float) y, (float) width, (float) height);
      }
      else
      {
        // use width for scaling
        var scale = image.Width / graphics.ClipBounds.Width;

        var width = graphics.ClipBounds.Width * zoomFactor;
        var height = image.Height / scale * zoomFactor;

        var x = CurrentPositionX + graphics.ClipBounds.Width / 2d - (width / 2d);
        var y = CurrentPositionY + graphics.ClipBounds.Height / 2d - (height / 2d);

        graphics.DrawImage (image, (float) x, (float) y, (float) width, (float) height);
      }
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
        CurrentPositionX = e.Location.X - MouseDownPositionX;
        CurrentPositionY = e.Location.Y - MouseDownPositionY;

//        CurrentPicture.Refresh ();
      }
    }
  }
}
