using System.Text;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Entity
{
    public partial class CommonProductLineVo : ICommonProductLineVo
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("ProductLineId: " + ProductLineId);
            sb.AppendLine("ProductLineName: " + ProductLineName);
            sb.AppendLine("DisplaySequence: " + DisplaySequence);
            return sb.ToString();
        }
    }
}