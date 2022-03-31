
namespace MetricsNet;

public class StackTraceParser
{

    public StackTrace? ParseStackTrace(string? stackTrace)
    {
        if (stackTrace == default)
            return default;

        var lines = stackTrace.Split(new char[] { '\r', '\n' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var deps = new List<StackDependency>();
        var originator = default(MemberSignature);
        var offender = default(MemberSignature);

        foreach (var line in lines)
        {
            var callAndSource = line.Split(new string[] { "at ", " in " }, StringSplitOptions.RemoveEmptyEntries);
            var traceParts = callAndSource[0].Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            var member = traceParts[traceParts.Length - 1];
            var type = traceParts[traceParts.Length - 2];
            var ns = string.Join('.', traceParts, 0, traceParts.Length - 2);
            var filePath = callAndSource[1];
            var lineNumber = string.Empty;

            if (filePath.IndexOf(':') > 0)
            {
                var sourceAndLine = filePath.Split(':', StringSplitOptions.TrimEntries);
                filePath = sourceAndLine[0];
                lineNumber = sourceAndLine[1];
            }

            var signature = new MemberSignature(ns, type, member);

            if (originator == null)
                offender = signature;
            else
                deps.Add(new StackDependency(signature, originator));

            originator = signature;
        }

        if (offender == default)
            throw new StackParsingException("A stack must have an originator");

        if (originator == default)
            throw new StackParsingException("A stack must have an offender");

        deps.Reverse();
        return new StackTrace(originator, offender, deps.ToArray());
    }
}