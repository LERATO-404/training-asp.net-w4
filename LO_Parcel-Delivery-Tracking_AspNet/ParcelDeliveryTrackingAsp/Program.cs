using log4net;
using Microsoft.AspNetCore.Builder;
using ParcelDeliveryTrackingAsp.Services;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace ParcelDeliveryTrackingAsp;

public class Program
{
    private static readonly ILog logger = LogManager.GetLogger("Program.main method");
    public static void Main(string[] args)
    {
        logger.Info("ASP.NET MVC App has Started!");

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientApp"));
        string clientBaseUrl = builder.Configuration.GetSection("ClientApp:ClientBaseUrl").Value;

        builder.Services.AddHttpClient();

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
            endpoints.MapAreaControllerRoute(
                 name: "AuthArea",
                 areaName: "AuthServices",
                 pattern: "AuthServices/{controller=Home}/{action=Index}/{id?}");

            endpoints.MapAreaControllerRoute(
                name: "AdminArea",
                areaName: "Admin",
                pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

            endpoints.MapAreaControllerRoute(
                name: "Deliverystatus",
                areaName: "Admin",
                pattern: "Admin/{controller=CrudDeliveries}/{action=DeliveriesByStatus}/Status/{status?}");

            endpoints.MapAreaControllerRoute(
                name: "Parcelstatus",
                areaName: "Admin",
                pattern: "Admin/{controller=CrudParcels}/{action=ParcelsByStatus}/Status/{status?}");

            endpoints.MapAreaControllerRoute(
               name: "ManagerArea",
               areaName: "Manager",
               pattern: "Manager/{controller=Home}/{action=Index}/{id?}");

            endpoints.MapAreaControllerRoute(
               name: "DriverArea",
               areaName: "Driver",
               pattern: "Driver/{controller=Home}/{action=Index}/{id?}");



           



            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

        });

        app.Run();
    }
}


