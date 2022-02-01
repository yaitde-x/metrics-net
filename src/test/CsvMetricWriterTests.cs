

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MetricsNet;

public class CsvMetricWriterTests
{
    [Fact]
    public async Task HappyPath()
    {
        var testObject = new CsvMetricWriter();
        var model = new CodeMetricRecord(DateTime.Parse("2020-01-01"), "target", "assembly", "namespace", "type", "member", "simple","name","return", Language.CSharp)
        {
            MaintainabilityIndex = 1,
            CyclomaticComplexity = 2,
            ClassCoupling = 3,
            DepthOfInheritance = 4,
            LinesOfCode = 5
        };

        using var testStream = new MemoryStream();
        await testObject.Write(testStream, new[] { model });

        testStream.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(testStream);

        var buffer = await reader.ReadToEndAsync(); 
        Assert.Equal("\"01/01/2020 00:00:00\",\"target\",\"assembly\",\"namespace\",\"type\",\"member\",1,2,3,4,5\n", buffer);

    }

    private Stream CreateReadableStream(string streamBuffer)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(streamBuffer));
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }
}
