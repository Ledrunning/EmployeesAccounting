using EA.RecognizerEngine.Contracts;
using EA.RecognizerEngine.Exceptions;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace EA.RecognizerEngine.Engines
{
    public class EigenFaceRecognition : FaceRecognitionBase, IEigenFaceRecognition
    {
        private readonly EigenFaceRecognizer _recognizer;

        public EigenFaceRecognition(EigenFaceRecognizer recognizer)
        {
            _recognizer = recognizer;
        }

        public bool IsImageTrained { get; set; }

        public override void Train()
        {
            if (TrainingImages.Count == 0)
            {
                IsImageTrained = false;
                throw new RecognizerEngineException("No training images provided.");
            }

            using (var vectorOfImages = new VectorOfMat())
            using (var vectorOfLabels = new VectorOfInt(Labels.ToArray()))
            {
                foreach (var image in TrainingImages)
                {
                    vectorOfImages.Push(image.Mat);
                }

                _recognizer.Train(vectorOfImages, vectorOfLabels);
                IsImageTrained = true;
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