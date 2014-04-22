using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Vo
{
    public class Cell: ICell
    {
        public int X { get; internal set; }

        public int Y { get; internal set; }
    }
}