using System.Threading.Tasks;
using System.Windows.Input;
using ImageClassifier.Services;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;

namespace ImageClassifier.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private readonly IClassifierService classifierService;
        private readonly IMedia media;
        
        public MainPageViewModel(IClassifierService classifierService, IMedia media)
        {
            this.classifierService = classifierService;
            this.media = media;
            TakePhotoCommand = new DelegateCommand(async () => await TakePhoto());
            PickPhotoCommand = new DelegateCommand(async () => await PickPhoto());
        }

        public ICommand TakePhotoCommand { get; }

        public ICommand PickPhotoCommand { get; }

        private async Task TakePhoto()
        {
            var photo = await media.TakePhotoAsync(new StoreCameraMediaOptions());
            await ProcessPhoto(photo);
        }

        private async Task PickPhoto()
        {
            var photo = await media.PickPhotoAsync();
            await ProcessPhoto(photo);
        }

        private async Task ProcessPhoto(MediaFile photo)
        {
            if (photo == null)
            {
                return;
            }

            var result = await classifierService.ProcessImage(photo);
        }
    }
}
