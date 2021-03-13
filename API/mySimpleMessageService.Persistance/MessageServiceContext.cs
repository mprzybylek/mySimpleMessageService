using Microsoft.EntityFrameworkCore;
using mySimpleMessageService.Persistance.Entities;
using Persistance.Entities;

namespace Persistance
{
    public class MessageServiceContext : DbContext
    {
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<EventsEntity> Events { get; set; }
        

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
