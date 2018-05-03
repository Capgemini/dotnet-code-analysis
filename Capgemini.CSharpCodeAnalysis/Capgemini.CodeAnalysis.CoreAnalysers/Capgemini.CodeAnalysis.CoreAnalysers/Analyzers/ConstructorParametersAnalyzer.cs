using System.Collections.Immutable;
using Capgemini.CodeAnalysis.Foundation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ConstructorParametersAnalyzer : AnalyzerBase
    {
        private const int MaximumNumberOfParametersWarning = 5;
        private const int MaximumNumberOfParametersError = 10;

        internal static DiagnosticDescriptor ErrorRule =
            new DiagnosticDescriptor(AnalyserConstants.ConstructorParameterAnalyzerId,
                nameof(ConstructorParametersAnalyzer),
                $"{nameof(ConstructorParametersAnalyzer)}: {{0}}",
                AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Error, true);

        internal static DiagnosticDescriptor WarningRule =
            new DiagnosticDescriptor(AnalyserConstants.ConstructorParameterAnalyzerId,
                nameof(ConstructorParametersAnalyzer),
                $"{nameof(ConstructorParametersAnalyzer)}: {{0}}",
                AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(ErrorRule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeConstructorDeclaration, SyntaxKind.ConstructorDeclaration);
        }

        private void AnalyzeConstructorDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declaration = Cast<ConstructorDeclarationSyntax>(context.Node);
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
                $"Constructor has a total of {parameterCount} Parameters which is {comparisonText} the recommended maximum of {MaximumNumberOfParametersError}. Please refactor the constructor / class.";
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
                $"Constructor has a total of {parameterCount} Parameters which is {comparisonText} the recommended maximum of {MaximumNumberOfParametersWarning}. Please consider refactoring the constructor / class.";

            DiagnosticsManager.ConstructorParameterDiagnostic(context, declarationLocation,
                WarningRule, warningMessage);
        }
    }
}