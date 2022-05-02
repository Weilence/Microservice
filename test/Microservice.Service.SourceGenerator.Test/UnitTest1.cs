using System;
using System.Collections.Generic;
using System.Linq;
using Microservice.Api;
using Microservice.Service.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Microservice.Service.SourceGenerator.Test;

public class UnitTest1
{
    [Fact]
    public void TestAutoService()
    {
        var expected = @"// Auto-generated code
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace Microservice.Api
{
    public class UserService : IUserService
    {
        private readonly string _url = ""http://localhost:5018/User"";
        private readonly HttpServiceClient _client;

        public UserService(HttpServiceClient client)
        {
            _client = client;
        }

        public string Login(string username, string password)
        {
            var url = QueryHelpers.AddQueryString(
                _url + ""/Login"",
                new Dictionary<string, string>()
                {
                    { nameof(username), username },
                    { nameof(password), password },
                }
            );
            return _client.Get<string>(url).Result;
        }

        public string Add(Dictionary<string, string> dic)
        {
            return _client.Post<string>(_url + ""/Add"", dic).Result;
        }
    }
}";
        var source = new[]
        {
            @"using System.Collections.Generic;

namespace Microservice.Api
{
    [Api(Address = ""http://localhost:5018/User"")]
    public interface IUserService
    {
        [Http(Method = ""GET"")]
        string Login(string username, string password);

        [Http(Method = ""POST"")]
        string Add(Dictionary<string, string> dic);
    }
}"
        };
        var actual = Run<HttpServiceGenerator>(source);

        Assert.Equal(expected, actual[0]);
    }

    private static List<string> Run<T>(params string[] sources) where T : ISourceGenerator, new()
    {
        var inputCompilation = CreateCompilation(sources);

        GeneratorDriver driver = CSharpGeneratorDriver.Create(new T());

        driver = driver
            .WithUpdatedAnalyzerConfigOptions(
                CreateAnalyzerConfigOptionsProvider(new Dictionary<string, string>()
                {
                    ["build_property.projectdir"] =
                        AppContext.BaseDirectory[..AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal)]
                })
            )
            .RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

        var runResult = driver.GetRunResult();

        var exception = runResult.Results.Select(m => m.Exception).FirstOrDefault();
        if (exception != null)
        {
            throw new Exception(exception.Message, exception);
        }

        return runResult.GeneratedTrees.Select(m => m.GetText().ToString()).ToList();
    }

    private static Compilation CreateCompilation(IEnumerable<string> sources)
    {
        var options = new CSharpCompilationOptions(OutputKind.ConsoleApplication);
        var reference = MetadataReference.CreateFromFile(typeof(ApiAttribute).Assembly.Location);
        var syntaxTrees = sources.Select(m => CSharpSyntaxTree.ParseText(m));
        var compilation = CSharpCompilation.Create("compilation", syntaxTrees, new[] { reference },
            options
        );

        return compilation;
    }

    private static AnalyzerConfigOptionsProvider
        CreateAnalyzerConfigOptionsProvider(Dictionary<string, string> dictionary) =>
        new MockAnalyzerConfigOptionsProvider(new MockAnalyzerConfigOptions(dictionary));

    public class MockAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
    {
        public MockAnalyzerConfigOptionsProvider(AnalyzerConfigOptions options)
        {
            GlobalOptions = options;
        }

        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
        {
            return GlobalOptions;
        }

        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
        {
            return GlobalOptions;
        }

        public override AnalyzerConfigOptions GlobalOptions { get; }
    }

    public class MockAnalyzerConfigOptions : AnalyzerConfigOptions
    {
        private readonly Dictionary<string, string> _dictionary;

        public MockAnalyzerConfigOptions(Dictionary<string, string> dictionary)
        {
            this._dictionary = dictionary;
        }

        public override bool TryGetValue(string key, out string value)
        {
            return _dictionary.TryGetValue(key, out value!);
        }
    }
}