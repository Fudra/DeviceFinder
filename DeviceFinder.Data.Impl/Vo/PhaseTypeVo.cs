using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Vo
{
    internal class PhaseTypeVo : IPhaseTypeVo
    {
        public bool Liquide { get; internal set; }

        public bool Gas { get; internal set; }

        public bool Steam { get; internal set; }
    }
}