using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class UserFcmtoken
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Fcmtoken { get; set; }
        public string Type { get; set; }
    }
}
