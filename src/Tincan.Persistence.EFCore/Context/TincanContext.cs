using Microsoft.EntityFrameworkCore;
using Tincan.Entities;

namespace Tincan.Persistence.EFCore.Context
{
    public class TincanContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public TincanContext(DbContextOptions<TincanContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasKey(m => m.Id);
        }
    }
}
