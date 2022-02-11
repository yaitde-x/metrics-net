
using Xunit;

namespace MetricsNet;

public class TypeScannerTests
{
    [Fact]
    public void HappyPath()
    {
        var scanner = new TypeScanner();
        scanner.Scan("/Users/sakamoto/Code/metrics-net/src/metrics-net/bin/Debug/net6.0");
    }
}