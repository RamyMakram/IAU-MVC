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
    
    public partial class Provider_Academic_Services
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Provider_Academic_Services()
        {
            this.Main_Services = new HashSet<Main_Services>();
            this.Request_Data = new HashSet<Request_Data>();
        }
    
        public int Provider_Academic_Services_ID { get; set; }
        public string Provider_Academic_Services_Name_EN { get; set; }
        public string Provider_Academic_Services_Name_AR { get; set; }
        public Nullable<bool> IS_Action { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Main_Services> Main_Services { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request_Data> Request_Data { get; set; }
    }
}
