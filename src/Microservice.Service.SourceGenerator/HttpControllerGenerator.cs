using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microservice.Service.SourceGenerator
{
    [Generator]
    public class HttpControllerGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new HttpControllerSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (HttpControllerSyntaxReceiver)context.SyntaxReceiver;

            foreach (var typeSyntax in receiver.TypeSyntaxList)
            {
                var semanticModel = context.Compilation.GetSemanticModel(typeSyntax.SyntaxTree);
                var type = ModelExtensions.GetTypeInfo(semanticModel, typeSyntax).Type;
                var classAttributeData = type.GetAttributes()
                    .SingleOrDefault(m => m.AttributeClass.Name == nameof(HttpAttribute));
                if (classAttributeData == null)
                {
                    continue;
                }

                var template = new HttpControllerTemplate();
                template.Assembly = type.ContainingNamespace.ToString();
                template.Namespace = context.Compilation.AssemblyName + ".Controllers";
                template.Name = type.Name.EndsWith("Service")
                    ? type.Name.Substring(0, type.Name.Length - "Service".Length)
                    : type.Name;
                if (template.Name.StartsWith("I"))
                {
                    template.Name = template.Name.Substring(1);
                }

                template.Methods = new List<Method>();

                var route = classAttributeData.NamedArguments
                    .FirstOrDefault(m => m.Key == nameof(HttpAttribute.Route))
                    .Value.Value?.ToString();
                if (!string.IsNullOrEmpty(route))
                {
                    template.Route = route;
                }

                var symbols = type.GetMembers().OfType<IMethodSymbol>().ToList();
                foreach (var methodSymbol in symbols)
                {
                    var method = new Method
                    {
                        Name = methodSymbol.Name,
                        ReturnType = methodSymbol.ReturnType.ToString(),
                        Parameters = methodSymbol.Parameters.ToDictionary(m => m.Name, m => m.Type.ToString())
                    };

                    var methodAttributeData = methodSymbol.GetAttributes()
                        .FirstOrDefault(m => m.AttributeClass.Name == nameof(HttpAttribute));
                    if (methodAttributeData == null)
                    {
                        continue;
                    }

                    method.HttpMethod = methodAttributeData.NamedArguments.FirstOrDefault(m => m.Key == "Method").Value
                        .Value?.ToString();
                    template.Methods.Add(method);
                }

                var source = template.TransformText();
                context.AddSource(template.Name + "Controller.g.cs", source);
            }
        }

        public class HttpControllerSyntaxReceiver : ISyntaxReceiver
        {
            public HashSet<TypeSyntax> TypeSyntaxList { get; } =
                new HashSet<TypeSyntax>();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is ClassDeclarationSyntax cds && cds.BaseList != null)
                {
                    foreach (var baseTypeSyntax in cds.BaseList.Types.OfType<SimpleBaseTypeSyntax>())
                    {
                        TypeSyntaxList.Add(baseTypeSyntax.Type);
                    }
                }
            }
        }
    }
}