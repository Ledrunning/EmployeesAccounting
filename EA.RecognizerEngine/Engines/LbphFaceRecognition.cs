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
        private readonly List<long> _labels = new List<long>();
        private readonly LBPHFaceRecognizer _recognizer;
        private readonly List<Image<Gray, byte>> _trainingImages;

        public LbphFaceRecognition()
        {
            _recognizer = new LBPHFaceRecognizer();
            _trainingImages = new List<Image<Gray, byte>>();
        }

        public void AddTrainingImage(Image<Gray, byte> image, long label)
        {
            _trainingImages.Add(image);
            _labels.Add(label);
        }

        public void Train()
        {
            if (_trainingImages.Count == 0)
            {
                throw new RecognizerEngineException("No training images provided.");
            }

            using (var vm = new VectorOfMat())
            using (var labelsMatrix = new Matrix<long>(_labels.ToArray()))
            {
                foreach (var img in _trainingImages)
                {
                    vm.Push(img.Mat);
                }

                _recognizer.Train(vm, labelsMatrix);
            }
        }

        public long Predict(Image<Gray, byte> queryImage)
        {
            var result = _recognizer.Predict(queryImage);
            return result.Label;
        }
    }
}