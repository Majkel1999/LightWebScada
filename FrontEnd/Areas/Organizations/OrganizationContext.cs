using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ScadaCommon;

namespace FrontEnd.Areas.Organizations.Data
{
    public class OrganizationContext : DbContext
    {
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationMember> Members { get; set; }
        public DbSet<ClientConfigEntity> Configurations { get; set; }

        public OrganizationContext(DbContextOptions<OrganizationContext> options) : base(options)
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
