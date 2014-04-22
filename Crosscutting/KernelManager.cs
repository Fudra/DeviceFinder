
using Ninject;

namespace Crosscutting
{
    public class KernelManager
    {
        #region Fields
        private static readonly KernelManager _kernel = new KernelManager();
        private StandardKernel _standardKernel;
        #endregion

        #region Singleton

        public static KernelManager Instance
        {
            get { return _kernel; }
        }

        private KernelManager()
        {
            _standardKernel = new StandardKernel();
            _standardKernel.Load("DeviceFinder.*.dll");
        }

        #endregion

        public IKernel GetKernel()
        {
            return _standardKernel;
        }
    }
}