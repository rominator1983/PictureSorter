using System.Configuration;
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
            var application = new Application();

            if (args.Length == 0)
            {
                MessageBox.Show("PictureSorter must be called with arguments.");
                MessageBox.Show(ConfigurationManager.AppSettings["EditProgram"]);
                return;
            }

            var fileCache = new PictureCache();
            var pictureFormController = 
                // NOTE: for trouble shooting/logging
                //new LoggingPictureViewController(
                new PictureViewController(fileCache);

            var keyInputHandler = new KeyInputHandler(pictureFormController);

            fileCache.Initialize(args[0]);
            application.Run(new PictureView(pictureFormController, keyInputHandler));
        }
    }
}