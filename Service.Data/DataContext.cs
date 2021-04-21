using Microsoft.EntityFrameworkCore;
using Service.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Promo> Promo { get; set; }
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<DiscountType> DiscountType { get; set; }
        public DbSet<VoucherType> VoucherType { get; set; }
        public DbSet<DiscountInputType> DiscountInputType { get; set; }
        public DataContext(DbContextOptions<DataContext> dbContext) : base(dbContext) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Promo>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Voucher>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<DiscountType>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<VoucherType>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<DiscountInputType>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
