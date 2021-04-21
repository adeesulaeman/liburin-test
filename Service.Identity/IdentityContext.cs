using Microsoft.EntityFrameworkCore;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity
{
    public class IdentityContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public IdentityContext(DbContextOptions<IdentityContext> dbContext) : base(dbContext) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<UserRole>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<UserType>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Group>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Role>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Permission>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Module>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Operation>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
