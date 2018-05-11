using System.Threading.Tasks;
using System.Windows.Input;
using ImageClassifier.Views;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace ImageClassifier.ViewModels
{
    public class HomePageViewModel : BindableBase
    {
        private readonly INavigationService navigationService;
        private readonly IMedia media;
        
        public HomePageViewModel(INavigationService navigationService, IMedia media)
        {
            this.navigationService = navigationService;
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

            var navigationParameters = new NavigationParameters{{ResultPageViewModel.ResultPagePhotoKey, photo}};
            await navigationService.NavigateAsync(nameof(ResultPage), navigationParameters);
        }
    }
}
