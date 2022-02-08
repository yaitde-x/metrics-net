namespace MetricsNet;

public class ExceptionGenerator
{
    public static void GenerateException()
    {
        NestedException();
    }

    private static void NestedException()
    {

        throw new Exception("Test exception");
    }

}