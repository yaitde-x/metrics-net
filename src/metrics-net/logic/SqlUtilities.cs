
using System.Data;

namespace MetricsNet;

public static class SqlUtilities
{
    public static IDbDataParameter CreateParameter(IDbCommand command, string parameterName, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = parameterName;
        parameter.Value = value;

        return parameter;
    }
}