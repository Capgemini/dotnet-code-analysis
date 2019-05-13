﻿using System.Collections.Generic;
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
        /// <summary>
        /// Analyzer Message Format
        /// </summary>
        protected static readonly LocalizableString MessageFormat = "XmlCommentsAnalyzer '{0}'";

        /// <summary>
        /// Analyzer CommentsManager
        /// </summary>
        protected CommentsManager CommentsManager => new CommentsManager();

        /// <summary>
        /// Analyzer DiagnosticsManager
        /// </summary>
        protected DiagnosticsManager DiagnosticsManager => new DiagnosticsManager();

        /// <summary>
        /// Analyzer RegexManager
        /// </summary>
        protected RegexManager RegexManager => new RegexManager();

        /// <summary>
        /// Cast node to specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        protected T Cast<T>(SyntaxNode node) where T : class => node as T;

        /// <summary>
        /// Determine the visiblity scope of a node
        /// </summary>
        /// <param name="tokenList"></param>
        /// <returns></returns>
        protected bool IsExternallyVisible(SyntaxTokenList tokenList) =>
            tokenList.Any(SyntaxKind.PublicKeyword) || tokenList.Any(SyntaxKind.InternalKeyword) || tokenList.Any(SyntaxKind.ProtectedKeyword);

        /// <summary>
        /// Determines if Comments are visible externally 
        /// </summary>
        /// <param name="tokenList"></param>
        /// <returns></returns>
        protected bool IsExternallyVisibleComments(SyntaxTokenList tokenList) =>
            tokenList.Any(SyntaxKind.PublicKeyword);

        /// <summary>
        /// Determines if parent node an Exception
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Is Private
        /// </summary>
        /// <param name="tokenList"></param>
        /// <returns></returns>
        protected bool IsPrivate(SyntaxTokenList tokenList) =>
            tokenList.Any(SyntaxKind.PrivateKeyword);

        /// <summary>
        /// Determines if Modifier Contains
        /// </summary>
        /// <param name="tokenList"></param>
        /// <param name="syntaxKind"></param>
        /// <returns></returns>
        protected bool ModifierContains(SyntaxTokenList tokenList, SyntaxKind syntaxKind) =>
            ModifierContains(tokenList, new List<SyntaxKind> { syntaxKind });

        /// <summary>
        /// Determines if Modifier Contains
        /// </summary>
        /// <param name="tokenList"></param>
        /// <param name="syntaxKinds"></param>
        /// <returns></returns>
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