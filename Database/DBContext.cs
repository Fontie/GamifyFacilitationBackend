using Microsoft.EntityFrameworkCore;

namespace GamifyBackEnd.DB
{
    public class GameDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<User> Users { get; set; }

        //for testing
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
        }

        public GameDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connection = Environment.GetEnvironmentVariable("DB_CONNECTION");
                optionsBuilder.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 23)));
            }
        }
    }
}