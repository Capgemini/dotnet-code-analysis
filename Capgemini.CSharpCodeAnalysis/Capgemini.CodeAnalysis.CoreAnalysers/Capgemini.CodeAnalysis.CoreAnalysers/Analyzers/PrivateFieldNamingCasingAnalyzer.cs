﻿using System;
using System.Collections.Immutable;
using Capgemini.CodeAnalysis.CoreAnalysers.Extensions;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    /// <summary>
    /// Implements the PrivateField Naming Casing Analyzer.
    /// All tests have been removed as, now this is deprecated, they fail and changes are not supported - deprecated ;-).
    /// </summary>
    [Obsolete("Please use StyleCop.Analyzers instead. This analyser will be removed in future versions. This analyser is now disabled by default.")]
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PrivateFieldNamingCasingAnalyzer : AnalyzerBase
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
                                                                                AnalyzerType.PrivateFieldNameCasingAnalyzerId.ToDiagnosticId(),
                                                                                nameof(PrivateFieldNamingCasingAnalyzer),
                                                                                $"{nameof(PrivateFieldNamingCasingAnalyzer)}: Field '{{0}}' does not satisfy naming convention.\nField '{{0}}' must start with one upper case character,\nnot end with uppercase character and not contain two consecutive upper case characters.",
                                                                                "Naming",
                                                                                DiagnosticSeverity.Warning,
                                                                                isEnabledByDefault: false);

        /// <summary>
        /// Gets the overridden the Supported Diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Initialises the analyzer.
        /// </summary>
        /// <param name="context">An instance of <see cref="AnalysisContext"/> to support the analysis.</param>
        public override void Initialize(AnalysisContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context), $"An instance of {nameof(context)} was not supplied.");
            }

            context.RegisterSyntaxNodeAction(AnalyzeFieldDeclaration, SyntaxKind.FieldDeclaration);
        }

        private void AnalyzeFieldDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<FieldDeclarationSyntax>(context.Node);
            if (IsExternallyVisible(declaration.Modifiers))
            {
                return;
            }

            var variableName = declaration.Declaration.Variables.FirstOrDefault()?.Identifier.Text;
            var location = declaration.Declaration.Variables.FirstOrDefault()?.Identifier.GetLocation();

            if (!string.IsNullOrWhiteSpace(variableName) && !IsExternallyVisible(declaration.Modifiers))
            {
                if (!variableName.StartsWith("_", StringComparison.Ordinal))
                {
                    var diagnostic = Diagnostic.Create(Rule, location, variableName);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}