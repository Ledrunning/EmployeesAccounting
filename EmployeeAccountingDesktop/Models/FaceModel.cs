using System.Collections.Generic;
using System.Drawing;

namespace EA.DesktopApp.Models
{
    /// <summary>
    ///     Model for future
    /// </summary>
    public class FaceModel
    {
        public List<Rectangle> Faces { get; } = new List<Rectangle>();
        public List<Rectangle> Eyes { get; } = new List<Rectangle>();
    }
}