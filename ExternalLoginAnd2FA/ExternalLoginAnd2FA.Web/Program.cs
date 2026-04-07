using ExternalLoginAnd2FA.Domain.Email;
using ExternalLoginAnd2FA.Domain.Utilities;
using ExternalLoginAnd2FA.Infrastructure.Data;
using ExternalLoginAnd2FA.Infrastructure.Extensions;
using ExternalLoginAnd2FA.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

//Bootstrap logger configuration
#region bootstrap_logger
Log.Logger = new LoggerConfiguration()
                 .WriteTo.File("Logs/log_book-.log", rollingInterval: RollingInterval.Day)
                 .CreateBootstrapLogger();
#endregion
try
{
    var builder = WebApplication.CreateBuilder(args);
    var googleClientId = builder.Configuration["web:client_id"];
    var googleClientSecret = builder.Configuration["web:client_secret"];
    var facebookAppId = builder.Configuration["Facebook:AppId"];
    var facebookAppSecret = builder.Configuration["Facebook:AppSecret"];

    // Add services to the container.
    //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
    //    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var connectionString = builder.Configuration.GetConnectionString("PostGreSqlConnection") ??
        throw new InvalidOperationException("Connection string 'PostGreSqlConnection' not found.");
    var migrationAssembly = Assembly.GetAssembly(typeof(ApplicationDbContext));

    //general logger configuration
    #region Serilog Configuration
    builder.Host.UseSerilog((context, lc) => lc
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)
        );
    #endregion
    builder.Services.AddApplicationDbContext(connectionString, migrationAssembly);
    builder.Services.AddSingleton<IEmailUtility, EmailUtility>();
    #region google login
    builder.Services.AddAuthentication().AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = googleClientId;
        googleOptions.ClientSecret = googleClientSecret;
    });
    #endregion
    builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = facebookAppId;
        facebookOptions.AppSecret = facebookAppSecret;
    });
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddModifiedIdentity();
    
    #region Mapping Mailtrap configuration with SmtpSettings class from appsettings.json
    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
    #endregion
    #region Mapping Ipstack API key with Ipstack class from secrets.json
    builder.Services.Configure<IpStack>(builder.Configuration.GetSection("Ipstack"));
    #endregion
    #region Mapping Userstack API key with Userstack class from secrets.json
    builder.Services.Configure<Userstack>(builder.Configuration.GetSection("Userstack"));
    #endregion
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    builder.Services.AddHttpClient();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapRazorPages()
       .WithStaticAssets();

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex,"Application crashed");
}
finally
{
    Log.CloseAndFlush();
}