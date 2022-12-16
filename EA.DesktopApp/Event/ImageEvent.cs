using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Event
{
    public delegate void ImageChangedEventHandler(Image<Bgr, byte> image);

    internal class ImageEvent : EventArgs
    {
        public ImageEvent(Image<Bgr, byte> image)
        {
            Image = image;
        }

        public Image<Bgr, byte> Image { get; set; }
    }
}