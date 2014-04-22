namespace DeviceFinder.Business.Interface.Models
{
    public interface IProductLineProperty
    {
        
        string Group { get; }

        int GroupId { get; }

        string Name { get; }

        int Id { get; }

        bool Value { get; } 
    }
}