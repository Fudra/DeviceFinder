//------------------------------------------------------------------------------
// <auto-generated>
//    Dieser Code wurde aus einer Vorlage generiert.
//
//    Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten Ihrer Anwendung.
//    Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeviceFinder.Data.Impl.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class CommonLocaleVo
    {
        public int LocaleId { get; set; }
        public int LanguageId { get; set; }
        public string LocaleCode { get; set; }
    
        public virtual CommonLanguageVo Common_Language { get; set; }
    }
}
