﻿//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShoppingCart.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ShoppingCartEntities : DbContext
    {
        public ShoppingCartEntities()
            : base("name=ShoppingCartEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<MemberSet> MemberSet { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<CartSet> CartSet { get; set; }
        public DbSet<ProductMessage> ProductMessage { get; set; }
    }
}