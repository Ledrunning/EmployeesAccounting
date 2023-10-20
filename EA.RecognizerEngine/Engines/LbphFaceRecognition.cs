using System;
using System.Collections.Generic;
using EA.RecognizerEngine.Contracts;
using EA.RecognizerEngine.Exceptions;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace EA.RecognizerEngine.Engines
{
    /// <summary>
    ///     Load data -> 2. Add to training set -> 3. Train -> 4. Predict.
    /// </summary>
    public class LbphFaceRecognition : ILbphFaceRecognition
    {
        private readonly List<int> _labels = new List<int>();
        private readonly LBPHFaceRecognizer _recognizer;
        private readonly List<Image<Gray, byte>> _trainingImages;

        public LbphFaceRecognition()
        {
            _recognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 1000.00);
            _trainingImages = new List<Image<Gray, byte>>();
        }

        //TODO I need some suggestion for change long into int;
        public void AddTrainingImage(Image<Gray, byte> image, long label)
        {
            _trainingImages.Add(image);
            _labels.Add(Convert.ToInt32(label));
        }

        public void Train()
        {
            if (_trainingImages.Count == 0)
            {
                throw new RecognizerEngineException("No training images provided.");
            }

            var faces = _trainingImages.ToArray();
            using (var vectorOfMat = new VectorOfMat())
            {
                vectorOfMat.Push(faces);
                var labels = _labels.ToArray();
                using (var vectorOfInt = new VectorOfInt())
                {
                    vectorOfInt.Push(labels);
                    _recognizer.Train(vectorOfMat, vectorOfInt);
                }
            }
        }

        public long Predict(Image<Gray, byte> queryImage)
        {
            var result = _recognizer.Predict(queryImage);
            return result.Label;
        }
    }
}