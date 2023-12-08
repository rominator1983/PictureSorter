using System;
using System.Collections.Generic;
using Eto.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Gtk;

namespace PictureSorter
{
  public class PictureCache : IPictureCache
  {
    public enum SortedBy
    {
      Date,
      Alphabetically
    }

    public string DirectoryName { get; private set; }
    public HashSet<string> DirectoryContent { get; private set; }

    public string CurrentFileName { get; private set; }
    public Bitmap CurrentBitmap { get; private set; }

    private Guid _loadingID = Guid.Empty;

    private readonly Object _cacheMutex = new object ();
    private readonly SortedDictionary<string, Bitmap> _cache = new SortedDictionary<string, Bitmap> ();
    private SortedBy Sorted = SortedBy.Alphabetically;


    public PictureCache ()
    {
    }

    public void Initialize (string fileName)
    {
      SetCurrent (fileName);

      DirectoryName = Path.GetDirectoryName (Path.GetFullPath(fileName));
      LoadFilesInFolder ();

      AssureEnoughFilesAreCached (fileName, 2, 3);
    }

    public void Next()
    {
      ChangePicture (1, 4, 10);
    }

    public void Previous ()
    {
      ChangePicture (-1, 5, 11);
    }

    private void ChangePicture (int offset, int downPictures, int upPictures)
    {
      var directoryContent = DirectoryContent.ToArray ();
      var i = GetIndexOf (directoryContent, CurrentFileName) + offset;

      if (i >= 0 && i < directoryContent.Length)
      {
        var fileName = directoryContent [i];
        SetCurrent (fileName);
        AssureEnoughFilesAreCachedInThread (fileName, downPictures, upPictures);
      }
    }

    public void RefreshAfterFileManipulation ()
    {
      LoadFilesInFolder ();

      if (DirectoryContent.Count == 0)
        return;

      if (!DirectoryContent.Contains (CurrentFileName))
      {
        CurrentBitmap.Dispose ();

        var directoryContentWithPreviousSelectedElement = new HashSet<string> (DirectoryContent);
        directoryContentWithPreviousSelectedElement.Add (CurrentFileName);
        var directoryContentWithPreviousSelectedElementOrdered = directoryContentWithPreviousSelectedElement.OrderBy (_ => _).ToArray();

        var index = GetIndexOf (directoryContentWithPreviousSelectedElementOrdered, CurrentFileName);

        index = Math.Min (DirectoryContent.Count - 1, index);

        var fileName = DirectoryContent.ToArray() [index];
        SetCurrent (fileName);

        AssureEnoughFilesAreCachedInThread (fileName, 5, 5);
      }
      else
      {
        CurrentBitmap.Dispose ();
        lock (_cacheMutex)
        {
          _cache.Remove (CurrentFileName);
          SetCurrent (CurrentFileName);
        }
      }
    }

    private void LoadFilesInFolder ()
    {
      Func<string, string, bool> fileFilter = (fileName, extension) => fileName.EndsWith (extension, StringComparison.CurrentCultureIgnoreCase);

      var filesOfFFolder = Directory.EnumerateFiles (DirectoryName)
        .Where (fileName => 
            fileFilter (fileName, ".jpg") ||
            fileFilter (fileName, ".jpeg") ||
            fileFilter (fileName, ".png") ||
            fileFilter (fileName, ".bmp") ||
            fileFilter (fileName, ".tiff") ||
            fileFilter (fileName, ".gif"));

      if (Sorted == SortedBy.Date)
        DirectoryContent = new HashSet<string> (filesOfFFolder.OrderBy (GetDateTakenFromImage));
      else
        DirectoryContent = new HashSet<string> (filesOfFFolder.OrderBy (_ => _));
    }

    private void SetCurrent (string fileName)
    {
      var bitmap = GetCachedBitmap (fileName, SetNewLoadingID ());

      if (bitmap == null)
        return;
      
      CurrentFileName = fileName;
      CurrentBitmap = bitmap;
    }

    private Guid SetNewLoadingID ()
    {
      return _loadingID = Guid.NewGuid ();
    }

    private void AssureEnoughFilesAreCachedInThread (string fileName, int downPictures, int upPictures)
    {
      var task = new Task (() => AssureEnoughFilesAreCached (fileName, downPictures, upPictures));
      task.Start ();
    }

