using ImageClassifier.Droid.Services;
using ImageClassifier.Services;
using Prism;
using Prism.Ioc;

namespace ImageClassifier.Droid
{
    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IClassifierService, ClassifierService>();
        }
    }
}