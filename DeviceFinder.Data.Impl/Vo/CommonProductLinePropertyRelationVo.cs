using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Vo
{
    public class CommonProductLinePropertyRelationVo : ICommonProductLinePropertyRelationVo
    {
        public int ProductLineId { get; internal set; }
        public int PropertyId { get; internal set; }
    }
}