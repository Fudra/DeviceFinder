using DeviceFinder.Business.Interface.Models;

namespace DeviceFinder.Business.Impl.Models
{
    public class View : IView
    {
        public string FieldName { get; internal set; }

        public int? OrderId { get; internal set; }
        
        public bool Visible { get; internal set; }
    }
}