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
    
    public partial class Personel_Data
    {
        public int Personel_Data_ID { get; set; }
        public int ID_Document { get; set; }
        public string ID_Number { get; set; }
        public Nullable<int> IAU_Affiliate_ID { get; set; }
        public string IAU_ID_Number { get; set; }
        public Nullable<int> Applicant_Type_ID { get; set; }
        public Nullable<int> Title_Middle_Names_ID { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public Nullable<int> Nationality_ID { get; set; }
        public Nullable<int> Country_ID { get; set; }
        public string City_Country_1 { get; set; }
        public string City_Country_2 { get; set; }
        public string Region_Postal_Code_1 { get; set; }
        public string Region_Postal_Code_2 { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string IS_Action { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual ID_Document ID_Document1 { get; set; }
        public virtual Nationality Nationality { get; set; }
        public virtual Title_Middle_Names Title_Middle_Names { get; set; }
    }
}
