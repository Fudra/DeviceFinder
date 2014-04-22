using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DeviceFinder
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            StartupUri = new Uri(@"pack://application:,,,/DeviceFinder.View;component/Views/DeviceFinderWindow.xaml", UriKind.Absolute);
        }
    }
}
