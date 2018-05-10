﻿using Capgemini.CodeAnalysis.Foundation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    /// <summary>
    /// This analyzer implements the following code review rule: No big/monster methods - break down big methods into a few smaller methods with meaningful names, so the code is self-descriptive and does not require excessive comments
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MonsterMethodAnalyzer : AnalyzerBase
    {
        private const int MethodMaxLine = 80;
        
        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyzerType.MonsterMethodAnalyzerId.ToDiagnosticId(), nameof(MonsterMethodAnalyzer),
            $"{nameof(MonsterMethodAnalyzer)}: {{0}}", AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declaration = Cast<MethodDeclarationSyntax>(context.Node);
            var numberOfStatements = declaration.Body?.Statements.Count;

            if (numberOfStatements != null && numberOfStatements.Value > MethodMaxLine)
            {
                var diagnostics = Diagnostic.Create(Rule, declaration.Identifier.GetLocation(), $"This method is longer than {MethodMaxLine} lines of executable code. Please consider splitting this method into smaller methods.");
                context.ReportDiagnostic(diagnostics);
            }
        }

    }
}