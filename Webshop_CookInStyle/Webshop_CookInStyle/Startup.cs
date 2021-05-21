using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop_CookInStyle.Data;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddRazorPages();

            services.AddControllersWithViews();

            services.AddDbContext<WebshopContext>(options
                => options.UseSqlServer(Configuration.GetConnectionString("WebshopConnection")));

            services.AddDefaultIdentity<Klant>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<WebshopContext>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User Settings
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=UserBestellingen}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            //CreateUserRoles(serviceProvider).Wait();
        }

        private async Task CreateUserRoles( IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            WebshopContext context = serviceProvider.GetRequiredService<WebshopContext>();

            IdentityResult roleResult;
            bool roleCheck = await roleManager.RoleExistsAsync("Admin");

            if (!roleCheck)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            IdentityUser user = context.Users.FirstOrDefault(x => x.Email == "stijn.roelants@gmail.com");
            if (user != null)
            {
                DbSet<IdentityUserRole<string>> roles = context.UserRoles;
                IdentityRole adminRole = context.Roles.FirstOrDefault(x => x.Name == "Admin");

                if (adminRole != null)
                {
                    if (!roles.Any(x => x.UserId == user.Id && x.RoleId == adminRole.Id))
                    {
                        roles.Add(new IdentityUserRole<string>() { UserId = user.Id, RoleId = adminRole.Id });
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
