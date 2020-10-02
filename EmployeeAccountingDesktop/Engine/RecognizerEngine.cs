using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EA.DesktopApp.Helpers;
using EA.DesktopApp.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace EA.DesktopApp.Engine
{
    /// <summary>
    ///     Eigen core class for facial recognize
    /// </summary>
    internal class RecognizerEngine
    {
        public int eigenTrainedImageCounter;
        public List<Image<Gray, byte>> eigenTrainingImages = new List<Image<Gray, byte>>();
        public List<int> eigenIntlabels = new List<int>();
        public List<string> eigenlabels = new List<string>();

        public EigenFaceRecognizer eigenFaceRecognizer;

        public bool IsTrained;

        public double eigenThreshold = 7000;

        private FaceRecognizer _faceRecognizer;
        private WebApiHelper _dataStoreAccess;
        private string _recognizerFilePath;
        private IList<Person> allFaces = new List<Person>();

        // Для проверки из файлов
        public RecognizerEngine(string recognizerFilePath)
        {
            _recognizerFilePath = recognizerFilePath;
            _faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
        }

        public RecognizerEngine(string databasePath, string recognizerFilePath)
        {
            _recognizerFilePath = recognizerFilePath;
            _dataStoreAccess = new WebApiHelper(databasePath);
            _faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);

            // Закешировал данные из БД.
            GetAllEmployee();
        }

        private void GetAllEmployee()
        {
            try
            {
                // Закешировал данные из БД.
                allFaces = _dataStoreAccess.GetAllAsync().Result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool TrainRecognizer()
        {
            
            if (allFaces != null)
            {
                var faceImages = new Image<Gray, byte>[allFaces.Count];
                var faceLabels = new int[allFaces.Count];
                for (int i = 0; i < allFaces.Count; i++)
                {
                    var stream = new MemoryStream();
                    var image = Convert.FromBase64String(allFaces[i].Photo);
                    stream.Write(image, 0, image.Length);
                    var faceImage = new Image<Gray, byte>(new Bitmap(stream));
                    faceImages[i] = faceImage.Resize(100, 100, INTER.CV_INTER_CUBIC);
                    faceLabels[i] = allFaces[i].Id;
                }
                _faceRecognizer.Train(faceImages, faceLabels);
                _faceRecognizer.Save(_recognizerFilePath);
            }
            return true;

        }

        public void LoadRecognizerData()
        {
            _faceRecognizer.Load(_recognizerFilePath);
        }

        public int RecognizeUser(Image<Gray, byte> userImage)
        {
            /*  Stream stream = new MemoryStream();
              stream.Write(userImage, 0, userImage.Length);
              var faceImage = new Image<Gray, byte>(new Bitmap(stream));*/
            _faceRecognizer.Load(_recognizerFilePath);

            var result = _faceRecognizer.Predict(userImage.Resize(100, 100, INTER.CV_INTER_CUBIC));
            return result.Label;
        }

        public bool TrainFromFolder()
        {
            if (IsTrained)
            {
                eigenTrainedImageCounter = 0;
                eigenTrainingImages.Clear();
                eigenIntlabels.Clear();
                eigenlabels.Clear();
                IsTrained = false;
            }

            try
            {
                string dataDirectory = Directory.GetCurrentDirectory() + @"\Traineddata";
                string[] files = Directory.GetFiles(dataDirectory, "*.jpg", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    Image<Gray, byte> trainedImage = new Image<Gray, byte>(file);
                    //trainedImage._EqualizeHist();
                    eigenTrainingImages.Add(trainedImage);
                    eigenlabels.Add(GetFileName(file));
                    eigenIntlabels.Add(eigenTrainedImageCounter);
                    eigenTrainedImageCounter++;
                    Console.WriteLine(eigenTrainedImageCounter);
                }

                try
                {
                    eigenFaceRecognizer = new EigenFaceRecognizer(eigenTrainedImageCounter, eigenThreshold);
                    eigenFaceRecognizer.Train(eigenTrainingImages.ToArray(), eigenIntlabels.ToArray());
                }
                catch (Exception e)
                {
                }
            }
            catch (Exception ex)
            {
                return IsTrained = false;
            }
            return IsTrained = true;
        }

        public async Task<bool> TrainFromDataBase(string url)
        {
            if (IsTrained)
            {
                eigenTrainedImageCounter = 0;
                eigenTrainingImages.Clear();
                eigenIntlabels.Clear();
                eigenlabels.Clear();
                IsTrained = false;
            }

            try
            {
                _dataStoreAccess = new WebApiHelper(url);
                var list = await _dataStoreAccess.GetAllAsync();

                if (list != null)
                {
                    foreach (var person in list)
                    {
                        using (var ms = new MemoryStream(Convert.FromBase64String(person.Photo)))
                        {
                            Image<Gray, byte> trainedImage = new Image<Gray, byte>((Bitmap)Image.FromStream(ms));
                            eigenTrainingImages.Add(trainedImage);
                            eigenlabels.Add($"{person.Name} {person.LastName}");
                            eigenIntlabels.Add(eigenTrainedImageCounter);
                            eigenTrainedImageCounter++;
                        }

                    }
                    eigenFaceRecognizer = new EigenFaceRecognizer(eigenTrainedImageCounter, eigenThreshold);
                    eigenFaceRecognizer.Train(eigenTrainingImages.ToArray(), eigenIntlabels.ToArray());
                }
            }
            catch (Exception ex)
            {
                return IsTrained = false;
            }
            return IsTrained = true;
        }

        private string GetFileName(string file)
        {
            string[] fileArr = file.Split('\\');
            var fileName = fileArr[fileArr.Length - 1].Split('_')[0];
            return fileName;
        }
    }
}