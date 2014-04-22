using System.Collections.Generic;

namespace DeviceFinder.Data.Interface.Vo
{
    public interface IProductLineVo
    {
        string Name { get; }

        //old
        //ITechnologyVo Technology { get; }

        //IPhaseTypeVo PhaseType { get; }

        // new
        IEnumerable<IProductLinePropertyVo> ProductLineProperties { get; }
    }
}