using System;
using Eto.Forms;

namespace PictureSorter
{
  public interface IKeyInputHandler
  {
    void Handle (KeyEventArgs e);
  }
}