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
    
    public partial class Required_Documents
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Required_Documents()
        {
            this.Request_File = new HashSet<Request_File>();
        }
    
        public int? ID { get; set; }
        public string Name_EN { get; set; }
        public string Name_AR { get; set; }
        public Nullable<int> SubServiceID { get; set; }
        public bool IS_Action { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DeletetAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request_File> Request_File { get; set; }
        public virtual Sub_Services Sub_Services { get; set; }
    }
}
