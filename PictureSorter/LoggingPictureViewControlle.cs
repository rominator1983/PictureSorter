using System;
using PictureSorter;

public class LoggingPictureViewController : IPictureViewController
{
    private readonly IPictureViewController _inner;

    public LoggingPictureViewController(IPictureViewController inner)
    {
        _inner = inner;
    }

    public void ToggleFullScreen()
    {
        Console.WriteLine("ToggleFullScreen method called");
        _inner.ToggleFullScreen();
    }

    public void SetPictureForm(IPictureView pictureView)
    {
        Console.WriteLine("SetPictureForm method called");
        _inner.SetPictureForm(pictureView);
    }

    public void Previous()
    {
        Console.WriteLine("Previous method called");
        _inner.Previous();
    }

    public void Next()
    {
        Console.WriteLine("Next method called");
        _inner.Next();
    }

    public void Close()
    {
        Console.WriteLine("Close method called");
        _inner.Close();
    }

    public void MoveCurrentToBestOf()
    {
        Console.WriteLine("MoveCurrentToBestOf method called");
        _inner.MoveCurrentToBestOf();
    }

    public void CopyCurrentToBestOf()
    {
        Console.WriteLine("CopyCurrentToBestOf method called");
        _inner.CopyCurrentToBestOf();
    }

    public void SetBestOfFolder()
    {
        Console.WriteLine("SetBestOfFolder method called");
        _inner.SetBestOfFolder();
    }

    public void RotateRight()
    {
        Console.WriteLine("RotateRight method called");
        _inner.RotateRight();
    }

    public void RotateLeft()
    {
        Console.WriteLine("RotateLeft method called");
        _inner.RotateLeft();
    }

    public void ZoomIn()
    {
        Console.WriteLine("ZoomIn method called");
        _inner.ZoomIn();
    }

    public void ZoomOut()
    {
        Console.WriteLine("ZoomOut method called");
        _inner.ZoomOut();
    }

    public void ZoomDefault()
    {
        Console.WriteLine("ZoomDefault method called");
        _inner.ZoomDefault();
    }

    public void ShowHelpScreen()
    {
        Console.WriteLine("ShowHelpScreen method called");
        _inner.ShowHelpScreen();
    }

    public void SetDroppedFile(string fileName)
    {
        Console.WriteLine("SetDroppedFile method called");
        _inner.SetDroppedFile(fileName);
    }

    public void Refresh()
    {
        Console.WriteLine("Refresh method called");
        _inner.Refresh();
    }

    public void Edit()
    {
        Console.WriteLine("Edit method called");
        _inner.Edit();
    }

    public void SortAlphabetically()
    {
        Console.WriteLine("SortAlphabetically method called");
        _inner.SortAlphabetically();
    }

    public void SortByDate()
    {
        Console.WriteLine("SortByDate method called");
        _inner.SortByDate();
    }

    public void MoveToTrashBin()
    {
        Console.WriteLine("MoveToTrashBin method called");
        _inner.MoveToTrashBin();
    }
}