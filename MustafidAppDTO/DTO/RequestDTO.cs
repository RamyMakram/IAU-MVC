using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Req_Code { get; set; }
        public string Req_Code_Nammed
        {
            get
            {
                return Req_Code ?? "لم يتم التخصيص";
            }
        }
        public DateTime? Req_Current_DateEnd { get; set; }
        public DateTime? Req_Current_DateStart { get; set; }
        public DateTime? Req_Added_Date { get; set; }
        public UnitsDTO Req_Current_Unit { get; set; }
        public UnitsDTO Req_Subject { get; set; }

        #region ApplicantData
        public string PD_IAUNumber { get; set; }
        [Required]
        public int? PD_APP_ID { get; set; }
        [Required]
        public int? PD_TitleNames_ID { get; set; }
        [Required]
        [MinLength(2)]
        public string PD_F_Name { get; set; }
        [Required]
        [MinLength(2)]
        public string PD_M_Name { get; set; }
        [Required]
        [MinLength(2)]
        public string PD_L_Name { get; set; }
        [Required]
        public int? PD_National_ID { get; set; }
        [Required]
        public int PD_ID_Doc_ID { get; set; }
        [Required]
        [MinLength(10)]
        public string PD_ID_Number { get; set; }
        [Required]
        public int? PD_C_ID { get; set; }//Country ID
        [Required]
        public int PD_Address_C_ID { get; set; }
        public int? PD_Address_City_ID { get; set; }
        public int? PD_Adress_R_ID { get; set; }
        public string PD_Address_City { get; set; }
        public string PD_Adress_Region { get; set; }
        public string PD_Postal { get; set; }
        [Required]
        public string PD_mail { get; set; }
        //[Required]
        //[MinLength(12)]
        //[MaxLength(12)]
        public string PD_Phone { get; set; }
        #endregion
        public PersonalDataDTO Req_ApplicantData { get; set; }
        //public IList<IFormFile> Req_RequiredDocs { get; set; }
        //public IList<IFormFile> Req_Files { get; set; }
        public string EformID { get; set; }
    }
}
