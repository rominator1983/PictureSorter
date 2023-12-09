using System;

namespace PictureSorter
{
  public class Picture
  {
    public Picture ()
    {
    }

    public string FileName { get; set; }
    public byte[] FileContent { get; set; }

    public SkiaSharp.SKBitmap Bitmap { get; set; }
  }
}