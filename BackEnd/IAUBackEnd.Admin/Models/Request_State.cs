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
    
    public partial class Request_State
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Request_State()
        {
            this.Request_Data = new HashSet<Request_Data>();
            this.Request_Log = new HashSet<Request_Log>();
        }
    
        public byte State_ID { get; set; }
        public string StateName_AR { get; set; }
        public string StateName_EN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request_Data> Request_Data { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request_Log> Request_Log { get; set; }
    }
}
