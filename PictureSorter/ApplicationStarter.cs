using System.Windows.Forms;

namespace PictureSorter
{
    public class ApplicationStarter
    {
        public ApplicationStarter ()
        {
        }

        public void Start (string[] args)
        {
            if (args.Length == 0)
            {
                MessageBox.Show ("Picture Viewer must be called with arguments.");
                return;
            }

            var fileCache = new PictureCache ();
            fileCache.Initialize (args [0]);
            var pictureFormController = new PictureViewController (fileCache);
            var keyInputHandler = new KeyInputHandler (pictureFormController);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run (new PictureView (pictureFormController, keyInputHandler));
        }
    }
}