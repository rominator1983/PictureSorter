using System;
using Eto.Forms;
using Eto.GtkSharp;
using PictureSorter.SkiaGtk;

namespace PictureSorter
{
  partial class PictureView
  {
    private MySKControl CurrentPicture;

    private void InitializeComponent()
    {
      Eto.Platform.Instance.Add<MySKControl.ISKControl>(() =>
      {
        return new MySKControlHandler { BackgroundColor = Eto.Drawing.Colors.Red };
      });

      // 
      // CurrentPicture
      // 
      this.CurrentPicture = new MySKControl();
      this.CurrentPicture.Cursor = Eto.Forms.Cursors.Pointer;
      this.CurrentPicture.Size = new Eto.Drawing.Size(396, 255);
      this.CurrentPicture.TabIndex = 1;
      this.CurrentPicture.MouseDoubleClick += new System.EventHandler<MouseEventArgs>(this.CurrentPicture_DoubleClick);
      this.CurrentPicture.MouseDown += new System.EventHandler<MouseEventArgs>(this.CurrentPicture_MouseDown);
      this.CurrentPicture.MouseMove += new System.EventHandler<MouseEventArgs>(this.CurrentPicture_MouseMove);

      this.CurrentPicture.PaintSurfaceAction = CurrentPictureOnPaint;

      // 
      // PictureView
      // 
      this.AllowDrop = true;
      this.BackgroundColor = Eto.Drawing.Color.FromRgb(0);
      this.ClientSize = new Eto.Drawing.Size(396, 255);
      this.Content = this.CurrentPicture;
      this.Title = "Picture Sorter";
      this.WindowState = Eto.Forms.WindowState.Maximized;
      this.KeyDown += new System.EventHandler<KeyEventArgs>(this.PictureForm_KeyDown);
      
      this.ToGtk().KeyReleaseEvent += new Gtk.KeyReleaseEventHandler(this.PictureForm_GtkKeyPress);

      this.MouseDoubleClick += new System.EventHandler<MouseEventArgs>(this.PictureView_MouseDoubleClick);
    }
  }
}