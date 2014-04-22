using System;
using System.Collections.Generic;

namespace DeviceFinder.ViewModel.Models
{
    public class ProductLineViewModel
    {
        public string Name { get; internal set; }

        public IEnumerable<ProductLineProperetyViewModel> ProductLineProperetyViewModels { get; internal set; }

        public bool NewItem { get; set; }

        public bool EditedItem { get; set; }

        //public bool Coriolis { get; internal set; }
        //public bool Rotameter { get; internal set; }
        //public bool Magnetic { get; internal set; }
        //public bool Ultrasonic { get; internal set; }
        //public bool Vortex { get; internal set; }
        //public bool Pressure { get; internal set; }
        //public bool Liquide { get; internal set; }
        //public bool Gas { get; internal set; }
        //public bool Steam { get; internal set; }
    }
}