﻿using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Analyzers
{
    [TestClass]
    public class NamingConventionAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void NamingConventionAnalyzer_Passes_ClassName()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [DataTestMethod]
        [DataRow("className", "", 16)]
        [DataRow("ClassNameG", "", 16)]
        [DataRow("ClassNAme", "", 16)]
        [DataRow("ClassNAMe", "", 16)]
        [DataRow("className", "private", 23)]
        [DataRow("ClassNameG", "private", 23)]
        [DataRow("ClassNAme", "private", 23)]
        [DataRow("ClassNAMe", "private", 23)]
        [DataRow("className", "public", 22)]
        [DataRow("ClassNameG", "public", 22)]
        [DataRow("ClassNAme", "public", 22)]
        [DataRow("ClassNAMe", "public", 22)]
        [DataRow("className", "public static", 29)]
        [DataRow("ClassNameG", "public static", 29)]
        [DataRow("ClassNAme", "public static", 29)]
        [DataRow("ClassNAMe", "public static", 29)]
        [DataRow("className", "internal", 24)]
        [DataRow("ClassNameG", "internal", 24)]
        [DataRow("ClassNAme", "internal", 24)]
        [DataRow("ClassNAMe", "internal", 24)]
        public void NamingConventionAnalyzer_RaisesDiagnostic_WhenClassNameBreaksNamingConventionRulesIgnoringAccessModifier(string className, string accessibilityModifier, int expectedColumnLocation)
        {
            var test = $@"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {{
        {accessibilityModifier} class {className}
        {{   
        }}
    }}";
            var expected = new DiagnosticResult
            {
                Id = "CAP0004",
                Message = $"{nameof(NamingConventionAnalyzer)}: {className} does not satisfy naming convention. \n{className} must start with one upper case character,\nnot end with uppercase character and not contain two consecutive upper case characters.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, expectedColumnLocation)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void NamingConventionAnalyzer_DoesNotRaiseDiagnostic_WhenPrivateFieldsMeetsNamingConvention()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string fieldNameDoesNotMeetNamingConvention;
        }
    }";

            VerifyCSharpDiagnostic(test);

        }

        [DataTestMethod]
        [DataRow("fieldName", "public", 27)]
        [DataRow("fieldNAme", "public", 27)]
        [DataRow("fieldNAMe", "public", 27)]
        [DataRow("fieldNamE", "public", 27)]
        [DataRow("fieldName", "public static", 34)]
        [DataRow("fieldNAme", "public static", 34)]
        [DataRow("fieldNAMe", "public static", 34)]
        [DataRow("fieldNamE", "public static", 34)]
        [DataRow("fieldName", "public readonly", 36)]
        [DataRow("fieldNAme", "public readonly", 36)]
        [DataRow("fieldNAMe", "public readonly", 36)]
        [DataRow("fieldNamE", "public readonly", 36)]
        [DataRow("fieldName", "protected", 30)]
        [DataRow("fieldNAme", "protected", 30)]
        [DataRow("fieldNAMe", "protected", 30)]
        [DataRow("fieldNamE", "protected", 30)]
        [DataRow("fieldName", "protected internal", 39)]
        [DataRow("fieldNAme", "protected internal", 39)]
        [DataRow("fieldNAMe", "protected internal", 39)]
        [DataRow("fieldNamE", "protected internal", 39)]
        [DataRow("fieldName", "internal", 29)]
        [DataRow("fieldNAme", "internal", 29)]
        [DataRow("fieldNAMe", "internal", 29)]
        [DataRow("fieldNamE", "internal", 29)]
        public void NamingConventionAnalyzer_RaisesDiagnostic_PublicFieldNameMustStartWithUpperCaseCharacter(string fieldName, string accessibilityModifier, int expectedColumnLocation)
        {
            var test = $@"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {{
        class TypeName
        {{   
            {accessibilityModifier} string {fieldName};
        }}
    }}";
            var expected = new DiagnosticResult
            {
                Id = "CAP0004",
                Message = $"{nameof(NamingConventionAnalyzer)}: {fieldName} does not satisfy naming convention. \n{fieldName} must start with one upper case character,\nnot end with uppercase character and not contain two consecutive upper case characters.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 13, expectedColumnLocation)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void NamingConventionAnalyzer_DoesNotRaiseDiagnostic_ForPrivatePropertyName()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            private string propertyName { get; set; }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [DataTestMethod]
        [DataRow("fieldName", "public", 27)]
        [DataRow("fieldNAme", "public", 27)]
        [DataRow("fieldNAMe", "public", 27)]
        [DataRow("fieldNamE", "public", 27)]
        [DataRow("fieldName", "public static", 34)]
        [DataRow("fieldNAme", "public static", 34)]
        [DataRow("fieldNAMe", "public static", 34)]
        [DataRow("fieldNamE", "public static", 34)]
        [DataRow("fieldName", "public readonly", 36)]
        [DataRow("fieldNAme", "public readonly", 36)]
        [DataRow("fieldNAMe", "public readonly", 36)]
        [DataRow("fieldNamE", "public readonly", 36)]
        [DataRow("fieldName", "protected", 30)]
        [DataRow("fieldNAme", "protected", 30)]
        [DataRow("fieldNAMe", "protected", 30)]
        [DataRow("fieldNamE", "protected", 30)]
        [DataRow("fieldName", "protected internal", 39)]
        [DataRow("fieldNAme", "protected internal", 39)]
        [DataRow("fieldNAMe", "protected internal", 39)]
        [DataRow("fieldNamE", "protected internal", 39)]
        [DataRow("fieldName", "internal", 29)]
        [DataRow("fieldNAme", "internal", 29)]
        [DataRow("fieldNAMe", "internal", 29)]
        [DataRow("fieldNamE", "internal", 29)]
        public void NamingConventionAnalyzer_RaisesDiagnostic_PublicPropertyNameMustStartWithUpperCaseCharacter(string propertyName, string accessibilityModifier, int expectedColumnLocation)
        {
            var test = $@"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {{
        class TypeName
        {{   
            {accessibilityModifier} string {propertyName} {{ get; set; }}
        }}
    }}";
            var expected = new DiagnosticResult
            {
                Id = "CAP0004",
                Message = $"{nameof(NamingConventionAnalyzer)}: {propertyName} does not satisfy naming convention. \n{propertyName} must start with one upper case character,\nnot end with uppercase character and not contain two consecutive upper case characters.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 13, expectedColumnLocation)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new NamingConventionAnalyzer();
        }
    }
}