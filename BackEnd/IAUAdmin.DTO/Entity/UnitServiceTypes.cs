using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class UnitServiceTypesDTO
	{
        public int ID { get; set; }
        public int ServiceTypeID { get; set; }
        public int UnitID { get; set; }

        public virtual ServiceTypeDTO Service_Type { get; set; }
        public virtual UnitsDTO Units { get; set; }
    }
}
