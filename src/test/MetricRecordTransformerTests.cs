using System.Collections.Generic;
using Xunit;

namespace MetricsNet;

public class MetricRecordTransformerTests
{
    [Fact]
    public void HappyPath()
    {
        var report = CreateTestReport();

        var testObject = new MetricRecordTransformer();
        var records = testObject.Transform(report);

        Assert.Single(records);

        var record = records[0];
        Assert.Equal("testtarget", record.Target);
        Assert.Equal("testassembly", record.Assembly);
        Assert.Equal("testnamespace", record.Namespace);
        Assert.Equal("testtype", record.Type);
        Assert.Equal("testmember", record.Member);
        Assert.Equal(1, record.MaintainabilityIndex);
        Assert.Equal(2, record.CyclomaticComplexity);
        Assert.Equal(3, record.ClassCoupling);
        Assert.Equal(4, record.DepthOfInheritance);
        Assert.Equal(5, record.LinesOfCode);
    }

    private CodeMetricsReport CreateTestReport()
    {
        return new CodeMetricsReport()
        {
            Targets = new List<CodeTarget>() {
                new CodeTarget() {
                    Name = "testtarget",
                    Assemblies = new List<CodeAssembly>() {
                        new CodeAssembly() {
                            Name = "testassembly",
                            Namespaces = new List<CodeNamespace>() {
                                new CodeNamespace() {
                                    Name = "testnamespace",
                                    Types = new List<CodeType>() {
                                        new CodeType() {
                                            Name = "testtype",
                                            Methods = new List<CodeMember>() {
                                                new CodeMember() {
                                                    Name = "testmember",
                                                    Metrics = new CodeMetrics() {
                                                        MaintainabilityIndex = 1,
                                                        CyclomaticComplexity = 2,
                                                        ClassCoupling = 3,
                                                        DepthOfInheritance = 4,
                                                        SourceLines = 5
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
    }
}