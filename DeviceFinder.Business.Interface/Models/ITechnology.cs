namespace DeviceFinder.Business.Interface.Models
{
    public interface ITechnology
    {
        bool Coriolis { get; }

        bool Rotameter { get; }

        bool Magnetic { get; }

        bool Ultrasonic { get; }

        bool Vortex { get; }

        bool Pressure { get; } 

        bool Visibility { get; }
    }
}