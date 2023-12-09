using System;
using Eto.Forms;
using Eto.GtkSharp;

namespace PictureSorter
{
  partial class PictureView
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose (bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose ();
      }
      base.Dispose (disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent ()
    {
      this.CurrentPicture = new Eto.Forms.Drawable();
      // 
      // CurrentPicture
      // 
      this.CurrentPicture.Cursor = Eto.Forms.Cursors.Crosshair;
      //this.CurrentPicture.Dock = Eto.Forms.DockStyle.Fill;
      //this.CurrentPicture.Location = new Eto.Drawing.Point(0, 0);
      //this.CurrentPicture.Name = "CurrentPicture";
      this.CurrentPicture.Size = new Eto.Drawing.Size(396, 255);
      //this.CurrentPicture.SizeMode = Eto.Forms.PictureBoxSizeMode.Zoom;
      this.CurrentPicture.TabIndex = 1;
      //this.CurrentPicture.TabStop = false;
      this.CurrentPicture.MouseDoubleClick += new System.EventHandler<MouseEventArgs>(this.CurrentPicture_DoubleClick);
      this.CurrentPicture.MouseDown += new System.EventHandler<MouseEventArgs>(this.CurrentPicture_MouseDown);
      this.CurrentPicture.MouseMove += new System.EventHandler<MouseEventArgs>(this.CurrentPicture_MouseMove);

      this.CurrentPicture.Paint += CurrentPictureOnPaint;

      // 
      // PictureView
      // 
      this.AllowDrop = true;
      //this.AutoScaleDimensions = new Eto.Drawing.SizeF(8F, 16F);
      //this.AutoScaleMode = Eto.Forms.AutoScaleMode.Font;
      //this.AutoValidate = Eto.Forms.AutoValidate.Disable;
      this.BackgroundColor = Eto.Drawing.Color.FromRgb(0);
      this.ClientSize = new Eto.Drawing.Size(396, 255);
      this.Content = this.CurrentPicture;
      //this.HelpButton = true;
      //this.KeyPreview = true;
      //this.Name = "PictureView";
      //this.SizeGripStyle = Eto.Forms.SizeGripStyle.Show;
      //this.StartPosition = Eto.Forms.FormStartPosition.CenterScreen;
      this.Title = "Picture Sorter";
      this.WindowState = Eto.Forms.WindowState.Maximized;
      //this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.PictureView_HelpButtonClicked);
      this.KeyDown += new System.EventHandler<KeyEventArgs>(this.PictureForm_KeyDown);
      //this.KeyDown += new System.EventHandler<KeyEventArgs>(this.PictureForm_KeyUp);
      this.ToGtk().KeyReleaseEvent += new Gtk.KeyReleaseEventHandler(this.PictureForm_KeyPress);

      this.MouseDoubleClick += new System.EventHandler<MouseEventArgs>(this.PictureView_MouseDoubleClick);
    }

    #endregion

    private Eto.Forms.Drawable CurrentPicture;
  }
}