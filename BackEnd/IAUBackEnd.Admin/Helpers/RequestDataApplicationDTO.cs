using IAUBackEnd.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAUBackEnd.Admin.Helpers
{
    public class RequestDataApplicationDTO
    {
        public int? Request_Data_ID { get; set; }
        public Nullable<int> Personel_Data_ID { get; set; }
        public Nullable<int> Unit_ID { get; set; }
        public Nullable<int> Sub_Services_ID { get; set; }
        public string Required_Fields_Notes { get; set; }
        public Nullable<int> Service_Type_ID { get; set; }
        public Nullable<int> Request_Type_ID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Code_Generate { get; set; }
        public string TempCode { get; set; }
        public byte Request_State_ID { get; set; }
        public Nullable<bool> IsTwasul_OC { get; set; }
        public Nullable<bool> Readed { get; set; }
        public Nullable<System.DateTime> ReadedDate { get; set; }
        public Nullable<System.DateTime> GenratedDate { get; set; }
        public bool Is_Archived { get; set; }

        public virtual ICollection<DelayedTransaction> DelayedTransaction { get; set; }
        public virtual ICollection<Person_Eform> Person_Eform { get; set; }
        public virtual Personel_Data Personel_Data { get; set; }
        public virtual Request_State Request_State { get; set; }
        public virtual Request_Type Request_Type { get; set; }
        public virtual Service_Type Service_Type { get; set; }
        public virtual Sub_Services Sub_Services { get; set; }
        public virtual Units Units { get; set; }
        public virtual ICollection<Request_File> Request_File { get; set; }
        public virtual ICollection<Request_Log> Request_Log { get; set; }
        public virtual ICollection<RequestTransaction> RequestTransaction { get; set; }
        public virtual ICollection<PhoneNumberNotification> PhoneNumberNotification { get; set; }
        public virtual ICollection<int> EformIDS { get; set; }
    }
}

