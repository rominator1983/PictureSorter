using System;
using System.Collections.Generic;
using Eto.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SkiaSharp;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor;
using Directory = System.IO.Directory;

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
    public SKBitmap CurrentBitmap { get; private set; }

    private Guid _loadingID = Guid.Empty;

    private readonly Object _cacheMutex = new object();
    private readonly SortedDictionary<string, SKBitmap> _cache = new SortedDictionary<string, SKBitmap>();
    private SortedBy Sorted = SortedBy.Alphabetically;


    public PictureCache()
    {
    }

    public void Initialize(string fileName)
    {
      fileName = Path.GetFullPath(fileName);

      if (Directory.Exists(fileName))
        fileName = Directory.GetFiles(fileName).FirstOrDefault();

      SetCurrent(fileName);

      DirectoryName = Path.GetDirectoryName(fileName);
      LoadFilesInFolder();

      AssureEnoughFilesAreCached(fileName, 2, 3);
    }

    public void Next()
    {
      ChangePicture(1, 4, 10);
    }

    public void Previous()
    {
      ChangePicture(-1, 5, 11);
    }

    private void ChangePicture(int offset, int downPictures, int upPictures)
    {
      var directoryContent = DirectoryContent.ToArray();
      var i = GetIndexOf(directoryContent, CurrentFileName) + offset;

      if (i >= 0 && i < directoryContent.Length)
      {
        var fileName = directoryContent[i];
        SetCurrent(fileName);
        AssureEnoughFilesAreCachedInThread(fileName, downPictures, upPictures);
      }
    }

    public void RefreshAfterFileManipulation()
    {
      LoadFilesInFolder();

      if (DirectoryContent.Count == 0)
        return;

      if (!DirectoryContent.Contains(CurrentFileName))
      {
        CurrentBitmap.Dispose();

        var directoryContentWithPreviousSelectedElement = new HashSet<string>(DirectoryContent);
        directoryContentWithPreviousSelectedElement.Add(CurrentFileName);
        var directoryContentWithPreviousSelectedElementOrdered = directoryContentWithPreviousSelectedElement.OrderBy(_ => _).ToArray();

        var index = GetIndexOf(directoryContentWithPreviousSelectedElementOrdered, CurrentFileName);

        index = Math.Min(DirectoryContent.Count - 1, index);

        var fileName = DirectoryContent.ToArray()[index];
        SetCurrent(fileName);

        AssureEnoughFilesAreCachedInThread(fileName, 5, 5);
      }
      else
      {
        CurrentBitmap.Dispose();
        lock (_cacheMutex)
        {
          _cache.Remove(CurrentFileName);
          SetCurrent(CurrentFileName);
        }
      }
    }

    private void LoadFilesInFolder()
    {
      Func<string, string, bool> fileFilter = (fileName, extension) => fileName.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase);

      var filesOfFFolder = System.IO.Directory.EnumerateFiles(DirectoryName)
        .Where(fileName =>
            fileFilter(fileName, ".jpg") ||
            fileFilter(fileName, ".jpeg") ||
            fileFilter(fileName, ".png") ||
            fileFilter(fileName, ".bmp") ||
            fileFilter(fileName, ".tiff") ||
            fileFilter(fileName, ".gif"));

      if (Sorted == SortedBy.Date)
        DirectoryContent = new HashSet<string>(filesOfFFolder.OrderBy(GetDateTakenFromImage));
      else
        DirectoryContent = new HashSet<string>(filesOfFFolder.OrderBy(_ => _));
    }

    private void SetCurrent(string fileName)
    {
      Console.WriteLine("SetCurrent: " + fileName);

      var bitmap = GetCachedBitmap(fileName, SetNewLoadingID());

      if (bitmap == null)
        return;

      CurrentFileName = fileName;
      CurrentBitmap = bitmap;
    }

    private Guid SetNewLoadingID()
    {
      return _loadingID = Guid.NewGuid();
    }

    private void AssureEnoughFilesAreCachedInThread(string fileName, int downPictures, int upPictures)
    {
      var task = new Task(() => AssureEnoughFilesAreCached(fileName, downPictures, upPictures));
      task.Start();
    }

    private void AssureEnoughFilesAreCached(string fileName, int downPictures, int upPictures)
    {
      var loadingIDForThread = _loadingID;
      var directoryContent = DirectoryContent.ToArray();

      var index = GetIndexOf(directoryContent, fileName);
      var lowerBound = Math.Max(0, index - downPictures);
      var upperBound = Math.Min(directoryContent.Length - 1, index + upPictures);

      var filesToLoad = directoryContent.Where(
          (takeFileName, takeIndex) => takeIndex >= lowerBound && takeIndex <= upperBound)
          .ToArray();

      var filesToUnload = directoryContent.Where(fileToUnload => !filesToLoad.Contains(fileToUnload));

      lock (_cacheMutex)
      {
        foreach (var fileToUnload in filesToUnload)
        {
          SKBitmap bitmapToDispose;
          if (_cache.TryGetValue(fileToUnload, out bitmapToDispose))
          {
            bitmapToDispose.Dispose();
            _cache.Remove(fileToUnload);
          }
        }
      }

      var filesToLoadStackMutex = new object();
      var filesToLoadStack = new Stack<string>(filesToLoad);

      System.Action loadPicture = () =>
      {
        while (filesToLoadStack.Count > 0)
        {
          string fileToLoad;
          lock (filesToLoadStackMutex)
          {
            if (filesToLoadStack.Count <= 0)
              break;

            fileToLoad = filesToLoadStack.Pop();
          }
          GetCachedBitmap(fileToLoad, loadingIDForThread);
        }
      };

      var task1 = new Task(loadPicture);
      var task2 = new Task(loadPicture);
      var task3 = new Task(loadPicture);
      var task4 = new Task(loadPicture);

      task1.Start();
      task2.Start();
      task3.Start();
      task4.Start();

      Task.WaitAll(task1, task2, task3, task4);
    }

    private int GetIndexOf(string[] directoryContent, string currentFileName)
    {
      return Array.IndexOf(directoryContent, currentFileName);
    }

    private SKBitmap GetCachedBitmap(string fileName, Guid loadingID)
    {
      Console.WriteLine("GetCachedBitmap: " + fileName + " " + loadingID);

      if (_loadingID != loadingID)
        return null;

      SKBitmap bitmap;
      if (_cache.TryGetValue(fileName, out bitmap))
        return bitmap;

      lock (_cacheMutex)
      {
        if (_cache.TryGetValue(fileName, out bitmap))
          return bitmap;

        bitmap = CreateBitmap(fileName);

        if (bitmap != null)
          _cache.Add(fileName, bitmap);

        return bitmap;
      }
    }

    public enum ImageOrientation
    {
      None = 0,
      FlipHorizontal,
      RotateBy180Degrees,
      FlipVertical,
      RotateBy90DegreesAndFlipVertical,
      RotateBy90Degrees,
      RotateBy270DegreesAndFlipVertical,
      RotateBy270Degrees,
    }

    private SKBitmap CreateBitmap(string fileName)
    {
      try
      {
        Console.WriteLine("CreateBitmap: " + fileName);
        using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
          var imageOrientation = GetOrientation(fileStream);
          Console.WriteLine("Orientation: " + imageOrientation);

          fileStream.Seek(0, SeekOrigin.Begin);
          return CorrectRotation(SKBitmap.Decode(fileStream), imageOrientation);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error reading image file: {fileName}\r\n{ex}");
        return null;
      }
    }

    private static ImageOrientation GetOrientation(FileStream fileStream)
    {
      var metadataDirectories = ImageMetadataReader.ReadMetadata(fileStream);
      fileStream.Seek(0, SeekOrigin.Begin);

      var exifDirectory = metadataDirectories.OfType<ExifDirectoryBase>().Where(_ => !(_ is ExifThumbnailDirectory)).FirstOrDefault();

      if (exifDirectory != null && exifDirectory.TryGetInt16(ExifDirectoryBase.TagOrientation, out var orientation))
      {
        var imageOrientation = (ImageOrientation)(orientation - 1);

        if (Enum.IsDefined(typeof(ImageOrientation), imageOrientation))
          return imageOrientation;
      }

      return ImageOrientation.None;
    }

    private static SKBitmap CorrectRotation(SKBitmap bitmap, ImageOrientation imageOrientation)
    {
      if (imageOrientation == ImageOrientation.None)
        return bitmap;

      SKBitmap corrected = null;

      switch (imageOrientation)
      {
        case ImageOrientation.FlipHorizontal:
        case ImageOrientation.RotateBy180Degrees:
        case ImageOrientation.FlipVertical:
          corrected = new SKBitmap(bitmap.Width, bitmap.Height);
          break;
        case ImageOrientation.RotateBy90DegreesAndFlipVertical:
        case ImageOrientation.RotateBy90Degrees:
        case ImageOrientation.RotateBy270DegreesAndFlipVertical:
        case ImageOrientation.RotateBy270Degrees:
          corrected = new SKBitmap(bitmap.Height, bitmap.Width);
          break;
      }

      using var canvas = new SKCanvas(corrected);

      switch (imageOrientation)
      {
        case ImageOrientation.FlipHorizontal:
          canvas.Translate(corrected.Width, 0);
          canvas.Scale(-1, 1);
          break;
        case ImageOrientation.RotateBy180Degrees:
          canvas.Translate(corrected.Width, corrected.Height);
          canvas.RotateDegrees(180);
          break;
        case ImageOrientation.FlipVertical:
          canvas.Translate(0, corrected.Height);
          canvas.Scale(1, -1);
          break;
        case ImageOrientation.RotateBy90Degrees:
          canvas.Translate(corrected.Width, 0);
          canvas.RotateDegrees(90);
          break;
        case ImageOrientation.RotateBy270Degrees:
          canvas.Translate(0, corrected.Height);
          canvas.RotateDegrees(270);
          break;

        case ImageOrientation.RotateBy90DegreesAndFlipVertical:
          canvas.Translate(corrected.Width, 0);
          canvas.RotateDegrees(90);
          canvas.Translate(0, corrected.Height);
          canvas.Scale(1, -1);
          break;
        case ImageOrientation.RotateBy270DegreesAndFlipVertical:
          canvas.Translate(0, corrected.Height);
          canvas.RotateDegrees(270);
          canvas.Translate(0, corrected.Height);
          canvas.Scale(1, -1);
          break;
      }

      canvas.DrawBitmap(bitmap, 0, 0);

      bitmap.Dispose();

      return corrected;
    }

    public void UnloadCache()
    {
      SetNewLoadingID();

      lock (_cacheMutex)
      {
        foreach (var cacheEntry in _cache.Values)
          cacheEntry.Dispose();

        _cache.Clear();
      }
    }

    public void SortByDate()
    {
      if (Sorted == SortedBy.Date)
        return;

      Sorted = SortedBy.Date;

      LoadFilesInFolder();
      AssureEnoughFilesAreCached(CurrentFileName, 2, 3);
    }

    public void SortAlphabetically()
    {
      if (Sorted == SortedBy.Alphabetically)
        return;

      Sorted = SortedBy.Alphabetically;

      LoadFilesInFolder();
      AssureEnoughFilesAreCached(CurrentFileName, 2, 3);
    }

    public static DateTime? GetDateTakenFromImage(string path)
    {
      using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

      var directories = ImageMetadataReader.ReadMetadata(fileStream);
      
      var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
      
      var dateTaken = subIfdDirectory?.GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);

      Console.WriteLine($"GetDateTakenFromImage: {path} -> {dateTaken}");
      
      return dateTaken;
    }
  }
}