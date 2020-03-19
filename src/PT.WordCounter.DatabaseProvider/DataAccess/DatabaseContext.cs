using Microsoft.EntityFrameworkCore;

namespace PT.WordCounter.DatabaseProvider.DataAccess
{
    internal class DatabaseContext: DbContext
    {
        public DbSet<Text> Texts { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Text>()
                .HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}
