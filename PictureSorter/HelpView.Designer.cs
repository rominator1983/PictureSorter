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
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      PixelLayout = new Eto.Forms.PixelLayout();

      // TODO: Add license NOTICES
      // TODO: Add DELETE key to help
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpView));
      this.hotKeyLabel = new Eto.Forms.Label();
      this.descriptionLabel = new Eto.Forms.Label();
      this.SuspendLayout();
      // 
      // hotKeyLabel
      // 
      this.hotKeyLabel.Size = new Eto.Drawing.Size(96, 737);
      this.hotKeyLabel.TabIndex = 0;
      this.hotKeyLabel.Text = "Left\r\n\r\nRight\r\n\r\ns\r\n\r\nb\r\n\r\nc\r\n\r\nl\r\n\r\nr\r\n\r\nEsc, Ctrl+F4\r\n\r\n+\r\n\r\n-\r\n\r\n0\r\n\r\nMouse wh" +
      "eel\r\n\r\nMouse drag\r\n\r\nFile drag/drop\r\n\r\ne\r\n\r\nF5\r\n\r\nAlt + Enter\r\n\r\nDouble Klick\r\n\r" +
      "\nF1\r\n\r\na\r\n\r\nd\r\n\r\ndel";
      // 
      // descriptionLabel
      // 
      //    this.descriptionLabel.Location = new Eto.Drawing.Point(142, 13);
      this.descriptionLabel.Size = new Eto.Drawing.Size(600, 737);
      this.descriptionLabel.TabIndex = 1;
      this.descriptionLabel.Text = resources.GetString("descriptionLabel.Text");
      // 
      // HelpView
      // 
      //this.AutoScaleDimensions = new Eto.Drawing.SizeF(8F, 16F);
      //this.AutoScaleMode = Eto.Forms.AutoScaleMode.Font;
      this.ClientSize = new Eto.Drawing.Size(700, 807);
      this.PixelLayout.Add(this.descriptionLabel, new Eto.Drawing.Point(142, 13));
      this.PixelLayout.Add(this.hotKeyLabel, new Eto.Drawing.Point(13, 13));
      this.Content = PixelLayout;
      //   this.StartPosition = Eto.Forms.FormStartPosition.CenterScreen;
      this.Title = "Help";
      this.KeyDown += new System.EventHandler<Eto.Forms.KeyEventArgs>(this.HelpView_PreviewKeyDown);
      this.ResumeLayout();
      //this.PerformLayout();

    }

    #endregion

    private Eto.Forms.PixelLayout PixelLayout;
    private Eto.Forms.Label hotKeyLabel;
    private Eto.Forms.Label descriptionLabel;
  }
}