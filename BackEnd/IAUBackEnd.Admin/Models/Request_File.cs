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
    
    public partial class Request_File
    {
        public int ID { get; set; }
        public string File_Name { get; set; }
        public string File_Path { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int Request_ID { get; set; }
    
        public virtual Request_Data Request_Data { get; set; }
    }
}
