using System.Collections.Generic;

namespace DeviceFinder.Business.Interface.Models
{
    public interface ITranslation
    {
        int Id { get; }

        string Language { get; }

        // key, translation
        IDictionary<string, string> Dictionary { get; } 
    }
}