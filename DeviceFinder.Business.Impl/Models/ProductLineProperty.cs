using System.Text;
using DeviceFinder.Business.Interface.Models;

namespace DeviceFinder.Business.Impl.Models
{
    public class ProductLineProperty : IProductLineProperty
    {
        public int Id { get; internal set; }

        public string Name { get; internal set; }

        public int GroupId { get; internal set; }
        
        public string Group { get; internal set; }

        public bool Value { get; internal set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Id: " + Id);
            sb.Append(", Name: " + Name);
            sb.Append(", GroupId: " + GroupId);
            sb.Append(", Group: " + Group);
            sb.Append(", Value: " + Value);
            return sb.ToString();
        }
    }
}