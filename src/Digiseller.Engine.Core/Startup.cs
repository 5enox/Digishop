using System;
using System.Linq;
using Digiseller.Client.Core;
using Digiseller.Client.Core.Enums;
using Digiseller.Engine.Core.Areas.Dashboard.Controllers;
using Digiseller.Engine.Core.Areas.Dashboard.Models.Settings;
using Digiseller.Engine.Core.Attributes;
using Digiseller.Engine.Core.Helpers;
using Digiseller.Engine.Core.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Digiseller.Engine.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddSingleton(
                new DigisellerClient(Configuration.GetValue<int>($"{nameof(DigisellerSettings)}:DigisellerId"),
                    Configuration.GetValue<string>($"{nameof(DigisellerSettings)}:DigisellerUid")
                )
            );

            services.AddScoped<AuthAttribute>();

            services.AddSingleton(typeof(IConfProvider), typeof(JsonConfProvider));

            services.AddMemoryCache();
            services.AddSession();
            services.AddCloudscribePagination();
            // Add framework services.
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfProvider conf)
        {
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.Use((httpContext, next) =>
            {
                const string url = "/Dashboard/Home/Install";
                if (!conf.Get<MainSettings>().Installed && httpContext.Request.Path != url)
                {
                    Console.WriteLine("Redirected");
                    httpContext.Response.Redirect($"http://{httpContext.Request.Host}{url}");
                }
                return next();
            });

            app.UseSession();
            
            // Session middleware
            app.Use((httpContext, nextMiddleware) =>
            {
                if (!httpContext.Session.Keys.Contains(SessionHelper.KeyCurrency))
                {
                    httpContext.Session.SetCurrency(Currency.RUR);
                }

                if (!httpContext.Session.Keys.Contains(SessionHelper.KeyCart))
                {
                    httpContext.Session.SetCartId(string.Empty);
                }

                return nextMiddleware();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "productListPager",
                    pattern: "Products/Page-{page:int}",
                    defaults: new { controller = "Product", action = "Index", category = 0 }
                );

                endpoints.MapControllerRoute(
                    name: "productListCategory",
                    pattern: "Products/Category-{category:int}",
                    defaults: new { controller = "Product", action = "Index", page = 1 }
                );

                endpoints.MapControllerRoute(
                    name: "productListCategoryPager",
                    pattern: "Products/Category-{category:int}/Page-{page:int}",
                    defaults: new { controller = "Product", action = "Index" }
                );


                endpoints.MapControllerRoute(
                    name: "productListSearch",
                    pattern: "Products/{search}",
                    defaults: new { controller = "Product", action = "Search", page = 1 }
                );

                endpoints.MapControllerRoute(
                    name: "productListSearchPager",
                    pattern: "Products/{search}/Page-{page:int}",
                    defaults: new { controller = "Product", action = "Search" }
                );

                endpoints.MapControllerRoute(
                    name: "productDetails",
                    pattern: "Product/{id:int}",
                    defaults: new { controller = "Product", action = "Details" }
                );

                endpoints.MapControllerRoute(
                    name: "productList",
                    pattern: "Products/",
                    defaults: new { controller = "Product", action = "Index", category = 0 }
                );

                endpoints.MapControllerRoute(
                    name: "cartView",
                    pattern: "Cart/",
                    defaults: new { controller = "Cart", action = "ViewCart" }
                );

                endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Product}/{action=Index}/{id?}");
            });
        }
    }
}
