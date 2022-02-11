
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace MetricsNet;

public class TypeRecord
{
    public string Namespace { get; set; }
    public string Typename { get; set; }

    public string MemberName { get; set; }
}

public class TypeScanner
{
    public TypeRecord[] Scan(string path)
    {

        var assembliesToLoad = new HashSet<Assembly>();
        var records = new List<TypeRecord>();
        var files = Directory.EnumerateFiles(path, "*.dll");

        foreach (var file in files)
        {
            assembliesToLoad.Add(Assembly.LoadFile(file));
        }

        var compilation1 = CSharpCompilation.Create("TestLibraryAssembly", null,
                assembliesToLoad.Select(assembly => MetadataReference.CreateFromFile(assembly.Location)).ToList(), null);

        foreach (var a in assembliesToLoad)
        {
            var types = a.GetTypes();
            foreach (var t in types)
            {
                var metaType = compilation1.GetTypeByMetadataName(t.FullName);
                if (metaType != null)
                {
                    var workspace = new AdhocWorkspace();
                    var sg = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);
                }
            }
        }

        return records.ToArray();
    }
}