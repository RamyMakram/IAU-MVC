using System.Collections.Generic;

namespace IAU.DTO.Entity
{
	public class RegionDTO
	{
		public int Region_ID { get; set; }
		public string Region_Name_AR { get; set; }
		public string Region_Name_EN { get; set; }
        public List<CityDTO> Cities { get; set; }
    }
}