using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.GPU;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Services
{
    public class FaceDetectionService : WebCamService
    {
        public delegate void ImageWithDetectionChangedEventHandler(object sender, Image<Bgr, byte> image);

        private const string EyeFileName = "haarcascade_eye.xml";

        private readonly int EyeRectangleThickness = 2;

        /// <summary>
        ///     Detecting algorithm for Open CV using xml files
        /// </summary>
        private const string FaceFileName = "haarcascade_frontalface_default.xml";
        private readonly int FaceRectanglethickness = 2;


        private Tuple<List<Rectangle>, List<Rectangle>> faceAndEyes =
            new Tuple<List<Rectangle>, List<Rectangle>>(new List<Rectangle>(), new List<Rectangle>());

        private List<Rectangle> faces = new List<Rectangle>();
        private CascadeClassifier eye;

        private CascadeClassifier face;
        private MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 1.0, 1.0);

        /// <summary>
        ///     For haarcascade algorithm
        /// </summary>
        private Image<Gray, byte> gray;

        /// <summary>
        ///     Flag when face is detecting
        /// </summary>
        private bool isDetecting;

        /// <summary>
        ///     Init for face detection method
        /// </summary>
        public FaceDetectionService()
        {
            InitializeServices();
        }

        public string EmployeeData { get; set; }

        public event ImageWithDetectionChangedEventHandler ImageWithDetectionChanged;

        /// <summary>
        ///     Event handler from web cam services
        /// </summary>
        private void InitializeServices()
        {
            //face = new HaarCascade(_faceFileName);
            //eye = new HaarCascade(_eyeFileName);
            ImageChanged += WebCamServiceImageChanged;
        }

        /// <summary>
        ///     Calling Image changet event delegate
        /// </summary>
        /// <param name="image"></param>
        private void RaiseImageWithDetectionChangedEvent(Image<Bgr, byte> image)
        {
            ImageWithDetectionChanged?.Invoke(this, image);
        }

        /// <summary>
        ///     Event when faces and eyes is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="image"></param>
        private async void WebCamServiceImageChanged(object sender, Image<Bgr, byte> image)
        {
            var isDelayed = false;

            if (!isDetecting)
            {
                isDetecting = true;
                var result = await FacesAndEyesAsync(image);

                isDelayed = true;
                faceAndEyes = result;
                isDetecting = false;
            }

            if (!isDelayed) // to prevent displaing delayed image
            {
                try
                {
                    DrawRectangles(image);
                    RaiseImageWithDetectionChangedEvent(image);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        ///     Method for draw rectangle around face and eyes with
        ///     DetectFacesAndEyeAsync method
        /// </summary>
        /// <param name="image"></param>
        private void DrawRectangles(Image<Bgr, byte> image)
        {
            foreach (var f in faceAndEyes.Item1)
            {
                image.Draw(f, new Bgr(Color.Red), FaceRectanglethickness);
                //image.Draw(EmployeeData, ref font, new Point(f.X - 2, f.Y - 5), new Bgr(Color.Red));

                foreach (var e in faceAndEyes.Item2)
                {
                    image.Draw(e, new Bgr(Color.Blue), EyeRectangleThickness);
                }
            }
        }

        /// <summary>
        ///     Face and eyes detection method
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private Task<Tuple<List<Rectangle>, List<Rectangle>>> FacesAndEyesAsync(Image<Bgr, byte> image)
        {
            return Task.Run(() =>
            {
                var faces = new List<Rectangle>();
                var eyes = new List<Rectangle>();

                DetectFaceAndEyes(image, faces, eyes);
                return new Tuple<List<Rectangle>, List<Rectangle>>(faces, eyes);
            });
        }

        #region EMGU face detection methods

        private void DetectFace(Image<Bgr, byte> image, List<Rectangle> faces)
        {
#if !IOS
            if (GpuInvoke.HasCuda)
            {
                using (var face = new GpuCascadeClassifier(FaceFileName))
                {
                    using (var gpuImage = new GpuImage<Bgr, byte>(image))
                    using (var gpuGray = gpuImage.Convert<Gray, byte>())
                    {
                        var faceRegion = face.DetectMultiScale(gpuGray, 1.1, 10, Size.Empty);
                        faces.AddRange(faceRegion);
                    }
                }
            }
            else
#endif
            {
                using (var face = new CascadeClassifier(FaceFileName))
                {
                    using (var gray = image.Convert<Gray, byte>()) //Convert it to Grayscale
                    {
                        //normalizes brightness and increases contrast of the image
                        gray._EqualizeHist();

                        //Detect the faces  from the gray scale image and store the locations as rectangle
                        //The first dimensional is the channel
                        //The second dimension is the index of the rectangle in the specific channel
                        var facesDetected = face.DetectMultiScale(
                            gray,
                            1.1,
                            10,
                            new Size(20, 20),
                            Size.Empty);
                        faces.AddRange(facesDetected);
                    }
                }
            }
        }

        private void DetectFaceAndEyes(Image<Bgr, byte> image, List<Rectangle> faces, List<Rectangle> eyes)
        {
#if !IOS
            if (GpuInvoke.HasCuda)
            {
                using (var face = new GpuCascadeClassifier(FaceFileName))
                using (var eye = new GpuCascadeClassifier(EyeFileName))
                {
                    using (var gpuImage = new GpuImage<Bgr, byte>(image))
                    using (var gpuGray = gpuImage.Convert<Gray, byte>())
                    {
                        var faceRegion = face.DetectMultiScale(gpuGray, 1.1, 10, Size.Empty);
                        faces.AddRange(faceRegion);
                        foreach (var f in faceRegion)
                        {
                            using (var faceImg = gpuGray.GetSubRect(f))
                            {
                                //For some reason a clone is required.
                                //Might be a bug of GpuCascadeClassifier in opencv
                                using (var clone = faceImg.Clone())
                                {
                                    var eyeRegion = eye.DetectMultiScale(clone, 1.1, 10, Size.Empty);

                                    foreach (var e in eyeRegion)
                                    {
                                        var eyeRect = e;
                                        eyeRect.Offset(f.X, f.Y);
                                        eyes.Add(eyeRect);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
#endif
            {
                using (var face = new CascadeClassifier(FaceFileName))
                using (var eye = new CascadeClassifier(EyeFileName))
                {
                    using (var gray = image.Convert<Gray, byte>()) //Convert it to Grayscale
                    {
                        //normalizes brightness and increases contrast of the image
                        gray._EqualizeHist();

                        //Detect the faces  from the gray scale image and store the locations as rectangle
                        //The first dimensional is the channel
                        //The second dimension is the index of the rectangle in the specific channel
                        var facesDetected = face.DetectMultiScale(
                            gray,
                            1.1,
                            10,
                            new Size(20, 20),
                            Size.Empty);
                        faces.AddRange(facesDetected);

                        foreach (var f in facesDetected)
                        {
                            //Set the region of interest on the faces
                            gray.ROI = f;
                            var eyesDetected = eye.DetectMultiScale(
                                gray,
                                1.1,
                                10,
                                new Size(20, 20),
                                Size.Empty);
                            gray.ROI = Rectangle.Empty;

                            foreach (var e in eyesDetected)
                            {
                                var eyeRect = e;
                                eyeRect.Offset(f.X, f.Y);
                                eyes.Add(eyeRect);
                            }
                        }
                    }
                }
            }
        }

        #endregion EMGU face detection methods
    }
}