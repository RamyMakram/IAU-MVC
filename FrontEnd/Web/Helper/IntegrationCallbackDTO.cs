using IAU.DTO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Helper
{
    public enum IntegrationTypeEnum
    {
        Nafath,
        Mustafeed
    }
    public class IntegrationCallbackDTO
    {
        public IntegrationTypeEnum IntegrationType { get; set; }
        public PersonalDataDTO Person { get; set; }
    }
}
