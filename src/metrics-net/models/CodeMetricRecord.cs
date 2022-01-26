namespace MetricsNet;

public class CodeMetricRecord
{
    public CodeMetricRecord(
        DateTime period, string target, string assembly, string ns, string type, string member)
    {
        Period = period;
        Assembly = assembly;
        Target = target;
        Namespace = ns;
        Type = type;
        Member = member;
    }

    public DateTime Period { get; set; }
    public string Target { get; set; }
    public string Assembly { get; set; }
    public string Namespace { get; set; }
    public string Type { get; set; }
    public string Member { get; set; }
    public int MaintainabilityIndex { get; set; }
    public int CyclomaticComplexity { get; set; }
    public int ClassCoupling { get; set; }
    public int DepthOfInheritance { get; set; }
    public int LinesOfCode { get; set; }
}
