namespace MetricsNet;

public interface IOutput {
    void WriteLine(string message);
}

public class ConsoleOutput : IOutput
{
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}