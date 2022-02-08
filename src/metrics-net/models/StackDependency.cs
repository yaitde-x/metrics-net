namespace MetricsNet;

public class StackDependency
{
    public StackDependency(MemberSignature caller, MemberSignature callee) : this(caller, callee, RelationshipType.Calls)
    {
    }

    public StackDependency(MemberSignature caller, MemberSignature callee, RelationshipType type)
    {
        Caller = caller;
        Callee = callee;
        Type = type;
    }

    public MemberSignature Caller { get; private set; }
    public MemberSignature Callee { get; private set; }
    public RelationshipType Type { get; private set; }

}
