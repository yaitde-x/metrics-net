namespace MetricsNet;

public class MethodData
{
    public MethodData(Language language, string methodSignature, string simplifiedSignature, 
                        string memberName, string returnType) {
        MethodSignature = methodSignature;
        SimplifiedSignature = simplifiedSignature;
        MemberName = memberName;
        Language = language;
        ReturnType = returnType;
    }

    public Language Language { get;set;}
    public string MethodSignature { get; set; }
    public string SimplifiedSignature { get; set; }
    public string MemberName { get; set; }
    public string ReturnType { get; set; }
    public string[]? ParameterTypes { get; set; }
}
