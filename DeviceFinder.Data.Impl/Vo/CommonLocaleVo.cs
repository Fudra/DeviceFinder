using System.Text;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Entity
{
    public partial class CommonLocaleVo : ICommonLocaleVo
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("LocaleId: " + LocaleId);
            sb.AppendLine("LanguageId: " + LanguageId);
            sb.AppendLine("LocaleCode: " + LocaleCode);
            return sb.ToString();
        }
    }
}