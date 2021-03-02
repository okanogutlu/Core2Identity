using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core2Identity.Infrastructure;
using Core2Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core2Identity
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
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //Identity için yapýlan iþlemler...
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddDefaultTokenProviders();

            services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>();//Ne zaman IPasswordValidator istenirse
            services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidator>();//Ne zaman IUserValidator  istenirse
            
            //CustomPasswordValidator gönderilecek...

            //Parolaya birtakým kýsýtlamalar koymak:
            //services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            //{
            //    options.Password.RequiredLength = 7;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //}
            //);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else 
            {
                app.UseExceptionHandler("/Home/Error");
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
