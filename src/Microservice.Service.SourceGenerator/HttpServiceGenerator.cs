using System.Collections.Generic;
using System.Linq;
using Microservice.Core;
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

            var classInfos = new List<ClassInfo>();
            foreach (var interfaceDeclarationSyntax in syntaxList)
            {
                var baseNamespaceDeclarationSyntax =
                    interfaceDeclarationSyntax.FirstAncestorOrSelf<BaseNamespaceDeclarationSyntax>();

                var serviceTemplate = new HttpServiceTemplate();
                serviceTemplate.Namespace = baseNamespaceDeclarationSyntax.Name.ToString();

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

                serviceTemplate.Methods = new List<Method>();
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

                    serviceTemplate.Methods.Add(method);
                }

                serviceTemplate.ClassName = interfaceDeclarationSyntax.Identifier.ValueText.Substring(1);
                serviceTemplate.Name = GetDictionaryValue(attributeValueDic, nameof(HostAttribute.Name));
                serviceTemplate.Server = GetDictionaryValue(attributeValueDic, nameof(HostAttribute.Server));
                serviceTemplate.Path = GetDictionaryValue(attributeValueDic, nameof(HostAttribute.Path));
                var source = serviceTemplate.TransformText();
                context.AddSource(interfaceDeclarationSyntax.Identifier.ValueText.Substring(1) + ".g.cs", source);

                classInfos.Add(new ClassInfo()
                {
                    Interface = serviceTemplate.Namespace + ".I" + serviceTemplate.ClassName,
                    Class = serviceTemplate.Namespace + "." + serviceTemplate.ClassName,
                });
            }

            if (classInfos.Count > 0)
            {
                var addMicroserviceTemplate = new AddMicroserviceTemplate()
                {
                    ClassInfos = classInfos
                };
                var transformText = addMicroserviceTemplate.TransformText();
                context.AddSource("MicroserviceAspNetCoreExtensions.g.cs", transformText);
            }
        }

        private string GetDictionaryValue(Dictionary<string, string> dictionary, string key)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : string.Empty;
        }
    }

    public class HttpServiceSyntaxReceiver : ISyntaxReceiver
    {
        public HashSet<InterfaceDeclarationSyntax> InterfaceDeclarationSyntaxList { get; } =
            new HashSet<InterfaceDeclarationSyntax>();

        public readonly List<string> Names = new List<string>() { HostAttribute.AttributeName, nameof(HostAttribute) };

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
}