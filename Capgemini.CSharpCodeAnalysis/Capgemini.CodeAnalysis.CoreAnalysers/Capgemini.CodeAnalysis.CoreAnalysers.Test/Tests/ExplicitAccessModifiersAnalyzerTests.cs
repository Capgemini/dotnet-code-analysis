using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.Foundation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Tests
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
                Id = AnalyserConstants.ExplicitAccessModifiersAnalyzerId,
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


        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new ExplicitAccessModifiersAnalyzer();
        }
    }
}