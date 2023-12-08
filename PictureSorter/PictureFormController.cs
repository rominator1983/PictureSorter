using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace PictureSorter
{
  public class PictureFormController : IPictureFormController
  {
    public IPictureForm PictureForm { get; private set; }
    public IPictureCache PictureCache { get; private set; }
    public string BestOfFolder { get; private set; }
    public double ZoomFactor { get; private set; }

    public PictureFormController (IPictureCache pictureCache)
    {
      PictureCache = pictureCache;
      ZoomFactor = 1.0;
    }

    public void SetPictureForm (IPictureForm pictureForm)
    {
      PictureForm = pictureForm;
      SetCurrentPicture ();
    }

    public void ToggleFullScreen ()
    {
      PictureForm.ToggleFullScreen ();
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

      File.Move (PictureCache.CurrentFileName, Path.Combine (BestOfFolder, Path.GetFileName (PictureCache.CurrentFileName)));
      
      PictureCache.RefreshAfterFileManipulation ();
      SetCurrentPicture ();
    }

    public void SetBestOfFolder ()
    {
      PictureForm.SetNonFullScreen ();
      var settingsViiew = new SettingsViiew ();

      settingsViiew.BestOfFolder = PictureCache.DirectoryName;
      settingsViiew.Show();

      BestOfFolder = settingsViiew.BestOfFolder;
    }

    private void SetCurrentPicture ()
    {
      var currentPicture = GetCurrentPicture ();
      PictureForm.SetCurrentPicture (currentPicture);
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
      PictureForm.Close ();
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
      var jpegRotatorExe = GetJpegLoslessRotateExePath ();

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
      // TODO: make this configurable
      return "";
      //return ConfigurationManager.AppSettings["JpegLoslessRotate"];
    }

    public void ZoomIn ()
    {
      ZoomFactor = ZoomFactor * 1.4142135623730950488016887242097;
      PictureForm.SetZoom (ZoomFactor);
    }

    public void ZoomOut ()
    {
      ZoomFactor = Math.Max (ZoomFactor / 1.4142135623730950488016887242097, 1.0);
      PictureForm.SetZoom (ZoomFactor);
    }

    public void ZoomDeefault ()
    {
      ZoomFactor = 1.0;
      PictureForm.ResetZoomAndPosition ();
    }
  }
}