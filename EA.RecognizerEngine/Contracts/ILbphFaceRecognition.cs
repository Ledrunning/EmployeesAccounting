using Emgu.CV;
using Emgu.CV.Structure;

namespace EA.RecognizerEngine.Contracts
{
    public interface ILbphFaceRecognition
    {
        void AddTrainingImage(Image<Gray, byte> image, long label);
        void Train();
        long Predict(Image<Gray, byte> queryImage);
    }
}