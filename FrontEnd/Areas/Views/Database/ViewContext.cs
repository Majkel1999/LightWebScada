using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FrontEnd.Areas.Organizations.Data
{
    public class ViewContext : DbContext
    {
        public DbSet<ViewObject> Views {get;set;}

        public ViewContext(DbContextOptions<OrganizationContext> options) : base(options) 
        { 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Startup.Configuration.GetConnectionString("UserContextConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("common");
            base.OnModelCreating(builder);
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var currentTableName = builder.Entity(entity.Name).Metadata.GetDefaultTableName();
                builder.Entity(entity.Name).ToTable(currentTableName.ToLower());
            }
        }
    }
}