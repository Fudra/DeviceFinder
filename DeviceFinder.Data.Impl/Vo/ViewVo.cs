using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Vo
{
    internal class ViewVo :IViewVo
    {
        public string FieldName { get; internal set; }
        public int? OrderId { get; internal  set; }
        public bool Visible { get; internal set; }
    }
}