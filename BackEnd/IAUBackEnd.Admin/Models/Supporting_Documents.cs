//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IAUBackEnd.Admin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Supporting_Documents
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supporting_Documents()
        {
            this.Request_SupportingDocs = new HashSet<Request_SupportingDocs>();
        }
    
        public int Supporting_Documents_ID { get; set; }
        public string Supporting_Documents_Name_EN { get; set; }
        public string Supporting_Documents_Name_AR { get; set; }
        public Nullable<bool> IS_Action { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request_SupportingDocs> Request_SupportingDocs { get; set; }
    }
}
