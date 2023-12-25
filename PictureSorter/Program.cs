using System;
using Eto.Forms;

namespace PictureSorter
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main (string[] args)
    {
      new ApplicationStarter ().Start (args);
    }
  }
}
