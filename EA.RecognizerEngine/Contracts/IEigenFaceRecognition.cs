using Emgu.CV;
using Emgu.CV.Structure;

namespace EA.RecognizerEngine.Contracts
{
    public interface IEigenFaceRecognition
    {
        bool IsImageTrained { get; set; }
        void AddTrainingImage(Image<Gray, byte> image, int label);
        void Train();
        int Predict(Image<Gray, byte> queryImage);
        (int Label, double Distance) PredictWithDistance(Image<Gray, byte> queryImage);
    }
}