using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ContestEACA.Data;
using ContestEACA.Models;
using ContestEACA.Services;

namespace ContestEACA
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

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(opts => {
                    opts.Password.RequiredLength = 5;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "87881739727-i3p27sbrbdva1dgrglelskk92u3scs7b.apps.googleusercontent.com";
                googleOptions.ClientSecret = "TO7T2VvDculmMh71NvSOXn-Y";
            });

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "2027478304198988";
                facebookOptions.AppSecret = "adc2b6b22b5f3f03b417f5f599f2473a";
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

           

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
