using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FrontEnd.Areas.Identity.Data;
using FrontEnd.DatabaseConnection;

[assembly: HostingStartup(typeof(FrontEnd.Areas.Identity.IdentityHostingStartup))]
namespace FrontEnd.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<UserContext>(options =>
                    options.UseNpgsql(
                        context.Configuration.GetConnectionString("UserContextConnection")));

                services.AddDefaultIdentity<FrontEndUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.User.RequireUniqueEmail = true;
                })
                    .AddEntityFrameworkStores<UserContext>()
                    .AddTokenProvider<DataProtectorTokenProvider<FrontEndUser>>("default");
            });
        }
    }
}