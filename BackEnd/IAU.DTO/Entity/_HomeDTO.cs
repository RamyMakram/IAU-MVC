using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAU.DTO.Entity
{
	public class _HomeDTO
	{
		public ICollection<ServiceTypeDTO> Service_Type { get; set; }
		public ICollection<RequestTypeDTO> Request_Type { get; set; }
		public ICollection<TitlesDTO> Titles { get; set; }
		public ICollection<CountryDTO> Country { get; set; }
		public ICollection<RegionDTO> Regions { get; set; }
		public ICollection<CityDTO> City { get; set; }
		public ICollection<IDDocDTO> IDS { get; set; }
	}
}
