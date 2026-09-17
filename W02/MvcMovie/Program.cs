using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("MvcMovieContext")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Show a simple page for HTTP 404 responses.
app.UseStatusCodePages(async statusCodeContext =>
{
    var response = statusCodeContext.HttpContext.Response;

    if (response.StatusCode == StatusCodes.Status404NotFound)
    {
        response.ContentType = "text/html";
        await response.WriteAsync("""
            <!DOCTYPE html>
            <html>
            <head>
                <title>404 - Page Not Found</title>
                <meta charset="utf-8" />
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        text-align: center;
                        margin-top: 100px;
                    }

                    h1 {
                        font-size: 48px;
                    }

                    p {
                        font-size: 20px;
                    }

                    a {
                        text-decoration: none;
                    }
                </style>
            </head>
            <body>
                <h1>404 - Page Not Found</h1>
                <p>The movie you requested could not be found.</p>
                <p>
                    <a href="/Movies">Back to Movies</a>
                </p>
            </body>
            </html>
            """);
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();