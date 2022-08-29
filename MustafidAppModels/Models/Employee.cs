using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class Employee
    {
        public Employee()
        {
            RequestLogs = new HashSet<RequestLog>();
        }

        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual ICollection<RequestLog> RequestLogs { get; set; }
    }
}
