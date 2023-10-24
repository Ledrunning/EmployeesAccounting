using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Converters
{
    // Took from:
    // https://stackoverflow.com/questions/29153967/convert-a-byte-into-an-emgu-opencv-image
    public static class EmguFormatImageConverter
    {
        private const int Width = 200; // Image Width
        private const int Height = 200; // Image Height
        private const int Stride = 1; // Image Stide - Bytes per Row (3 bytes per pixel)

        public static Image<Gray, byte> ByteArrayToGrayImage(byte[] byteArray)
        {
            // Create data for an Image 200x200 GRAY - 40_000 Bytes - 0x9C40
            // Pin the imgData in memory and create an IntPtr to it's location
            var pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            var pointer = pinnedArray.AddrOfPinnedObject();

            // Create an image from the imgData
            var img = new Image<Gray, byte>(Width, Height, Stride, pointer);

            // Free the memory
            pinnedArray.Free();

            return img;
        }
    }
}