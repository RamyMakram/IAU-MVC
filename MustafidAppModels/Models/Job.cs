using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class Job
    {
        public Job()
        {
            JobPermissions = new HashSet<JobPermission>();
            Users = new HashSet<User>();
        }

        public int UserPermissionsTypeId { get; set; }
        public string UserPermissionsTypeNameAr { get; set; }
        public string UserPermissionsTypeNameEn { get; set; }
        public bool IsModear { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<JobPermission> JobPermissions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
