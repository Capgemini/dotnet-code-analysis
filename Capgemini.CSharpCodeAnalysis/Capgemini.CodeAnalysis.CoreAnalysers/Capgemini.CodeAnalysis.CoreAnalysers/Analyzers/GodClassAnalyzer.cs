﻿using System;
using System.Collections.Immutable;
using System.Linq;
using Capgemini.CodeAnalysis.CoreAnalysers.Extensions;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    /// <summary>
    /// "God" classes are not allowed - each class should be small, self-contained and do only one thing well (follow single responsibility pattern from SOLID principles). Anybody reading the code must be able to read a complete method from start to end without scrolling in the window.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class GodClassAnalyzer : AnalyzerBase
    {
        private const int ClassMaxNumberOfPublicMethods = 20;
        private static readonly DiagnosticDescriptor Rule =
                                                            new DiagnosticDescriptor(
                                                                AnalyzerType.GodClassAnalyzerId.ToDiagnosticId(),
                                                                nameof(GodClassAnalyzer),
                                                                $"{nameof(GodClassAnalyzer)} \'{{0}}\'",
                                                                AnalyserCategoryConstants.CodeStructure,
                                                                DiagnosticSeverity.Error,
                                                                true);

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

            context.RegisterSyntaxNodeAction(AnalyzeDeclararion, SyntaxKind.ClassDeclaration);
        }

        private void AnalyzeDeclararion(SyntaxNodeAnalysisContext context)
        {
            if (context.IsAutomaticallyGeneratedCode())
            {
                return;
            }

            var declaration = Cast<ClassDeclarationSyntax>(context.Node);
            var methodCount = declaration.Members.OfType<MethodDeclarationSyntax>().Count(a => !IsPrivate(a.Modifiers));
            var constructorCount = declaration.Members.OfType<ConstructorDeclarationSyntax>().Count(a => !IsPrivate(a.Modifiers));

            var totalExternallyVisibleMethods = methodCount + constructorCount;

            if (totalExternallyVisibleMethods > ClassMaxNumberOfPublicMethods)
            {
                var diagnostics = Diagnostic.Create(Rule, declaration.Identifier.GetLocation(), $"This class has {totalExternallyVisibleMethods} methods which is more than the recommended {ClassMaxNumberOfPublicMethods} methods. \nPlease consider applying the SOLID principles to the class design. \nIt is recommended to have small focused classes.");
                context.ReportDiagnostic(diagnostics);
            }
        }
    }
}