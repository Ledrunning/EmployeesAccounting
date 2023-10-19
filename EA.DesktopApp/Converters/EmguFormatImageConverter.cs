using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Converters
{
    // TODO: https://stackoverflow.com/questions/29153967/convert-a-byte-into-an-emgu-opencv-image
    public static class EmguFormatImageConverter
    {
        private const int Width = 400; // Image Width
        private const int Height = 400; // Image Height
        private const int Stride = 400 * 1; // Image Stide - Bytes per Row (3 bytes per pixel)

        public static Image<Gray, byte> ByteArrayToGrayImage(byte[] byteArray)
        {
            // Create data for an Image 400x400 GRAY - 160 000 Bytes - 0x27100
            var sourceImgData = new byte[0x27100];

            // Pin the imgData in memory and create an IntPtr to it's location
            var pinnedArray = GCHandle.Alloc(sourceImgData, GCHandleType.Pinned);
            var pointer = pinnedArray.AddrOfPinnedObject();

            // Create an image from the imgData
            var img = new Image<Gray, byte>(Width, Height, 1, pointer);

            // Free the memory
            pinnedArray.Free();

            return img;
        }
    }
}