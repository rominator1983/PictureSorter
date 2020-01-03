using System;

namespace PictureSorter
{
  public interface IPictureFormController
  {
    void ToggleFullScreen ();
    void SetPictureForm (IPictureForm pictureForm);
    
    void Previous ();
    void Next ();

    void Close ();
    void MoveCurrentToBestOf();
    void SetBestOfFolder ();
    
    void RotateRight ();
    void RotateLeft ();

    void ZoomIn ();
    void ZoomOut ();
    void ZoomDeefault ();
  }
}