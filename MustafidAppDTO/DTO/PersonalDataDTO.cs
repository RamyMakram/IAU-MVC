using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustafidAppDTO.DTO
{
    public class PersonalDataDTO
    {
        public int PD_ID { get; set; }
        public string PD_IAUNumber { get; set; }
        public int? PD_APP_ID { get; set; }
        public int? PD_TitleNames_ID { get; set; }
        public string PD_F_Name { get; set; }
        public string PD_M_Name { get; set; }
        public string PD_L_Name { get; set; }
        public int? PD_National_ID { get; set; }
        public int PD_ID_Doc_ID { get; set; }
        public string PD_ID_Number { get; set; }
        public int? PD_C_ID { get; set; }//Country ID
        public int PD_Address_C_ID { get; set; }
        public int? PD_Address_City_ID { get; set; }
        public int? PD_Adress_R_ID { get; set; }
        public string PD_Address_City { get; set; }
        public string PD_Adress_Region { get; set; }
        public string PD_Postal { get; set; }
        public string PD_mail { get; set; }
        public string PD_Phone { get; set; }
    }
}
