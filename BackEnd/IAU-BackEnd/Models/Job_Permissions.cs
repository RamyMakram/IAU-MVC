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
    
    public partial class Job_Permissions
    {
        public int ID { get; set; }
        public Nullable<int> PrivilageID { get; set; }
        public Nullable<int> Job_ID { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }
    
        public virtual Job Job { get; set; }
        public virtual Privilage Privilage { get; set; }
    }
}
