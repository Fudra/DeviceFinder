namespace DeviceFinder.Business.Interface.Models
{
    public interface IView
    {
        string FieldName { get; }

        int? OrderId { get; }

        bool Visible { get; } 
    }
}