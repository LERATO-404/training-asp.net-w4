using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ParcelDeliveryTrackingAPI.AuthModels;
using ParcelDeliveryTrackingAPI.Interfaces;
using ParcelDeliveryTrackingAPI.Models;
using ParcelDeliveryTrackingAPI.Repositories;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace ParcelDeliveryTrackingAPI;

public class Program
{

    private static readonly ILog logger = LogManager.GetLogger("Program.main method");

    public static void Main(string[] args)
    {
        logger.Info("Parcel Delivery Tracking REST API Started!");

        var builder = WebApplication.CreateBuilder(args);
        string tmpKeyIssuer = builder.Configuration.GetSection("ApplicationSettings:JWT_Site_URL").Value;
        string tmpKeySign = builder.Configuration.GetSection("ApplicationSettings:SigningKey").Value;

        #pragma warning disable CS8604 // Possible null reference argument.
        var key = Encoding.UTF8.GetBytes(tmpKeySign);
        #pragma warning restore CS8604 // Possible null reference argument.


        // Add services to the container.
        builder.Services.AddDbContext<ParcelDeliveryTrackingDBContext>(
                           options =>
                           {
                               options.UseSqlServer(builder.Configuration.GetConnectionString("ParcelDeliveryTrackingConnectionString"));
                           });
        builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));

        builder.Services.AddDbContext<AuthenticationContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnectionString")));

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;               // Enforce at least one digit
            options.Password.RequireLowercase = true;           // Enforce at least one lowercase letter
            options.Password.RequireUppercase = true;           // Enforce at least one uppercase letter
            options.Password.RequireNonAlphanumeric = true;     // Enforce at least one special character
            options.Password.RequiredLength = 8;                // Require a minimum length of 8 characters
        });

        builder.Services.AddDefaultIdentity<ApplicationUser>()
                 .AddRoles<IdentityRole>()
                 .AddEntityFrameworkStores<AuthenticationContext>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidIssuer = tmpKeyIssuer,
                            ValidAudience = tmpKeyIssuer,
                            ClockSkew = TimeSpan.Zero,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen
            (
                options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "JWT Authorization using Bearer"
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[]{}

                        }
                    });
                }
            );

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
