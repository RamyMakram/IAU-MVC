using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class UnitMainServicesDTO
	{
        public int ID { get; set; }
        public int UnitID { get; set; }
        public int MainServiceID { get; set; }

        public virtual MainServiceDTO Main_Services { get; set; }
        public virtual UnitsDTO Units { get; set; }
    }
}
