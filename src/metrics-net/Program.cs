
using System.CommandLine;
using MetricsNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var output = new ConsoleOutput();

var serviceProvider = new ServiceCollection()
            .AddLogging(opt =>
                {
                    opt.AddConsole();
                })
            .AddCliCommands()
            .BuildServiceProvider();


if (serviceProvider == null)
{
    return 1;
}

var logger = serviceProvider.GetService<ILoggerFactory>()
    ?.CreateLogger<Program>();
logger?.LogDebug("Start");

var dispatcher = CommandUtilities.BuildDispatcher(serviceProvider);

try
{
    //var result = dispatcher.Invoke(args);
    var result = dispatcher.Invoke(args);

    logger?.LogDebug("All done!");

    return result;
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message);
    return -1;
}