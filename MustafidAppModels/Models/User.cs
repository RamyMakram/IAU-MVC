using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class User
    {
        public User()
        {
            PhoneNumberNotifications = new HashSet<PhoneNumberNotification>();
            SystemLogs = new HashSet<SystemLog>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserMobile { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public int? JobId { get; set; }
        public string IsActive { get; set; }
        public string TempLogin { get; set; }
        public int? UnitId { get; set; }
        public DateTime? LoginDate { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Job Job { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<PhoneNumberNotification> PhoneNumberNotifications { get; set; }
        public virtual ICollection<SystemLog> SystemLogs { get; set; }
    }
}
