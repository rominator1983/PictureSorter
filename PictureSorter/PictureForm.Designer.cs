using System;
using Eto.Forms;

namespace PictureSorter
{
  partial class PictureForm
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
      this.CurrentPicture = new Eto.Forms.ImageView();
      ((System.ComponentModel.ISupportInitialize)(this.CurrentPicture)).BeginInit();
      this.SuspendLayout();
      // 
      // CurrentPicture
      // 
      //this.CurrentPicture.Dock = Eto.Forms.DockStyle.Fill;
      //this.CurrentPicture.Location = new Eto.Drawing.Point(0, 0);
      //this.CurrentPicture.Name = "CurrentPicture";
      this.CurrentPicture.Size = new Eto.Drawing.Size(357, 287);
      //this.CurrentPicture.SizeMode = Eto.Forms.;
      this.CurrentPicture.TabIndex = 1;
      //this.CurrentPicture.TabStop = false;
      this.CurrentPicture.MouseDown += new EventHandler<Eto.Forms.MouseEventArgs>(this.CurrentPicture_MouseDown);
      this.CurrentPicture.MouseMove += new EventHandler<Eto.Forms.MouseEventArgs>(this.CurrentPicture_MouseMove);
      // 
      // PictureForm
      // 
//      this.AutoScaleDimensions = new Eto.Drawing.SizeF(8F, 16F);
//      this.AutoScaleMode = Eto.Forms.AutoScaleMode.Font;
//      this.AutoValidate = Eto.Forms.AutoValidate.Disable;
      this.BackgroundColor = Eto.Drawing.Color.FromRgb(0);
      this.ClientSize = new Eto.Drawing.Size(357, 287);
      this.Content = this.CurrentPicture;
      //this.KeyPreview = true;
      //this.Name = "PictureForm";
      //this.SizeGripStyle = Eto.Forms.SizeGripStyle.Show;
      //this.StartPosition = Eto.Forms.FormStartPosition.CenterScreen;
      this.Title = "Picture Sorter";
      this.WindowState = Eto.Forms.WindowState.Maximized;
      this.KeyDown += new System.EventHandler<KeyEventArgs>(this.PictureForm_KeyDown);
      ((System.ComponentModel.ISupportInitialize)(this.CurrentPicture)).EndInit();
      this.ResumeLayout();

    }

    #endregion

    private Eto.Forms.ImageView CurrentPicture;
  }
}