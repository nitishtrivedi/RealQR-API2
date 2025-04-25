using Microsoft.EntityFrameworkCore;
using RealQR_API.Models;

namespace RealQR_API.DBContext
{
    public class RealQRDBContext : DbContext
    {
        public RealQRDBContext(DbContextOptions<RealQRDBContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Enquiry> Enquiry { get; set; }
        public DbSet<EnquiryQuestionnaire> EnquiryQuestionnaire { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); //Disables reverse query tracking
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enquiry>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enquiries)
                .HasForeignKey(e => e.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            //Ensures 1-1 relation between Enquiry and enquiryQuestionnaire
            modelBuilder.Entity<Enquiry>()
                .HasOne(e => e.EnquiryQuestionnaire)
                .WithOne() // No reverse tracking on the model
                .HasForeignKey<EnquiryQuestionnaire>(q => q.EnquiryId)
                .OnDelete(DeleteBehavior.Cascade); //Delete the dependant entry in the table as well.
        }
    }
}
