using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FrontEnd.Areas.Identity.Data;
using FrontEnd.Areas.Organizations.Data;
using FrontEnd.DatabaseConnection;
using FrontEnd.DataHandlers;

namespace FrontEnd
{
    public class Startup
    {
        private static IConfiguration m_configuration;

        public static IConfiguration Configuration => m_configuration;

        public Startup(IConfiguration configuration)
        {
            m_configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpClient();

            services.AddScoped<TokenProvider>();
            services.AddScoped<ApiKeyGenerator>();
            services.AddSingleton<ConfigHandler>();

            services.AddDbContext<UserContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("UserContextConnection")));

            services.AddDbContext<OrganizationContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("UserContextConnection")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Utility/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/Utility/_Host");
            });
        }
    }
}
