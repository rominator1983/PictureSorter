using System;
using System.IO;
using Eto.Forms;

namespace PictureSorter
{
  public partial class SettingsView : Dialog
  {
    public string BestOfFolder { get; set; }

    public SettingsView()
    {
      InitializeComponent();

      MinimumSize = Size;
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      var suggestedBestOf = BestOfFolder + "/Best Of";

      if (Directory.Exists(suggestedBestOf))
      {
        BestOfFolder = suggestedBestOf;
      }
      else
      {
        var result = MessageBox.Show("Best of folder must exist. Create?", MessageBoxButtons.YesNo);

        if (result == DialogResult.Yes)
        {
          Directory.CreateDirectory(suggestedBestOf);
          BestOfFolder = suggestedBestOf;
        }
      }

      BestOfFolderText.Text = BestOfFolder;
    }

    private void ApplyButton_Click(object sender, EventArgs e)
    {
      BestOfFolder = BestOfFolderText.Text;

      if (!Directory.Exists(BestOfFolder))
      {
        var result = MessageBox.Show("Best of folder must exist. Create?", MessageBoxButtons.YesNo);

        if (result == DialogResult.Yes)
        {
          Directory.CreateDirectory(BestOfFolder);
          Close();
        }
      }
      else
        Close();
    }

    private void ChooseFolderButton_Click(object sender, EventArgs e)
    {
      var folderDialog = new SelectFolderDialog
      {
        Directory = BestOfFolderText.Text
      };

      if (folderDialog.ShowDialog(this) != DialogResult.Ok)
        return;

      if (Directory.Exists(folderDialog.Directory))
        BestOfFolderText.Text = folderDialog.Directory;
      else
      {
        var result = MessageBox.Show("Best of folder must exist. Create?", MessageBoxButtons.YesNo);

        if (result == DialogResult.Yes)
        {
          Directory.CreateDirectory(BestOfFolder);
          Close();
        }
      }
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
      BestOfFolder = null;
      Close();
    }
  }
}
