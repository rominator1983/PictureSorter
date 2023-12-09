using Eto.Forms;

namespace PictureSorter
{
    public class ApplicationStarter
    {
        public ApplicationStarter()
        {
        }

        public void Start(string[] args)
        {
            Application application = new Application();

            if (args.Length == 0)
            {
                MessageBox.Show("Picture Viewer must be called with arguments.");
                return;
            }

            var fileCache = new PictureCache();
            var pictureFormController = new LoggingPictureViewController(new PictureViewController(fileCache));
            var keyInputHandler = new KeyInputHandler(pictureFormController);

            fileCache.Initialize(args[0]);
            application.Run(new PictureView(pictureFormController, keyInputHandler));
        }
    }
}