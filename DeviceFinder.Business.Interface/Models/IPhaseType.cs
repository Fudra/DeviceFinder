namespace DeviceFinder.Business.Interface.Models
{
    public interface IPhaseType
    {
        bool Liquide { get; }

        bool Gas { get; }

        bool Steam { get; } 

        bool Visible { get; }
    }
}