using Emgu.CV;
using Emgu.CV.Structure;

namespace EA.RecognizerEngine.Contracts
{
    public interface ILbphFaceRecognition
    {
        void AddTrainingImage(Image<Gray, byte> image, int label);
        void Train();
        int Predict(Image<Gray, byte> queryImage);
    }
}