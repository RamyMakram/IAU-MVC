using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustafidAppDTO.DTO
{
    public class PersonalDataDTO
    {
        public int PD_ID { get; set; }
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
		public virtual ICollection<EformAnsDTO> PD_EFormAnswer { get; set; }

    }
}
