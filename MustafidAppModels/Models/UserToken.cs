using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class UserToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string RefToken { get; set; }
        public bool Expired { get; set; }
        public string Phone { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
