﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Microservice.Service.SourceGenerator
{
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class HttpServiceTemplate : HttpServiceTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("// Auto-generated code\r\nusing System.Collections.Generic;\r\nusing Microsoft.AspNetCore.WebUtilities;\r\nusing Microservice.Service;\r\n\r\nnamespace ");
            
            #line 10 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n    public class ");
            
            #line 12 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write(" : I");
            
            #line 12 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write("\r\n    {\r\n        private readonly HttpServiceClient _client;\r\n        private readonly IResolveUrl _resolveUrl;\r\n        private readonly string _server = \"");
            
            #line 16 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Server));
            
            #line default
            #line hidden
            this.Write("\";\r\n        private readonly string _name = \"");
            
            #line 17 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Name));
            
            #line default
            #line hidden
            this.Write("\";\r\n        private readonly string _path = \"");
            
            #line 18 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Path));
            
            #line default
            #line hidden
            this.Write("\";\r\n\r\n        public ");
            
            #line 20 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write("(HttpServiceClient client, IResolveUrl resolveUrl)\r\n        {\r\n            _client = client;\r\n            _resolveUrl = resolveUrl;\r\n        }\r\n");
            
            #line 25 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"

    foreach (var method in Methods)
    {

            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 30 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"

        if (method.HttpMethod == "GET")
        {

            
            #line default
            #line hidden
            this.Write("        public ");
            
            #line 34 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.ReturnType));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 34 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 34 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(string.Join(", ", method.Parameters.Select(m => m.Value + " " + m.Key))));
            
            #line default
            #line hidden
            this.Write(")\r\n        {\r\n            var url = QueryHelpers.AddQueryString(\r\n                _resolveUrl.ResolveUrl(_server, _name, _path) + \"/");
            
            #line 38 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("\",\r\n                new Dictionary<string, string>()\r\n                {\r\n");
            
            #line 41 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"

            foreach (var key in method.Parameters.Keys)
            {

            
            #line default
            #line hidden
            this.Write("                    { nameof(");
            
            #line 45 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(key));
            
            #line default
            #line hidden
            this.Write("), ");
            
            #line 45 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(key));
            
            #line default
            #line hidden
            this.Write(" },\r\n");
            
            #line 46 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"

            }

            
            #line default
            #line hidden
            this.Write("                }\r\n            );\r\n");
            
            #line 51 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"

            if (method.ReturnType == "void")
            {

            
            #line default
            #line hidden
            this.Write("            _client.Get<string>(url).Wait();\r\n");
            
            #line 56 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"

            }
            else
            {

            
            #line default
            #line hidden
            this.Write("            return _client.Get<string>(url).Result;\r\n");
            
            #line 62 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"

            }

            
            #line default
            #line hidden
            this.Write("        }\r\n");
            
            #line 66 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"

        }
        else if (method.HttpMethod == "POST")
        {

            
            #line default
            #line hidden
            this.Write("        public ");
            
            #line 71 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.ReturnType));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 71 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 71 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(string.Join(", ", method.Parameters.Select(m => m.Value + " " + m.Key))));
            
            #line default
            #line hidden
            this.Write(")\r\n        {\r\n");
            
            #line 74 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"

            if (method.ReturnType == "void")
            {

            
            #line default
            #line hidden
            this.Write("            _client.Post<string>(_resolveUrl.ResolveUrl(_server, _name, _path) + \"/");
            
            #line 78 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("\", ");
            
            #line 78 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Parameters.Keys.FirstOrDefault()));
            
            #line default
            #line hidden
            this.Write(").Wait();\r\n");
            
            #line 80 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"

            }
            else
            {

            
            #line default
            #line hidden
            this.Write("            return _client.Post<string>(_resolveUrl.ResolveUrl(_server, _name, _path) + \"/");
            
            #line 85 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Name));
            
            #line default
            #line hidden
            this.Write("\", ");
            
            #line 85 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(method.Parameters.Keys.FirstOrDefault()));
            
            #line default
            #line hidden
            this.Write(").Result;\r\n");
            
            #line 87 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"

            }

            
            #line default
            #line hidden
            this.Write("        }\r\n");
            
            #line 91 "D:\source\repos\Microservice\src\Microservice.Service.SourceGenerator\HttpServiceTemplate.tt"

        }
    }

            
            #line default
            #line hidden
            this.Write("    }\r\n}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class HttpServiceTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
