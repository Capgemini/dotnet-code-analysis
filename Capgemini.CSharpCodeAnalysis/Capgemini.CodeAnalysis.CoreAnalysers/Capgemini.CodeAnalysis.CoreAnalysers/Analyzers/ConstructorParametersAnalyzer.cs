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
    /// Implements the Constructor Parameter Analyzer.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ConstructorParametersAnalyzer : AnalyzerBase
    {
        private const int MaximumNumberOfParametersWarning = 5;
        private const int MaximumNumberOfParametersError = 10;
        private static readonly DiagnosticDescriptor ErrorRule =
                                                                new DiagnosticDescriptor(
                                                                                        AnalyzerType.ConstructorParametersAnalyzerId.ToDiagnosticId(),
                                                                                        nameof(ConstructorParametersAnalyzer),
                                                                                        $"{nameof(ConstructorParametersAnalyzer)}: {{0}}",
                                                                                        AnalyserCategoryConstants.CodeStructure,
                                                                                        DiagnosticSeverity.Error,
                                                                                        true);

        private static readonly DiagnosticDescriptor WarningRule =
                                                                new DiagnosticDescriptor(
                                                                                        AnalyzerType.ConstructorParametersAnalyzerId.ToDiagnosticId(),
                                                                                        nameof(ConstructorParametersAnalyzer),
                                                                                        $"{nameof(ConstructorParametersAnalyzer)}: {{0}}",
                                                                                        AnalyserCategoryConstants.CodeStructure,
                                                                                        DiagnosticSeverity.Warning,
                                                                                        true);

        /// <summary>
        /// Gets the overridden the Supported Diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(ErrorRule);

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

            context.RegisterSyntaxNodeAction(AnalyzeConstructorDeclaration, SyntaxKind.ConstructorDeclaration);
        }

        private static bool ParameterCountIsBetweenWarningAndErrorThreshold(int parameterCount)
        {
            return parameterCount > MaximumNumberOfParametersWarning && parameterCount < MaximumNumberOfParametersError;
        }

        private static void RaiseErrorDiagnostic(SyntaxNodeAnalysisContext context, int parameterCount, string comparisonText, Location declarationLocation)
        {
            if (parameterCount == MaximumNumberOfParametersError)
            {
                comparisonText = "equal to";
            }

            var warningMessage =
                $"Constructor has a total of {parameterCount} parameters which is {comparisonText} the recommended maximum of {MaximumNumberOfParametersError}. Please refactor the constructor / class.";
            DiagnosticsManager.ConstructorParameterDiagnostic(context, declarationLocation, ErrorRule, warningMessage);
        }

        private static void RaiseWarningDiagnostic(SyntaxNodeAnalysisContext context, int parameterCount, string comparisonText, Location declarationLocation)
        {
            if (parameterCount == MaximumNumberOfParametersWarning)
            {
                comparisonText = "equal to";
            }

            var warningMessage =
                $"Constructor has a total of {parameterCount} Parameters which is {comparisonText} the recommended maximum of {MaximumNumberOfParametersWarning}. Please consider refactoring the constructor / class.";

            DiagnosticsManager.ConstructorParameterDiagnostic(context, declarationLocation, WarningRule, warningMessage);
        }

        private void AnalyzeConstructorDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<ConstructorDeclarationSyntax>(context.Node);
            var parameterCount = declaration.ParameterList.Parameters.Count;
            var declarationLocation = declaration.Identifier.GetLocation();
            var comparisonText = "greater-than";

            if (parameterCount >= MaximumNumberOfParametersError)
            {
                RaiseErrorDiagnostic(context, parameterCount, comparisonText, declarationLocation);
            }
            else if (ParameterCountIsBetweenWarningAndErrorThreshold(parameterCount))
            {
                RaiseWarningDiagnostic(context, parameterCount, comparisonText, declarationLocation);
            }
        }
    }
}