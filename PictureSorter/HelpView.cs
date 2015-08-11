using System;
using System.Windows.Forms;

namespace PictureSorter
{
  public partial class HelpView : Form
  {
    public HelpView ()
    {
      InitializeComponent ();
    }

    private void HelpView_PreviewKeyDown (object sender, PreviewKeyDownEventArgs e)
    {
      Close ();
    }
  }
}
