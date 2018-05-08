using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using ImageClassifier.Services;
using Org.Tensorflow.Contrib.Android;
using Plugin.Media.Abstractions;

namespace ImageClassifier.Droid.Services
{
    public class ClassifierService : IClassifierService
    {
        private const int InputSize = 227;
        private const string InputName = "Placeholder";
        private const string OutputName = "loss";

        private readonly TensorFlowInferenceInterface inferenceInterface;
        private readonly List<string> labels;

        public ClassifierService()
        {
            var assets = Application.Context.Assets;
            inferenceInterface = new TensorFlowInferenceInterface(assets, "model.pb");
            var sr = new StreamReader(assets.Open("labels.txt"));
            labels = sr.ReadToEnd()
                .Split('\n')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();
        }

        public IList<string> Labels => labels;

        public async Task<LabelsConfidence> ProcessImage(MediaFile mediaFile)
        {
            var bitmap = await BitmapFactory.DecodeStreamAsync(mediaFile.GetStreamWithImageRotatedForExternalStorage());
            return RecognizeImage(bitmap);
        }

        public LabelsConfidence RecognizeImage(Bitmap bitmap)
        {
            var outputNames = new[] {OutputName};
            var floatValues = GetBitmapPixels(bitmap);
            var outputs = new float[labels.Count];

            inferenceInterface.Feed(InputName, floatValues, 1, InputSize, InputSize, 3);
            inferenceInterface.Run(outputNames);
            inferenceInterface.Fetch(OutputName, outputs);

            var dictionary = Labels.Zip(outputs, (k, v) => new {k, v}).ToDictionary(x => x.k, x => x.v);

            return new LabelsConfidence(dictionary);
        }

        private float[] GetBitmapPixels(Bitmap bitmap)
        {
            var floatValues = new float[227 * 227 * 3];

            using (var scaledBitmap = Bitmap.CreateScaledBitmap(bitmap, 227, 227, false))
            {
                using (var resizedBitmap = scaledBitmap.Copy(Bitmap.Config.Argb8888, false))
                {
                    var intValues = new int[227 * 227];
                    resizedBitmap.GetPixels(intValues, 0, resizedBitmap.Width, 0, 0, resizedBitmap.Width,
                        resizedBitmap.Height);

                    for (int i = 0; i < intValues.Length; ++i)
                    {
                        var val = intValues[i];

                        floatValues[i * 3 + 0] = (val & 0xFF) - 104;
                        floatValues[i * 3 + 1] = ((val >> 8) & 0xFF) - 117;
                        floatValues[i * 3 + 2] = ((val >> 16) & 0xFF) - 123;
                    }

                    resizedBitmap.Recycle();
                }

                scaledBitmap.Recycle();
            }

            return floatValues;
        }
    }
}