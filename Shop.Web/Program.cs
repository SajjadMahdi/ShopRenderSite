using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.Infra.Data.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Shop.Infra.IoC;
using GoogleReCaptcha.V3.Interface;
using GoogleReCaptcha.V3;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddControllersWithViews();

#region Context
builder.Services.AddDbContext<ShopDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
});
#endregion

#region Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/log-Out";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
});
#endregion

#region IoC
DependencyContainer.RegisterService(builder.Services);
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));

builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Ensure the database is created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ShopDbContext>();
        dbContext.Database.EnsureCreated(); // Creates the database if it doesn't exist
                                            // Alternatively, use dbContext.Database.Migrate() to apply pending migrations
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}

app.Run();