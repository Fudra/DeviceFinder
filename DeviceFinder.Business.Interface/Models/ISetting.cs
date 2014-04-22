using System;
using Crosscutting.Enums;

namespace DeviceFinder.Business.Interface.Models
{
    public interface ISetting
    {
        string Filename { get; set; }
        
        string ConnectionString { get; set; }
        
        #region Excel Sheets

        string ProductLineSheet { get; set; }

        string TranslationSheet { get; set; }
        
        string ViewSheet { get; set; }

        string PropertiesSheet { get; set; }
        
        #endregion

        string LanguageExcel { get; set; }

        string LanguageDB { get; set; }

        string Version { get; }

        ExcelFileFormat ExcelFileFormat { get; set; }

        event EventHandler SettingChanged;
    }
}