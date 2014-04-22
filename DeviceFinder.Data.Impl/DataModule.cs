using DeviceFinder.Data.Impl.Dao;
using DeviceFinder.Data.Impl.Vo;
using DeviceFinder.Data.Interface.Dao;
using DeviceFinder.Data.Interface.Vo;
using Ninject.Modules;
namespace DeviceFinder.Data.Impl
{
    public class DataModule : NinjectModule
    {
        public override void Load()
        {
            //Daos
            Bind<IDataAccessObject>().To<DataAccessObject>().InSingletonScope();
          
            //Vos
            Bind<IModelVo>().To<ModelVo>();
        }
    }
}