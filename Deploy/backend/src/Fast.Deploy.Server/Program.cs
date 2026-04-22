using Fast.Deploy.Server.Data;
using Fast.Deploy.Server.Hubs;
using Fast.Deploy.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DeployDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=deploy.db"));

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();

builder.Services.AddScoped<LogService>();
builder.Services.AddSingleton<DeployService>();

builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DeployDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors();
app.UseStaticFiles();
app.UseRouting();

// Serve uploaded packages
var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "uploads");
Directory.CreateDirectory(uploadsPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

app.MapControllers();
app.MapHub<DeployHub>("/hubs/deploy");

app.Run();
