using DeviceFinder.Business.Impl.Factories;
using DeviceFinder.Business.Impl.Models;
using DeviceFinder.Business.Interface.Factories;
using DeviceFinder.Business.Interface.Models;
using Ninject.Modules;

namespace DeviceFinder.Business.Impl
{
    public class BusinessModule : NinjectModule 
    {
        public override void Load()
        {
            // factroy
            Bind<IDeviceFinderFactory>().To<DeviceFinderFactory>().InSingletonScope();

            //model
            Bind<ISetting>().To<Setting>().InSingletonScope();
        }
    }
}
