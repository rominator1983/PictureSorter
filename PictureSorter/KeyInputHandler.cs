using System;
using Eto.Forms;

namespace PictureSorter
{
  public class KeyInputHandler : IKeyInputHandler
  {
    public IPictureViewController PictureViewController { get; private set; }

    public KeyInputHandler (IPictureViewController pictureViewController)
    {
      PictureViewController = pictureViewController;
    }

    public void Handle (KeyEventArgs e)
    {
      //Console.WriteLine($"e.KeyData: {e.KeyData}");

      // TODO: implement
      //if (e.KeyData == Keys.Delete)
      //  PictureViewController.MoveToTrashBin (handle);

      if (e.KeyData == (Keys.Enter | Keys.LeftAlt) || e.KeyData == (Keys.Enter | Keys.RightAlt)  || e.KeyData == (Keys.Enter | Keys.Alt))
        PictureViewController.ToggleFullScreen ();

      if (e.KeyData == Keys.Left)
        PictureViewController.Previous ();

      if (e.KeyData == Keys.Right)
        PictureViewController.Next ();

      if (e.KeyData == Keys.Escape)
        PictureViewController.Close ();

      if (e.KeyData == Keys.S)
        PictureViewController.SetBestOfFolder ();

      if (e.KeyData == Keys.B)
        PictureViewController.MoveCurrentToBestOf ();

      if (e.KeyData == Keys.C)
        PictureViewController.CopyCurrentToBestOf ();

      if (e.KeyData == Keys.R)
        PictureViewController.RotateRight ();

      if (e.KeyData == Keys.L)
        PictureViewController.RotateLeft ();

      if (e.KeyData == Keys.Plus || e.KeyData == Keys.Add)
        PictureViewController.ZoomIn ();

      if (e.KeyData == Keys.Minus || e.KeyData == Keys.Subtract)
        PictureViewController.ZoomOut ();

      if (e.KeyData == Keys.D0 || e.KeyData == Keys.Keypad0)
        PictureViewController.ZoomDefault ();

      if (e.KeyData == Keys.F1)
        PictureViewController.ShowHelpScreen ();

      if (e.KeyData == Keys.F5)
        PictureViewController.Refresh ();

      if (e.KeyData == Keys.E)
        PictureViewController.Edit ();

      if (e.KeyData == Keys.A)
        PictureViewController.SortAlphabetically ();

      if (e.KeyData == Keys.D)
        PictureViewController.SortByDate ();
    }
  }
}