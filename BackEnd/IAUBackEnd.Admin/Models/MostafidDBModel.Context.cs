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
        public virtual DbSet<CheckBox_Type> CheckBox_Type { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<DelayedTransaction> DelayedTransaction { get; set; }
        public virtual DbSet<E_Forms> E_Forms { get; set; }
        public virtual DbSet<E_Forms_Answer> E_Forms_Answer { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<ID_Document> ID_Document { get; set; }
        public virtual DbSet<Input_Type> Input_Type { get; set; }
        public virtual DbSet<Job_Permissions> Job_Permissions { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Main_Services> Main_Services { get; set; }
        public virtual DbSet<Paragraph> Paragraph { get; set; }
        public virtual DbSet<Person_Eform> Person_Eform { get; set; }
        public virtual DbSet<Personel_Data> Personel_Data { get; set; }
        public virtual DbSet<Preview_EformApproval> Preview_EformApproval { get; set; }
        public virtual DbSet<Preview_TableCols> Preview_TableCols { get; set; }
        public virtual DbSet<Privilage> Privilage { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Radio_Type> Radio_Type { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Request_Data> Request_Data { get; set; }
        public virtual DbSet<Request_File> Request_File { get; set; }
        public virtual DbSet<Request_Log> Request_Log { get; set; }
        public virtual DbSet<Request_State> Request_State { get; set; }
        public virtual DbSet<Request_Type> Request_Type { get; set; }
        public virtual DbSet<RequestTransaction> RequestTransaction { get; set; }
        public virtual DbSet<Required_Documents> Required_Documents { get; set; }
        public virtual DbSet<Separator> Separator { get; set; }
        public virtual DbSet<Service_Type> Service_Type { get; set; }
        public virtual DbSet<Sub_Services> Sub_Services { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Table_Columns> Table_Columns { get; set; }
        public virtual DbSet<Tables_Answare> Tables_Answare { get; set; }
        public virtual DbSet<Title_Middle_Names> Title_Middle_Names { get; set; }
        public virtual DbSet<Unit_Signature> Unit_Signature { get; set; }
        public virtual DbSet<UnitLevel> UnitLevel { get; set; }
        public virtual DbSet<UnitMainServices> UnitMainServices { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<Units_Location> Units_Location { get; set; }
        public virtual DbSet<Units_Request_Type> Units_Request_Type { get; set; }
        public virtual DbSet<Units_Type> Units_Type { get; set; }
        public virtual DbSet<UnitServiceTypes> UnitServiceTypes { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<ValidTo> ValidTo { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<SystemLog> SystemLog { get; set; }
    }
}
