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
    
    public partial class Service_Type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service_Type()
        {
            this.Main_Services = new HashSet<Main_Services>();
            this.Request_Data = new HashSet<Request_Data>();
            this.Units = new HashSet<Units>();
            this.UnitServiceTypes = new HashSet<UnitServiceTypes>();
        }
    
        public int Service_Type_ID { get; set; }
        public string Service_Type_Name_EN { get; set; }
        public string Service_Type_Name_AR { get; set; }
        public Nullable<bool> IS_Action { get; set; }
        public string Desc_AR { get; set; }
        public string Desc_EN { get; set; }
        public string Image_Path { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Main_Services> Main_Services { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request_Data> Request_Data { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Units> Units { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitServiceTypes> UnitServiceTypes { get; set; }
    }
}
