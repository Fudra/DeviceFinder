using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using DeviceFinder.ViewModel;
using DeviceFinder.ViewModel.Models;
using DeviceFinder.ViewModel.ViewModels;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace DeviceFinder.View.Views
{
    /// <summary>
    /// Interaktionslogik für DeviceFinderControl.xaml
    /// </summary>
    public partial class DeviceFinderControl : Window
    {
        private IEnumerable<ProductLineViewModel> _data;
        private DeviceFinderWindowViewModel vm;
      
        public DeviceFinderControl()
        {
            InitializeComponent();
            // ViewModelManager.Instance.DeviceFinderWindowViewModel
            // vm = KernelManager.Instance.GetKernel().Get<DeviceFinderWindowViewModel>();
            vm = ViewModelManager.Instance.DeviceFinderWindowViewModel;
            //  _data = ViewModelManager.Instance.DeviceFinderWindowViewModel.ProductLinesExcel;

            ViewModelManager.Instance.DeviceFinderWindowViewModel.IsDataChanged += DeviceFinderWindowViewModelIsDataChanged;
          //  ViewModelManager.Instance.DeviceFinderWindowViewModel.PropertyChanged += DeviceFinderWindowViewModelOnPropertyChanged;
            
        }

        private void DeviceFinderWindowViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {

            
            Console.WriteLine("PropertyName : " + propertyChangedEventArgs.PropertyName);
            if (propertyChangedEventArgs.PropertyName.Contains("ProductLinesExcel"))
                RefreshExcelGridView();
           // Debug.WriteLine("############# ProductLinesExcel");
           if(propertyChangedEventArgs.PropertyName.Contains("ProductLinesDb"))
               RefreshDatabaseGridView();
            //   Debug.WriteLine("############# ProductLinesDb");
        }

        void DeviceFinderWindowViewModelIsDataChanged(object sender, EventArgs e)
        {
            RefreshDatabaseGridView();
            RefreshExcelGridView();
        }


        /// <summary>
        ///  Schreibe Daten aus der Excel Datei in dir GridView
        /// </summary>
        private void RefreshExcelGridView()
        {

            // Clear
            ExcelProductLinesGridView.Columns.Clear();
            ExcelProductLinesGridView.ColumnGroups.Clear();
            //ExcelProductLinesGridView.ItemsSource = null;

            // get current Data from the VM
            var data = ViewModelManager.Instance.DeviceFinderWindowViewModel.ProductLinesExcel;

            var productLineViewModels = data as ProductLineViewModel[] ?? data.ToArray();
            var dbData = ViewModelManager.Instance.DeviceFinderWindowViewModel.ProductLinesDb;
            var comparedData = CompareProductLines(productLineViewModels, dbData);
            var firstItem = productLineViewModels.FirstOrDefault();
            IEnumerable<string> group = new List<string>();
            if(firstItem!=null)
            @group = firstItem.ProductLineProperetyViewModels.Select(i => i.Group).Distinct();

            // Add Groups
            foreach (var model in group)
            {
                // Debug.WriteLine("Type: " + model);
                ExcelProductLinesGridView.ColumnGroups.Add(new GridViewColumnGroup()
                {
                    Header = model,
                    Name = model
                });
            }

            // Add data
            // add productline Name
            var column = new GridViewDataColumn
            {
                DataMemberBinding = new Binding("Name"),
                Header = "Product Line",
                UniqueName = "ProductLine",
            };
            ExcelProductLinesGridView.Columns.Add(column);
            //   ExcelProductLinesGridView.ItemsSource = data;
            var firstDataItem = productLineViewModels.First();

            var modelPropertys = firstDataItem.ProductLineProperetyViewModels;

            //  to fix
            var dumbCount = 1;
            //
            foreach (var mprop in modelPropertys)
            {
                // add productline Name
                //create dumbCountBindung
                var bind = "X" + dumbCount;
                dumbCount++;
                //
                var prop = new GridViewDataColumn
                {
                    //  DataMemberBinding = new Binding(mprop.Name),
                    DataMemberBinding = new Binding(bind),
                    Header = mprop.Name,
                    UniqueName = mprop.Name,
                    ColumnGroupName = mprop.Group,
                };
                ExcelProductLinesGridView.Columns.Add(prop);
            } 
            
            ExcelProductLinesGridView.RowStyleSelector = new ProductLineStyle();
            ExcelProductLinesGridView.ItemsSource = CreateDumbData(comparedData);

        }

        /// <summary>
        /// Schreibe daten in das GridView aus der Datenbank
        /// </summary>
        private void RefreshDatabaseGridView()
        {
            // Clear
            DatabaseModelsGridView.Columns.Clear();
            DatabaseModelsGridView.ColumnGroups.Clear();
            DatabaseModelsGridView.ItemsSource = null;
           
            //Current Datasource
            var data = ViewModelManager.Instance.DeviceFinderWindowViewModel.ProductLinesDb;
           
            if (data == null)
                return;

            var firstItem = data.FirstOrDefault(j => j.ProductLineProperetyViewModels.Any());
           // if(firstItem!= null)
            IEnumerable<string> group = new List<string>();
            if(firstItem != null )
                group = firstItem.ProductLineProperetyViewModels.Select(i => i.Group).Distinct();
            
          
            // Add Groups
            foreach (var model in group)
            {
                DatabaseModelsGridView.ColumnGroups.Add(new GridViewColumnGroup()
                {
                    Header = model,
                    Name = model
                });
            }

            // Add data
            // add productline Name
            var column = new GridViewDataColumn
            {
                DataMemberBinding = new Binding("Name"),
                Header = "Product Line",
                UniqueName = "ProductLine",
            };
            DatabaseModelsGridView.Columns.Add(column);

            var firstDataItem = data.FirstOrDefault(j => j.ProductLineProperetyViewModels.Any()); 
           
            IEnumerable<ProductLineProperetyViewModel> modelPropertys = new List<ProductLineProperetyViewModel>();
            if(firstDataItem != null)
                modelPropertys = firstDataItem.ProductLineProperetyViewModels;

            //  to fix
            var dumbCount = 1;
            
            foreach (var mprop in modelPropertys)
            {
                // add productline Name
                //create dumbCountBindung
                var bind = "X" + dumbCount;
                dumbCount++;
                //
                var prop = new GridViewDataColumn
                {
                    DataMemberBinding = new Binding(bind),
                    Header = mprop.Name,
                    UniqueName = mprop.Name,
                    ColumnGroupName = mprop.Group
                };

                DatabaseModelsGridView.Columns.Add(prop);
            }
            DatabaseModelsGridView.ItemsSource = CreateDumbData(data);
      
        }


        /// <summary>
        /// Vergleicht die ProductLine aus der der Excel und aus der DB miteinander
        /// </summary>
        /// <param name="excleDataSource">Product Line aus Excel</param>
        /// <param name="dbDataSource">product line aus DB</param>
        /// <returns>gibt die Excel datei wieder, mit markierungen</returns>
        private IEnumerable<ProductLineViewModel> CompareProductLines(IEnumerable<ProductLineViewModel> excleDataSource,
            IEnumerable<ProductLineViewModel> dbDataSource)
        {
           // var items = new List<ProductLineViewModel>();

            var excelData = excleDataSource as ProductLineViewModel[] ?? excleDataSource.ToArray();
            var dbData = dbDataSource as ProductLineViewModel[] ?? dbDataSource.ToArray();

            foreach (var excel in excelData)
            {
                // prüfe ob das Item vorhanden ist
                excel.NewItem = !dbData.Any(i => i.Name.Contains(excel.Name));
              //  Debug.WriteLine("Excle Item: " + excel.Name + " ,IsNew:  " + excel.NewItem) ;
                if(excel.NewItem) continue;

               // excel.EditedItem = !dbData.Any(i => i.Name.Contains(excel.Name))
                // bei vorhandensein, überprüfe die Attribute

                foreach (var db in dbData.Where(db => db.Name.Contains(excel.Name)))
                {
                    excel.EditedItem = !db.ProductLineProperetyViewModels.Any();
                }

                //TODO: Genauer Erkennung von IsEdited implementieren...
            }
            return excelData;
        }


        /// <summary>
        /// Erzeuge Daten zum Anzeigen in der GridView
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IEnumerable<Dumb> CreateDumbData(IEnumerable<ProductLineViewModel> data)
        {
            var dumbData = new List<Dumb>();

            try
            {
                foreach (var plvm in data)
                {
                    var cnt = plvm.ProductLineProperetyViewModels.Count();
                    var d = new Dumb
                    {
                        Name = plvm.Name,
                        X1 = 1 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[0].Value,
                        X2 = 2 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[1].Value,
                        X3 = 3 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[2].Value,
                        X4 = 4 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[3].Value,
                        X5 = 5 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[4].Value,
                        X6 = 6 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[5].Value,
                        X7 = 7 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[6].Value,
                        X8 = 8 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[7].Value,
                        X9 = 9 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[8].Value,
                        X10 = 10 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[9].Value,
                        X11 = 11 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[10].Value,
                        X12 = 12 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[11].Value,
                        X13 = 13 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[12].Value,
                        X14 = 14 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[13].Value,
                        X15 = 15 <= cnt && plvm.ProductLineProperetyViewModels.ToArray()[14].Value,

                        New = plvm.NewItem,
                        Edited = plvm.EditedItem
                        
                    };
                    dumbData.Add(d);
                }
            }
            catch (Exception)
            {
                throw new DataException();
            }
            return dumbData;
        }

     
        // todo Fix Q&D 
        public class Dumb
        {
            public string Name { get; set; }
            public bool X1 { get; set; }
            public bool X2 { get; set; }
            public bool X3 { get; set; }
            public bool X4 { get; set; }
            public bool X5 { get; set; }
            public bool X6 { get; set; }
            public bool X7 { get; set; }
            public bool X8 { get; set; }
            public bool X9 { get; set; }
            public bool X10 { get; set; }
            public bool X11 { get; set; }
            public bool X12 { get; set; }
            public bool X13 { get; set; }
            public bool X14 { get; set; }
            public bool X15 { get; set; }

            public bool New { get; set; }
            public bool Edited { get; set; }
        }

        public class ProductLineStyle : StyleSelector
        {
            private readonly SolidColorBrush _itemColorBrush = new SolidColorBrush(Color.FromArgb(153, 144, 238, 144));
            private readonly SolidColorBrush _newItemColorBrush = new SolidColorBrush(Color.FromArgb(153,238, 144, 144)); 
            private readonly SolidColorBrush _editItemColorBrush = new SolidColorBrush(Color.FromArgb(153, 255, 255, 144));
            public override Style SelectStyle(object item, DependencyObject container)
            {
                if (item is Dumb)
                {
                    var dumb = item as Dumb;
                    var style = new Style(typeof (GridViewRow));

                    if (dumb.New)
                    {
                        style.Setters.Add(new Setter(BackgroundProperty, _newItemColorBrush));
                        return style;
                    }
                    if (dumb.Edited)
                    {
                        style.Setters.Add(new Setter(BackgroundProperty, _editItemColorBrush));
                        return style;
                    }
                    style.Setters.Add(new Setter(BackgroundProperty, _itemColorBrush));
                    return style;
                }
                return null;
            }
        }

    }
}