using System;

namespace PictureSorter
{
  partial class SettingsView
  {
    private void InitializeComponent()
    {
      this.BestOfFolderLabel = new Eto.Forms.Label();
      this.BestOfFolderText = new Eto.Forms.TextBox();
      this.ChooseFolderButton = new Eto.Forms.Button();
      this.ApplyButton = new Eto.Forms.Button();
      this.CancelSettingsButton = new Eto.Forms.Button();
      this.SuspendLayout();
      // 
      // BestOfFolderLabel
      // 
      //this.BestOfFolderLabel.Location = new Eto.Drawing.Point(14, 19);
      this.BestOfFolderLabel.Size = new Eto.Drawing.Size(100, 17);
      this.BestOfFolderLabel.TabIndex = 0;
      this.BestOfFolderLabel.Text = "Best-Of Folder";
      // 
      // BestOfFolderText
      // 
      //this.BestOfFolderText.Location = new Eto.Drawing.Point(120, 16);
      this.BestOfFolderText.Size = new Eto.Drawing.Size(602, 22);
      this.BestOfFolderText.TabIndex = 1;
      // 
      // ChooseFolderButton
      // 
      // this.ChooseFolderButton.Location = new Eto.Drawing.Point(729, 13);
      this.ChooseFolderButton.Size = new Eto.Drawing.Size(104, 29);
      this.ChooseFolderButton.TabIndex = 2;
      this.ChooseFolderButton.Text = "&Choose";
      this.ChooseFolderButton.Click += new System.EventHandler<EventArgs>(this.ChooseFolderButton_Click);
      // 
      // ApplyButton
      // 
      // this.ApplyButton.Location = new Eto.Drawing.Point(839, 13);
      this.ApplyButton.Size = new Eto.Drawing.Size(104, 29);
      this.ApplyButton.TabIndex = 3;
      this.ApplyButton.Text = "&Apply";
      this.ApplyButton.Click += new System.EventHandler<System.EventArgs>(this.ApplyButton_Click);
      // 
      // CancelSettingsButton
      // 
      // this.CancelSettingsButton.Location = new Eto.Drawing.Point(949, 13);
      this.CancelSettingsButton.Size = new Eto.Drawing.Size(104, 29);
      this.CancelSettingsButton.TabIndex = 4;
      this.CancelSettingsButton.Text = "&Cancel";
      this.CancelSettingsButton.Click += new System.EventHandler<System.EventArgs>(this.CancelButton_Click);
      // 
      // SettingsViiew
      // 
      //this.AcceptButton = this.ApplyButton;
      //this.CancelButton = this.CancelSettingsButton;
      this.ClientSize = new Eto.Drawing.Size(1061, 51);
      // this.Controls.Add(this.CancelSettingsButton);
      // this.Controls.Add(this.ApplyButton);
      // this.Controls.Add(this.ChooseFolderButton);
      // this.Controls.Add(this.BestOfFolderText);
      // this.Controls.Add(this.BestOfFolderLabel);
      //this.Name = "SettingsViiew";
      this.Title = "Settings";
      this.ResumeLayout();
      //this.PerformLayout();

    }

    private Eto.Forms.Label BestOfFolderLabel;
    private Eto.Forms.TextBox BestOfFolderText;
    private Eto.Forms.Button ChooseFolderButton;
    private Eto.Forms.Button ApplyButton;
    private Eto.Forms.Button CancelSettingsButton;
  }
}