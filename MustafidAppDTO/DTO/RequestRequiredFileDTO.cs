using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustafidAppDTO.DTO
{
    public class RequestRequiredFileDTO
    {
        public int RD_ID { get; set; }
        public IFormFile File { get; set; }
    }
}
