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
    
    public partial class Units_Request_Type
    {
        public int Units_Request_Type_ID { get; set; }
        public Nullable<int> Units_ID { get; set; }
        public Nullable<int> Request_Type_ID { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }
    
        public virtual Request_Type Request_Type { get; set; }
        public virtual Units Units { get; set; }
    }
}
