using System.Text;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Entity
{
    public partial class CommonTranslateSizingWordIdVo : ICommonTranslateSizingWordIdVo
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("WordId: " + WordId);
            return sb.ToString();
        }
    }
}