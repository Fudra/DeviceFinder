using DeviceFinder.Business.Interface.Models;

namespace DeviceFinder.Business.Impl.Models
{
    public class PhaseType : IPhaseType
    {
        public bool Liquide { get; internal set; }

        public bool Gas { get; internal set; }

        public bool Steam { get; internal set; }

        public bool Visible { get; internal set; }
    }
}