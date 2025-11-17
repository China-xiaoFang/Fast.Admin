using Fast.Cache;
using Fast.Core;
using Fast.DependencyInjection;
using Fast.DynamicApplication;
using Fast.JwtBearer;
using Fast.Logging;
using Fast.NET.Core;
using Fast.OpenApi;
using Fast.Runtime;
using Fast.Scheduler;
using Fast.Scheduler.HostedService;
using Fast.Serialization;
using Fast.SqlSugar;
using Fast.Swagger;
using Fast.UnifyResult;
using IGeekFan.AspNetCore.Knife4jUI;

var builder = WebApplication.CreateBuilder(args);

// 初始化框架
builder.Initialize();

// 添加序列化服务
builder.Services.AddSerialization();

// 添加日志服务
builder.Services.AddLoggingService(builder.Configuration);

// 添加跨域服务
builder.Services.AddCorsAccessor(builder.Configuration);

// 添加 Gzip 压缩服务
builder.Services.AddGzipCompression();

// 添加依赖注入服务
builder.Services.AddDependencyInjection();

// 邮件配置验证
builder.Services.AddConfigurableOptions<MailSettingsOptions>();

// 上传文件配置验证
builder.Services.AddConfigurableOptions<UploadFileSettingsOptions>();

// 添加缓存服务
builder.Services.AddCache();

var redisOptions = builder.Configuration.GetSection("RedisSettings")
    .Get<RedisSettingsOptions>();
if (redisOptions != null)
{
    // 添加分布式缓存
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration =
            $"{redisOptions.ServiceIp}:{redisOptions.Port ?? 6379},password={redisOptions.DbPwd},defaultDatabase={redisOptions.DbName ?? 2},abortConnect=false";
        options.InstanceName = $"{nameof(Fast)}:";
    });
}

// 添加雪花Id
builder.Services.AddSnowflake(builder.Configuration);

// 添加 SqlSugar
builder.Services.AddSqlSugar(builder.Configuration, builder.Environment);

builder.Services.AddHttpClient();

// 添加 JwtBearer 授权
builder.Services.AddJwtBearer(builder.Configuration);

// Add Controllers.
builder.Services.AddControllers()
    // 请求日志拦截
    .AddMvcFilter<RequestActionFilter>()
    .AddSerialization();

// 添加动态Api服务
builder.Services.AddDynamicApplication();

// 添加规范化返回服务
builder.Services.AddUnifyResult();

// 添加 Swagger 服务
builder.Services.AddSwaggerDocuments(builder.Configuration);

// 添加 OpenApi 服务
builder.Services.AddOpenApi(builder.Configuration);

// 添加 Swagger Newtonsoft.Json 库支持
builder.Services.AddSwaggerGenNewtonsoftSupport();

// 添加 Quartz 服务
builder.Services.AddQuartzService(builder.Configuration);

// 添加删除日志托管服务
builder.Services.AddHostedService<DeleteLogHostedService>();

// 添加同步 Api 托管服务
builder.Services.AddHostedService<SyncApiHostedService>();

// 添加同步字典托管服务
builder.Services.AddHostedService<SyncDictionaryHostedService>();

// 添加生成Api文件托管服务
builder.Services.AddHostedService<GenerateApiFileHostedService>();

// 添加调度后台托管服务
builder.Services.AddHostedService<SchedulerHostedService>();

var app = builder.Build();

//// 强制使用 Https
//app.UseHttpsRedirection();

// 启用静态文件
app.UseStaticFiles();

// 启用 Body 重复读功能
app.EnableBuffering();

// 请求中间件
app.UseMiddleware<RequestMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 启用 Swagger 文档
app.UseSwaggerDocuments();

// 配置 Swagger Knife4UI
app.UseKnife4UI(options =>
{
    options.RoutePrefix = "knife4j";
    foreach (var groupInfo in SwaggerDocumentBuilder.GetOpenApiGroups())
    {
        options.SwaggerEndpoint("/" + groupInfo.RouteTemplate, groupInfo.Title);
    }
});

app.MapControllers();

app.Run();