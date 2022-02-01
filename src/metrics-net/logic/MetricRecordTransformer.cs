namespace MetricsNet;

public class MetricRecordTransformer
{
    public CodeMetricRecord[] Transform(CodeMetricsReport report)
    {
        var result = new List<CodeMetricRecord>();

        foreach (var target in report.Targets)
        {
            foreach (var assembly in target.Assemblies)
            {
                foreach (var ns in assembly.Namespaces)
                {
                    foreach (var type in ns.Types)
                    {
                        foreach (var member in type.Members)
                        {
                            var data = CodemetricsUtilities.ProcessMethodSignature(member.Name);
                            if (data == null)
                                continue;

                            var record = new CodeMetricRecord(DateTime.Now,
                                                            target.Name ?? string.Empty,
                                                            assembly.Name ?? string.Empty,
                                                            ns.Name ?? string.Empty,
                                                            type.Name ?? string.Empty,
                                                            member.Name ?? string.Empty,
                                                            data.SimplifiedSignature,
                                                            data.MemberName,
                                                            data.ReturnType,
                                                            data.Language
                                                            );

                            if (member.Metrics != null)
                            {
                                record.MaintainabilityIndex = member.Metrics.MaintainabilityIndex;
                                record.ClassCoupling = member.Metrics.ClassCoupling;
                                record.CyclomaticComplexity = member.Metrics.CyclomaticComplexity;
                                record.DepthOfInheritance = member.Metrics.DepthOfInheritance;
                                record.LinesOfCode = member.Metrics.SourceLines;
                            }

                            result.Add(record);
                        }
                    }
                }
            }
        }
        return result.ToArray();
    }
}
