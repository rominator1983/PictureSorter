using System.Windows.Forms;

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
      if (e.Alt && e.KeyCode == Keys.Enter)
        PictureViewController.ToggleFullScreen ();

      if (e.KeyCode == Keys.Left)
        PictureViewController.Previous ();

      if (e.KeyCode == Keys.Right)
        PictureViewController.Next ();

      if (e.KeyCode == Keys.Escape || (e.KeyCode == Keys.F4 && e.Control))
        PictureViewController.Close ();

      if (e.KeyCode == Keys.S)
        PictureViewController.SetBestOfFolder ();

      if (e.KeyCode == Keys.B)
        PictureViewController.MoveCurrentToBestOf ();

      if (e.KeyCode == Keys.C)
        PictureViewController.CopyCurrentToBestOf ();

      if (e.KeyCode == Keys.R)
        PictureViewController.RotateRight ();

      if (e.KeyCode == Keys.L)
        PictureViewController.RotateLeft ();

      if (e.KeyValue == 187 || e.KeyValue == 107)
        PictureViewController.ZoomIn ();

      if (e.KeyValue == 189 || e.KeyValue == 109)
        PictureViewController.ZoomOut ();

      if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
        PictureViewController.ZoomDeefault ();

      if (e.KeyCode == Keys.F1)
        PictureViewController.ShowHelpScreen ();

      if (e.KeyCode == Keys.F5)
        PictureViewController.Refresh ();

      if (e.KeyCode == Keys.E)
        PictureViewController.Edit ();

      if (e.KeyCode == Keys.A)
        PictureViewController.SortAlphabetically ();

      if (e.KeyCode == Keys.D)
        PictureViewController.SortByDate ();
    }
  }
}