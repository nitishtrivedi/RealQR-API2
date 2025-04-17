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
    }
}
