using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class Unit
    {
        public Unit()
        {
            DelayedTransactions = new HashSet<DelayedTransaction>();
            EForms = new HashSet<EForm>();
            InverseSub = new HashSet<Unit>();
            PreviewEformApprovals = new HashSet<PreviewEformApproval>();
            RequestData = new HashSet<RequestDatum>();
            RequestTransactionFromUnits = new HashSet<RequestTransaction>();
            RequestTransactionToUnits = new HashSet<RequestTransaction>();
            UnitMainServices = new HashSet<UnitMainService>();
            UnitServiceTypes = new HashSet<UnitServiceType>();
            UnitsRequestTypes = new HashSet<UnitsRequestType>();
            Users = new HashSet<User>();
        }

        public int UnitsId { get; set; }
        public string UnitsNameAr { get; set; }
        public string UnitsNameEn { get; set; }
        public int? UnitsLocationId { get; set; }
        public int? UnitsTypeId { get; set; }
        public string RefNumber { get; set; }
        public string BuildingNumber { get; set; }
        public int? LevelId { get; set; }
        public int? SubId { get; set; }
        public int? ServiceTypeId { get; set; }
        public string Code { get; set; }
        public bool? IsAction { get; set; }
        public bool? IsMostafid { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual UnitLevel Level { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual Unit Sub { get; set; }
        public virtual UnitsLocation UnitsLocation { get; set; }
        public virtual UnitsType UnitsType { get; set; }
        public virtual UnitSignature UnitSignature { get; set; }
        public virtual ICollection<DelayedTransaction> DelayedTransactions { get; set; }
        public virtual ICollection<EForm> EForms { get; set; }
        public virtual ICollection<Unit> InverseSub { get; set; }
        public virtual ICollection<PreviewEformApproval> PreviewEformApprovals { get; set; }
        public virtual ICollection<RequestDatum> RequestData { get; set; }
        public virtual ICollection<RequestTransaction> RequestTransactionFromUnits { get; set; }
        public virtual ICollection<RequestTransaction> RequestTransactionToUnits { get; set; }
        public virtual ICollection<UnitMainService> UnitMainServices { get; set; }
        public virtual ICollection<UnitServiceType> UnitServiceTypes { get; set; }
        public virtual ICollection<UnitsRequestType> UnitsRequestTypes { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
