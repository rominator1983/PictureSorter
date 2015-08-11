using System;

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
      this.CurrentPicture = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.CurrentPicture)).BeginInit();
      this.SuspendLayout();
      // 
      // CurrentPicture
      // 
      this.CurrentPicture.Cursor = System.Windows.Forms.Cursors.Hand;
      this.CurrentPicture.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CurrentPicture.Location = new System.Drawing.Point(0, 0);
      this.CurrentPicture.Name = "CurrentPicture";
      this.CurrentPicture.Size = new System.Drawing.Size(396, 255);
      this.CurrentPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.CurrentPicture.TabIndex = 1;
      this.CurrentPicture.TabStop = false;
      this.CurrentPicture.DoubleClick += new System.EventHandler(this.CurrentPicture_DoubleClick);
      this.CurrentPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CurrentPicture_MouseDown);
      this.CurrentPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CurrentPicture_MouseMove);
      // 
      // PictureView
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
      this.BackColor = System.Drawing.Color.Black;
      this.ClientSize = new System.Drawing.Size(396, 255);
      this.Controls.Add(this.CurrentPicture);
      this.HelpButton = true;
      this.KeyPreview = true;
      this.Name = "PictureView";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Picture Sorter";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.PictureView_HelpButtonClicked);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PictureForm_KeyDown);
      this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PictureView_MouseDoubleClick);
      ((System.ComponentModel.ISupportInitialize)(this.CurrentPicture)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox CurrentPicture;
  }
}