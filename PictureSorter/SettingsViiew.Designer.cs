using System;

namespace PictureSorter
{
  partial class SettingsViiew
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
      this.BestOfFolderLabel = new System.Windows.Forms.Label();
      this.BestOfFolderText = new System.Windows.Forms.TextBox();
      this.ChooseFolderButton = new System.Windows.Forms.Button();
      this.ApplyButton = new System.Windows.Forms.Button();
      this.CancelSettingsButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // BestOfFolderLabel
      // 
      this.BestOfFolderLabel.AutoSize = true;
      this.BestOfFolderLabel.Location = new System.Drawing.Point(14, 19);
      this.BestOfFolderLabel.Name = "BestOfFolderLabel";
      this.BestOfFolderLabel.Size = new System.Drawing.Size(100, 17);
      this.BestOfFolderLabel.TabIndex = 0;
      this.BestOfFolderLabel.Text = "Best-Of Folder";
      // 
      // BestOfFolderText
      // 
      this.BestOfFolderText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.BestOfFolderText.Location = new System.Drawing.Point(120, 16);
      this.BestOfFolderText.Name = "BestOfFolderText";
      this.BestOfFolderText.Size = new System.Drawing.Size(602, 22);
      this.BestOfFolderText.TabIndex = 1;
      // 
      // ChooseFolderButton
      // 
      this.ChooseFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ChooseFolderButton.Location = new System.Drawing.Point(729, 13);
      this.ChooseFolderButton.Name = "ChooseFolderButton";
      this.ChooseFolderButton.Size = new System.Drawing.Size(104, 29);
      this.ChooseFolderButton.TabIndex = 2;
      this.ChooseFolderButton.Text = "&Choose";
      this.ChooseFolderButton.UseVisualStyleBackColor = true;
      this.ChooseFolderButton.Click += new System.EventHandler(this.ChooseFolderButton_Click);
      // 
      // ApplyButton
      // 
      this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ApplyButton.Location = new System.Drawing.Point(839, 13);
      this.ApplyButton.Name = "ApplyButton";
      this.ApplyButton.Size = new System.Drawing.Size(104, 29);
      this.ApplyButton.TabIndex = 3;
      this.ApplyButton.Text = "&Apply";
      this.ApplyButton.UseVisualStyleBackColor = true;
      this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
      // 
      // CancelSettingsButton
      // 
      this.CancelSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.CancelSettingsButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelSettingsButton.Location = new System.Drawing.Point(949, 13);
      this.CancelSettingsButton.Name = "CancelSettingsButton";
      this.CancelSettingsButton.Size = new System.Drawing.Size(104, 29);
      this.CancelSettingsButton.TabIndex = 4;
      this.CancelSettingsButton.Text = "&Cancel";
      this.CancelSettingsButton.UseVisualStyleBackColor = true;
      this.CancelSettingsButton.Click += new System.EventHandler(this.CancelButton_Click);
      // 
      // SettingsViiew
      // 
      this.AcceptButton = this.ApplyButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.CancelSettingsButton;
      this.ClientSize = new System.Drawing.Size(1061, 51);
      this.Controls.Add(this.CancelSettingsButton);
      this.Controls.Add(this.ApplyButton);
      this.Controls.Add(this.ChooseFolderButton);
      this.Controls.Add(this.BestOfFolderText);
      this.Controls.Add(this.BestOfFolderLabel);
      this.Name = "SettingsViiew";
      this.Text = "Settings";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label BestOfFolderLabel;
    private System.Windows.Forms.TextBox BestOfFolderText;
    private System.Windows.Forms.Button ChooseFolderButton;
    private System.Windows.Forms.Button ApplyButton;
    private System.Windows.Forms.Button CancelSettingsButton;
  }
}