namespace DeviceFinder.Data.Interface.Vo
{
    public interface ITechnologyVo
    {
        bool Coriolis { get; }

        bool Rotameter { get; }

        bool Magnetic { get; }

        bool Ultrasonic { get; }

        bool Vortex { get; }

        bool Pressure { get; }
    }
}