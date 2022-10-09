using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasahelAdmin.Models.VM
{
    public class HomeVM : TasahelHomeSetting
    {
        [NotMapped]
        public IFormFile _NewReqICo { get; set; }

        [NotMapped]
        public IFormFile _FollowIco { get; set; }

    }
}
