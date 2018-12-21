using System.Collections.Generic;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    /// <summary>
    /// This class provides a domain specific base for all analyzers within this solution
    /// <seealso cref="Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer" />
    /// </summary>
    /// <example>
    /// Register an action that is executed at the completion of the semantic analysis of a syntax node.
    /// <code>
    /// public override void Initialize(AnalysisContext context)
    /// {
    ///     context.RegisterSyntaxNodeAction(MethodName, SyntaxKind.IdentifierName);
    /// }
    /// </code>
    /// <code>
    /// private void MethodName(SyntaxNodeAnalysisContext context)
    /// {
    ///     var root = context.Node;
    ///     //Code analysis logic implementation
    ///
    ///     //Create a diagnostic at the node location
    ///     var diagnostics = Diagnostics.Create(Rule, root.GetLocation(), message);
    ///     context.ReportDiagnostic(diagnostics);
    /// }
    /// </code>
    /// </example>
    public abstract class AnalyzerBase : DiagnosticAnalyzer
    {
        protected static readonly LocalizableString MessageFormat = "XmlCommentsAnalyzer '{0}'";

        protected CommentsManager CommentsManager => new CommentsManager();
        protected DiagnosticsManager DiagnosticsManager => new DiagnosticsManager();
        protected RegexManager RegexManager => new RegexManager();

        protected T Cast<T>(SyntaxNode node) where T : class => node as T;

        protected bool IsExternallyVisible(SyntaxTokenList tokenList) =>
            tokenList.Any(SyntaxKind.PublicKeyword) || tokenList.Any(SyntaxKind.InternalKeyword) || tokenList.Any(SyntaxKind.ProtectedKeyword);

        protected bool IsExternallyVisibleComments(SyntaxTokenList tokenList) =>
            tokenList.Any(SyntaxKind.PublicKeyword);

        protected bool IsParentAnException(SyntaxNode node)
        {
            var result = false;
            var testNode = node;
            while (testNode != null)
            {
                if (testNode.IsKind(SyntaxKind.ThrowStatement))
                {
                    result = true;
                    break;
                }
                testNode = testNode.Parent;
            }

            return result;
        }

        protected bool IsPrivate(SyntaxTokenList tokenList) =>
            tokenList.Any(SyntaxKind.PrivateKeyword);

        protected bool ModifierContains(SyntaxTokenList tokenList, SyntaxKind syntaxKind) =>
            ModifierContains(tokenList, new List<SyntaxKind> { syntaxKind });

        protected bool ModifierContains(SyntaxTokenList tokenList, List<SyntaxKind> syntaxKinds)
        {
            var containsSyntaxKind = false;
            foreach (var syntaxKind in syntaxKinds)
            {
                containsSyntaxKind = tokenList.Any(syntaxKind);
                if (containsSyntaxKind)
                {
                    break;
                }
            }
            return containsSyntaxKind;
        }
    }
}