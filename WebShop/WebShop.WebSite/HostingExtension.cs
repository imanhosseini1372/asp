using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebShop.Application.Repositories.Framework.Interfaces.Contexts;
using WebShop.Application.Repositories.Users.Interfaces.Queries;
using WebShop.Application.Repositories.Users.Services.Queries;
using WebShop.Persistence.AddAuditFieldInterceptors;
using WebShop.Persistence.Contexts;

namespace WebShop.WebSite
{
    public static class HostingExtension
    {
        #region Service
        public static WebApplication ConfigService(this WebApplicationBuilder applicationBuilder)
        {
           
            #region  AddServices
            applicationBuilder.Services.AddControllersWithViews();

            #region Authentication
          
            #endregion

            #region DiContainer
            applicationBuilder.Services.AddScoped<IWebShopDbContext, WebShopDbContext>();
            applicationBuilder.Services.AddScoped<IGetUsers, GetUsers>();
            var cnn = applicationBuilder.Configuration.GetConnectionString("SqlConnection");
            applicationBuilder.Services.AddEntityFrameworkSqlServer().AddDbContext<WebShopDbContext>(option => option.UseSqlServer(cnn));
            
            #region AddServiceAuditFields
            applicationBuilder.Services.AddHttpContextAccessor(); // برای دسترسی به context
            applicationBuilder.Services.AddScoped<AddAuditFieldInterceptor>(); // خودش وابسته است به HttpContext
            #endregion

            #endregion
            #endregion
            return applicationBuilder.Build();
        }
        #endregion

        #region Pipeline
        public static WebApplication ConfigPipeLine(this WebApplication application)
        {

            if (!application.Environment.IsDevelopment())
            {
                application.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                application.UseHsts();
            }

            application.UseHttpsRedirection();
            application.UseRouting();

            application.UseAuthorization();

            application.MapStaticAssets();

            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            application.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            return application;
        }
        #endregion
    }
}
