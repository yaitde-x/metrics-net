namespace MetricsNet;

public class StackTrace
{
    public StackTrace(MemberSignature originator, MemberSignature offender, StackDependency[] callChain)
    {
        Originator = originator;
        Offender = offender;
        CallChain = callChain;
    }

    public MemberSignature Originator { get; private set; }

    public MemberSignature Offender { get; private set; }
    public StackDependency[] CallChain { get; private set; }
}
