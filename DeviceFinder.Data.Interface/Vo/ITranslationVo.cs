using System.Collections.Generic;

namespace DeviceFinder.Data.Interface.Vo
{
    public interface ITranslationVo
    {
          int Id{ get;  }

         string Language { get; }

                 // key, translation
        IDictionary<string,string> Dictionary { get;  }
    }
}