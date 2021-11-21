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
    
    public partial class Units
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Units()
        {
            this.Eform_Approval = new HashSet<Eform_Approval>();
            this.Request_Data = new HashSet<Request_Data>();
            this.RequestTransaction = new HashSet<RequestTransaction>();
            this.RequestTransaction1 = new HashSet<RequestTransaction>();
            this.UnitMainServices = new HashSet<UnitMainServices>();
            this.Units_Request_Type = new HashSet<Units_Request_Type>();
            this.Units1 = new HashSet<Units>();
            this.UnitServiceTypes = new HashSet<UnitServiceTypes>();
        }
    
        public int Units_ID { get; set; }
        public string Units_Name_AR { get; set; }
        public string Units_Name_EN { get; set; }
        public Nullable<int> Units_Location_ID { get; set; }
        public Nullable<int> Units_Type_ID { get; set; }
        public string Ref_Number { get; set; }
        public string Building_Number { get; set; }
        public Nullable<int> LevelID { get; set; }
        public Nullable<int> SubID { get; set; }
        public Nullable<int> ServiceTypeID { get; set; }
        public string Code { get; set; }
        public Nullable<bool> IS_Action { get; set; }
        public bool IS_Mostafid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Eform_Approval> Eform_Approval { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request_Data> Request_Data { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestTransaction> RequestTransaction { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestTransaction> RequestTransaction1 { get; set; }
        public virtual Service_Type Service_Type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitMainServices> UnitMainServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Units_Request_Type> Units_Request_Type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Units> Units1 { get; set; }
        public virtual Units Units2 { get; set; }
        public virtual Units_Type Units_Type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitServiceTypes> UnitServiceTypes { get; set; }
    }
}
