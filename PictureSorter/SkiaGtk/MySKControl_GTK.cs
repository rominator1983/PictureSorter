/*

Copied/Obtained from https://github.com/DanWBR/Eto.Forms.SkiaSharp/tree/master/Eto.Forms.Controls.SkiaSharp.GTK
The only change is that this control always paints itself unconditional of the base call of OnDrawn.

License
Copyright (c) 2017 Daniel Medeiros

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


SkiaSharp License

Copyright (c) 2015-2016 Xamarin, Inc.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


Eto.Forms License

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

    Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

    Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/

using System;
using Cairo;
using Gtk;
using SkiaSharp;

namespace PictureSorter.SkiaGtk
{
    public class AlwaysDraginSKControl_GTK : EventBox
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