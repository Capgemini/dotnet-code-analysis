using System.Collections.Generic;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    /// <summary>
    /// This class provides a domain specific base for all analyzers within this solution
    /// </summary>
    /// <seealso cref="Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer" />
    public abstract class AnalyzerBase : DiagnosticAnalyzer
    {
        internal static readonly LocalizableString MessageFormat = "XmlCommentsAnalyzer '{0}'";

        protected CommentsManager CommentsManager => new CommentsManager();
        protected DiagnosticsManager DiagnosticsManager => new DiagnosticsManager();
        protected RegexManager RegexManager => new RegexManager();

        protected T Cast<T>(SyntaxNode node) where T : class
        {
            return node as T;
        }

        protected bool IsPrivate(SyntaxTokenList tokenList)
        {
            return tokenList.Any(SyntaxKind.PrivateKeyword);
        }
        protected bool IsExternallyVisibleComments(SyntaxTokenList tokenList)
        {
            return tokenList.Any(SyntaxKind.PublicKeyword);
        }

        protected bool IsExternallyVisible(SyntaxTokenList tokenList)
        {
            return tokenList.Any(SyntaxKind.PublicKeyword) || tokenList.Any(SyntaxKind.InternalKeyword) || tokenList.Any(SyntaxKind.ProtectedKeyword);
        }

        protected bool ModifierContains(SyntaxTokenList tokenList, SyntaxKind syntaxKind)
        {
            return ModifierContains(tokenList, new List<SyntaxKind> { syntaxKind });
        }

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
    }
}