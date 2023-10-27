using Fast.CorsAccessor.Extensions;
using Fast.DependencyInjection.Extensions;
using Fast.Logging.Extensions;
using Fast.Mapster.Extensions;
using Fast.NET.Core;
using Fast.NET.Core.Extensions;
using Fast.Serialization.Extensions;
using Fast.SpecificationProcessor.DataValidation.Extensions;
using Fast.SpecificationProcessor.DynamicApplication.Extensions;
using Fast.SpecificationProcessor.SpecificationDocument.Extensions;
using Fast.SpecificationProcessor.UnifyResult.Extensions;
using Fast.Test.Api;

var builder = WebApplication.CreateBuilder(args).Initialize();

// Customize the console log output template.
builder.Logging.AddConsoleFormatter(options => { options.DateFormat = "yyyy-MM-dd HH:mm:ss"; });

// ��־
builder.AddLogging();

// ��������
builder.Services.AddCorsAccessor(builder.Configuration);

// GZIP ѹ��
builder.Services.AddGzipBrotliCompression();

// JSON ���л�����
builder.Services.AddJsonOptions();

// ע��ȫ������ע��
builder.Services.AddDependencyInjection();

// ��Ӷ���ӳ��
builder.Services.AddObjectMapper();

builder.Services.AddControllers();

var a = App.EffectiveTypes;

// �ĵ�
builder.AddSpecificationDocuments();

// ��̬ API
builder.Services.AddDynamicApplication();

// ������֤
builder.Services.AddDataValidation();

// �Ѻ��쳣
//builder.Services.AddFriendlyException();

// �淶����
builder.Services.AddUnifyResult<RESTfulResultProvider>();


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

// ��־�м��
app.UseLogging();

// �����м��
app.UseCorsAccessor();

// ����ע���м��
app.UseDependencyInjection();

// Enable compression.
//app.UseResponseCompression();

// Add the status code interception middleware.
app.UseUnifyResultStatusCodes();


app.UseStaticFiles();

app.UseRouting();

// Here, the default address is/API if no argument is entered, and/directory if string.empty is entered. If any string is entered, the/arbitrary string directory.
app.UseSpecificationDocuments();

app.MapControllers();

app.Run();