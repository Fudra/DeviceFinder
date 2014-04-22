using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using DeviceFinder.Business.Interface.Factories;
using DeviceFinder.Business.Interface.Models;
using DeviceFinder.ViewModel.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DeviceFinder.ViewModel.ViewModels
{
    public class DeviceFinderWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDeviceFinderFactory _deviceFinderFactory;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly ISetting _setting;
        private IEnumerable<string> _languagesExcel; 
        private bool _isBusy;
        private readonly ViewModelFactory _viewModelFactory;
        private IEnumerable<ModelViewModel> _excelModels;
       // private IEnumerable<ModelViewModel> _dbModels;
        private IEnumerable<ProductLineViewModel> _productLinesExcel;
        private IEnumerable<ProductLineViewModel> _productLinesDb;
        private IEnumerable<string> _languagesDb;


        #endregion

        public DeviceFinderWindowViewModel(IDeviceFinderFactory deviceFinderFactory, ISetting setting,
            ViewModelFactory viewModelFactory)
        {
            _deviceFinderFactory = deviceFinderFactory;
            _viewModelFactory = viewModelFactory;
            _setting = setting;
            //Componenten initialisierung
            Init();
        }

        private void OnSettingChanged(object sender, EventArgs e)
        {
            RefreshCommand.Execute(null);
        }

        private void Init()
        {
            // Init Timer
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += TimerOnTick;
            _timer.Start();
            // settings
            _setting.SettingChanged += OnSettingChanged;
            // init Commands
            InitCommands();
            // refresh
            RefreshCommand.Execute(null);

        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            _timer.Stop();
            RefreshAync();
        }

        private async void RefreshAync()
        {
            IsBusy = true;
            
            //lade Dateien aus der Excel Datei
           // var excelModels = await Task.Run(() => _deviceFinderFactory.GetAllModelsFromExcelSheet().ToArray());
            //xcelModels = _viewModelFactory.Create(excelModels);


           LanguagesExcel = await Task.Run(() =>
                _deviceFinderFactory.GetSupportedTranslationsExcel(_setting));

           LanguagesDb = await Task.Run(() =>
                _deviceFinderFactory.GetSupportedTranslationsDb());

           ProductLinesExcel = await Task.Run(() =>
            {
                var m = _deviceFinderFactory.GetProductLinesExcel(_setting).ToArray();
                return _viewModelFactory.Create(m);
            });

            ProductLinesDb = await Task.Run(() =>
            {
                var lines = _deviceFinderFactory.GetProductLinesDb(_setting).ToArray();
                return _viewModelFactory.Create(lines);
            });

            // Lade Datein aus der DB
            //var dbModels = await Task.Run(() => _deviceFinderFactory.GetAllModelsFormDB().ToArray);
            //DBModels = _viewModelFactory.Create(dbModels);


            IsBusy = false;
            OnIsDataChanged(new EventArgs());
        }

        private async Task RefreshAync1()
        {
            IsBusy = true;

            var tasks = new List<Task>(4)
            {
                Task.Run(() =>
                {
                    var m = _deviceFinderFactory.GetProductLinesExcel(_setting).ToArray();
                    return _viewModelFactory.Create(m);
                }).ContinueWith((task) =>
                {
                    ProductLinesExcel = task.Result;
                }),
                Task.Run(() =>
                {
                    var lines = _deviceFinderFactory.GetProductLinesDb(_setting).ToArray();
                    return _viewModelFactory.Create(lines);
                }).ContinueWith((task) =>
                {
                    ProductLinesDb = task.Result;
                }),
                Task.Run(() =>
                    _deviceFinderFactory.GetSupportedTranslationsExcel(_setting)
                    ).ContinueWith((task) =>
                    {
                        LanguagesExcel = task.Result;
                    }),
                Task.Run(() =>
                    _deviceFinderFactory.GetSupportedTranslationsDb()
                    ).ContinueWith((task) =>
                    {
                        LanguagesDb = task.Result;
                    })
            };

            //lade Dateien aus der Excel Datei
            // var excelModels = await Task.Run(() => _deviceFinderFactory.GetAllModelsFromExcelSheet().ToArray());
            //xcelModels = _viewModelFactory.Create(excelModels);

            await Task.Run(() =>
            {
                Task.WaitAll(tasks.ToArray());
                IsBusy = false;
            });

            // Lade Datein aus der DB
            //var dbModels = await Task.Run(() => _deviceFinderFactory.GetAllModelsFormDB().ToArray);
            //DBModels = _viewModelFactory.Create(dbModels);
            
           // OnIsDataChanged(new EventArgs());
        }

        #region Commands

        public ICommand RefreshCommand { get; internal set; }
        public ICommand SyncCommand { get; internal set; }


        private void InitCommands()
        {
            RefreshCommand = new RelayCommand<object>(RefreshCommandExcecute, RefreshCommandCanExcecute);
            SyncCommand = new RelayCommand<object>(SyncCommandExcecute,SyncCommandCanExcecute);
        }

        private void RefreshCommandExcecute(object o)
        {
            _timer.Stop();
            _timer.Start();
        }

        private bool RefreshCommandCanExcecute(object o)
        {
            return !IsBusy;
        }


        private async void SyncCommandExcecute(object o)
        {
            IsBusy = true;
            await Task.Run(() => _deviceFinderFactory.SyncProductLines());
            IsBusy = false;
            RefreshCommand.Execute(null);
        }

        private bool SyncCommandCanExcecute(object o)
        {
            return !IsBusy;
        }

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public IEnumerable<ModelViewModel> ExcelModels
        {
            get { return _excelModels; }
            set
            {
                _excelModels = value;
                RaisePropertyChanged(()=>ExcelModels);
            }
        }

        public IEnumerable<ProductLineViewModel> ProductLinesExcel
        {
            get{ return _productLinesExcel; }
            set
            {
                _productLinesExcel = value;
                RaisePropertyChanged(()=>ProductLinesExcel);
            }
        }

        public IEnumerable<ProductLineViewModel> ProductLinesDb
        {
            get { return _productLinesDb; }
            set
            {
                _productLinesDb = value;
               RaisePropertyChanged(()=>ProductLinesDb);
            }
        }

        public IEnumerable<string> LanguagesExcel
        {
            get { return _languagesExcel; }
            set
            {
                _languagesExcel = value;
                RaisePropertyChanged(()=>LanguagesExcel);
            }
        }

        public IEnumerable<string> LanguagesDb
        {
            get { return _languagesDb; }
            set
            {
                _languagesDb = value;
                RaisePropertyChanged(()=>LanguagesDb);
            }
        }

        public ISetting Setting
        {
            get { return _setting; }
        }
        

        #endregion

        #region Event for CodeBehind

        public event EventHandler IsDataChanged;

        protected void OnIsDataChanged(EventArgs args)
        {
            var handler = IsDataChanged;
            if (handler != null)
                handler(this, args);
        }

        #endregion

    }   
}