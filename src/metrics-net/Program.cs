
using MetricsNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var output = new ConsoleOutput();

var serviceProvider = new ServiceCollection()
            .AddLogging(opt =>
                {
                    opt.AddConsole();
                })
            .BuildServiceProvider();


if (serviceProvider == null)
{
    return 1;
}

var logger = serviceProvider.GetService<ILoggerFactory>()
    ?.CreateLogger<Program>();
logger?.LogDebug("Star");


logger?.LogDebug("All done!");

return 0;