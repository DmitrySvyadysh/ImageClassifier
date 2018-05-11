using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace ImageClassifier.Services
{
    public interface IClassifierService
    {
        IList<string> Labels { get; }

        Task<IList<LabelConfidence>> ProcessImage(MediaFile mediaFile);
    }
}
