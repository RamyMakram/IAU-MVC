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
    
    public partial class Event_Publication_Violation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Event_Publication_Violation()
        {
            this.Purchase_Order_Form = new HashSet<Purchase_Order_Form>();
        }
    
        public int Event_Publication_Violation_ID { get; set; }
        public string Event_Publication_Violation_Name_AR { get; set; }
        public string Event_Publication_Violation_Name_EN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchase_Order_Form> Purchase_Order_Form { get; set; }
    }
}
