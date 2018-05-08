using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ImageClassifier
{
    public partial class App
    {
        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(typeof(IMedia), CrossMedia.Current);
            containerRegistry.RegisterForNavigation<Views.HomePage>();
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            Container.Resolve<INavigationService>().NavigateAsync($"/{nameof(Views.HomePage)}").Wait();
        }
    }
}