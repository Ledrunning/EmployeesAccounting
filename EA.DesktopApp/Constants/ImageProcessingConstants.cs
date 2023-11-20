using System.Drawing;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Constants
{
    internal static class ImageProcessingConstants
    {
        public const string NotFound = "Not found";
        public const int RectangleThickness = 2;
        public const int MinimumNeighbors = 10;
        public const double ScaleFactor = 1.1;
        public const int GrayPhotoHeight = 200;
        public const int GrayPhotoWidth = 200;
        public static readonly Point SourcePoint = new Point(10, 80);
        public static readonly Bgr RectanglesColor = new Bgr(Color.Aqua);
        public static readonly Bgr TextColor = new Bgr(Color.Aqua);
    }
}