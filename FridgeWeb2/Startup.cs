using System;
using System.IO;
using FridgeData;
using FridgeData.Authorization;
using FridgeData.Configuration;
using FridgeData.Helpers;
using FridgeData.Models;
using FridgeData.Standings;
using FridgeCoreWeb.Authorization;
using FridgeCoreWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;


namespace FridgeCoreWeb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            // Add framework services.
            services.AddMvc(config =>
            {
                //var defaultPolicy = new AuthorizationPolicyBuilder()
                //    .RequireAuthenticatedUser()
                //    .Build();

                //config.Filters.Add(new AuthorizeFilter(defaultPolicy));

            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrators", policy =>
                    policy.RequireRole("Administrator"));
            });


            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            services.AddDbContext<FridgeContext>(options => options.UseMySql(Configuration.GetConnectionString("Default")));

            ConfigureIdentity(services);

            services.AddOptions();

            services.Configure<TimeSettings>(Configuration.GetSection("TimeSettings"));

            services.AddScoped<IFridgeContext, FridgeContext>();
            services.AddScoped<IGameViewRepository, GameViewRepository>();
            services.AddScoped<ILinesRepository, LinesRepository>();
            services.AddScoped<IAuthorizationHandler, UserAuthorizationHandler>();
            services.AddScoped<ITimeHelper, TimeHelper>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStandingsProvider, StandingsProvider>();
            services.AddScoped<IStandingsSorter, StandingsSorter>();
            services.AddScoped<IWeeklyPickTotalProvider, WeeklyPickTotalProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/docs")),
                RequestPath = new PathString("/docs")
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{Id?}");
            });
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
             .AddEntityFrameworkStores<FridgeContext>()
             .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //// Password settings
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.LoginPath = "/Account/LogIn";
                options.LogoutPath = "/Account/LogOut";
            });
        }
    }
}
