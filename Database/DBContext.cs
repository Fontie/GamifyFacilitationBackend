using Microsoft.EntityFrameworkCore;

namespace GamifyBackEnd.DB
{
    public class GameDbContext : DbContext
    {
        public DbSet<Game> games { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Badge> badges { get; set; }
        public DbSet<BadgeUser> badgeusers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseMySql("server=localhost;database=gamify_db;user=gamify_db;password=michael&kingsleyRock!;",
            options.UseMySql(Environment.GetEnvironmentVariable("DB_CONNECTION"),
                new MySqlServerVersion(new Version(8, 0, 23))); // Adjust version as needed
        }
    }
}