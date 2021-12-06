using System;

namespace IAUAdmin.DTO.Entity
{
    public class EformApprovalDTON
    {
        public string AR { get; set; }
        public string EN { get; set; }
        public int UnitID { get; set; }
        public bool OwnEform { get; set; }
        public Nullable<DateTime> SignDate { get; set; }
    }
}