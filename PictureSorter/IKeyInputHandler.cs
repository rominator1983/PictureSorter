using System;
using System.Windows.Forms;

namespace PictureSorter
{
  public interface IKeyInputHandler
  {
    void Handle (IntPtr handle, KeyEventArgs e);
  }
}