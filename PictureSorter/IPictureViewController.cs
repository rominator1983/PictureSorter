using System;

namespace PictureSorter
{
  public interface IPictureViewController
  {
    void ToggleFullScreen ();
    void SetPictureForm (IPictureView pictureView);

    void Previous ();
    void Next ();

    void Close ();
    void MoveCurrentToBestOf ();
    void CopyCurrentToBestOf ();
    void SetBestOfFolder ();

    void RotateRight ();
    void RotateLeft ();

    void ZoomIn ();
    void ZoomOut ();
    void ZoomDeefault ();
    void ShowHelpScreen ();
    void SetDroppedFile (string fileName);
    void Refresh ();
    void Edit ();

    void SortAlphabetically ();
    void SortByDate ();
    void MoveToTrashBin (IntPtr handle);
  }
}