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
    
    public partial class Radio_Type
    {
        public int ID { get; set; }
        public int Question_ID { get; set; }
        public string Name { get; set; }
        public string Name_EN { get; set; }
    
        public virtual Question Question { get; set; }
    }
}
