using System.Text;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Entity
{
    public partial class CommonProductFamilyVo : ICommonProductFamilyVo
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("ProductFamilyId: " + ProductFamilyId);
            sb.AppendLine("ProductFamilyName: " + ProductFamilyName);
            sb.AppendLine("DisplaySequence: " + DisplaySequence);
            return sb.ToString();
        }
    }
}