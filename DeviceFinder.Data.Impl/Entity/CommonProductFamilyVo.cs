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
    
    public partial class CommonProductFamilyVo
    {
        public CommonProductFamilyVo()
        {
            this.Common_ProductLine = new HashSet<CommonProductLineVo>();
            this.Common_ProductProperty = new HashSet<CommonProductPropertyVo>();
        }
    
        public int ProductFamilyId { get; set; }
        public int ProductFamilyName { get; set; }
        public int DisplaySequence { get; set; }
    
        public virtual CommonTranslateSizingWordIdVo Common_TranslateSizingWordId { get; set; }
        public virtual ICollection<CommonProductLineVo> Common_ProductLine { get; set; }
        public virtual ICollection<CommonProductPropertyVo> Common_ProductProperty { get; set; }
    }
}
