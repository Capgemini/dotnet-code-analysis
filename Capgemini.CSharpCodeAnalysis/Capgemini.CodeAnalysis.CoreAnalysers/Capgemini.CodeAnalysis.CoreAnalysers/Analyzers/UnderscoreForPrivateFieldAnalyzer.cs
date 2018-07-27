using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UnderscoreForPrivateFieldAnalyzer : DiagnosticAnalyzer
    {
        // This works
        public const string DiagnosticId = "FUJJB001b";

        static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, DiagnosticId, "Field {0} does not start with `_`", "Naming", DiagnosticSeverity.Warning, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeField, SymbolKind.Field);
        }

        static void AnalyzeField(SymbolAnalysisContext context)
        {
            var field = (IFieldSymbol)context.Symbol;
            if (field.IsStatic)
            {
                return;
            }

            if (field.ContainingType.TypeKind == TypeKind.Struct)
            {
                return;
            }

            if (!field.Name.StartsWith("_", StringComparison.Ordinal))
            {
                var diagnostic = Diagnostic.Create(Rule, field.Locations[0], field.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
