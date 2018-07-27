using System.Collections.Generic;
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
    /// This analyzer implements the following code review rule: Explicit access modifiers must be used for all members (eg. Private fields should use the 'private' keyword)
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ExplicitAccessModifiersAnalyzer : AnalyzerBase
    {
        /// <summary>
        /// The rule
        /// </summary>
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyzerType.ExplicitAccessModifiersAnalyzerId.ToDiagnosticId(), nameof(ExplicitAccessModifiersAnalyzer),
            $"{nameof(ExplicitAccessModifiersAnalyzer)}: {{0}}", AnalyserCategoryConstants.CodeStructure, DiagnosticSeverity.Error, true);
       
        /// <summary>
        /// Returns a set of descriptors for the diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <summary>
        /// Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzedMethodDeclaration, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzedPropertyDeclaration, SyntaxKind.PropertyDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzedConstructorDeclaration, SyntaxKind.ConstructorDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzedClassDeclaration, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzedInterfaceDeclaration, SyntaxKind.InterfaceDeclaration);
        }

        private void AnalyzedConstructorDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            { return; }
            var declaration = Cast<ConstructorDeclarationSyntax>(context.Node);

            if (!declaration.Modifiers.Any(SyntaxKind.StaticKeyword))
            {
                if (!(IsExternallyVisible(declaration.Modifiers) || ModifierContains(declaration.Modifiers,
                          new List<SyntaxKind> {SyntaxKind.PrivateKeyword})))
                {
                    DiagnosticsManager.CreateExplicitAccessDiagnostic(context,
                        $"Constructor {declaration.Identifier.Text}", declaration.Identifier.GetLocation(), Rule);
                }
            }
        }

        private void AnalyzedInterfaceDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            { return; }
            var declaration = Cast<InterfaceDeclarationSyntax>(context.Node);

            if (!(IsExternallyVisible(declaration.Modifiers) || declaration.Modifiers.Any(SyntaxKind.PrivateKeyword)))
            {
                DiagnosticsManager.CreateExplicitAccessDiagnostic(context, declaration.Identifier.Text, declaration.GetLocation(), Rule);
            }
        }

        private void AnalyzedPropertyDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            { return; }
            var declaration = Cast<PropertyDeclarationSyntax>(context.Node);

            //if this property is within an interface then we do not need to process with access qualifier check
            var interfaceDeclaration = declaration.Parent as InterfaceDeclarationSyntax;
            if (interfaceDeclaration == null && !(
                                                    ModifierContains(declaration.Modifiers, new List<SyntaxKind> { SyntaxKind.PublicKeyword, SyntaxKind.InternalKeyword, SyntaxKind.ProtectedKeyword, SyntaxKind.PrivateKeyword }) ))
            {
                DiagnosticsManager.CreateExplicitAccessDiagnostic(context, declaration.Identifier.Text, declaration.Identifier.GetLocation(), Rule);
            }
        }

        private void AnalyzedClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            { return; }
            var declaration = Cast<ClassDeclarationSyntax>(context.Node);

            if (!(
                 ModifierContains(declaration.Modifiers, new List<SyntaxKind> { SyntaxKind.PublicKeyword, SyntaxKind.InternalKeyword, SyntaxKind.ProtectedKeyword, SyntaxKind.PrivateKeyword }))
                )
            {
                DiagnosticsManager.CreateExplicitAccessDiagnostic(context, declaration.Identifier.Text, declaration.Identifier.GetLocation(), Rule);
            }
        }

        private void AnalyzedMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            { return; }
            var declaration = Cast<MethodDeclarationSyntax>(context.Node);

            //if this method is within an interface then we do not need to process with access qualifier check
            var interfaceDeclaration = declaration.Parent as InterfaceDeclarationSyntax;
            if (interfaceDeclaration == null && !(
                ModifierContains(declaration.Modifiers, new List<SyntaxKind> { SyntaxKind.PublicKeyword, SyntaxKind.InternalKeyword, SyntaxKind.ProtectedKeyword, SyntaxKind.PrivateKeyword })))
            {
                DiagnosticsManager.CreateExplicitAccessDiagnostic(context, declaration.Identifier.Text, declaration.Identifier.GetLocation(), Rule);
            }
        }

    }
}