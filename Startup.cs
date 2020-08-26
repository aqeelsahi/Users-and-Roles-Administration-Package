using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using DESI8N.com.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DESI8N.com.Models;
using Microsoft.Extensions.Options;

namespace DESI8N.com
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
           // var lockoutOptions = new LockoutOptions()
            //{
            //  AllowedForNewUsers = true,
            //  DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5),
            //  MaxFailedAccessAttempts = 2
            //};


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            services.AddAuthentication()
                .AddGoogle(Options =>
                {
                    Options.ClientId = "799507130875-c7pmi53m1u97vr3uqa29bqoa71t2f8eb.apps.googleusercontent.com";
                    Options.ClientSecret = "kDa_mPhSllq0i_5HRvAb_QSe";
                });
            //.AddMicrosoftAccount(microsoftOptions => {
            //   microsoftOptions.ClientId = Configuration["7b6dac25-c100-4d5e-b67d-153f213795ba"];
            //   microsoftOptions.ClientSecret = Configuration["sIh2yK3ls~2_1X28v7xifI_4d5y_JJ5vBh"];
            // });
            //.AddTwitter(twitterOptions => { ... })
            // .AddFacebook(facebookOptions => { ... });
            /*
            services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
            {
                Options.Lockout = lockoutOptions;
            });
            */
            services.AddIdentityCore<ApplicationUser>(options =>
            {
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
                .AddRoles<IdentityRole>()
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.AddRazorPages();
            //Creating Claims Policy
            //Adding Multiple Claims to Policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role", "true"));
                options.AddPolicy("EditRolePolicy",
                    policy => policy.RequireClaim("Edit Role", "true"));
                options.AddPolicy("CreateRolePolicy",
                    policy => policy.RequireClaim("Create Role", "true"));
                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Administrator")
                    );
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
