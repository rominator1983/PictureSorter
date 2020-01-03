using System;

namespace PictureSorter
{
  public interface IPictureForm
  {
    void SetCurrentPicture (Picture picture);
    void ToggleFullScreen();
    void Close ();
    void SetNonFullScreen ();
    void SetZoom (double zoomFactor);
    void ResetZoomAndPosition ();
  }
}