﻿using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.CoreAnalysers.Extensions;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Analyzers
{
    [TestClass]
    public class ExplicitAccessModifiersAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ExplicitAccessModifiersAnalyzer_Passes_Class_WithPublicKeyword()
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
        public class TypeName
        {   
            public TypeName()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ExplicitAccessModifiersAnalyzer_Passes_Class_WithInternalKeyword()
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
        public class TypeName
        {   
            internal TypeName()
{
} 
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ExplicitAccessModifiersAnalyzer_Passes_Class_WithPrivateKeyword()
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
        public class TypeName
        {    
            private TypeName()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ExplicitAccessModifiersAnalyzer_PassesForStaticConstructorWithNoAccessModifier()
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
        public class TypeName
        {   
            static TypeName()
            {
            }
        }
    }";
            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ExplicitAccessModifiersAnalyzer_FailsForNonStaticConstructorWithNoAccessModifier()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0001",
                Message = $"{nameof(ExplicitAccessModifiersAnalyzer)}: Constructor TypeName must include an access modifier.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                            new[] {
                                    new DiagnosticResultLocation("Test0.cs", 13, 13)
                                }
            };

            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
            TypeName()
            {
            }
        }
    }";
            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void RaiseErrorForClassWithNoAccessModifierSpecified()
        {
            var original = @"
namespace ConsoleApplication1
{
    class MyClass
    {
    }
}";

            VerifyCSharpDiagnostic(original,
                new DiagnosticResult
                {
                    Id = AnalyzerType.ExplicitAccessModifiersAnalyzerId.ToDiagnosticId(),
                    Message = $"{nameof(ExplicitAccessModifiersAnalyzer)}: MyClass must include an access modifier.",
                    Severity = DiagnosticSeverity.Error,
                    Locations =
                    new DiagnosticResultLocation[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 4, 11)
                    }
                });
        }

        [TestMethod]
        public void RaiseErrorForStaticClassWithNoAccessModifierSpecified()
        {
            var original = @"
namespace ConsoleApplication1
{
    static class MyClass
    {
    }
}";
            VerifyCSharpDiagnostic(original,
                new DiagnosticResult
                {
                    Id = AnalyzerType.ExplicitAccessModifiersAnalyzerId.ToDiagnosticId(),
                    Message = $"{nameof(ExplicitAccessModifiersAnalyzer)}: MyClass must include an access modifier.",
                    Severity = DiagnosticSeverity.Error,
                    Locations =
                    new DiagnosticResultLocation[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 4, 18)
                    }
                });
        }

        [TestMethod]
        public void RaiseErrorForObsoleteClassWithNoAccessModifierSpecified()
        {
            var original = @"
using System;

namespace ConsoleApplication1
{
    [Obsolete]
    class MyClass
    {
        public void Method() { }
    }
}";

            VerifyCSharpDiagnostic(original,
                new DiagnosticResult
                {
                    Id = AnalyzerType.ExplicitAccessModifiersAnalyzerId.ToDiagnosticId(),
                    Message = $"{nameof(ExplicitAccessModifiersAnalyzer)}: MyClass must include an access modifier.",
                    Severity = DiagnosticSeverity.Error,
                    Locations =
                    new DiagnosticResultLocation[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 7, 11)
                    }
                });
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new ExplicitAccessModifiersAnalyzer();
        }
    }
}