// See https://aka.ms/new-console-template for more information
using ConsoleApp2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#if false
ILog log = LogManager.GetLogger(typeof(Program));

var assembly = Assembly.GetEntryAssembly();
if (assembly is  null) throw new InvalidOperationException("Could not determine entry assembly ");

var logRepository = LogManager.GetRepository(assembly);
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

for (int i = 0; i < 1024 *  100; i++)
{
    log.Info("Application starting...");
    log.Debug("This is a debug message");
    log.Error("This is an error message");
}
#endif


void ConfigureLogging(ILoggingBuilder logging)
{
    logging.ClearProviders();
    logging.AddProvider(new Log4NetProvider("log4net.config"));
}

void ConfigureDelegate(IServiceCollection services)
{
    services.AddTransient<MyWorker>();
}

var defaultBuilder = Host.CreateDefaultBuilder(args);
defaultBuilder.ConfigureLogging(ConfigureLogging);
defaultBuilder.ConfigureServices(ConfigureDelegate);
using var host = defaultBuilder.Build();
    
var worker = host.Services.GetRequiredService<MyWorker>();
await worker.RunAsync();
