﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TasahelEntities : DbContext
    {
        public TasahelEntities()
            : base("name=TasahelEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<About> About { get; set; }
        public virtual DbSet<Domain> Domain { get; set; }
        public virtual DbSet<DomainInfo> DomainInfo { get; set; }
        public virtual DbSet<SubDomains> SubDomains { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TasahelHomeSetting> TasahelHomeSetting { get; set; }
    }
}
