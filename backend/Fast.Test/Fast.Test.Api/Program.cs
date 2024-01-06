using Fast.NET.Core.Extensions;
using Fast.SpecificationProcessor.Swagger.Extensions;
using Fast.Test.Api;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

var builder = WebApplication.CreateBuilder(args).Initialize();

//// ��־
//builder.Services.AddLogging(builder.Configuration);

//// ��������
//builder.Services.AddCorsAccessor(builder.Configuration);

//// GZIP ѹ��
//builder.Services.AddGzipBrotliCompression();

//// JSON ���л�����
//builder.Services.AddJsonOptions();

//// ע��ȫ������ע��
//builder.Services.AddDependencyInjection();

//// ��Ӷ���ӳ��
//builder.Services.AddObjectMapper();

//builder.Services.AddJwt();

//builder.Services.AddSqlSugar(builder.Configuration);

var a1 = builder.Services.FirstOrDefault(f => f.ServiceType == typeof(ApplicationPartManager));

var a = builder.Services.FirstOrDefault(f => f.ServiceType == typeof(ApplicationPartManager))?.ImplementationInstance as
    ApplicationPartManager;

builder.Services.AddControllers();

var b1 = builder.Services.FirstOrDefault(f => f.ServiceType == typeof(ApplicationPartManager));

var b = builder.Services.FirstOrDefault(f => f.ServiceType == typeof(ApplicationPartManager))?.ImplementationInstance as
    ApplicationPartManager;

// �ĵ�
builder.Services.AddSwaggerDocument(builder.Configuration);

//// ��̬ API
//builder.Services.AddDynamicApplication();

//// ������֤
//builder.Services.AddDataValidation();

//// �Ѻ��쳣
//builder.Services.AddFriendlyException();

//// �淶����
//builder.Services.AddUnifyResult<RESTfulResultProvider>();


//builder.Services.AddSqlSugar();

// Add event bus.
//builder.Services.AddEventBus(options =>
//{
//    // �������ӹ���
//    var factory = App.GetConfig<ConnectionFactory>("RabbitMQConnection");

//    // ����Ĭ���ڴ�ͨ���¼�Դ����
//    var mqEventSourceStorer = new RabbitMQEventSourceStorer(factory, "WMS.Event.Bus", 3000);

//    // �滻Ĭ���¼����ߴ洢��
//    options.ReplaceStorer(serviceProvider => mqEventSourceStorer);

//    // ע���¼��������Է���
//    options.AddFallbackPolicy<EventFallbackPolicy>();
//});
//builder.Services.AddEventBus();

var app = builder.Build();

// Mandatory Https.
app.UseHttpsRedirection();

//// �����м��
//app.UseCorsAccessor();

// Enable compression.
//app.UseResponseCompression();

//// Add the status code interception middleware.
//app.UseUnifyResultStatusCodes();


app.UseStaticFiles();

app.UseRouting();

// Here, the default address is/API if no argument is entered, and/directory if string.empty is entered. If any string is entered, the/arbitrary string directory.
app.UseSwaggerDocument();

app.MapControllers();

app.Run();