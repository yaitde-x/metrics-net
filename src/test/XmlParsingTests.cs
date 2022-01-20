using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.FileProviders;
using Xunit;

namespace MetricsNet;

public class XmlParsingTests
{
    [Fact]
    public void HappyPath()
    {
        var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        var result = embeddedProvider.GetDirectoryContents("metrics_net_test").ToList(); ;

        // Console.WriteLine(Assembly.GetExecutingAssembly().FullName);
        // string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        // foreach (string resourceName in resourceNames)
        // {
        //     System.Diagnostics.Trace.WriteLine(resourceName);
        // }

        //var nestLevel = 0;

        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("metrics_net_test.test_files.metrics.xml"))
        {
            var parser = new XmlMetricsReportParser();
            var report = parser.Parse(stream);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(report, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("/Users/sakamoto/Temp/metrics.json", json);
        }
        // using (var reader = new XmlTextReader(stream))
        // {

        //     while (reader.Read())
        //     {
        //         if (!string.IsNullOrEmpty(reader.Name) && !reader.Name.Equals("xml"))
        //         {
        //             if (reader.NodeType == XmlNodeType.EndElement)
        //                 nestLevel--;

        //             var tag = $"{new string('\t', nestLevel)}{reader.Name}={reader.GetAttribute("Name")} ({reader.GetAttribute("Value")})";

        //             Console.WriteLine(tag);

        //             if (reader.IsStartElement() && !reader.IsEmptyElement)
        //                 nestLevel++;
        //         }
        //     }
        // }
    }

    [Fact]
    public async Task FusionMetrics()
    {
       
        using (var stream = File.OpenRead("/Users/sakamoto/Temp/fusion-metrics.xml"))
        {
            var parser = new XmlMetricsReportParser();
            var report = parser.Parse(stream);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(report, Newtonsoft.Json.Formatting.Indented);
            await File.WriteAllTextAsync("/Users/sakamoto/Temp/metrics.json", json);
        }
        // using (var reader = new XmlTextReader(stream))
        // {

        //     while (reader.Read())
        //     {
        //         if (!string.IsNullOrEmpty(reader.Name) && !reader.Name.Equals("xml"))
        //         {
        //             if (reader.NodeType == XmlNodeType.EndElement)
        //                 nestLevel--;

        //             var tag = $"{new string('\t', nestLevel)}{reader.Name}={reader.GetAttribute("Name")} ({reader.GetAttribute("Value")})";

        //             Console.WriteLine(tag);

        //             if (reader.IsStartElement() && !reader.IsEmptyElement)
        //                 nestLevel++;
        //         }
        //     }
        // }
    }
}