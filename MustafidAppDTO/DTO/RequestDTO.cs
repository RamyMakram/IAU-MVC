﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustafidAppDTO.DTO
{
    public class RequestDTO
    {
        public int Req_ID { get; set; }
        public int? Req_U_ID { get; set; }//UnitID
        public int? Req_SS_ID { get; set; }//SubServive
        public string Req_Notes { get; set; }
        public int? Req_S_ID { get; set; }//ServiceType
        public int? Req_R_ID { get; set; }//RequestType
        public bool? Req_Is_Mos { get; set; }//Is Tawasel
        public int? Req_Status { get; set; }
        public DateTime? Req_Current_DateEnd { get; set; }
        public UnitsDTO Req_Current_Unit { get; set; }

        public PersonalDataDTO Req_ApplicantData { get; set; }
        //public List<RequestTransactionDTO> Req_Trans { get; set; }
    }
}