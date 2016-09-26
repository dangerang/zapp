using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Coddit.Models.Entities;

namespace Zapp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connString = @"Data Source=(localdb)\MsSqlLocalDb;Initial Catalog=Zapp;Integrated Security=True";
            string connStringEF = @"Server=tcp:coddit.database.windows.net,1433;Initial Catalog=CodditEF;Persist Security Info=False;User ID=aladinsane;Password=Handtag1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            // To automatically generate classes, add the following dependencies:
            // Microsoft.EntityFrameworkCore.SqlServer.Design
            // Microsoft.EntityFrameworkCore.Tools (dependecy+tool)
            // In the Package Manager Console, enter:
            // Scaffold-DbContext "Server=tcp:coddit.database.windows.net,1433;Initial Catalog=Coddit;Persist Security Info=False;User ID=aladinsane;Password=Handtag1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir "../../Models/Entities" -Force
            // Scaffold-DbContext "Data Source=(localdb)\MsSqlLocalDb;Initial Catalog=Zapp;Integrated Security=True" -Force

            services.AddDbContext<CodditContext>(o => o.UseSqlServer(connString));
            services.AddDbContext<IdentityDbContext>(o => o.UseSqlServer(connStringEF));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Cookies.ApplicationCookie.LoginPath = "/landing/index";
            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseIdentity()
              .UseCookieAuthentication(new CookieAuthenticationOptions
              {
                  LoginPath = "/landing/index",

              });

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "landing",
                    template: "{controller=Landing}/{action=Index}/{id?}");
            });
        }
    }
}
