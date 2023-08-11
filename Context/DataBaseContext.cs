using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;
using System.Runtime.ConstrainedExecution;

namespace SocialNetwork.Context
{
    public class DataBaseContext : DbContext
    {

        private readonly static string? DB_USER = /*Environment.GetEnvironmentVariable("DB_USER");*/ "postgres";
        private readonly static string? DB_PASSWORD = /*Environment.GetEnvironmentVariable("DB_PASSWORD");*/ "postgres";
        private readonly static string? DB_HOST = /*Environment.GetEnvironmentVariable("DB_HOST");*/ "localhost";
        private readonly static string? DB_PORT = /*Environment.GetEnvironmentVariable("DB_PORT");*/ "5432";
        private readonly static string? DB_NAME = /*Environment.GetEnvironmentVariable("DB_NAME");*/ "social";

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
             => optionsBuilder.UseNpgsql($"Username={DB_USER};Password={DB_PASSWORD};Host={DB_HOST};Port={DB_PORT};Database={DB_NAME};Integrated Security=true;Pooling=true");
    }
}
