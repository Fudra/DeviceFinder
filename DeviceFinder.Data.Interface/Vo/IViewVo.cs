namespace DeviceFinder.Data.Interface.Vo
{
    public interface IViewVo
    {
        string FieldName { get; }

        int? OrderId { get; }

        bool Visible { get; }
    }
}