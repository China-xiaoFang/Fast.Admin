using DeploymentCenter.Server;
using DeploymentCenter.Server.Controllers;
using DeploymentCenter.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<IDeploymentRepository, InMemoryDeploymentRepository>();
builder.Services.AddSingleton<IAgentControlClient, SignalRAgentControlClient>();
builder.Services.AddSingleton<IDeploymentEventPublisher, SignalRDeploymentEventPublisher>();
builder.Services.AddSingleton<IDeploymentStrategy, SingleDeploymentStrategy>();
builder.Services.AddSingleton<IDeploymentStrategy, RollingDeploymentStrategy>();
builder.Services.AddSingleton<IDeploymentStrategy, BlueGreenDeploymentStrategy>();
builder.Services.AddSingleton<IDeploymentOrchestrator, DeploymentOrchestrator>();
builder.Services.AddSingleton<IStorageProvider>(new LocalStorageProvider(Path.Combine(builder.Environment.ContentRootPath, "data", "packages")));
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
        .AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true).AllowCredentials()));

var app = builder.Build();
app.UseCors();
app.MapControllers();
app.MapHub<DeploymentHub>("/hubs/deployment");
app.MapHub<AgentHub>("/hubs/agent");
app.MapHealthChecks("/health");
app.Run();
