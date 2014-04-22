using DeviceFinder.Business.Interface.Models;

namespace DeviceFinder.Business.Impl.Models
{
    public class Model :IModel
    {
        public string Matnr { get; internal set; }
        public bool Coriolis { get; internal set; }
        public bool Rotameter { get; internal set; }
        public bool Magnetic { get; internal set; }
        public bool Ultrasonic { get; internal set; }
        public bool Vortex { get; internal set; }
        public bool Pressure { get; internal set; }
        public bool Liquide { get; internal set; }
        public bool Gas { get; internal set; }
        public bool Steam { get; internal set; }
    }
}