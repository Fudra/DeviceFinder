using System.Text;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Entity
{
    public partial class CommonTranslateSizingWordVo : ICommonTranslateSizingWordVo
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("LangId: " + LangId);
            sb.AppendLine("WordId: " + WordId);
            sb.AppendLine("Word: " + Word);
            return sb.ToString();
        }
    }
}