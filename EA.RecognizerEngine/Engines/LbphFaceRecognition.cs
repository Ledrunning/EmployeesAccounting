using System;
using System.Collections.Generic;
using EA.RecognizerEngine.Contracts;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace EA.RecognizerEngine.Engines
{
    public class LbphFaceRecognition : ILbphFaceRecognition
    {
        private readonly List<int> _labels;
        private readonly LBPHFaceRecognizer _recognizer;
        private readonly List<Image<Gray, byte>> _trainingImages;

        public LbphFaceRecognition()
        {
            _recognizer = new LBPHFaceRecognizer();
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

            using (var vm = new VectorOfMat())
            using (var labelsMatrix = new Matrix<int>(_labels.ToArray()))
            {
                foreach (var img in _trainingImages)
                {
                    vm.Push(img.Mat);
                }

                _recognizer.Train(vm, labelsMatrix);
            }
        }

        public int Predict(Image<Gray, byte> queryImage)
        {
            var result = _recognizer.Predict(queryImage);
            return result.Label;
        }
    }
}