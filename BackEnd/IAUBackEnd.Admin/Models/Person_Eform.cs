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
    
    public partial class Person_Eform
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Person_Eform()
        {
            this.E_Forms_Answer = new HashSet<E_Forms_Answer>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Name_EN { get; set; }
        public Nullable<int> Person_ID { get; set; }
        public System.DateTime FillDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<E_Forms_Answer> E_Forms_Answer { get; set; }
        public virtual Personel_Data Personel_Data { get; set; }
    }
}
