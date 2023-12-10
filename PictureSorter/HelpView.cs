using System;
using Eto.Forms;

namespace PictureSorter
{
  public partial class HelpView : Dialog
  {
    public HelpView ()
    {
      InitializeComponent ();
    }

    private void HelpView_PreviewKeyDown (object sender, KeyEventArgs e)
    {
      Close ();
    }
  }
}
