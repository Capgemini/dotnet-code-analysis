﻿using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Analyzers
{
    [TestClass]
    public class PrivateFieldNameCasingAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void PrivateFieldNamingCasingAnalyzer_Should_NotRaiseDiagnosticForPublicField()
        {
            var original = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            public string fieldThatDoesNotMatch;
        }
    }";
            VerifyCSharpDiagnostic(original);
        }

        [DataTestMethod]
        [DataRow("FieldThatDoesNotMatch", "private", 28)]
        [DataRow("FIeldThatDoesNotMatch", "private", 28)]
        [DataRow("FieldThatDoesNotMatcH", "private", 28)]
        [DataRow("fieldTHatDoesNotMatch", "private", 28)]
        [DataRow("FieldThatDoesNotMatch", "private static", 35)]
        [DataRow("FIeldThatDoesNotMatch", "private static", 35)]
        [DataRow("FieldThatDoesNotMatcH", "private static", 35)]
        [DataRow("fieldTHatDoesNotMatch", "private static", 35)]
        [DataRow("FieldThatDoesNotMatch", "private readonly", 37)]
        [DataRow("FIeldThatDoesNotMatch", "private readonly", 37)]
        [DataRow("FieldThatDoesNotMatcH", "private readonly", 37)]
        [DataRow("fieldTHatDoesNotMatch", "private readonly", 37)]
        public void PrivateFieldNamingCasingAnalyzer_Should_RaiseDiagnosticForPrivateField(string nonMatchingFieldName, string accessModifier, int expectedColumn)
        {
            var original = $@"
    using System;

    namespace ConsoleApplication1
    {{
        class TypeName
        {{   
            {accessModifier} string {nonMatchingFieldName};
        }}
    }}";

            var expected = new DiagnosticResult
            {
                Id = "CAP0018",
                Message = $"{nameof(PrivateFieldNamingCasingAnalyzer)}: Field '{nonMatchingFieldName}' does not satisfy naming convention.\nField '{nonMatchingFieldName}' must start with one upper case character,\nnot end with uppercase character and not contain two consecutive upper case characters.",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                          new[] {
                                    new DiagnosticResultLocation("Test0.cs", 8, expectedColumn)
                              }
            };

            VerifyCSharpDiagnostic(original, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new PrivateFieldNamingCasingAnalyzer();
        }
    }
}
