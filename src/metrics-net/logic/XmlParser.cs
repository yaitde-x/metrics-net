using System.Xml;

namespace MetricsNet;
public class XmlMetricsReportParser
{
    // private class CodeMetricsReportContext
    // {

    //     public CodeMetricsReportContext(CodeMetricsReport report)
    //     {
    //         Report = report;
    //     }
    //     public CodeMetricsReport Report { get; }
    //     public CodeTarget? CurrentTarget { get; set; }
    //     public CodeAssembly? CurrentAssembly { get; set; }
    //     public CodeNamespace? CurrentNamespace { get; set; }
    //     public CodeType? CurrentType { get; set; }
    //     public CodeMember? CurrentMember { get; set; }

    //     public ICodeNode? CurrentNode { get; set; }

    // }

    public CodeMetricsReport Parse(Stream? stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        var report = new CodeMetricsReport();
        var nodeDispatch = new Dictionary<string, Func<XmlTextReader, CodeMetricsReport, ICodeNode?, ICodeNode?>>()
        {
            {"Target", ProcessTargetNode},
            {"Assembly", ProcessAssemblyNode},
            {"Namespace", ProcessNamespaceNode},
            {"NamedType", ProcessTypeNode},
            {"Method", ProcessMemberNode},
            {"Field", ProcessMemberNode},
            {"Property", ProcessMemberNode},
            {"Metric", ProcessMetricsNode}
        };

        var activeMetricsNode = default(ICodeNode);
        using (var reader = new XmlTextReader(stream))
        {
            while (reader.Read())
            {
                if (nodeDispatch.ContainsKey(reader.Name) && reader.NodeType != XmlNodeType.EndElement)
                {
                    var proposedNewActiveNode = nodeDispatch[reader.Name](reader, report, activeMetricsNode);
                    if (proposedNewActiveNode != null)
                        activeMetricsNode = proposedNewActiveNode;

                }
            }
        }

        return report;
    }

    private ICodeNode? ProcessTargetNode(XmlTextReader reader, CodeMetricsReport report, ICodeNode? activeNode)
    {
        var target = new CodeTarget() { Name = reader.GetAttribute("Name") };
        report.Targets.Add(target);
        return target;
    }

    private ICodeNode? ProcessAssemblyNode(XmlTextReader reader, CodeMetricsReport report, ICodeNode? activeNode)
    {
        var node = new CodeAssembly()
        {
            Name = reader.GetAttribute("Name"),
        };

        var target = report.Targets[report.Targets.Count - 1];
        target.Assemblies.Add(node);

        return node;
    }

    private ICodeNode? ProcessNamespaceNode(XmlTextReader reader, CodeMetricsReport report, ICodeNode? activeNode)
    {
        var node = new CodeNamespace()
        {
            Name = reader.GetAttribute("Name"),
        };

        var target = report.Targets[report.Targets.Count - 1];
        target.Assemblies[target.Assemblies.Count - 1].Namespaces.Add(node);

        return node;
    }

    private ICodeNode? ProcessTypeNode(XmlTextReader reader, CodeMetricsReport report, ICodeNode? activeNode)
    {
        var node = new CodeType()
        {
            Name = reader.GetAttribute("Name"),
        };

        var target = report.Targets[report.Targets.Count - 1];
        var assembly = target.Assemblies[target.Assemblies.Count - 1];
        assembly.Namespaces[assembly.Namespaces.Count - 1].Types.Add(node);

        return node;
    }

    private ICodeNode? ProcessMemberNode(XmlTextReader reader, CodeMetricsReport report, ICodeNode? activeNode)
    {
        var node = new CodeMember()
        {
            Name = reader.GetAttribute("Name"),
        };

        var target = report.Targets[report.Targets.Count - 1];
        var assembly = target.Assemblies[target.Assemblies.Count - 1];
        var ns = assembly.Namespaces[assembly.Namespaces.Count - 1];
        ns.Types[ns.Types.Count - 1].Members.Add(node);

        return node;
    }

    private ICodeNode? ProcessMetricsNode(XmlTextReader reader, CodeMetricsReport report, ICodeNode? activeNode)
    {
        var name = reader.GetAttribute("Name");
        var value = Convert.ToInt32(reader.GetAttribute("Value"));

        if (activeNode == null || name == null)
            return default(ICodeNode);

        if (activeNode.Metrics == null)
            activeNode.Metrics = new CodeMetrics();

        if (name.Equals("MaintainabilityIndex"))
        {
            activeNode.Metrics.MaintainabilityIndex = value;
        }
        else if (name.Equals("CyclomaticComplexity"))
        {
            activeNode.Metrics.CyclomaticComplexity = value;
        }
        else if (name.Equals("ClassCoupling"))
        {
            activeNode.Metrics.ClassCoupling = value;
        }
        else if (name.Equals("DepthOfInheritance"))
        {
            activeNode.Metrics.DepthOfInheritance = value;
        }
        else if (name.Equals("SourceLines"))
        {
            activeNode.Metrics.SourceLines = value;
        }
        else if (name.Equals("ExecutableLines"))
        {
            activeNode.Metrics.ExecutableLines = value;
        }

        return default(ICodeNode);
    }
}