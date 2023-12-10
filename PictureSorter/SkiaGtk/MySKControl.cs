
using System;
using Eto;
using Eto.Forms.Controls.SkiaSharp;
using SkiaSharp;

namespace PictureSorter.SkiaGtk
{

    // TODO: license accordingly
    [Handler(typeof(ISKControl))]
    public class MySKControl : SKControl
    {
        public interface ISKControl : IHandler, Eto.Widget.IHandler
        {
            Action<SKSurface> PaintSurfaceAction { get; set; }
        }

        private new ISKControl Handler => (ISKControl)((Eto.Widget)this).Handler;

        public Action<SKSurface> PaintSurfaceAction
        {
            get
            {
                return Handler.PaintSurfaceAction;
            }
            set
            {
                Handler.PaintSurfaceAction = value;
            }
        }
    }
}