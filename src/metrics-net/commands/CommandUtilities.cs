
using System.CommandLine;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsNet;

public static class CommandUtilities {
    public static IServiceCollection AddCliCommands(this IServiceCollection services)
    {
        services.AddSingleton<Command, ParseCommand>();
        return services;
    }

    public static Command BuildDispatcher(ServiceProvider serviceProvider)
    {
        var rootCommand = new RootCommand();

        foreach (Command command in serviceProvider.GetServices<Command>())
        {
            rootCommand.AddCommand(command);
        }

        return rootCommand;
    }
}