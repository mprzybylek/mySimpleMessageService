using Microsoft.EntityFrameworkCore;
using Persistance.Entities;
using System.Linq;

namespace Persistance
{
    public class MessageServiceContext : DbContext
    {
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }

        public MessageServiceContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageEntity>()
                .HasOne(c => c.ContactSent)
                .WithMany(m => m.MessagesSent);

            modelBuilder.Entity<MessageEntity>()
                .HasOne(c => c.ContactReceived)
                .WithMany(m => m.MessagesReceived);

            base.OnModelCreating(modelBuilder);

        }

        
    }
}
