using Microsoft.EntityFrameworkCore;

namespace GamifyBackEnd.DB
{
    public class GameDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseMySql("server=localhost;database=gamify_db;user=gamify_db;password=michael&kingsleyRock!;",
            options.UseMySql(Environment.GetEnvironmentVariable("DB_CONNECTION"),
                new MySqlServerVersion(new Version(8, 0, 23))); // Adjust version as needed
        }
    }
}