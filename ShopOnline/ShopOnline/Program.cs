﻿using AspNetCoreHero.ToastNotification;
using ShopOnline.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Identity;

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
        builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
        builder.Services.AddSession();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(p =>
            {
                //p.Cookie.Name = "UserLoginCookie";
                //p.ExpireTimeSpan = TimeSpan.FromDays(1);
                p.LoginPath = "/dang-nhap.html";
                p.LogoutPath = "/dang-xuat/html";
                p.AccessDeniedPath = "/Admin/Account/TBQuyen";
            });
        //builder.Services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("AdminRole", policy => policy.RequireRole("Admin"));
        //    options.AddPolicy("EmployeeRole", policy => policy.RequireRole("Employee"));
        //});
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
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
            endpoints.MapControllerRoute(
           name: "default",
           pattern: "{controller=Home}/{action=Index}/{id?}");
        });


        app.Run();
    }
}