using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Vo
{
    internal class TechnologyVo : ITechnologyVo
    {
        public bool Coriolis { get; internal set; }

        public bool Rotameter { get; internal set; }

        public bool Magnetic { get; internal set; }

        public bool Ultrasonic { get; internal set; }

        public bool Vortex { get; internal set; }

        public bool Pressure { get; internal set; }
    }
}