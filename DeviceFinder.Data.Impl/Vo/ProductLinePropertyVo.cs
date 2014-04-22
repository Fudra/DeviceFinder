using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Vo
{
    public class ProductLinePropertyVo : IProductLinePropertyVo
    {
        public string Group { get; internal set; }
        public string Name { get; internal set; }
        public bool Value { get; internal set; }
    }
}