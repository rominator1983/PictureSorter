using System;
using System.Windows.Forms;

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
      Application.EnableVisualStyles ();
      Application.SetCompatibleTextRenderingDefault (false);

      new ApplicationStarter ().Start (args);
    }
  }
}
