using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.UI.Services;
using employeestest.Areas.Identity;
using employeestest.Data;

namespace milletmentor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("MilletMentorConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 28))));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor(options =>
            {
                options.DetailedErrors = true;
                options.DetailedErrors = true;
                options.DisconnectedCircuitMaxRetained = 100;
                options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(10);
            });



            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseRequestLocalization(GetLocalizationOptions(configuration));

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }

        public static RequestLocalizationOptions GetLocalizationOptions(IConfiguration configuration)
        {
            var cultures = configuration.GetSection("Cultures")
                .GetChildren().ToDictionary(x => x.Key, x => x.Value);

            var supportedCultures = cultures.Keys.ToArray();

            var localizationOptions = new RequestLocalizationOptions()
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            return localizationOptions;
        }
    }
}
