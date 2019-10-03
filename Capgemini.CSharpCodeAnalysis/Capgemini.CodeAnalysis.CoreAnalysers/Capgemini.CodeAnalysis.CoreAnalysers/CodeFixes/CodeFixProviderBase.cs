using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;

namespace Capgemini.CodeAnalysis.CoreAnalysers.CodeFixes
{
    /// <summary>
    /// Implements the CodeFix ProviderBase.
    /// </summary>
    public class CodeFixProviderBase : CodeFixProvider
    {
        /// <summary>
        /// Gets the overridden FixableDiagnosticIds.
        /// </summary>
        public override ImmutableArray<string> FixableDiagnosticIds => Array.Empty<string>().ToImmutableArray();

        /// <summary>
        /// Overrides RegisterCodeFixesAsync.
        /// </summary>
        /// <param name="context">An instance of <see cref="CodeFixContext"/> to support the analysis.</param>
        /// <returns>A task that, in this case will actually throw a <see cref="NotImplementedException"/>.</returns>
        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overrides GetFixAllProvider.
        /// </summary>
        /// <returns>A suitable instance of <see cref="FixAllProvider"/>.</returns>
        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        /// <summary>
        /// Is Externally Visible.
        /// </summary>
        /// <param name="tokenList">An instance of <see cref="SyntaxTokenList"/> containing the Syntax token list to check.</param>
        /// <returns><c>true</c> if the list contains an externally visible token, otherwise <c>false</c>.</returns>
        protected static bool IsExternallyVisible(SyntaxTokenList tokenList)
        {
            return tokenList.Any(SyntaxKind.PublicKeyword) || tokenList.Any(SyntaxKind.InternalKeyword) || tokenList.Any(SyntaxKind.ProtectedKeyword);
        }
    }
}