﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IAUBackEnd.Admin.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MostafidDBEntities : DbContext
    {
        public MostafidDBEntities()
            : base("name=MostafidDBEntities")
        {
			this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Applicant_Type> Applicant_Type { get; set; }
        public virtual DbSet<Authority_Holder> Authority_Holder { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<College_Administration> College_Administration { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<E_Forms> E_Forms { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Engagements_Department> Engagements_Department { get; set; }
        public virtual DbSet<Event_Publication_Violation> Event_Publication_Violation { get; set; }
        public virtual DbSet<FollowUp_Publication_Violations> FollowUp_Publication_Violations { get; set; }
        public virtual DbSet<ID_Document> ID_Document { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<Job_Permissions> Job_Permissions { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Main_Services> Main_Services { get; set; }
        public virtual DbSet<Personel_Data> Personel_Data { get; set; }
        public virtual DbSet<Privilage> Privilage { get; set; }
        public virtual DbSet<Purchase_Order_Form> Purchase_Order_Form { get; set; }
        public virtual DbSet<Purchase_Order_Form_Required> Purchase_Order_Form_Required { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Request_File> Request_File { get; set; }
        public virtual DbSet<Request_Log> Request_Log { get; set; }
        public virtual DbSet<Request_Type> Request_Type { get; set; }
        public virtual DbSet<RequestTransaction> RequestTransaction { get; set; }
        public virtual DbSet<Required_Documents> Required_Documents { get; set; }
        public virtual DbSet<Service_Type> Service_Type { get; set; }
        public virtual DbSet<Sub_Services> Sub_Services { get; set; }
        public virtual DbSet<Supplier_Names> Supplier_Names { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Title_Middle_Names> Title_Middle_Names { get; set; }
        public virtual DbSet<Type_Support_Project> Type_Support_Project { get; set; }
        public virtual DbSet<UnitLevel> UnitLevel { get; set; }
        public virtual DbSet<UnitMainServices> UnitMainServices { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<Units_Location> Units_Location { get; set; }
        public virtual DbSet<Units_Request_Type> Units_Request_Type { get; set; }
        public virtual DbSet<Units_Type> Units_Type { get; set; }
        public virtual DbSet<UnitServiceTypes> UnitServiceTypes { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<ValidTo> ValidTo { get; set; }
        public virtual DbSet<Statement_Department_File_Sent_Them> Statement_Department_File_Sent_Them { get; set; }
        public virtual DbSet<Request_Data> Request_Data { get; set; }
        public virtual DbSet<Request_State> Request_State { get; set; }
        public virtual DbSet<DelayedTransaction> DelayedTransaction { get; set; }
    }
}
