namespace ConsoleApp2;

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class MyWorker(ILogger<MyWorker> logger)
{
    public Task RunAsync()
    {
        for (int i = 0; i < 100; i++)
        {
            logger.LogInformation("Application starting...");
            logger.LogDebug("This is a debug message");
            logger.LogError("This is an error message");
        }
        return Task.CompletedTask;
    }
}