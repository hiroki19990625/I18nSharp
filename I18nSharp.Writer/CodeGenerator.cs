using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CSharp;

namespace I18nSharp.Writer
{
    public static class CodeGenerator
    {
        private static Regex _invalidKeyReplaceRegex =
            new Regex(@"[-!#$%&()=~^|+{};*:<>?,./{}\[\]\`\\ ]", RegexOptions.Compiled);

        public static string Generate(LanguageFile[] languageFiles)
        {
            string[] keys = new string[0];
            string[] cultures = new string[0];
            cultures = languageFiles.Select(languageFile => languageFile.LanguageFileDictionary.CultureString)
                .ToArray();
            keys = languageFiles.Select(languageFile => languageFile.LanguageFileDictionary.LanguageFileContents)
                .SelectMany(contents => contents.Select(c => c.Key)).Distinct().ToArray();

            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace();
            codeNamespace.Imports.Add(new CodeNamespaceImport("I18nSharp"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("I18nSharp.Content"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));

            CodeTypeDeclaration type = new CodeTypeDeclaration("I18n_Generated");
            CodeMemberField langProp = new CodeMemberField
            {
                Name = "SelectLanguage", Attributes = MemberAttributes.Static | MemberAttributes.Public,
                Type = new CodeTypeReference(typeof(string))
            };
            type.Members.Add(langProp);
            foreach (string key in keys)
            {
                CodeMemberProperty memberProperty = new CodeMemberProperty
                {
                    Name = InvalidKeyReplace(key),
                    HasGet = true,
                    HasSet = false,
                    Attributes = MemberAttributes.Static | MemberAttributes.Public,
                    Type = new CodeTypeReference("LanguageFileText")
                };
                memberProperty.GetStatements.Add(new CodeMethodReturnStatement(
                    new CodeCastExpression(new CodeTypeReference("LanguageFileText"), new CodeIndexerExpression(
                        new CodeMethodInvokeExpression(
                            new CodeTypeReferenceExpression("I18n"),
                            "GetLanguage",
                            new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("I18n_Generated"),
                                "SelectLanguage")), new CodePrimitiveExpression(key)))));
                type.Members.Add(memberProperty);
            }

            CodeNamespace body = new CodeNamespace("I18nSharp.Generated");
            body.Types.Add(type);
            compileUnit.Namespaces.Add(codeNamespace);
            compileUnit.Namespaces.Add(body);

            CSharpCodeProvider provider = new CSharpCodeProvider();
            using (StringWriter writer = new StringWriter())
            {
                provider.GenerateCodeFromCompileUnit(compileUnit, writer, new CodeGeneratorOptions());
                provider.Dispose();

                return writer.ToString();
            }
        }

        private static string InvalidKeyReplace(string key)
        {
            return _invalidKeyReplaceRegex.Replace(key, "_").Replace('"', '_').Replace('\'', '_');
        }
    }
}