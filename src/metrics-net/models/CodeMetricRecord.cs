namespace MetricsNet;

public class CodeMetricRecord
{
    public CodeMetricRecord(
        DateTime period, string target, string assembly, string ns, string type, string raw,
                string simplified, string memberName, string returnType, Language language)
    {
        Period = period;
        Assembly = assembly;
        Target = target;
        Namespace = ns;
        Type = type;
        Signature = simplified;  
        MemberName = memberName;
        Raw = raw;
        ReturnType = returnType;
        Language = language;
    }

    public DateTime Period { get; set; }
    public string Target { get; set; }
    public string Assembly { get; set; }
    public string Namespace { get; set; }
    public string Type { get; set; }
    public string Signature { get; set; }
    public Language Language {get;set;}
    public string Raw {get;set;}
    public string MemberName {get;set;}
    public string ReturnType {get;set;}
    public int MaintainabilityIndex { get; set; }
    public int CyclomaticComplexity { get; set; }
    public int ClassCoupling { get; set; }
    public int DepthOfInheritance { get; set; }
    public int LinesOfCode { get; set; }
}
