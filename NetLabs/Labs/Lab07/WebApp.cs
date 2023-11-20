using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace NetLabs.Labs.Lab07;

public static class WebApp
{
    static WebApplication? app;

	public static bool IsRunning => app != null;


	public static void Start()
	{
		if (app != null)
            throw new InvalidOperationException("WebApp is already running");

        var builder = WebApplication.CreateBuilder();

        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddRazorPages(builder => builder.RootDirectory = "/Labs/Lab07/Pages");
        builder.Services.AddSession(options =>
        {
            options.Cookie.Name = ".NetLabs.Labs.Lab07.Session";
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => false;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
        app.UseSession();

        app.MapRazorPages();

        app.RunAsync();
	}

	public static void Stop()
	{
		if (app == null)
            throw new InvalidOperationException("WebApp is not running");

        app.StopAsync();
        app = null;
	}

}
