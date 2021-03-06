﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PictureSorter
{
  public class PictureViewController : IPictureViewController
  {
    public struct SHITEMID
{
    public long cb;
    public byte abID;
}

  public struct ITEMIDLIST
{
    public SHITEMID mkid;
}


    //// The Desktop - virtual folder
    private const long CSIDL_DESKTOP = 0x0;
    //// Program Files
    private const long CSIDL_PROGRAMS = 2;
    //// Control Panel - virtual folder
    private const long CSIDL_CONTROLS = 3;
    //// Printers - virtual folder
    private const long CSIDL_PRINTERS = 4;
    //// My Documents
    private const long CSIDL_DOCUMENTS = 5;
    //// Favourites
    private const long CSIDL_FAVORITES = 6;
    //// Startup Folder
    private const long CSIDL_STARTUP = 7;
    //// Recent Documents
    private const long CSIDL_RECENT = 8;
    //// Send To Folder
    private const long CSIDL_SENDTO = 9;
    //// Recycle Bin - virtual folder
    private const long CSIDL_BITBUCKET = 10;
    //// Start Menu
    private const long CSIDL_STARTMENU = 11;
    //// Desktop folder
    private const long CSIDL_DESKTOPFOLDER = 16;
    //// My Computer - virtual folder
    private const long CSIDL_DRIVES = 17;
    //// Network Neighbourhood - virtual folder
    private const long CSIDL_NETWORK = 18;
    //// NetHood Folder
    private const long CSIDL_NETHOOD = 19;
    //// Fonts folder
    private const long CSIDL_FONTS = 20;
    //// ShellNew folder
    private const long CSIDL_SHELLNEW = 21;

    public const int MAX_PATH = 260;


    [DllImport ("Shell32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern long SHGetSpecialFolderLocation (IntPtr hwndOwner, long nFolder, ITEMIDLIST pidl);

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

      if (FileExists(targetPath))
        return;

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

      if (FileExists(targetPath))
        return;

      File.Copy (PictureCache.CurrentFileName, targetPath);
    }

    private static bool FileExists (string targetPath)
    {
      if (File.Exists (targetPath))
      {
        MessageBox.Show (string.Format ("File '{0}' already exists.\r\n\r\nNothing has been changed.", targetPath),
          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        return true;
      }
      return false;
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

    public void MoveToTrashBin (IntPtr handle)
    {
      ITEMIDLIST IDL = default (ITEMIDLIST);
      var trashbinPath = SHGetSpecialFolderLocation (handle, CSIDL_PROGRAMS, IDL);
      var targetPath = Path.Combine (BestOfFolder, Path.GetFileName (PictureCache.CurrentFileName));

      if (FileExists (targetPath))
        return;

      File.Move (PictureCache.CurrentFileName, targetPath);

      PictureCache.RefreshAfterFileManipulation ();
      SetCurrentPicture ();
    }
  }
}