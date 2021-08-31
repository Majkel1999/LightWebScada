using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FrontEnd.Areas.Datasets;
using FrontEnd.Areas.Identity.Data;
using FrontEnd.Areas.Organizations.Data;
using FrontEnd.DatabaseConnection;
using FrontEnd.DataHandlers;
using FrontEnd.Hubs;
using Plk.Blazor.DragDrop;

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
            services.AddBlazorDragDrop();
            services.AddLocalization(Options=> Options.ResourcesPath = "Resources");
            services.AddControllers();

            services.AddScoped<TokenProvider>();
            services.AddScoped<ApiKeyGenerator>();
            services.AddScoped<DatasetContext>();
            services.AddSingleton<ConfigHandler>();

            services.AddDbContext<UserContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("UserContextConnection")));

            services.AddDbContext<OrganizationContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("UserContextConnection")));

            services.AddDbContext<ViewContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("UserContextConnection")));

            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

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

            var supportedCultures = new[] { "en-US"};
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            app.UseRequestLocalization(localizationOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapHub<ViewHub>("/viewhub");
                endpoints.MapFallbackToPage("/Utility/_Host");
            });
        }
    }
}
