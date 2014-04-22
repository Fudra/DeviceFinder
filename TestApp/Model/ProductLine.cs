using System.Collections.Generic;
using System.Text;

namespace TestApp.Model
{
    public class ProductLine
    {
        public string Name { get; set; }

        public IEnumerable<ProductLineProperty> ProductLineProperties { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Name: " + Name.ToString());

            foreach (var property in ProductLineProperties)
            {
                sb.AppendLine("\t" +property.Group + ", " + property.Name + ", " + property.Value);
            }

            return sb.ToString();
        }
    }
}