using App.Domain.Interfaces;
using App.Domain.Models;
using App.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace App.Infra.Data.Context
{
    public class MySQLContext : DbContext
    {

        public MySQLContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new VideosMap());
        }

        public virtual DbSet<VideoBD> Videos { get; set; }
    }

    public class MySQLContextFactory 
    {
        private readonly string _connectionString;

        public MySQLContextFactory(IConfiguration configuration)
        {
            
            _connectionString = configuration["ConnectionVideos"];
        }

        public virtual MySQLContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySQLContext>();
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
            return new MySQLContext(optionsBuilder.Options);
        }
    }

}
