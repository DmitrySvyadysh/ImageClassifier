using System.Collections.Generic;

namespace ImageClassifier.Services
{
    public class LabelsConfidence : Dictionary<string, float>
    {
        public LabelsConfidence(IDictionary<string, float> dictionary)
            : base(dictionary)
        {
        }
    }
}
