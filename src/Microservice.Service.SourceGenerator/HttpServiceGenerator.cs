using System.Collections.Generic;
using System.Linq;
using Microservice.Api;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microservice.Service.SourceGenerator
{
    [Generator]
    public class HttpServiceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new HttpServiceSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (HttpServiceSyntaxReceiver)context.SyntaxReceiver;
            var syntaxList = receiver.InterfaceDeclarationSyntaxList;

            var template = new HttpServiceTemplate();
            foreach (var interfaceDeclarationSyntax in syntaxList)
            {
                var baseNamespaceDeclarationSyntax =
                    interfaceDeclarationSyntax.FirstAncestorOrSelf<BaseNamespaceDeclarationSyntax>();
                template.Namespace = baseNamespaceDeclarationSyntax.Name.ToString();

                var attributeValueDic = new Dictionary<string, string>();

                var semanticModel = context.Compilation.GetSemanticModel(interfaceDeclarationSyntax.SyntaxTree);
                foreach (var attributeSyntax in interfaceDeclarationSyntax.AttributeLists.SelectMany(m => m.Attributes))
                {
                    if (!receiver.Names.Contains(attributeSyntax.Name.ToString()))
                    {
                        continue;
                    }

                    foreach (var attributeArgumentSyntax in attributeSyntax.ArgumentList.Arguments)
                    {
                        var key = attributeArgumentSyntax.NameEquals.Name.ToString();
                        var value = semanticModel.GetConstantValue(attributeArgumentSyntax.Expression).Value.ToString();
                        attributeValueDic[key] = value;
                    }
                }

                template.Methods = new List<Method>();
                foreach (var methodDeclarationSyntax in interfaceDeclarationSyntax.Members
                             .OfType<MethodDeclarationSyntax>())
                {
                    var method = new Method();
                    method.Name = methodDeclarationSyntax.Identifier.ValueText;
                    method.Parameters = new Dictionary<string, string>();
                    foreach (var parameterSyntax in methodDeclarationSyntax.ParameterList.Parameters.ToList())
                    {
                        method.Parameters[parameterSyntax.Identifier.ValueText] = parameterSyntax.Type.ToString();
                    }

                    method.ReturnType = methodDeclarationSyntax.ReturnType.ToString();

                    foreach (var attributeSyntax in methodDeclarationSyntax.AttributeLists.SelectMany(m =>
                                 m.Attributes))
                    {
                        if (!new[] { HttpAttribute.AttributeName, nameof(HttpAttribute) }.Contains(attributeSyntax.Name
                                .ToString()))
                        {
                            continue;
                        }

                        foreach (var attributeArgumentSyntax in attributeSyntax.ArgumentList.Arguments)
                        {
                            var key = attributeArgumentSyntax.NameEquals.Name.ToString();
                            if (key == nameof(HttpAttribute.Method))
                            {
                                var value = semanticModel.GetConstantValue(attributeArgumentSyntax.Expression).Value
                                    .ToString();
                                method.HttpMethod = value;
                            }
                        }
                    }

                    template.Methods.Add(method);
                }

                template.ClassName = interfaceDeclarationSyntax.Identifier.ValueText.Substring(1);
                template.Address = attributeValueDic["Address"];
                var source = template.TransformText();
                context.AddSource(interfaceDeclarationSyntax.Identifier.ValueText.Substring(1) + "g.cs", source);
            }
        }
    }

    public class HttpServiceSyntaxReceiver : ISyntaxReceiver
    {
        public HashSet<InterfaceDeclarationSyntax> InterfaceDeclarationSyntaxList { get; } =
            new HashSet<InterfaceDeclarationSyntax>();

        public readonly List<string> Names = new List<string>() { ApiAttribute.AttributeName, nameof(ApiAttribute) };

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is AttributeSyntax cds && cds.Name is IdentifierNameSyntax identifierName &&
                Names.Contains(identifierName.Identifier.ValueText))
            {
                var syntax = cds.FirstAncestorOrSelf<InterfaceDeclarationSyntax>();
                if (syntax == null) return;
                InterfaceDeclarationSyntaxList.Add(syntax);
            }
        }
    }

    public class Method
    {
        public string Name { get; set; }

        public string ReturnType { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public string HttpMethod { get; set; }
    }
}