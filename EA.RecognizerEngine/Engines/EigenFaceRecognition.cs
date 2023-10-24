using System;
using System.Collections.Generic;
using EA.RecognizerEngine.Contracts;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace EA.RecognizerEngine.Engines
{
    public class EigenFaceRecognition : IEigenFaceRecognition
    {
        private readonly List<int> _labels;
        private readonly EigenFaceRecognizer _recognizer;
        private readonly List<Image<Gray, byte>> _trainingImages;

        public EigenFaceRecognition()
        {
            _recognizer = new EigenFaceRecognizer();
            _trainingImages = new List<Image<Gray, byte>>();
            _labels = new List<int>();
        }

        public void AddTrainingImage(Image<Gray, byte> image, int label)
        {
            _trainingImages.Add(image);
            _labels.Add(label);
        }

        public void Train()
        {
            if (_trainingImages.Count == 0)
            {
                throw new Exception("No training images provided.");
            }

            using (var vectorOfImages = new VectorOfMat())
            using (var vectorOfLabels = new VectorOfInt(_labels.ToArray()))
            {
                foreach (var image in _trainingImages)
                {
                    vectorOfImages.Push(image.Mat);
                }

                _recognizer.Train(vectorOfImages, vectorOfLabels);
            }
        }


        public int Predict(Image<Gray, byte> queryImage)
        {
            var result = _recognizer.Predict(queryImage);
            return result.Label;
        }

        // Optionally, if you want to access the confidence (distance) of the prediction:
        public (int Label, double Distance) PredictWithDistance(Image<Gray, byte> queryImage)
        {
            var result = _recognizer.Predict(queryImage);
            return (result.Label, result.Distance);
        }
    }
}