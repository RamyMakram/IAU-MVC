using System;
using System.Collections.Generic;

#nullable disable

namespace TasahelAdmin.Models
{
    public partial class DomainEmail
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
        public string Name { get; set; }
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MessageAppSid { get; set; }
        public string Sender { get; set; }
        public bool UseMessages { get; set; }

        public virtual Domain Domain { get; set; }
    }
}
