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
        public void PrependClassDeclarationWithPublicAccessModifierForStaticClassPreservingLeadingTrivia()
        {
            var original = @"
namespace ConsoleApplication1
{
    /// <summary>
    /// Leading Trivia
    /// </summary>
    static class MyClass
    {
    }
}";

            var result = @"
namespace ConsoleApplication1
{
    /// <summary>
    /// Leading Trivia
    /// </summary>
    public static class MyClass
    {
    }
}";

            VerifyCSharpFix(original, result);
        }

        [TestMethod]
        public void PrependClassDeclarationWithInternalAccessModifier()
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
    internal class MyClass
    {
    }
}";
            VerifyCSharpFix(original, result, 1);
        }

        [TestMethod]

        public void PrependClassDeclarationWithInternalAccessModifierPreservingLeadingTrivia()
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
    internal class MyClass
    {
    }
}";
            VerifyCSharpFix(original, result, 1);
        }

        [TestMethod]
        public void PrependClassDeclarationWithInternalAccessModifierForStaticClass()
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
    internal static class MyClass
    {
    }
}";

            VerifyCSharpFix(original, result, 1);
        }
        [TestMethod]
        public void PrependClassDeclarationWithProtectedAccessModifier()
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
    protected class MyClass
    {
    }
}";
            VerifyCSharpFix(original, result, 2);
        }

        [TestMethod]

        public void PrependClassDeclarationWithProtectedAccessModifierPreservingLeadingTrivia()
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
    protected class MyClass
    {
    }
}";
            VerifyCSharpFix(original, result, 2);
        }

        [TestMethod]
        public void PrependClassDeclarationWithProtectedAccessModifierForStaticClass()
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
    protected static class MyClass
    {
    }
}";

            VerifyCSharpFix(original, result, 2);
        }

        [TestMethod]
        public void PrependClassDeclarationWithProtectedAccessModifierForStaticClassPreservingLeadingTrivia()
        {
            var original = @"
namespace ConsoleApplication1
{
    /// <summary>
    /// Leading Trivia
    /// </summary>
    static class MyClass
    {
    }
}";

            var result = @"
namespace ConsoleApplication1
{
    /// <summary>
    /// Leading Trivia
    /// </summary>
    protected static class MyClass
    {
    }
}";

            VerifyCSharpFix(original, result, 2);
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