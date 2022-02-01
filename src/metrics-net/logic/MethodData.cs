namespace MetricsNet;

public class MethodData
{
    public MethodData(Language language, string methodSignature, string simplifiedSignature, string memberName) {
        MethodSignature = methodSignature;
        SimplifiedSignature = simplifiedSignature;
        MemberName = memberName;
        Language = language;
    }

    public Language Language { get;set;}
    public string MethodSignature { get; set; }
    public string SimplifiedSignature { get; set; }
    public string MemberName { get; set; }
    public string[]? ParameterTypes { get; set; }
}
