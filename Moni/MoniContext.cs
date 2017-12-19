using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moni.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Moni
{
    public class MoniContext : DbContext
    {
        public MoniContext(DbContextOptions<MoniContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            LoggerFactory = loggerFactory;
        }

        private ILoggerFactory LoggerFactory { get; set; }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Revenue> Revenues { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }

        public override int SaveChanges()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;

            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);

                Validator.ValidateObject(entity, validationContext);
            }

            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLoggerFactory(LoggerFactory);
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //}

        //public override int SaveChanges(bool acceptAllChangesOnSuccess)
        //{
        //    ChangeTracker.DetectChanges();

        //    foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
        //    {
        //        entry.P
        //    }

        //    ChangeTracker.AutoDetectChangesEnabled = false;
        //    var result = base.SaveChanges(acceptAllChangesOnSuccess);
        //    ChangeTracker.AutoDetectChangesEnabled = true;

        //    return result;
        //}

    }
}
