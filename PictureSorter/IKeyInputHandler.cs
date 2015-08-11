using System;
using System.Windows.Forms;

namespace PictureSorter
{
  public interface IKeyInputHandler
  {
    void Handle (KeyEventArgs e);
  }
}