using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Extensions
{
    public static class ExcludeGeneratedCodeExtensions
    {
        public static bool IsGeneratedCode(this SyntaxNodeAnalysisContext context)
        {
            var collection = context.Node.Ancestors();
            foreach (var item in collection)
            {
                if (item.IsGenerated())
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsGenerated(this SyntaxNode node)
        {
            var declaration = node as NamespaceDeclarationSyntax;
            if (declaration == null)
            {
                return false;
            }
            var filePath = declaration.SyntaxTree.FilePath;

            return filePath.IsGeneratedFileName();
        }
    }
}