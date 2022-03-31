
using System.CommandLine;

namespace MetricsNet;

public class StackTraceCommand : Command
{
    public StackTraceCommand()
        : base("stack", "parse a stack trace")
    {

    }

    public void Handle()
    {
        throw new NotImplementedException();
    }

}
