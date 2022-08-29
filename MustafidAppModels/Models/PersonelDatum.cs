using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class PersonelDatum
    {
        public PersonelDatum()
        {
            PersonEforms = new HashSet<PersonEform>();
            RequestData = new HashSet<RequestDatum>();
        }

        public int PersonelDataId { get; set; }
        public string IauIdNumber { get; set; }
        public int? ApplicantTypeId { get; set; }
        public int? TitleMiddleNamesId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int? NationalityId { get; set; }
        public int IdDocument { get; set; }
        public int? CountryId { get; set; }
        public string IdNumber { get; set; }
        public int AddressCountryId { get; set; }
        public int? AddressCityId { get; set; }
        public int? AdressRegionId { get; set; }
        public string AddressCity { get; set; }
        public string AdressRegion { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string IsAction { get; set; }

        public virtual City AddressCityNavigation { get; set; }
        public virtual Country AddressCountry { get; set; }
        public virtual Region AdressRegionNavigation { get; set; }
        public virtual ApplicantType ApplicantType { get; set; }
        public virtual Country Country { get; set; }
        public virtual IdDocument IdDocumentNavigation { get; set; }
        public virtual Country Nationality { get; set; }
        public virtual TitleMiddleName TitleMiddleNames { get; set; }
        public virtual ICollection<PersonEform> PersonEforms { get; set; }
        public virtual ICollection<RequestDatum> RequestData { get; set; }
    }
}
