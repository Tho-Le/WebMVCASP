using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebMVCDemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebMVCDemo
{
    public class Startup
    {
        private readonly int passwordLength = 8;
        private readonly int uniqueCharsInPassword = 4;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", config => {
            //    config.Cookie.Name = "Grandmas.Cookie";
            //    config.LoginPath = "/Sessions/Authenticate";
            //});

            services.AddControllersWithViews();

            //AddItentiy registers the services. This creates the infrastructure to allows the identity to communicate with the database 
            //which allows us to create users and allow authentication and authorization.
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<LoginDbContext>()
                .AddRoles<IdentityRole>();

            //This configurations allows the entity framework to work with databases. In this case we want to use Microsoft SQL Server so we
            //configure it to UseSQLServer. The get connectionstring finds the information to connect our database from the appsettings.json
            services.AddDbContext<CountryContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("CountryContext")));
            services.AddDbContext<LoginDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("LoginDbContext"));
            });

            //Configuring the password to contain certain characters and to be a certain length
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = passwordLength;
                options.Password.RequiredUniqueChars = uniqueCharsInPassword;
                options.Password.RequireNonAlphanumeric = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            });

            //services.AddMvc(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .Build();
            //    options.Filters.Add(new AuthorizeFiler)
            //});


            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //This checks if our environment is in development mode. If it is, displays a developer exception page which contains detail information
            //about exceptions which is very useful for debugging.
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

            app.UseAuthorization();

            //This method indicates what page we want to end up and what route we want to take.
            //It this case, it directs the app to use a controller to route for us. In this case we direct the app to the Home Controller and to find
            //a method called Index. In our case, the index method simply displays the index.cshtml page.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
