//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IAU.DTO.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Input_Type
    {
        public int? ID { get; set; }
        public int Question_ID { get; set; }
        public bool ISNum { get; set; }
        public bool Date { get; set; }
        public string PlaceHolder { get; set; }
        public string PlaceholderEN { get; set; }
    
        public virtual Question Question { get; set; }
    }
}
