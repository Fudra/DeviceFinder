using System.Collections.Generic;
using DeviceFinder.Business.Interface.Models;

namespace DeviceFinder.Business.Impl.Models
{
    public class Translation : ITranslation
    {
        public int Id { get; internal set; }
        public string Language { get; internal set; }
        public IDictionary<string, string> Dictionary { get; internal set; }
    }
}