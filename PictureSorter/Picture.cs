using System;
using System.Drawing;

namespace PictureSorter
{
  public class Picture
  {
    public Picture ()
    {
    }

    public string FileName { get; set; }
    public byte[] FileContent { get; set; }

    public Bitmap Bitmap { get; set; }
  }
}