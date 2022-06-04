﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IAUAdmin.DTO.Entity
{
    public class E_FormsDTO
    {
        public int ID { get; set; }
        public int UnitToApprove { get; set; }
        public string Name { get; set; }
        public string Name_EN { get; set; }
        public int SubServiceID { get; set; }
        public bool IS_Action { get; set; }
        public string QTY { get; set; }
        public string Del_QTY { get; set; }
        public string Code { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public virtual SubServicesDTO Sub_Services { get; set; }
        public virtual ICollection<Question> Question { get; set; }
        public virtual UnitsDTO Eform_Approval { get; set; }
        public string Approval { get; set; }


    }
}
