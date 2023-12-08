using System;
using System.IO;
using Eto.Forms;

namespace PictureSorter
{
  public partial class SettingsViiew : Form
  {
    public string BestOfFolder { get; set; }

    public SettingsViiew ()
    {
      InitializeComponent ();

      MinimumSize = Size;
    }

    protected override void OnLoad (EventArgs e)
    {
      base.OnLoad (e);

      var suggestedBestOf = BestOfFolder + "\\Best Of";

      if (Directory.Exists (suggestedBestOf))
        BestOfFolder = suggestedBestOf;

      BestOfFolderText.Text = BestOfFolder;
    }

    private void ApplyButton_Click (object sender, EventArgs e)
    {
      BestOfFolder = BestOfFolderText.Text;
      
      if (!Directory.Exists (BestOfFolder))
        MessageBox.Show ("Folder must exist.");
      else
        Close ();
    }

    private void ChooseFolderButton_Click (object sender, EventArgs e)
    {
      var folderDialog = new SelectFolderDialog() /* { ShowNewFolderButton = true } */;
      folderDialog.Directory = BestOfFolderText.Text;

      if (folderDialog.ShowDialog (this) != DialogResult.Ok)
        return;

      if (Directory.Exists (folderDialog.Directory))
        BestOfFolderText.Text = folderDialog.Directory;
      else
        MessageBox.Show ("Folder must exist.");
    }

    private void CancelButton_Click (object sender, EventArgs e)
    {
      BestOfFolder = null;
      Close ();
    }
  }
}
