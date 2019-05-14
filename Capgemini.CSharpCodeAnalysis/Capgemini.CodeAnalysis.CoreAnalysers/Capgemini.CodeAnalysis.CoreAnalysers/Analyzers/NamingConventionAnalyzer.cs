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
    /// Classes, Methods and Variables must be named appropriately 
    /// - don't use generic names as class1, test1, use meaningful names reflecting the functionality
    /// follow MSDN naming conventions https://msdn.microsoft.com/en-us/library/ms229045(v=vs.110).aspx
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NamingConventionAnalyzer : AnalyzerBase
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyzerType.NamingConventionAnalyzerId.ToDiagnosticId(),
            nameof(NamingConventionAnalyzer),
            $"{nameof(NamingConventionAnalyzer)}: {{0}}",
            AnalyserCategoryConstants.NamingConvention,
            DiagnosticSeverity.Error,
            true);
        
        /// <summary>
        /// Overrides the Supported Diagnostics property
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
        
        /// <summary>
        /// Initialises the analyzer
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeFieldDeclaration, SyntaxKind.FieldDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeInterfaceDeclaration, SyntaxKind.InterfaceDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeConstructorDeclaration, SyntaxKind.ConstructorDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzePropertyDeclaration, SyntaxKind.PropertyDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeLocalDeclaration, SyntaxKind.LocalDeclarationStatement);
        }

        private void AnalyzeLocalDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            {
                return;
            }

            var declaration = Cast<LocalDeclarationStatementSyntax>(context.Node);

            var variableDeclarator = declaration.Declaration.Variables.FirstOrDefault()?.Identifier.Text;
            RegexManager.DoesNotSatisfyLocalVariableNameRule(context, variableDeclarator, declaration.GetLocation(), Rule);
        }

        private void AnalyzeConstructorDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            {
                return;
            }

            var declaration = Cast<ConstructorDeclarationSyntax>(context.Node);
            RegexManager.DoesNotSatisfyNonPrivateNameRule(context, declaration.Identifier.Text, declaration.Identifier.GetLocation(), Rule);

            foreach (var parameter in declaration.ParameterList.Parameters)
            {
                RegexManager.DoesNotSatisfyLocalVariableNameRule(context, parameter.Identifier.Text,
                    parameter.Identifier.GetLocation(), Rule);
            }
        }

        private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            {
                return;
            }

            var declaration = Cast<ClassDeclarationSyntax>(context.Node);
            RegexManager.DoesNotSatisfyNonPrivateNameRule(context, declaration.Identifier.Text, declaration.Identifier.GetLocation(), Rule);
        }

        private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            {
                return;
            }

            var declaration = Cast<MethodDeclarationSyntax>(context.Node);
            RegexManager.DoesNotSatisfyNonPrivateNameRule(context, declaration.Identifier.Text, declaration.Identifier.GetLocation(), Rule);

            foreach (var parameter in declaration.ParameterList.Parameters)
            {
                RegexManager.DoesNotSatisfyLocalVariableNameRule(context, parameter.Identifier.Text,
                    parameter.Identifier.GetLocation(), Rule);
            }
        }

        private void AnalyzeInterfaceDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            {
                return;
            }

            var declaration = Cast<InterfaceDeclarationSyntax>(context.Node);
            RegexManager.DoesNotSatisfyInterfaceRule(context, declaration.Identifier.Text, declaration.Identifier.GetLocation(), Rule);
        }

        private void AnalyzeFieldDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            {
                return;
            }

            var declaration = Cast<FieldDeclarationSyntax>(context.Node);
            if(!IsExternallyVisible(declaration.Modifiers))
            {
                return;
            }

            var variableDeclarator = declaration.Declaration.Variables.FirstOrDefault()?.Identifier.Text;
            var location = declaration.Declaration.Variables.FirstOrDefault()?.Identifier.GetLocation();

            RegexManager.DoesNotSatisfyNonPrivateNameRule(context, variableDeclarator, location, Rule);
        }

        private void AnalyzePropertyDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGeneratedCode())
            {
                return;
            }

            var declaration = Cast<PropertyDeclarationSyntax>(context.Node);
            if (!IsExternallyVisible(declaration.Modifiers))
            {
                return;
            }

            var propertiesString = declaration.Identifier.Text;
            RegexManager.DoesNotSatisfyNonPrivateNameRule(context, propertiesString, declaration.Identifier.GetLocation(), Rule);
        }
    }
}