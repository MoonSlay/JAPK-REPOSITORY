﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class JAPKDBEntities : DbContext
    {
        public JAPKDBEntities()
            : base("name=JAPKDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TBL_LOGIN> TBL_LOGIN { get; set; }
        public virtual DbSet<TBL_USER_DETAILS> TBL_USER_DETAILS { get; set; }
        public virtual DbSet<TBL_USER_PASSWORDS> TBL_USER_PASSWORDS { get; set; }
    }
}
