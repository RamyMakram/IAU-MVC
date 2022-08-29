using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class ValidTo
    {
        public int Id { get; set; }
        public int MainServiceId { get; set; }
        public int ApplicantTypeId { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ApplicantType ApplicantType { get; set; }
        public virtual MainService MainService { get; set; }
    }
}
