using System;
using Eto.Forms;

namespace PictureSorter
{
  partial class SettingsView
  {
    private void InitializeComponent()
    {
      PixelLayout = new Eto.Forms.PixelLayout();

      this.BestOfFolderLabel = new Eto.Forms.Label();
      this.BestOfFolderText = new Eto.Forms.TextBox();
      this.ChooseFolderButton = new Eto.Forms.Button();
      this.ApplyButton = new Eto.Forms.Button();
      this.CancelSettingsButton = new Eto.Forms.Button();
      this.SuspendLayout();
      // 
      // BestOfFolderLabel
      // 
      this.BestOfFolderLabel.Size = new Eto.Drawing.Size(100, 17);
      this.BestOfFolderLabel.TabIndex = 0;
      this.BestOfFolderLabel.Text = "Best-Of Folder";
      // 
      // BestOfFolderText
      // 
      this.BestOfFolderText.Size = new Eto.Drawing.Size(602, 22);
      this.BestOfFolderText.TabIndex = 1;
      // 
      // ChooseFolderButton
      // 
      this.ChooseFolderButton.Size = new Eto.Drawing.Size(104, 29);
      this.ChooseFolderButton.TabIndex = 2;
      this.ChooseFolderButton.Text = "&Choose";
      this.ChooseFolderButton.Click += new System.EventHandler<EventArgs>(this.ChooseFolderButton_Click);
      // 
      // ApplyButton
      // 
      this.ApplyButton.Size = new Eto.Drawing.Size(104, 29);
      this.ApplyButton.TabIndex = 3;
      this.ApplyButton.Text = "&Apply";
      this.ApplyButton.Click += new System.EventHandler<System.EventArgs>(this.ApplyButton_Click);
      // 
      // CancelSettingsButton
      // 
      this.CancelSettingsButton.Size = new Eto.Drawing.Size(104, 29);
      this.CancelSettingsButton.TabIndex = 4;
      this.CancelSettingsButton.Text = "&Cancel";
      this.CancelSettingsButton.Click += new System.EventHandler<System.EventArgs>(this.CancelButton_Click);
      // 
      // SettingsViiew
      // 
      this.ClientSize = new Eto.Drawing.Size(1061, 51);
      this.PixelLayout.Add(this.CancelSettingsButton, new Eto.Drawing.Point(949, 13));
      this.PixelLayout.Add(this.ApplyButton, new Eto.Drawing.Point(839, 13));
      this.PixelLayout.Add(this.ChooseFolderButton, new Eto.Drawing.Point(729, 13));
      this.PixelLayout.Add(this.BestOfFolderText, new Eto.Drawing.Point(120, 16));
      this.PixelLayout.Add(this.BestOfFolderLabel, new Eto.Drawing.Point(14, 19));
      this.Content = PixelLayout;
      this.Title = "Settings";
      this.ResumeLayout();
    }

    private Eto.Forms.PixelLayout PixelLayout;
    private Eto.Forms.Label BestOfFolderLabel;
    private Eto.Forms.TextBox BestOfFolderText;
    private Eto.Forms.Button ChooseFolderButton;
    private Eto.Forms.Button ApplyButton;
    private Eto.Forms.Button CancelSettingsButton;
  }
}