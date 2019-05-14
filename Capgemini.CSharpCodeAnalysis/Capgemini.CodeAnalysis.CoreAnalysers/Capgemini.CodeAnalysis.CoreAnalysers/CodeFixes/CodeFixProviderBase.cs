using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;

namespace Capgemini.CodeAnalysis.CoreAnalysers.CodeFixes
{    /// <summary>
    /// Implements the CodeFix ProviderBase
    /// </summary>
    public class CodeFixProviderBase : CodeFixProvider
    {
        
        /// <summary>
        /// Ovverrides FixableDiagnosticIds
        /// </summary>
        public override ImmutableArray<string> FixableDiagnosticIds => throw new NotImplementedException();

        /// <summary>
        /// Ovverides RegisterCodeFixesAsync
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ovverides GetFixAllProvider
        /// </summary>
        /// <returns></returns>
        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        /// <summary>
        /// Is Externally Visible
        /// </summary>
        /// <param name="tokenList"></param>
        /// <returns></returns>
        protected bool IsExternallyVisible(SyntaxTokenList tokenList)
        {
            return tokenList.Any(SyntaxKind.PublicKeyword) || tokenList.Any(SyntaxKind.InternalKeyword) || tokenList.Any(SyntaxKind.ProtectedKeyword);
        }
    }
}
