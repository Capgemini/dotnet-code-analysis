using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;

namespace Capgemini.CodeAnalysis.CoreAnalysers.CodeFixes
{
    public class CodeFixProviderBase : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => throw new NotImplementedException();

        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            throw new NotImplementedException();
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        protected bool IsExternallyVisible(SyntaxTokenList tokenList)
        {
            return tokenList.Any(SyntaxKind.PublicKeyword) || tokenList.Any(SyntaxKind.InternalKeyword) || tokenList.Any(SyntaxKind.ProtectedKeyword);
        }
    }
}
