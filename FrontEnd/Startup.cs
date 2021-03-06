using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
using Blazored.Modal;
using Plk.Blazor.DragDrop;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Authorization;

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
            services.AddControllers();
            services.AddBlazoredModal();

            services.AddDataProtection()
                .SetApplicationName("LightWebScada")
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo("keys"))
                .SetDefaultKeyLifetime(TimeSpan.FromDays(3650));

            services.AddScoped<TokenProvider>();
            services.AddScoped<ApiKeyGenerator>();
            services.AddScoped<DatasetContext>();

            services.AddSingleton<ConfigHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ShowViewsPolicy, policy => policy.Requirements.Add(new ShowViewRequirement()));
                options.AddPolicy(Policies.EditViewsPolicy, policy => policy.Requirements.Add(new EditViewRequirement()));
                options.AddPolicy(Policies.EditConfigurationsPolicy, policy => policy.Requirements.Add(new EditConfigurationRequirement()));
                options.AddPolicy(Policies.CreateReportsPolicy, policy => policy.Requirements.Add(new CreateReportRequirement()));
                options.AddPolicy(Policies.AdminPolicy, policy => policy.Requirements.Add(new AdminRequirement()));
                options.AddPolicy(Policies.OrganizationPolicy, policy => policy.Requirements.Add(new OrganizationRequirement()));
            });
            services.AddTransient<IAuthorizationHandler, ShowViewHandler>();
            services.AddTransient<IAuthorizationHandler, EditViewHandler>();
            services.AddTransient<IAuthorizationHandler, EditConfigurationHandler>();
            services.AddTransient<IAuthorizationHandler, CreateReportHandler>();
            services.AddTransient<IAuthorizationHandler, AdminHandler>();
            services.AddTransient<IAuthorizationHandler, OrganizationHandler>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            });

            services.AddDbContext<UserContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("UserContextConnection")));

            services.AddDbContext<OrganizationContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("UserContextConnection")));

            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            app.UseResponseCompression();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Utility/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

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
