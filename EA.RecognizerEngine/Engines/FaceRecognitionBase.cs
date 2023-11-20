using Emgu.CV.Structure;
using Emgu.CV;
using System.Collections.Generic;

namespace EA.RecognizerEngine.Engines
{
    public abstract class FaceRecognitionBase
    {
        protected readonly List<int> Labels = new List<int>();
        protected readonly List<Image<Gray, byte>> TrainingImages;

        protected FaceRecognitionBase()
        {
            TrainingImages = new List<Image<Gray, byte>>();
        }

        public void AddTrainingImage(Image<Gray, byte> image, int label)
        {
            TrainingImages.Add(image);
            Labels.Add(label);
        }

        public abstract void Train();
    }
}