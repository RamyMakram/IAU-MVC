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
    
    public partial class Request_SupportingDocs
    {
        public int ID { get; set; }
        public int RequestID { get; set; }
        public int SupportingDocID { get; set; }
        public string Path { get; set; }
    
        public virtual Request_Data Request_Data { get; set; }
        public virtual Required_Documents Required_Documents { get; set; }
    }
}
