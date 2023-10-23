using Emgu.CV;
using Emgu.CV.Structure;

namespace EA.RecognizerEngine.Contracts
{
    public interface IEigenFaceRecognition
    {
        void AddTrainingImage(Image<Gray, byte> image, int label);
        void Train();
        int Predict(Image<Gray, byte> queryImage);
        (int Label, double Distance) PredictWithDistance(Image<Gray, byte> queryImage);
    }
}