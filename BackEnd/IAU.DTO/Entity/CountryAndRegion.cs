using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAU.DTO.Entity
{
	public class CountryAndRegion
	{
		public ICollection<RegionDTO> Regions { get; set; }
		public ICollection<CityDTO> City { get; set; }
	}
}
