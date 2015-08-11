namespace PictureSorter
{
  partial class HelpView
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpView));
      this.hotKeyLabel = new System.Windows.Forms.Label();
      this.descriptionLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // hotKeyLabel
      // 
      this.hotKeyLabel.AutoSize = true;
      this.hotKeyLabel.Location = new System.Drawing.Point(13, 13);
      this.hotKeyLabel.Name = "hotKeyLabel";
      this.hotKeyLabel.Size = new System.Drawing.Size(96, 697);
      this.hotKeyLabel.TabIndex = 0;
      this.hotKeyLabel.Text = "Left\r\n\r\nRight\r\n\r\ns\r\n\r\nb\r\n\r\nc\r\n\r\nl\r\n\r\nr\r\n\r\nEsc\r\n\r\n+\r\n\r\n-\r\n\r\n0\r\n\r\nMouse wheel\r\n\r\nMo" +
    "use drag\r\n\r\nFile drag/drop\r\n\r\ne\r\n\r\nF5\r\n\r\nAlt + Enter\r\n\r\nDouble Klick\r\n\r\nF1\r\n\r\na\r" +
    "\n\r\nd";
      // 
      // descriptionLabel
      // 
      this.descriptionLabel.AutoSize = true;
      this.descriptionLabel.Location = new System.Drawing.Point(127, 13);
      this.descriptionLabel.Name = "descriptionLabel";
      this.descriptionLabel.Size = new System.Drawing.Size(385, 697);
      this.descriptionLabel.TabIndex = 1;
      this.descriptionLabel.Text = resources.GetString("descriptionLabel.Text");
      // 
      // HelpView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(527, 725);
      this.Controls.Add(this.descriptionLabel);
      this.Controls.Add(this.hotKeyLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "HelpView";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "HelpView";
      this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.HelpView_PreviewKeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label hotKeyLabel;
    private System.Windows.Forms.Label descriptionLabel;
  }
}