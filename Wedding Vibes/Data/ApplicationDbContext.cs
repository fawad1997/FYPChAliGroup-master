using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeddingVibes.Models;
using WeddingVibes.Models.Menu;
using WeddingVibes.Models.Reservation;
using WeddingVibes.Models.Feedback;

namespace WeddingVibes.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<WeddingVibes.Models.ApplicationUser> ApplicationUser { get; set; }
        public DbSet<WeddingVibes.Models.Service> Service { get; set; }
        public DbSet<WeddingVibes.Models.Feedback.Feedback> Feedback { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
