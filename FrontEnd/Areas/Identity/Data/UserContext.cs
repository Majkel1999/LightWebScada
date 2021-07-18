using FrontEnd.Areas.Identity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.DatabaseConnection
{
    public class UserContext : IdentityDbContext<FrontEndUser>
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Startup.Configuration.GetConnectionString("UserContextConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("private");
            base.OnModelCreating(builder);
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var currentTableName = builder.Entity(entity.Name).Metadata.GetDefaultTableName();
                builder.Entity(entity.Name).ToTable(currentTableName.ToLower());
            }
        }
    }
}
