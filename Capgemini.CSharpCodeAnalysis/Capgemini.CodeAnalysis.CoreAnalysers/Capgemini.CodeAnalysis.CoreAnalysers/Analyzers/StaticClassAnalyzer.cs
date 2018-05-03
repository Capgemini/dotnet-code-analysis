using System.Collections.Immutable;
using System.Linq;
using Capgemini.CodeAnalysis.Foundation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    /// <summary>
    /// This analyzer implements the following code review rule: Static classes must be avoided if not required as these make testing more difficult
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class StaticClassAnalyzer : AnalyzerBase
    {
        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(AnalyserConstants.StaticClassAnalyzerId, nameof(StaticClassAnalyzer),
             $"{nameof(StaticClassAnalyzer)}: {{0}}", AnalyserCategoryConstants.StaticAnalyzer, DiagnosticSeverity.Error, true);

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
            context.RegisterSyntaxNodeAction(AnalysisDeclaration, SyntaxKind.ClassDeclaration);
        }

        private void AnalysisDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declaration = Cast<ClassDeclarationSyntax>(context.Node);
            if (declaration.Modifiers.Any(SyntaxKind.StaticKeyword))
            {
                var isExtensionClass = IsAnExtensionClass(declaration);

                if (!isExtensionClass)
                {
                    DiagnosticsManager.CreateStaticClassDiagnostic(context, declaration.Identifier.GetLocation(), Rule);
                }
            }
        }

        private static bool IsAnExtensionClass(ClassDeclarationSyntax declaration)
        {
            var isExtensionClass = false;
            var methods = declaration.Members.OfType<MethodDeclarationSyntax>().ToList();
            foreach (var method in methods)
            {
                foreach (var parameter in method.ParameterList.Parameters)
                {
                    if (parameter.Modifiers.Any(SyntaxKind.ThisKeyword))
                    {
                        isExtensionClass = true;
                        break;
                    }
                }

                if (isExtensionClass)
                {
                    break;
                }
            }
            return isExtensionClass;
        }
    }
}