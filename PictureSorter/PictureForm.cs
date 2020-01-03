using System;
using System.Drawing;
using System.Windows.Forms;

namespace PictureSorter
{
  public partial class PictureForm : Form, IPictureForm
  {
    public IKeyInputHandler KeyInputHandler { get; private set; }
    public IPictureFormController PictureFormController { get; private set; }
    public Bitmap CurrentCitmap { get; private set; }
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
      //var window = System.Windows.Window.GetWindow(this);
      //var wih = new System.Windows.Interop.WindowInteropHelper(window);
      //IntPtr hWnd = wih.Handle;

      KeyInputHandler.Handle (IntPtr.Zero, e);
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

      var grfxFactor = graphics.VisibleClipBounds.Width / (double) graphics.VisibleClipBounds.Height;
      var imageFactor = image.Width / (double) image.Height;

      if (grfxFactor > imageFactor)
      {
        // use height for scaling
        var scale = image.Height / graphics.VisibleClipBounds.Height;

        var width = image.Width / scale * zoomFactor;
        var height = graphics.VisibleClipBounds.Height * zoomFactor;

        var x = CurrentPositionX + graphics.VisibleClipBounds.Width / 2d - (width / 2d);
        var y = CurrentPositionY + graphics.VisibleClipBounds.Height / 2d - (height / 2d);

        graphics.DrawImage (image, (float) x, (float) y, (float) width, (float) height);
      }
      else
      {
        // use width for scaling
        var scale = image.Width / graphics.VisibleClipBounds.Width;

        var width = graphics.VisibleClipBounds.Width * zoomFactor;
        var height = image.Height / scale * zoomFactor;

        var x = CurrentPositionX + graphics.VisibleClipBounds.Width / 2d - (width / 2d);
        var y = CurrentPositionY + graphics.VisibleClipBounds.Height / 2d - (height / 2d);

        graphics.DrawImage (image, (float) x, (float) y, (float) width, (float) height);
      }
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
  }
}
