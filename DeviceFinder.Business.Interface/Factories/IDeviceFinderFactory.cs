using System.Collections.Generic;
using DeviceFinder.Business.Interface.Models;

namespace DeviceFinder.Business.Interface.Factories
{
    public interface IDeviceFinderFactory
    {
        IEnumerable<IModel> GetAllModelsFromExcelSheet();

        IEnumerable<IProductLine> GetProductLinesExcel(ISetting setting);
       
        IEnumerable<IProductLine> GetProductLinesDb(ISetting setting);

        IEnumerable<string> GetSupportedTranslationsExcel(ISetting setting);

        IEnumerable<string> GetSupportedTranslationsDb();

        IEnumerable<ITranslation> GetTranslationsExcel(ISetting setting);
       
        IEnumerable<ITranslation> GetTranslationsDb();

        void SyncProductLines();
    }
}