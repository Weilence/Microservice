using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microservice.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Xunit;

namespace Microservice.Service.SourceGenerator.Test;

public class UnitTest1
{
    [Fact]
    public void TestHttpService()
    {
        var expected = @"// Auto-generated code
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using Microservice.Service;

namespace Microservice.Api
{
    public class UserService : IUserService
    {
        private readonly HttpServiceClient _client;
        private readonly IResolveUrl _resolveUrl;
        private readonly string _server = ""localhost:5018"";
        private readonly string _name = ""User"";
        private readonly string _path = ""/User"";

        public UserService(HttpServiceClient client, IResolveUrl resolveUrl)
        {
            _client = client;
            _resolveUrl = resolveUrl;
        }

        public string Login(string username, string password)
        {
            var url = QueryHelpers.AddQueryString(
                _resolveUrl.ResolveUrl(_server, _name, _path) + ""/Login"",
                new Dictionary<string, string>()
                {
                    { nameof(username), username },
                    { nameof(password), password },
                }
            );
            return _client.Get<string>(url).Result;
        }

        public void Test(int p1, int p2)
        {
            var url = QueryHelpers.AddQueryString(
                _resolveUrl.ResolveUrl(_server, _name, _path) + ""/Test"",
                new Dictionary<string, string>()
                {
                    { nameof(p1), Convert.ToString(p1) },
                    { nameof(p2), Convert.ToString(p2) },
                }
            );
            _client.Get<string>(url).Wait();
        }

        public void Add(TestJson json)
        {
            _client.Post<string>(_resolveUrl.ResolveUrl(_server, _name, _path) + ""/Add"", json).Wait();
        }
    }
}";
        var source = new[]
        {
            @"using System;
using Microservice.Core;

namespace Microservice.Api
{
    [Host(Server = ""localhost:5018"", Name = ""User"", Path = ""/User"")]
    [Http]
    public interface IUserService
    {
        [Http(Method = ""GET"")]
        string Login(string username, string password);

        [Http(Method = ""GET"")]
        void Test(int p1, int p2);

        [Http(Method = ""POST"")]
        void Add(TestJson json);
    }

    public class TestJson
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
    }
}"
        };
        var actual = Run<HttpServiceGenerator>(source);

        Assert.Equal(expected, actual[0]);
    }

    [Fact]
    public void TestHttpController()
    {
        var expected = @"// Auto-generated code
using compilation.Api;
using Microsoft.AspNetCore.Mvc;

namespace compilation.Controllers
{
    [ApiController]
    [Route(""api/[controller]/[action]"")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<string> Login(string username, string password)
        {
            var obj = _service.Login(username, password);
            return new JsonResult(obj);
        }

        [HttpPost]
        public IActionResult Add(compilation.Api.TestJson json)
        {
            _service.Add(json);
            return Ok();
        }
    }
}";
        var source = new[]
        {
            @"using compilation.Api;

namespace compilation.Services;

public class UserService : IUserService
{
    public string Login(string username, string password)
    {
        if (username == ""admin"" && password == ""123456"")
        {
            return username + "":"" + password;
        }

        return ""error"";
    }

    public void Add(TestJson json)
    {
    }
}",
            @"using System;
using Microservice.Service;
using Microservice.Core;

namespace compilation.Api
{
    [Host(Name = ""User"", Path = ""/User"")]
    [Http(Route = ""api/[controller]/[action]"")]
    public interface IUserService
    {
        [Http(Method = ""GET"")]
        string Login(string username, string password);

        [Http(Method = ""POST"")]
        void Add(TestJson json);
    }

    public class TestJson
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
    }
}"
        };

        var actual = Run<HttpControllerGenerator>(source);

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

        var list = outputCompilation.GetDiagnostics().ToList();
        foreach (var diagnostic in list)
        {
        }

        foreach (var diagnostic in diagnostics)
        {
        }

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
        var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
        var syntaxTrees = sources.Select(m => CSharpSyntaxTree.ParseText(m));

        var assemblies = ReferenceAssemblies.NetStandard.NetStandard20.ResolveAsync(null, CancellationToken.None)
            .Result.ToList();
        assemblies.Add(MetadataReference.CreateFromFile(typeof(HttpAttribute).Assembly.Location));
        assemblies.Add(MetadataReference.CreateFromFile(typeof(HttpGetAttribute).Assembly.Location));
        assemblies.Add(MetadataReference.CreateFromFile(typeof(IActionResult).Assembly.Location));

        var compilation = CSharpCompilation.Create("compilation", syntaxTrees,
            assemblies,
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