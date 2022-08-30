using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustafidAppDTO.DTO
{
    public class RegionDTO
    {
        public int R_ID { get; set; }
        public string R_Name { get; set; }
        public string R_Name_EN { get; set; }

        public IEnumerable<CityDTO> R_Cities { get; set; }
    }
}
