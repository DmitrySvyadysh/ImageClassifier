using System.Collections.Generic;
using System.ComponentModel;
using ImageClassifier.Services;
using Plugin.Media.Abstractions;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace ImageClassifier.ViewModels
{
    public class ResultPageViewModel : BindableBase, INavigationAware
    {
        public static readonly string ResultPagePhotoKey = "PhotoKey";

        private readonly IClassifierService classifierService;

        private MediaFile photo;
        private ImageSource photoSource;
        private IList<LabelConfidence> labelsConfidences;

        public ResultPageViewModel(IClassifierService classifierService)
        {
            this.classifierService = classifierService;
        }

        public ImageSource PhotoSource
        {
            get => photoSource;
            set => SetProperty(ref photoSource, value);
        }

        public IList<LabelConfidence> LabelsConfidences
        {
            get => labelsConfidences;
            set => SetProperty(ref labelsConfidences, value);
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            photo = (MediaFile)parameters[ResultPagePhotoKey];
            PhotoSource = ImageSource.FromStream(photo.GetStream);
        }


        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            LabelsConfidences = await classifierService.ProcessImage(photo);
        }
    }
}
