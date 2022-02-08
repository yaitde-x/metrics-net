namespace MetricsNet;

public class MemberSignature
{

    public MemberSignature(string nameSpace, string type, string member)
    {
        NameSpace = nameSpace;
        Type = type;
        Member = member;
    }

    public string NameSpace { get; private set; }
    public string Type { get; private set; }
    public string Member { get; private set; }
}
