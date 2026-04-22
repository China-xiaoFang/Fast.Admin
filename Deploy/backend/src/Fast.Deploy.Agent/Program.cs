using Fast.Deploy.Agent.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<DeployExecutor>();
builder.Services.AddSingleton<HealthChecker>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();
