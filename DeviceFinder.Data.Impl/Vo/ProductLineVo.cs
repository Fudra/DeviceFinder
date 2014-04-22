using System.Collections.Generic;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Vo
{
    internal class ProductLineVo : IProductLineVo
    {
        public string Name { get; internal set; }

        //public ITechnologyVo Technology { get; internal set; }

        //public IPhaseTypeVo PhaseType { get; internal set; }

        // new
        public IEnumerable<IProductLinePropertyVo> ProductLineProperties { get; internal set; }
    }
}