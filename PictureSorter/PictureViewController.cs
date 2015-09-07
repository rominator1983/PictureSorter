using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PictureSorter
{
  public class PictureViewController : IPictureViewController
  {
    public IPictureView PictureView { get; private set; }
    public IPictureCache PictureCache { get; private set;}
    public string BestOfFolder { get; private set; }
    public double ZoomFactor { get; private set;} 

    public PictureViewController (IPictureCache pictureCache)
    {
      PictureCache = pictureCache;
      ZoomFactor = 1.0;
    }

    public void SetPictureForm (IPictureView pictureView)
    {
      PictureView = pictureView;
      SetCurrentPicture ();
    }

    public void ToggleFullScreen ()
    {
      PictureView.ToggleFullScreen ();
    }

    public void Previous ()
    {
      PictureCache.Previous ();
      SetCurrentPicture ();
    }

    public void Next ()
    {
      PictureCache.Next ();
      SetCurrentPicture ();
    }

    public void MoveCurrentToBestOf()
    {
      if (BestOfFolder == null)
        SetBestOfFolder ();

      if (BestOfFolder == null)
        return;


      var targetPath = Path.Combine (BestOfFolder, Path.GetFileName (PictureCache.CurrentFileName));

      if (File.Exists (targetPath))
      {
        MessageBox.Show (string.Format ("File '{0}' already exists.\r\n\r\nNothing has been changed.", targetPath),
          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        return;
      }

      File.Move (PictureCache.CurrentFileName, targetPath);

      PictureCache.RefreshAfterFileManipulation ();
      SetCurrentPicture ();
    }

    public void CopyCurrentToBestOf ()
    {
      if (BestOfFolder == null)
        SetBestOfFolder ();

      if (BestOfFolder == null)
        return;

      var targetPath = Path.Combine (BestOfFolder, Path.GetFileName (PictureCache.CurrentFileName));

      if (File.Exists(targetPath))
      {
        MessageBox.Show(string.Format("File '{0}' already exists.\r\n\r\nNothing has been changed.", targetPath),
          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        return;
      }

      File.Copy (PictureCache.CurrentFileName, targetPath);
    }

    public void SetBestOfFolder ()
    {
      PictureView.SetNonFullScreen ();
      var settingsViiew = new SettingsViiew {BestOfFolder = PictureCache.DirectoryName};

      settingsViiew.ShowDialog ();

      BestOfFolder = settingsViiew.BestOfFolder;
    }

    public void SetDroppedFile (string fileName)
    {
      PictureCache.UnloadCache ();
      PictureCache.Initialize (fileName);

      SetCurrentPicture ();
    }

    private void SetCurrentPicture ()
    {
      var currentPicture = GetCurrentPicture ();
      PictureView.SetCurrentPicture (currentPicture);
    }

    private Picture GetCurrentPicture ()
    {
      return new Picture
      {
        FileName = PictureCache.CurrentFileName,
        Bitmap = PictureCache.CurrentBitmap,
      };
    }

    public void Close ()
    {
      PictureView.Close ();
    }

    public void RotateRight ()
    {
      Rotate ("r");
      PictureCache.RefreshAfterFileManipulation ();
      SetCurrentPicture ();
    }

    public void RotateLeft ()
    {
      Rotate ("l");
      PictureCache.RefreshAfterFileManipulation ();
      SetCurrentPicture ();
    }

    private void Rotate (string rotateParameter)
    {
      // Use DLL ?"C:\PROGRA~1\JPEGLO~1\contmenu.dll"

      var jpegRotatorExe = GetJpegLoslessRotateExePath ();

      if (!File.Exists (jpegRotatorExe))
      {
        MessageBox.Show (string.Format ("Jpeg Lossless rotator not found at '{0}'. Check configuration file.", jpegRotatorExe));
        return;
      }

      var process = new Process ();
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.FileName = jpegRotatorExe;
      process.StartInfo.CreateNoWindow = true;

      process.StartInfo.Arguments = string.Format ("-{0} \"{1}\"", rotateParameter, PictureCache.CurrentFileName);

      process.Start ();
      process.WaitForExit ();
    }

    private string GetJpegLoslessRotateExePath ()
    {
      return ConfigurationManager.AppSettings["JpegLoslessRotate"];
    }

    public void ZoomIn ()
    {
      ZoomFactor = Math.Min (ZoomFactor * 1.4142135623730950488016887242097, 1000);
      PictureView.SetZoom (ZoomFactor);
    }

    public void ZoomOut ()
    {
      ZoomFactor = ZoomFactor / 1.4142135623730950488016887242097;

      if (ZoomFactor <= 1.0)
      {
        ZoomFactor = 1.0;
        PictureView.ResetZoomAndPosition ();
      }
      else
        PictureView.SetZoom (ZoomFactor);
    }

    public void ZoomDeefault ()
    {
      ZoomFactor = 1.0;
      PictureView.ResetZoomAndPosition ();
    }

    public void ShowHelpScreen ()
    {
      new HelpView ().ShowDialog ();
    }

    public void Refresh ()
    {
      PictureCache.RefreshAfterFileManipulation ();
      SetCurrentPicture ();
    }

    public void Edit ()
    {
      var editProgramm = ConfigurationManager.AppSettings["EditProgram"];

      var process = new Process ();
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.FileName = editProgramm;
      process.StartInfo.CreateNoWindow = true;

      process.StartInfo.Arguments = string.Format ("\"{0}\"", PictureCache.CurrentFileName);

      try
      {
        process.Start ();
      }
      catch (Exception exception)
      {
        MessageBox.Show ("Error starting edit program:\r\n\r\n" + exception.Message);
      }
    }

    public void SortAlphabetically ()
    {
      PictureCache.SortAlphabetically ();
    }

    public void SortByDate ()
    {
      PictureCache.SortByDate ();
    }
  }
}