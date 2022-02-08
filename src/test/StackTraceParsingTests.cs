using System;
using Xunit;

namespace MetricsNet;

public class StackTraceParsingTests
{
    [Fact]
    public void HappyPath()
    {
        try
        {
            ExceptionGenerator.GenerateException();
        }
        catch (Exception ex)
        {
            var parser = new StackTraceParser();
            var stackTrace = parser.ParseStackTrace(ex.StackTrace);

            Assert.NotNull(stackTrace);
            Assert.Equal("HappyPath()", stackTrace?.Originator.Member);
            Assert.Equal("NestedException()", stackTrace?.Offender.Member);
            Assert.Equal(2, stackTrace?.CallChain.Length);

            var call1 = stackTrace?.CallChain[0];
            Assert.Equal("HappyPath()", call1?.Caller.Member);
            Assert.Equal("GenerateException()", call1?.Callee.Member);

            var call2 = stackTrace?.CallChain[1];
            Assert.Equal("GenerateException()", call2?.Caller.Member);
            Assert.Equal("NestedException()", call2?.Callee.Member);

        }
    }
}
