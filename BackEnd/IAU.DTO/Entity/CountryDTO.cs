using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAU.DTO.Entity
{
	public class CountryDTO
	{
		public int Country_ID { get; set; }
		public string Country_Name_EN { get; set; }
		public string Country_Name_AR { get; set; }

        public List<RegionDTO> Regions { get; set; }
    }
}
