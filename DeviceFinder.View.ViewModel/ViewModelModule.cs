using DeviceFinder.ViewModel.ViewModels;
using Ninject.Modules;

namespace DeviceFinder.ViewModel
{
    public class ViewModelModule : NinjectModule
    {
        public override void Load()
        {
            Bind<DeviceFinderWindowViewModel>().ToSelf().InSingletonScope();

            //Factory
            Bind<ViewModelFactory>().ToSelf().InSingletonScope();
        }
    }
}