using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
    public class LogDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Method { get; set; }
        public System.DateTime TransDate { get; set; }
        public int ClassType { get; set; }
        public string Oldval { get; set; }
        public string Newval { get; set; }
        public string CallPath { get; set; }
        public Nullable<int> ReferID { get; set; }
        public string Notes { get; set; }

        public virtual UserDTO Users { get; set; }
        public ApplicantTypeDTO ApplicantType { get; set; }
        public E_FormsDTO E_Forms { get; set; }
        public JobDTO Job { get; set; }
        public LocationsDTO Location { get; set; }
        public MainServiceDTO Mainservice { get; set; }
        public PrivilgesDTO Privilges { get; set; }
        public RequestTypeDTO RequestType { get; set; }
        public ReqestDTO Request { get; set; }
        public ServiceTypeDTO ServiceType { get; set; }
        public SubServicesDTO SubServices { get; set; }
        public UnitLevelDTO UnitLevel { get; set; }
        public UnitsDTO Unit { get; set; }
        public UnitsLocDTO UnitsLocation { get; set; }
        public UnitTypeDTO UnitType { get; set; }
        public UserDTO User { get; set; }
        public Unit_Signature Unit_Signature { get; set; }
        public PersonEfDTO Ef { get; set; }
    }
}
