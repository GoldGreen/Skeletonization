using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Skeletonization.DataLayer.Implementations.DatabaseSending
{

    public class SkeletonizationContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<BodyPart> BodyParts { get; set; }
        public DbSet<Human> Humans { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Pose> Poses { get; set; }
        public DbSet<Report> Reports { get; set; }

        public SkeletonizationContext(IConfiguration configuration)
        {
            _configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Skeletonization"));
        }
    }
}
