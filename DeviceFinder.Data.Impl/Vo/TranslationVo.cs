using System.Collections.Generic;
using System.Text;
using DeviceFinder.Data.Interface.Vo;

namespace DeviceFinder.Data.Impl.Vo
{
    public class TranslationVo : ITranslationVo
    {
        public int Id{ get; internal set; }

        public string Language { get; internal set; }

                 // key, translation
        public IDictionary<string,string> Dictionary { get; internal set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("ID: " + Id);
            sb.AppendLine("Language: " + Language);

            foreach (var dict in Dictionary)
            {
                sb.AppendLine("Key: " + dict.Key + ", Value: " + dict.Value);
            }
            return sb.ToString();
        }
    }
}