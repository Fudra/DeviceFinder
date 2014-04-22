using System.Text;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Entity
{
    public partial class CommonLanguageVo : ICommonLanguageVo
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("LangId: " + LangId);
            sb.AppendLine("Name: " + Name);
            return sb.ToString();
        }
    }
}