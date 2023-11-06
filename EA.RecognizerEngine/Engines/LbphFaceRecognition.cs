using System;
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
    public class LbphFaceRecognition : FaceRecognitionBase, ILbphFaceRecognition
    {
        private readonly LBPHFaceRecognizer _recognizer;

        public LbphFaceRecognition()
        {
            _recognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 400.00);
        }

        //TODO I need some suggestion for change long into int;
        public void AddTrainingImage(Image<Gray, byte> image, long label)
        {
            TrainingImages.Add(image);
            Labels.Add(Convert.ToInt32(label));
        }

        public override void Train()
        {
            if (TrainingImages.Count == 0)
            {
                throw new RecognizerEngineException("No training images provided.");
            }

            var faces = TrainingImages.ToArray();
            using (var vectorOfMat = new VectorOfMat())
            {
                vectorOfMat.Push(faces);
                var labels = Labels.ToArray();
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