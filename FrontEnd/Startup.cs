using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FrontEnd.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FrontEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(option =>
            {
                option.LoginPath = "/Login";
                option.AccessDeniedPath = "/Forbidden";
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CookiePolicyOptions cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always
            };

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Utility/Error");
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCookiePolicy(cookiePolicyOptions);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/Utility/_Host");
            });
        }
    }
}
