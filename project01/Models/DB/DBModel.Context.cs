﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace project01.Models.DB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBOutbondEntities1 : DbContext
    {
        public DBOutbondEntities1()
            : base("name=DBOutbondEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<laporan> laporans { get; set; }
        public DbSet<LOOKUPRole> LOOKUPRoles { get; set; }
        public DbSet<outbond> outbonds { get; set; }
        public DbSet<pelanggan> pelanggans { get; set; }
        public DbSet<SYSUser> SYSUsers { get; set; }
        public DbSet<SYSUserProfile> SYSUserProfiles { get; set; }
        public DbSet<SYSUserRole> SYSUserRoles { get; set; }

        public System.Data.Entity.DbSet<project01.Models.ViewModel.UserSignUpView> UserSignUpViews { get; set; }

        public System.Data.Entity.DbSet<project01.Models.ViewModel.UserLogInView> UserLogInViews { get; set; }
    }
}