    private void AssureEnoughFilesAreCached (string fileName, int downPictures, int upPictures)
    {
      var loadingIDForThread = _loadingID;
      var directoryContent = DirectoryContent.ToArray ();

      var index = GetIndexOf (directoryContent, fileName);
      var lowerBound = Math.Max (0, index - downPictures);
      var upperBound = Math.Min (directoryContent.Length - 1, index + upPictures);

      var filesToLoad = directoryContent.Where (
          (takeFileName, takeIndex) => takeIndex >= lowerBound && takeIndex <= upperBound)
          .ToArray ();

      var filesToUnload = directoryContent.Where (fileToUnload => !filesToLoad.Contains (fileToUnload));

      lock (_cacheMutex)
      {
        foreach (var fileToUnload in filesToUnload)
        {
          Bitmap bitmapToDispose;
          if (_cache.TryGetValue (fileToUnload, out bitmapToDispose))
          {
            bitmapToDispose.Dispose ();
            _cache.Remove (fileToUnload);
          }
        }
      }

      var filesToLoadStackMutex = new object ();
      var filesToLoadStack = new Stack<string> (filesToLoad);

      System.Action loadPicture = () =>
      {
        while (filesToLoadStack.Count > 0)
        {
          string fileToLoad;
          lock (filesToLoadStackMutex)
          {
            if (filesToLoadStack.Count <= 0)
              break;

            fileToLoad = filesToLoadStack.Pop ();
          }
          GetCachedBitmap (fileToLoad, loadingIDForThread);
        }
      };

      var task1 = new Task (loadPicture);
      var task2 = new Task (loadPicture);
      var task3 = new Task (loadPicture);
      var task4 = new Task (loadPicture);

      task1.Start ();
      task2.Start ();
      task3.Start ();
      task4.Start ();
    }

    private int GetIndexOf (string[] directoryContent, string currentFileName)
    {
      return Array.IndexOf (directoryContent, currentFileName);
    }

    private Bitmap GetCachedBitmap (string fileName, Guid loadingID)
    {
      if (_loadingID != loadingID)
        return null;

      Bitmap bitmap;
      if (_cache.TryGetValue (fileName, out bitmap))
        return bitmap;

      lock (_cacheMutex)
      {
        if (_cache.TryGetValue (fileName, out bitmap))
          return bitmap;

        bitmap = CreateBitmap (fileName);

        if (bitmap != null)
          _cache.Add (fileName, bitmap);

        return bitmap;
      }
    }

    private Bitmap CreateBitmap (string fileName)
    {
      try
      {
        using (var fileStream = new FileStream (fileName, FileMode.Open))
          return CorrectRotation (new Bitmap (fileStream));
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    private static Bitmap CorrectRotation (Bitmap bitmap)
    {
      // TODO: re-implement rotation correction
     // if (Array.IndexOf (bitmap.PropertyIdList, 274) > -1)
     // {
     //   var values = bitmap.GetPropertyItem (274).Value;
     //   //var dateTakenPropertyItem = bitmap.GetPropertyItem (0x9003);
     //   //var dateTakenString = Encoding.UTF8.GetString (dateTakenPropertyItem.Value);
     //   //var regex = new Regex (":");
     //   //dateTakenString = dateTakenString.Substring (0, dateTakenString.Length - 1);
     //   //dateTakenString = regex.Replace (dateTakenString, "-", 2);
     //   //var dateTaken = DateTime.Parse (dateTakenString);
//
     //   if (values.Length < 1)
     //     return bitmap;
//
     //   var orientation = (int) values [0];
     //   switch (orientation)
     //   {
     //     case 1:
     //       // No rotation required.
     //       break;
     //     case 2:
     //       bitmap.RotateFlip (RotateFlipType.RotateNoneFlipX);
     //       break;
     //     case 3:
     //       bitmap.RotateFlip (RotateFlipType.Rotate180FlipNone);
     //       break;
     //     case 4:
     //       bitmap.RotateFlip (RotateFlipType.Rotate180FlipX);
     //       break;
     //     case 5:
     //       bitmap.RotateFlip (RotateFlipType.Rotate90FlipX);
     //       break;
     //     case 6:
     //       bitmap.RotateFlip (RotateFlipType.Rotate90FlipNone);
     //       break;
     //     case 7:
     //       bitmap.RotateFlip (RotateFlipType.Rotate270FlipX);
     //       break;
     //     case 8:
     //       bitmap.RotateFlip (RotateFlipType.Rotate270FlipNone);
     //       break;
     //   }
     //   // This EXIF data is now invalid and should be removed.
     //   bitmap.RemovePropertyItem (274);
     // }
      return bitmap;
    }

    public void UnloadCache ()
    {
      SetNewLoadingID ();

      lock (_cacheMutex)
      {
        foreach (var cacheEntry in _cache.Values)
          cacheEntry.Dispose ();

        _cache.Clear ();
      }
    }

    public void SortByDate ()
    {
      if (Sorted == SortedBy.Date)
        return;

      Sorted = SortedBy.Date;

      LoadFilesInFolder ();
      AssureEnoughFilesAreCached (CurrentFileName, 2, 3);
    }

    public void SortAlphabetically ()
    {
      if (Sorted == SortedBy.Alphabetically)
        return;

      Sorted = SortedBy.Alphabetically;

      LoadFilesInFolder ();
      AssureEnoughFilesAreCached (CurrentFileName, 2, 3);
    }

    public static DateTime GetDateTakenFromImage (string path)
    {
      var regex = new Regex(":");

      using (var fileStream = new FileStream (path, FileMode.Open, FileAccess.Read))
      
      using (var image = new Eto.Drawing.Bitmap(fileStream))
      {
        var propertyItem = image.Properties.Get<string>(36867);
        var dateTaken = regex.Replace (propertyItem, "-", 2);
        return DateTime.Parse (dateTaken);
      }
    }
  }
}