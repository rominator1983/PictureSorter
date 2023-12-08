using System;
using Eto.Forms;

namespace PictureSorter
{
  public partial class HelpView : Form
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
