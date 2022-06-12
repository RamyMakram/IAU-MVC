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
    
    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            this.SystemLog = new HashSet<SystemLog>();
        }
    
        public int User_ID { get; set; }
        public string User_Name { get; set; }
        public string User_Mobile { get; set; }
        public string User_Email { get; set; }
        public string User_Password { get; set; }
        public Nullable<int> Job_ID { get; set; }
        public string IS_Active { get; set; }
        public string TEMP_Login { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<System.DateTime> LoginDate { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }
    
        public virtual Job Job { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SystemLog> SystemLog { get; set; }
        public virtual Units Units { get; set; }
    }
}
