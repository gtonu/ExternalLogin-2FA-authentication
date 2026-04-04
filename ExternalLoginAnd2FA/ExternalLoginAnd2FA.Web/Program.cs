using ExternalLoginAnd2FA.Infrastructure.Data;
using ExternalLoginAnd2FA.Infrastructure.Extensions;
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
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();

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


