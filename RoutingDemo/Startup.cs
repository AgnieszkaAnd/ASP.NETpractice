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
using Microsoft.AspNetCore.Session;
using RoutingDemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RoutingDemo {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDistributedMemoryCache();

            // Session uses a cookie to track and identify requests from a single browser. 
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromSeconds(1800);
                options.Cookie.Name = ".Routing.Session";
                // Because the cookie default doesn't specify a domain, it isn't made available to the client-side script on the page (because HttpOnly defaults to true).
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();

            services.AddDbContext<UserContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("UserContext")));

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error/Index");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // The order of middleware is important. Call UseSession after UseRouting and before UseEndpoints. See Middleware Ordering.
            // HttpContext.Session is available after session state is configured.
            // HttpContext.Session can't be accessed before UseSession has been called.
            app.UseSession();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "students",
                    pattern: "{controller=Students}/{action=Index}/{id?}",
                    constraints: new { id = @"\d+" });
                endpoints.MapControllerRoute(
                    name: "error",
                    pattern: "{*any}",
                    defaults: "{controller=Error}/{action=Index}");
            });

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404) {
                    context.Request.Path = "/Error/Index";
                    await next();
                }
            });
        }
    }
}
