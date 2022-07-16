using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NewsApp.DAL;
using NewsApp.Interfaces;
using System;
using System.IO;
namespace NewsApp
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddControllersWithViews().AddSessionStateTempDataProvider(); ;
            services.AddDistributedMemoryCache();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddRazorPages().AddSessionStateTempDataProvider(); ;
            services.AddSession();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
                      {
                          options.Cookie.Name = "Login";
                          options.ExpireTimeSpan = TimeSpan.FromDays(10);
                          options.SlidingExpiration = true;
                          options.LoginPath = "/admin/account/login";
                          options.AccessDeniedPath = "/Forbidden/";
                      }
                );


            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Or you can also register as follows

            services.AddHttpContextAccessor();
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


            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.None,
            };
            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resources")),
                RequestPath = "/Resources",
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(env.WebRootPath, "Resources")),
                RequestPath = "/Resources"
            });

            app.UseEndpoints(endpoints =>
            {


                endpoints.MapControllerRoute(
                    name: "MyArea",
                    pattern: "{area:exists}/{controller=Account}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "read",
                    pattern: "doc/{meta}/{id}",
                    new { controller = "Home", action = "ReadArticle" });

                endpoints.MapControllerRoute(
                    name: "findByCategory",
                    pattern: "danh-muc/{metatitle}/{cateid}",
                    new { controller = "Home", action = "FindArticleByCate" });



                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");



            });

            app.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("404 Page not found");
            });
        }

    }

}
