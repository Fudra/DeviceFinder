using Crosscutting;
using DeviceFinder.ViewModel.ViewModels;
using Ninject;

namespace DeviceFinder.ViewModel
{
    public class ViewModelManager
    {
        #region Fields

        private static readonly ViewModelManager _viewModelManager = new ViewModelManager();

        #endregion

        #region Singleton

        private ViewModelManager()
        {
            
        }

        public static ViewModelManager Instance
        {
            get { return _viewModelManager; }
        }

        #endregion

        public DeviceFinderWindowViewModel DeviceFinderWindowViewModel
        {
            get { return KernelManager.Instance.GetKernel().Get<DeviceFinderWindowViewModel>(); }
        }

    }
}