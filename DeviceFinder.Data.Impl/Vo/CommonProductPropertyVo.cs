using System.Text;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Entity
{
    public partial class CommonProductPropertyVo : ICommonProductPropertyVo
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("ProductPropertyId: " + ProductPropertyId);
            sb.AppendLine("ProductPropertyNameId: " + ProductPropertyNameId);
            sb.AppendLine("ProductPropertyGroupId: " + ProductPropertyGroupId);
            sb.AppendLine("DisplaySequence: " + DisplaySequence);
            return sb.ToString();
        }
    }
}