//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IAU_BackEnd.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sub_Services
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sub_Services()
        {
            this.Request_Data = new HashSet<Request_Data>();
            this.Required_Documents = new HashSet<Required_Documents>();
        }
    
        public int Sub_Services_ID { get; set; }
        public string Sub_Services_Name_EN { get; set; }
        public string Sub_Services_Name_AR { get; set; }
        public Nullable<int> Main_Services_ID { get; set; }
        public Nullable<bool> IS_Action { get; set; }
    
        public virtual Main_Services Main_Services { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request_Data> Request_Data { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Required_Documents> Required_Documents { get; set; }
    }
}
