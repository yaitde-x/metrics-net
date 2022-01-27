using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("metrics_net_test.test_files.metrics.xml"))
        {
            var parser = new XmlMetricsReportParser();
            var report = parser.Parse(stream);

            Assert.NotNull(report);
            Assert.Single(report.Targets);

            var target = report.Targets[0];
            Assert.Equal("ConsoleApp20.csproj", target.Name);

            Assert.Single(target.Assemblies);
            var assembly = target.Assemblies[0];
            Assert.Equal("ConsoleApp20, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", assembly.Name);

            Assert.Single(assembly.Namespaces);
            var ns = assembly.Namespaces[0];
            Assert.Equal("ConsoleApp20", ns.Name);

            Assert.Single(ns.Types);
            var type = ns.Types[0];
            Assert.Equal("Program", type.Name);

            Assert.Single(type.Members);
            var method = type.Members[0];
            Assert.Equal("void Program.Main(string[] args)", method.Name);
        }
    }

    [Fact]
    public void NotSoHappy()
    {
        var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        var result = embeddedProvider.GetDirectoryContents("metrics_net_test").ToList(); ;

        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("metrics_net_test.test_files.metrics-two-targets.xml"))
        {
            var parser = new XmlMetricsReportParser();
            var report = parser.Parse(stream);

            var ns = report.Targets[0].Assemblies[0].Namespaces[0];
            Assert.Equal("CompanyX.Services.PrintManager.Server", ns.Name);

            Assert.Equal(5, ns.Types.Count);
            var type = ns.Types[4];
            Assert.Equal("RegistrationManager", type.Name);

            Assert.Equal(6, type.Members.Count);
            var member = type.Members[0];
            Assert.Equal("MgrPrintManager RegistrationManager._MgrPrintManager", member.Name);

            Assert.Equal(2, report.Targets.Count);
            Assert.Equal(14, report.Targets[1].Assemblies[0].Namespaces[0].Types.Count);

        }
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