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

        private readonly string _eyeFileName = "haarcascade_eye.xml";

        private readonly int _eyeRectangleThickness = 2;

        public string EmployeeData { get; set; }

        /// <summary>
        ///     Detecting algorithm for Open CV using xml files
        /// </summary>
        private readonly string _faceFileName = "haarcascade_frontalface_default.xml";

        //private FaceModel faceModel = new FaceModel();
        private readonly int _faceRectanglethickness = 2;
        //private FaceModel _faceAndEyes = new FaceModel();

        private Tuple<List<Rectangle>, List<Rectangle>> _faceAndEyes =
            new Tuple<List<Rectangle>, List<Rectangle>>(new List<Rectangle>(), new List<Rectangle>());

        private List<Rectangle> _faces = new List<Rectangle>();
        private HaarCascade eye;

        private HaarCascade face;
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
                _faceAndEyes = result;
                isDetecting = false;
            }

            if (!isDelayed) // to prevent displaing delayed image
                try
                {
                    DrawRectangles(image);
                    RaiseImageWithDetectionChangedEvent(image);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

            // Used with deprecated Haarcascade method
            //await Task.Run(() => DetectFaceAndEyes(image));
            //RaiseImageWithDetectionChangedEvent(image);
        }

        /// <summary>
        ///     Detect face and eyes using Haarcascade
        /// </summary>
        /// <param name="image"></param>

        #region Face detection using HaarCascade
        
        [Obsolete]
        private void DetectFaceAndEyes(Image<Bgr, byte> image)
        {
            //Convert it to Grayscale
            gray = image.Convert<Gray, byte>();

            //Face Detector
            var facesDetected = gray.DetectHaarCascade(
                face,
                1.2,
                10,
                HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

            //Action for each element detected
            foreach (var f in facesDetected[0])
            {
                //result = image.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                image.Draw(f.rect, new Bgr(Color.Red), 2);
                image.Draw("Osman", ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                gray.ROI = f.rect;
                var eyesDetected = gray.DetectHaarCascade(
                    eye,
                    1.1,
                    10,
                    HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                    new Size(20, 20));
                gray.ROI = Rectangle.Empty;

                foreach (var ey in eyesDetected[0])
                {
                    var eyeRect = ey.rect;
                    eyeRect.Offset(f.rect.X, f.rect.Y);
                    image.Draw(eyeRect, new Bgr(Color.Blue), 2);
                }
            }
        }

        #endregion Face detection using HaarCascade

        /// <summary>
        ///     Method for async faces detection
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private Task<List<Rectangle>> DetectFacesAsync(Image<Bgr, byte> image)
        {
            return Task.Run(() =>
            {
                var faces = new List<Rectangle>();

                DetectFace(image, faces);

                return faces;
            });
        }

        #region using Tuple

        /// <summary>
        ///     Method for draw rectangle around face and eyes with
        ///     DetectFacesAndEyeAsync method
        /// </summary>
        /// <param name="image"></param>
        private void DrawRectangles(Image<Bgr, byte> image)
        {
            foreach (var f in _faceAndEyes.Item1)
            {
                image.Draw(f, new Bgr(Color.Red), _faceRectanglethickness);
                image.Draw(EmployeeData, ref font, new Point(f.X - 2, f.Y - 5), new Bgr(Color.Red));

                foreach (var e in _faceAndEyes.Item2)
                {
                    image.Draw(e, new Bgr(Color.Blue), _eyeRectangleThickness);
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

        #endregion using Tuple

        #region EMGU face detection methods

        private void DetectFace(Image<Bgr, byte> image, List<Rectangle> faces)
        {
#if !IOS
            if (GpuInvoke.HasCuda)
                using (var face = new GpuCascadeClassifier(_faceFileName))
                {
                    using (var gpuImage = new GpuImage<Bgr, byte>(image))
                    using (var gpuGray = gpuImage.Convert<Gray, byte>())
                    {
                        var faceRegion = face.DetectMultiScale(gpuGray, 1.1, 10, Size.Empty);
                        faces.AddRange(faceRegion);
                    }
                }
            else
#endif
                using (var face = new CascadeClassifier(_faceFileName))
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

        private void DetectFaceAndEyes(Image<Bgr, byte> image, List<Rectangle> faces, List<Rectangle> eyes)
        {
#if !IOS
            if (GpuInvoke.HasCuda)
                using (var face = new GpuCascadeClassifier(_faceFileName))
                using (var eye = new GpuCascadeClassifier(_eyeFileName))
                {
                    using (var gpuImage = new GpuImage<Bgr, byte>(image))
                    using (var gpuGray = gpuImage.Convert<Gray, byte>())
                    {
                        var faceRegion = face.DetectMultiScale(gpuGray, 1.1, 10, Size.Empty);
                        faces.AddRange(faceRegion);
                        foreach (var f in faceRegion)
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
            else
#endif
                using (var face = new CascadeClassifier(_faceFileName))
                using (var eye = new CascadeClassifier(_eyeFileName))
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

        #endregion EMGU face detection methods
    }
}