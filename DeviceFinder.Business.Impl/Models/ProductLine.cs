using System.Collections.Generic;
using System.Text;
using DeviceFinder.Business.Interface.Models;

namespace DeviceFinder.Business.Impl.Models
{
    public class ProductLine : IProductLine
    {
        public int Id { get; internal set; }
        
        public string Name { get; internal set; }

        public IEnumerable<IProductLineProperty> ProductLineProperties { get; internal set; }

        //public IPhaseType PhaseType { get; set; }

        //public ITechnology Technology { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id: " + Id);
            sb.AppendLine("Name: " + Name);
            foreach (var property in ProductLineProperties)
            {
                sb.AppendLine(property.ToString());
            }
            return sb.ToString();
        }
    }
}