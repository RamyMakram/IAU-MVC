using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class UnitsLocDTO
	{
        public int Units_Location_ID { get; set; }
        public string Units_Location_Name_AR { get; set; }
        public string Units_Location_Name_EN { get; set; }
        public string Name { get; set; }
        public Nullable<int> Location_ID { get; set; }
        public Nullable<bool> IS_Action { get; set; }

        public virtual LocationsDTO Location { get; set; }
        //public virtual ICollection<Units> Units { get; set; }
    }
}
