﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
    public class PersonEfDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Name_EN { get; set; }
        public int? Person_ID { get; set; }
        public System.DateTime FillDate { get; set; }

        public virtual List<EformApprovalDTON> Eform_Approval { get; set; }
        public virtual List<E_Forms_Answer> E_Forms_Answer { get; set; }
        //public virtual PersonalDataDTO Personel_Data { get; set; }
    }
}