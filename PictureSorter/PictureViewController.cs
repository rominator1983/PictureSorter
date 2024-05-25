using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Eto.Forms;

namespace PictureSorter
{
  public class PictureViewController : IPictureViewController
  {
    public IPictureView PictureView { get; private set; }
    public IPictureCache PictureCache { get; private set; }
    public string BestOfFolder { get; private set; }
    public double ZoomFactor { get; private set; }

    public PictureViewController(IPictureCache pictureCache)
    {
      PictureCache = pictureCache;
      ZoomFactor = 1.0;
    }

    public void SetPictureForm(IPictureView pictureView)
    {
      PictureView = pictureView;
      SetCurrentPicture();
    }

    public void ToggleFullScreen()
    {
      PictureView.ToggleFullScreen();
    }

    public void Previous()
    {
      PictureCache.Previous();
      SetCurrentPicture();
    }

    public void Next()
    {
      PictureCache.Next();
      SetCurrentPicture();
    }

    public void MoveCurrentToBestOf()
    {
      if (BestOfFolder == null)
        SetBestOfFolder();

      if (BestOfFolder == null)
        return;

      var targetPath = Path.Combine(BestOfFolder, Path.GetFileName(PictureCache.CurrentFileName));

      if (FileExists(targetPath))
        return;

      File.Move(PictureCache.CurrentFileName, targetPath);

      PictureCache.RefreshAfterFileManipulation();
      SetCurrentPicture();
    }

    public void CopyCurrentToBestOf()
    {
      if (BestOfFolder == null)
        SetBestOfFolder();

      if (BestOfFolder == null)
        return;

      var targetPath = Path.Combine(BestOfFolder, Path.GetFileName(PictureCache.CurrentFileName));

      if (FileExists(targetPath))
        return;

      File.Copy(PictureCache.CurrentFileName, targetPath);
    }

    private static bool FileExists(string targetPath)
    {
      if (File.Exists(targetPath))
      {
        MessageBox.Show(string.Format("File '{0}' already exists.\r\n\r\nNothing has been changed.", targetPath),
          "Error", MessageBoxButtons.OK, MessageBoxType.Warning);

        return true;
      }
      return false;
    }

    public void SetBestOfFolder()
    {
      var settingsView = new SettingsView { BestOfFolder = PictureCache.DirectoryName };

      settingsView.ShowModal();

      BestOfFolder = settingsView.BestOfFolder;
    }

    public void SetDroppedFile(string fileName)
    {
      PictureCache.UnloadCache();
      PictureCache.Initialize(fileName);

      SetCurrentPicture();
    }

    private void SetCurrentPicture()
    {
      var currentPicture = GetCurrentPicture();
      PictureView.SetCurrentPicture(currentPicture);
    }

    private Picture GetCurrentPicture()
    {
      return new Picture
      {
        FileName = PictureCache.CurrentFileName,
        Bitmap = PictureCache.CurrentBitmap,
      };
    }

    public void Close()
    {
      PictureView.Close();
    }

    public void RotateRight()
    {
      Rotate("-rotate 90");
      PictureCache.RefreshAfterFileManipulation();
      SetCurrentPicture();
    }

    public void RotateLeft()
    {
      Rotate("-rotate 270");
      PictureCache.RefreshAfterFileManipulation();
      SetCurrentPicture();
    }

    private void Rotate(string rotateParameter)
    {
      try
      {
        var jpegRotatorExe = GetJpegTranPath();

        var process = new Process();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = jpegRotatorExe;
        process.StartInfo.CreateNoWindow = true;

        process.StartInfo.Arguments = string.Format("{0} -outfile \"{1}\" \"{1}\"", rotateParameter, PictureCache.CurrentFileName);

        process.Start();
        process.WaitForExit();
        if (process.ExitCode != 0)
          throw new Exception($"Error rotating picture with jpegtran. Exit code: {process.ExitCode}");

      }
      catch (Exception exception)
      {
        MessageBox.Show("Error rotating picture. Probably jpegtran not installed.\r\nInstall with:\r\nsudo apt install libjpeg-turbo-progs\r\n\r\nException:\r\n\r\n" + exception.Message);
      }
    }

    private string GetJpegTranPath() =>
    ConfigurationManager.AppSettings["JpegLoslessRotate"];

    public void ZoomIn()
    {
      ZoomFactor = Math.Min(ZoomFactor * 1.4142135623730950488016887242097, 1000);
      PictureView.SetZoom(ZoomFactor);
    }

    public void ZoomOut()
    {
      ZoomFactor = ZoomFactor / 1.4142135623730950488016887242097;

      if (ZoomFactor <= 1.0)
      {
        ZoomFactor = 1.0;
        PictureView.ResetZoomAndPosition();
      }
      else
        PictureView.SetZoom(ZoomFactor);
    }

    public void ZoomDefault()
    {
      ZoomFactor = 1.0;
      PictureView.ResetZoomAndPosition();
    }

    public void ShowHelpScreen()
    {
      new HelpView().ShowModal();
    }

    public void Refresh()
    {
      PictureCache.RefreshAfterFileManipulation();
      SetCurrentPicture();
    }

    public void Edit()
    {
      var editProgramm = ConfigurationManager.AppSettings["EditProgram"];

      var process = new Process();
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.FileName = editProgramm;
      process.StartInfo.CreateNoWindow = true;

      process.StartInfo.Arguments = string.Format("\"{0}\"", PictureCache.CurrentFileName);

      try
      {
        process.Start();
      }
      catch (Exception exception)
      {
        MessageBox.Show("Error starting edit program:\r\n\r\n" + exception.Message);
      }
    }

    public void SortAlphabetically()
    {
      PictureCache.SortAlphabetically();
    }

    public void SortByDate()
    {
      PictureCache.SortByDate();
    }

    public void MoveToTrashBin()
    {
      var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
      var fileName = Path.GetFileName(PictureCache.CurrentFileName);
      File.Move(PictureCache.CurrentFileName, $"{homeFolder}/.local/share/Trash/files/{fileName}", true);

      PictureCache.RefreshAfterFileManipulation();
      SetCurrentPicture();
    }
  }
}