using Xunit;

namespace MetricsNet;

public class SignatureProcessingTests
{
    [Fact]
    public void CSharpNoParamsNoReturnType()
    {
        var testCase = "void SomeType.SomeMethod()";
        var result = CodeMetricsUtilities.ProcessMethodSignature(testCase);

        Assert.NotNull(result);
        Assert.Equal(Language.CSharp, result?.Language);
        Assert.Equal(testCase, result?.MethodSignature);
        Assert.Equal("SomeMethod", result?.MemberName);
        Assert.Equal("SomeMethod() : void", result?.SimplifiedSignature);

    }

    [Fact]
    public void CSharpOneParamWithReturnType()
    {
        var testCase = "int SomeType.SomeMethod(string someParam)";
        var result = CodeMetricsUtilities.ProcessMethodSignature(testCase);

        Assert.NotNull(result);
        Assert.Equal("SomeMethod(string) : int", result?.SimplifiedSignature);

    }

    [Fact]
    public void CSharpTwoParamsWithReturnType()
    {
        var testCase = "string  SomeType.SomeMethod(CustomType  someParam, int intParam )";
        var result = CodeMetricsUtilities.ProcessMethodSignature(testCase);

        Assert.NotNull(result);
        Assert.Equal("SomeMethod(CustomType, int) : string", result?.SimplifiedSignature);

    }

    [Fact]
    public void VBNetNoParamsNoReturnType()
    {
        var testCase = "Sub SomeType.SomeMethod()";
        var result = CodeMetricsUtilities.ProcessMethodSignature(testCase);

        Assert.NotNull(result);
        Assert.Equal(Language.VBNet, result?.Language);
        Assert.Equal(testCase, result?.MethodSignature);
        Assert.Equal("SomeMethod", result?.MemberName);
        Assert.Equal("SomeMethod() : void", result?.SimplifiedSignature);
    }

    [Fact]
    public void VBNetOneParamWithReturnType()
    {
        var testCase = "Function SomeType.SomeMethod(someParam as string) as Integer";
        var result = CodeMetricsUtilities.ProcessMethodSignature(testCase);

        Assert.NotNull(result);
        Assert.Equal("SomeMethod(string) : Integer", result?.SimplifiedSignature);

    }

    [Fact]
    public void VBNetTwoParamsWithReturnType()
    {
        var testCase = "Function SomeType.SomeMethod(someParam As CustomType, intParam As int) As string";
        var result = CodeMetricsUtilities.ProcessMethodSignature(testCase);

        Assert.NotNull(result);
        Assert.Equal("SomeMethod(CustomType, int) : string", result?.SimplifiedSignature);

    }
    
        [Fact]
    public void VBNetGenericParamsAndGenericReturnType()
    {
        var testCase = "Function SomeType.SomeMethod(Of T)(someParam As T, intParam As int) As Blah(Of T)";
        var result = CodeMetricsUtilities.ProcessMethodSignature(testCase);

        Assert.NotNull(result);
        Assert.Equal("SomeMethod<T>(T, int) : Blah<T>", result?.SimplifiedSignature);

    }

    [Fact]
    public void VBNetRealWorldFail()
    {
        var testCase = "Property AnotherType._someProperty As Lazy(Of SomeType)";
        var result = CodeMetricsUtilities.ProcessMethodSignature(testCase);

        Assert.NotNull(result);
        Assert.Equal("_someProperty : Lazy<SomeType>", result?.SimplifiedSignature);

    }

    [Fact]
    public void CSharpRealWorlFail()
    {
        var testCase = "Lazy<SomeType> AnotherType._someProperty";
        var result = CodeMetricsUtilities.ProcessMethodSignature(testCase);

        Assert.NotNull(result);
        Assert.Equal("_someProperty : Lazy<SomeType>", result?.SimplifiedSignature);

    }
    //"Lazy<ILogManager> MgrPrintManager._logManager"
}
