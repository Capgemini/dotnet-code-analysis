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
    /// Implements the Method Parameters Analyzer
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MethodParametersAnalyzer : AnalyzerBase
    {
        private const int MaximumNumberOfParametersWarning = 5;
        private const int MaximumNumberOfParametersError = 10;

        internal static DiagnosticDescriptor ErrorRule =
            new DiagnosticDescriptor(AnalyzerType.MethodParametersAnalyzerId.ToDiagnosticId(),
                nameof(MethodParametersAnalyzer),
                $"{nameof(MethodParametersAnalyzer)}: {{0}}",
                AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Error, true);

        internal static DiagnosticDescriptor WarningRule =
            new DiagnosticDescriptor(AnalyzerType.MethodParametersAnalyzerId.ToDiagnosticId(),
                nameof(MethodParametersAnalyzer),
                $"{nameof(MethodParametersAnalyzer)}: {{0}}",
                AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Warning, true);
        
        /// <summary>
        /// Overrides the Supported Diagnostics property
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(ErrorRule);
        
        /// <summary>
        /// Initialises the analyzer
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            {
                return;
            }

            var declaration = Cast<MethodDeclarationSyntax>(context.Node);
            var parameterCount = declaration.ParameterList.Parameters.Count;
            var declarationLocation = declaration.Identifier.GetLocation();
            var comparisonText = "greater-than";

            if (parameterCount >= MaximumNumberOfParametersError)
            {
                RaiseErrorDiagnostic(context, parameterCount, comparisonText, declarationLocation);
            }
            else if (parameterCount > MaximumNumberOfParametersWarning &&
                     parameterCount < MaximumNumberOfParametersError)
            {
                RaiseWarningDiagnostic(context, parameterCount, comparisonText, declarationLocation);
            }
        }

        private void RaiseErrorDiagnostic(SyntaxNodeAnalysisContext context, int parameterCount, string comparisonText,
            Location declarationLocation)
        {
            if (parameterCount == MaximumNumberOfParametersError)
            {
                comparisonText = "equal to";
            }

            var warningMessage =
                $"Method has a total of {parameterCount} Parameters which is {comparisonText} the recommended maximum of {MaximumNumberOfParametersError}. Please refactor the method / class.";
            DiagnosticsManager.ConstructorParameterDiagnostic(context, declarationLocation, ErrorRule,
                warningMessage);
        }

        private void RaiseWarningDiagnostic(SyntaxNodeAnalysisContext context, int parameterCount, string comparisonText,
            Location declarationLocation)
        {
            if (parameterCount == MaximumNumberOfParametersWarning)
            {
                comparisonText = "equal to";
            }

            var warningMessage =
                $"Method has a total of {parameterCount} Parameters which is {comparisonText} the recommended maximum of {MaximumNumberOfParametersWarning}. Please consider refactoring the method / class.";

            DiagnosticsManager.ConstructorParameterDiagnostic(context, declarationLocation,
                WarningRule, warningMessage);
        }
    }
}