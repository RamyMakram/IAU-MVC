﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace MustafidApp.Helpers.SaveRequest
{
    public class SaveReq_Preview_TableCols
    {
        public int ID { get; set; }
        public int EFormAnswareID { get; set; }
        public string Name { get; set; }
        public string Name_En { get; set; }
        public virtual ICollection<SaveReq_Tables_Answare> Tables_Answare { get; set; }
    }
}