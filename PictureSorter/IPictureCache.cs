using System;
using Eto.Drawing;
using SkiaSharp;

namespace PictureSorter
{
  public interface IPictureCache
  {
    void Initialize (string fileName);
    void Next();
    void Previous ();
    string CurrentFileName { get; }
    SKBitmap CurrentBitmap { get; }
    string DirectoryName { get; }
    void RefreshAfterFileManipulation ();
    void UnloadCache();
    void SortByDate ();
    void SortAlphabetically ();
  }
}