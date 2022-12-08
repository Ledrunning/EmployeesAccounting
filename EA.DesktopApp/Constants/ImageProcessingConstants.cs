﻿using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.DesktopApp.Constants
{
    internal static class ImageProcessingConstants
    {
        public const int RectangleThickness = 2;
        public const int MinimumNeighbors = 10;
        public const double ScaleFactor = 1.1;
        public static readonly Bgr RectanglesColor = new Bgr(Color.Aqua);
    }
}
