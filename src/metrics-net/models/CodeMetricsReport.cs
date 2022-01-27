namespace MetricsNet;

public interface ICodeNode {
    public string? Name { get; set; }
    public CodeMetrics? Metrics { get; set; }
}
public class CodeMetricsReport
{
    public List<CodeTarget> Targets { get; set; } = new List<CodeTarget>();
}

public class CodeTarget : ICodeNode
{
    public string? Name { get; set; }
    public CodeMetrics? Metrics { get; set; }
    public List<CodeAssembly> Assemblies { get; set; } = new List<CodeAssembly>();
}

public class CodeAssembly : ICodeNode
{
    public string? Name { get; set; }
    public CodeMetrics? Metrics { get; set; }
    public List<CodeNamespace> Namespaces { get; set; } = new List<CodeNamespace>();
}

public class CodeNamespace : ICodeNode
{
    public string? Name { get; set; }
    public CodeMetrics? Metrics { get; set; }
    public List<CodeType> Types { get; set; } = new List<CodeType>();
}

public class CodeType : ICodeNode
{
    public string? Name { get; set; }
    public CodeMetrics? Metrics { get; set; }
    public List<CodeMember> Members { get; set; } = new List<CodeMember>();
}

public class CodeMember : ICodeNode
{
    public string? Name { get; set; }
    public CodeMetrics? Metrics { get; set; }
}

public class CodeMetrics
{
    public int MaintainabilityIndex { get; set; }
    public int CyclomaticComplexity { get; set; }
    public int ClassCoupling { get; set; }
    public int DepthOfInheritance { get; set; }
    public int SourceLines { get; set; }
    public int ExecutableLines { get; set; }

}