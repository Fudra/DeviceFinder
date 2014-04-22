using System.Text;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Entity
{
    public partial class CommonProductPropertyGroupVo : ICommonProductPropertyGroupVo
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("ProductPropertyGroupId: " + ProductPropertyGroupId);
            sb.AppendLine("ProductPropertyGroupNameId: " + ProductPropertyGroupNameId);
            sb.AppendLine("DisplaySequence: " + DisplaySequence);
            return sb.ToString();
        }
    }
}