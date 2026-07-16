using DeploymentCenter.Agent;
using DeploymentCenter.Shared;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IAgentTaskExecutor, ScriptAgentTaskExecutor>();
builder.Services.AddHostedService<Worker>();
var host = builder.Build();
host.Run();
