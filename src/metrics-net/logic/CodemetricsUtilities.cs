using System.Text;

namespace MetricsNet;

public static class CodeMetricsUtilities
{
    public static MethodData? ProcessMethodSignature(string? methodSignature)
    {
        if (methodSignature == null)
            return null;

        var language = GetLanguageFromSignature(methodSignature);
        return CreateMethodData(language, methodSignature);
        //return language == Language.VBNet ? ProcessVBNetSignature(methodSignature) : ProcessCSharpSignature(methodSignature);

    }

    private static MethodData CreateMethodData(Language language, string methodSignature)
    {
        return new MethodData(language, methodSignature, string.Empty, string.Empty, string.Empty);
    }

    enum SignatureType
    {
        DontKnow = 0, Sub = 1, Function = 2, Property = 3
    }

    enum SigState
    {
        DontKnow = 0, LookingForType = 1, LookingForMember = 2,
        LookingForParams = 3, InParam = 4, NextParam = 5, LookingForEnd = 6
    }

    private static MethodData ProcessVBNetSignature(string methodSignature)
    {
        var isWhitespace = (char c) => c == ' ';
        var isPunctuation = (char c) => c == ',';
        var isDot = (char c) => c == '.';
        var isOpenParen = (char c) => c == '(';
        var isCloseParen = (char c) => c == ')';
        var isOpenTag = (char c) => c == '<';
        var isCloseTag = (char c) => c == '>';

        var sti = new Dictionary<string, SignatureType>()
        {
            {"Sub", SignatureType.Sub},
            {"Function", SignatureType.Function},
            {"Property", SignatureType.Property}
        };

        var sigState = SigState.DontKnow;
        var sigType = SignatureType.DontKnow;
        var newSig = new StringBuilder();
        var memberName = new StringBuilder();
        var returnType = new StringBuilder();
        var accum = new StringBuilder();

        //Function SomeType.SomeMethod(someParam as string) as Integer
        foreach (var c in methodSignature)
        {
            if ((isPunctuation(c) || isCloseParen(c)) && sigState == SigState.InParam)
            {
                var buf = accum.ToString();
                newSig.Append(buf);

                sigState = SigState.NextParam;

                accum.Clear();
            }
            else if (isWhitespace(c) && sigState == SigState.NextParam)
            {
                accum.Clear();
            }
            else if (isPunctuation(c) && sigState == SigState.InParam)
            {
                var buf = accum.ToString();
                newSig.Append(buf);

                sigState = SigState.NextParam;

                accum.Clear();
            }
            else if (sigState == SigState.NextParam)
            {
                sigState = SigState.InParam;
            }
            else if (isWhitespace(c) && sigState == SigState.LookingForParams)
            {
                var buf = accum.ToString();
                // the buf should be param name, we can skip
                sigState = SigState.InParam;

                accum.Clear();
            }
            else if (isWhitespace(c) && sigType == SignatureType.DontKnow)
            {
                var buf = accum.ToString();
                if (sti.ContainsKey(buf))
                {
                    sigType = sti[buf];
                    sigState = SigState.LookingForType;
                }

                accum.Clear();
            }
            else if (isDot(c) && sigState == SigState.LookingForType)
            {
                // we just discard the type
                accum.Clear();
                sigState = SigState.LookingForMember;
            }
            else if (isOpenParen(c) && sigState == SigState.LookingForMember)
            {
                memberName.Append(accum.ToString());
                newSig.Append(accum.ToString());

                accum.Clear();
                sigState = SigState.LookingForParams;
            }
            else if (isCloseParen(c) && sigState == SigState.LookingForParams)
            {
                var paramList = accum.ToString();
                accum.Clear();

                if (string.IsNullOrEmpty(paramList))
                    newSig.Append("()");

                sigState = SigState.LookingForEnd;
            }
            else
            {
                accum.Append(c);
            }
        }

        if (sigState == SigState.LookingForEnd && returnType.Length == 0)
            returnType.Append("void");

        newSig.Append(" : ");
        newSig.Append(returnType);

        return new MethodData(Language.VBNet, methodSignature, newSig.ToString(), memberName.ToString(), returnType.ToString());
    }

    private static MethodData ProcessVBNetSignatureOld(string methodSignature)
    {
        var parts = methodSignature.Split(' ');
        var isProperty = methodSignature.StartsWith("Property");
        var methodType = parts[0];
        var returnType = methodType.Equals("Sub") ? "void" : parts[parts.Length - 1];

        var paramStart = methodSignature.IndexOf("(");
        var paramEnd = methodSignature.IndexOf(")");

        if (isProperty)
        {
            paramStart = -1;
            paramEnd = -1;
        }

        var paramList = methodSignature.Substring(paramStart + 1, paramEnd - paramStart - 1).Trim();

        var temp = paramList.Split(new[] { ',' });
        var temp2 = temp.Select(p => p.Split(' ', StringSplitOptions.RemoveEmptyEntries).Last().Trim());
        var paramsSplit = string.IsNullOrEmpty(paramList) ? "" : string.Join(", ", temp2);
        var memberName = methodSignature.Substring(0, paramStart).Split(new char[] { '.' }, StringSplitOptions.TrimEntries)[1].Trim();

        var simplified = $"{memberName}({paramsSplit}) : {returnType}";
        return new MethodData(Language.VBNet, methodSignature, simplified, memberName, returnType);
    }

    private static MethodData ProcessCSharpSignature(string methodSignature)
    {
        var parts = methodSignature.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var returnType = parts[0];
        var fqmn = methodSignature.Substring(methodSignature.IndexOf(returnType));

        var paramStart = methodSignature.IndexOf("(");
        var paramEnd = methodSignature.IndexOf(")");


        var paramList = paramStart == -1 ?
                    string.Empty :
                    methodSignature.Substring(paramStart + 1, paramEnd - paramStart - 1).Trim();
        var temp = paramList.Split(new[] { ',' });
        var temp2 = temp.Select(p => p.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].Trim());
        var paramsSplit = string.IsNullOrEmpty(paramList) ? "" : string.Join(", ", temp2);
        var memberName = methodSignature
                            .Substring(0, paramStart == -1 ? methodSignature.Length : paramStart)
                            .Split(new char[] { '.' }, StringSplitOptions.TrimEntries)[1].Trim();

        if (!string.IsNullOrEmpty(paramsSplit))
            paramsSplit = $"({paramsSplit})";

        var simplified = $"{memberName}{paramsSplit} : {returnType}";

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
        return methodSignature.StartsWith("Sub", StringComparison.OrdinalIgnoreCase)
        || methodSignature.StartsWith("Function", StringComparison.OrdinalIgnoreCase)
        || methodSignature.StartsWith("Property", StringComparison.OrdinalIgnoreCase) ?
                Language.VBNet : Language.CSharp;

    }
}
