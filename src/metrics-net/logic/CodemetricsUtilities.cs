namespace MetricsNet;

public static class CodemetricsUtilities
{
    public static MethodData? ProcessMethodSignature(string? methodSignature)
    {
        if (methodSignature == null)
            return null;

        var language = GetLanguageFromSignature(methodSignature);

        return language == Language.VBNet ? ProcessVBNetSignature(methodSignature) : ProcessCSharpSignature(methodSignature);

    }

    private static MethodData ProcessVBNetSignature(string methodSignature)
    {
        var parts = methodSignature.Split(' ');
        var methodType = parts[0];
        var returnType = methodType.Equals("Sub") ? "void" : parts[parts.Length - 1];

        var paramStart = methodSignature.IndexOf("(");
        var paramEnd = methodSignature.IndexOf(")");

        var paramList = methodSignature.Substring(paramStart + 1, paramEnd - paramStart - 1).Trim();

        var temp = paramList.Split(new[] { ',' });
        var temp2 = temp.Select(p => p.Split(' ', StringSplitOptions.RemoveEmptyEntries).Last().Trim());
        var paramsSplit = string.IsNullOrEmpty(paramList) ? "" : string.Join(", ", temp2);
        var memberName = methodSignature.Substring(0, paramStart).Split(new char[] { '.' }, StringSplitOptions.TrimEntries)[1].Trim();

        var simplified =$"{memberName}({paramsSplit}) : {returnType}";
        return new MethodData(Language.VBNet, methodSignature, simplified, memberName, returnType);
    }

    private static MethodData ProcessCSharpSignature(string methodSignature)
    {
        var parts = methodSignature.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var returnType = parts[0];
        var fqmn = methodSignature.Substring(methodSignature.IndexOf(returnType));

        var paramStart = methodSignature.IndexOf("(");
        var paramEnd = methodSignature.IndexOf(")");

        var paramList = methodSignature.Substring(paramStart + 1, paramEnd - paramStart - 1).Trim();
        var temp = paramList.Split(new[] { ',' });
        var temp2 = temp.Select(p => p.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].Trim());
        var paramsSplit = string.IsNullOrEmpty(paramList) ? "" : string.Join(", ", temp2);
        var memberName = methodSignature.Substring(0, paramStart).Split(new char[] { '.' }, StringSplitOptions.TrimEntries)[1].Trim();

        var simplified = $"{memberName}({paramsSplit}) : {returnType}";

        return new MethodData(Language.CSharp, methodSignature, simplified, memberName, returnType);
    }

    private static string ExtractCSharpMemberName(string methodSignature)
    {
        var parts = methodSignature.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var returnType = parts[0];
        var memberName = parts[1];

        return parts[1].Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("(", string.Empty);
    }

    private static Language GetLanguageFromSignature(string methodSignature)
    {
        return methodSignature.StartsWith("Sub", StringComparison.OrdinalIgnoreCase) || methodSignature.StartsWith("Function", StringComparison.OrdinalIgnoreCase) ?
                Language.VBNet : Language.CSharp;

    }
}
