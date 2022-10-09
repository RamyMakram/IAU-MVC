using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasahelAdmin.Models.VM
{
    public class DomainStyleVM : DomainStyle
    {
        [NotMapped]
        public IFormFile _Icon { get; set; }
        [NotMapped]
        public IFormFile _FavIcon { get; set; }

    }
}
