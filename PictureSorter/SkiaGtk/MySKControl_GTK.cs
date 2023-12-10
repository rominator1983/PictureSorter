using System;
using Cairo;
using Gtk;
using SkiaSharp;

namespace PictureSorter.SkiaGtk
{
    // TODO: license accordingly
    public class MySKControl_GTK : EventBox
    {
        public Action<SKSurface> PaintSurface;

        override protected bool OnDrawn(Cairo.Context cr)
        {
            bool v = base.OnDrawn(cr);

            Gdk.Rectangle allocation = base.Allocation;
            if (allocation.Width > 0 && allocation.Height > 0)
            {
                SKColorType colorType = SKColorType.Bgra8888;
                using SKBitmap sKBitmap = new SKBitmap(allocation.Width, allocation.Height, colorType, SKAlphaType.Premul);
                if (sKBitmap == null)
                {
                    throw new InvalidOperationException("Bitmap is null");
                }

                IntPtr length;
                using SKSurface sKSurface = SKSurface.Create(new SKImageInfo(sKBitmap.Info.Width, sKBitmap.Info.Height, colorType, SKAlphaType.Premul), sKBitmap.GetPixels(out length), sKBitmap.Info.RowBytes);
                if (sKSurface == null)
                {
                    throw new InvalidOperationException("skSurface is null");
                }

                if (PaintSurface != null)
                {
                    PaintSurface(sKSurface);
                }

                sKSurface.Canvas.Flush();
                using Surface surface = new ImageSurface(sKBitmap.GetPixels(out length), Format.Argb32, sKBitmap.Width, sKBitmap.Height, sKBitmap.Width * 4);
                surface.MarkDirty();
                cr.SetSourceSurface(surface, 0, 0);
                cr.Paint();
            }
            return true;
        }
    }
}