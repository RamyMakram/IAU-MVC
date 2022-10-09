using System.Collections.Generic;

namespace TasahelAdmin.Models.Enum
{
    public class ListOFSubDomains
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public static List<ListOFSubDomains> SubDomainsEnum = new List<ListOFSubDomains>
        {
            new ListOFSubDomains{Name="Frontend AdminPanel Path",Value="FE_Admin"},
            new ListOFSubDomains{Name="Frontend Mostafid Path",Value="FE_Mos"},
            new ListOFSubDomains{Name="Backend AdminPanel Path",Value="BE_Admin"},
            new ListOFSubDomains{Name="Backend Mostafid Path",Value="BE_Mos"}
        };

    }
}
