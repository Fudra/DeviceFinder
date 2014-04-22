using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using Crosscutting.Enums;
using DeviceFinder.Business.Impl.Helper;
using DeviceFinder.Business.Interface.Models;

namespace DeviceFinder.Business.Impl.Models
{
    public class Setting : ObjectNotifyPropertyChanged, ISetting
    {
        private string _filename;
        private ExcelFileFormat _excelFileFormat;
        private string _productLineSheet;
        private string _translationSheet;
        private string _viewSheet;
        private string _propertiesSheet;
        private string _languageExcel;
        private string _languageDb;
        private string _connectionString;

        public Setting()
        {
            Filename = "../../../Excel/model_productLine_de.xlsm"; // with german translations

            // ConnectionString
            // todo: --> data source an datei anpassen
            ConnectionString = @"data source=LOCALHOST\sqlexpress;initial catalog=FlowConfiguration;integrated security=True;";

           // SheetNames
            ProductLineSheet = "ProductLine";
            TranslationSheet = "Translations";
            ViewSheet = "View";
            PropertiesSheet = "Properties";

            // Language
            LanguageExcel = "englisch";
            LanguageDB = "English";

        }

        public string Filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                OnSettingChanged(new EventArgs());
                OnPropertyChanged(()=>Filename);
            }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                _connectionString = value;
                OnSettingChanged(new EventArgs());
                OnPropertyChanged(()=>ConnectionString);
            }
        }

        public string ViewSheet
        {
            get { return _viewSheet; }
            set
            {
                _viewSheet = value;
                OnSettingChanged(new EventArgs());
                OnPropertyChanged(()=>ViewSheet);
            }
        }

        public string PropertiesSheet
        {
            get { return _propertiesSheet; }
            set
            {
                _propertiesSheet = value;
                OnSettingChanged(new EventArgs());
                OnPropertyChanged(()=>PropertiesSheet);
            }
        }

        public string LanguageExcel
        {
            get { return _languageExcel; }
            set
            {
                _languageExcel = value;
                OnSettingChanged(new EventArgs());
                OnPropertyChanged(()=>LanguageExcel);
            }
        }

        public string LanguageDB
        {
            get { return _languageDb; }
            set
            {
                _languageDb = value;
                OnSettingChanged(new EventArgs());
                OnPropertyChanged(()=>LanguageDB);
            }
        }

        public string ProductLineSheet
        {
            get { return _productLineSheet; }
            set
            {
                _productLineSheet = value;
                OnSettingChanged(new EventArgs());
                OnPropertyChanged(() => ProductLineSheet);
            }
        }

        public string TranslationSheet
        {
            get { return _translationSheet; }
            set
            {
                _translationSheet = value;
                OnSettingChanged(new EventArgs());
                OnPropertyChanged(()=> TranslationSheet);
            }
        }

        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }


        // not supported
        public ExcelFileFormat ExcelFileFormat
        {
            get { return _excelFileFormat; }
            set
            {
                _excelFileFormat = value;
                OnSettingChanged(new EventArgs());
                OnPropertyChanged(() => ExcelFileFormat);
            }
        }

        #region Event

        public event EventHandler SettingChanged;

        protected void OnSettingChanged(EventArgs args)
        {
            var handler = SettingChanged;
            if (handler != null)
                handler(this, args);
        }

        #endregion
    }
}