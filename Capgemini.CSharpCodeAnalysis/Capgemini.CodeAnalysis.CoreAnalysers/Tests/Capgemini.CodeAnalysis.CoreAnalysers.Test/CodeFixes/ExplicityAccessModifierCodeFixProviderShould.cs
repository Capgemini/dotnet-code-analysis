using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.CoreAnalysers.CodeFixes;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.CodeFixes
{
    [TestClass]
    public class ExplicityAccessModifierCodeFixProviderShould : CodeFixVerifier
    {
        [TestMethod]
        public void PrependClassDeclarationWithPublicAccessModifier()
        {
            var original = @"
namespace ConsoleApplication1
{
    class MyClass
    {
    }
}";

            var result = @"
namespace ConsoleApplication1
{
    public class MyClass
    {
    }
}";
            VerifyCSharpFix(original, result);
        }

        [TestMethod]

        public void PrependClassDeclarationWithPublicAccessModifierPreservingLeadingTrivia()
        {
            var original = @"
namespace ConsoleApplication1
{
    /// <summary>
    /// Leading Trivia
    /// </summary>
    class MyClass
    {
    }
}";

            var result = @"
namespace ConsoleApplication1
{
    /// <summary>
    /// Leading Trivia
    /// </summary>
    public class MyClass
    {
    }
}";
            VerifyCSharpFix(original, result);
        }

        [TestMethod]
        public void PrependClassDeclarationWithPublicAccessModifierForStaticClass()
        {
            var original = @"
namespace ConsoleApplication1
{
    static class MyClass
    {
    }
}";

            var result = @"
namespace ConsoleApplication1
{
    public static class MyClass
    {
    }
}";

            VerifyCSharpFix(original, result);
        }

        [TestMethod]
        public void ExplicitAccessModifiers_ClassDeclaration_ContainsAccessModifier_DoesNotInvokeWarning()
        {
            var original = @"
namespace ConsoleApplication1
{
    public class MyClass
    {
    }
}";

            VerifyCSharpDiagnostic(original);
        }

        [TestMethod]
        public void ExplicitAccessModifiers_ClassDeclaration_OnlyChangesAccessModifiers_InvokesWarning()
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

            var result = @"
using System;

namespace ConsoleApplication1
{
    [Obsolete]
    public class MyClass
    {
        public void Method() { }
    }
}";

            VerifyCSharpFix(original, result);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new ExplicitAccessModifiersAnalyzer();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new ExplicitAccessModifierCodeFixProvider();
        }
    }
}