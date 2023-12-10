using System;
using Eto.Forms;
using Eto.GtkSharp.Forms;
using Gtk;
using SkiaSharp;

namespace PictureSorter.SkiaGtk
{
    // TODO: license accordingly
    public class MySKControlHandler : GtkControl<EventBox, MySKControl, Control.ICallback>, MySKControl.ISKControl, Control.IHandler, Eto.Widget.IHandler
    {
        private MySKControl_GTK nativecontrol;

        public Action<SKSurface> PaintSurfaceAction
        {
            get
            {
                return nativecontrol.PaintSurface;
            }
            set
            {
                nativecontrol.PaintSurface = value;
            }
        }

        public MySKControlHandler()
        {
            nativecontrol = new MySKControl_GTK();
            base.Control = nativecontrol;
        }
    }
}