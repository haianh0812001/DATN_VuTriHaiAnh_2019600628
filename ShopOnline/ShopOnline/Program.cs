using AspNetCoreHero.ToastNotification;
using ShopOnline.Models;
using System.Configuration;
using ShopOnline.Models;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<dbMarketsContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnectionString"));

        });
        builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
        builder.Services.AddNotyf(config =>
        {
            config.DurationInSeconds = 3;
            config.IsDismissable = true;
            config.Position = NotyfPosition.TopRight;
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
        });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}