using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Analyzers
{
    [TestClass]
    public class CommentsAnalyzerTests : CodeFixVerifier
    {
        private readonly int _threshold = 30;

        [TestMethod]
        public void MethodDeclaration_NoComments_Passes()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
            public void Method1()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodDeclaration_SummaryTagsComments_Passes()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
        /// <summary>
        /// </summary>
            public void Method1()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodDeclaration_LessThan30LinesComments_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public void Method1()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodDeclaration_30LinesComments_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(28, "/// Method1 comments")}
        /// </summary>
            public void Method1()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodDeclaration_More30LinesComments_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: Documentation comments from Method1 exceed the allowed maximum number of lines {_threshold}.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 38, 25)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(29, "/// Method1 comments")}
        /// </summary>
            public void Method1()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void PropertyDeclaration_NoComments_Passes()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
            public string GetTypeName{get;set;}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void PropertyDeclaration_SummaryTagsComments_Passes()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
        /// <summary>
        /// </summary>
            public string GetTypeName{get;set;}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void PropertyDeclaration_LessThan30LinesComments_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public string GetTypeName{{get;set;}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void PropertyDeclaration_30LinesComments_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(28, "/// Method1 comments")}
        /// </summary>
            public string GetTypeName{{get;set;}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void PropertyDeclaration_More30LinesComments_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: Documentation comments from GetTypeName exceed the allowed maximum number of lines {_threshold}.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 38, 27)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(29, "/// Method1 comments")}
        /// </summary>
            public string GetTypeName{{get;set;}}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }


        [TestMethod]
        public void ClassDeclaration_NoComments_Passes()
        {
            var test = @"

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
        public void ClassDeclaration_SummaryTagsComments_Passes()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        /// <summary>
        /// </summary>
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
        public void ClassDeclaration_LessThan30LinesComments_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Class comments")}
        /// </summary>
        public class TypeName
        {{   
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ClassDeclaration_30LinesComments_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        /// <summary>
{GenerateMultipleLinesOfText(28, "/// Method1 comments")}
        /// </summary>
        public class TypeName
        {{   
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ClassDeclaration_More30LinesComments_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: Documentation comments from TypeName exceed the allowed maximum number of lines {_threshold}.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 36, 22)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        /// <summary>
{GenerateMultipleLinesOfText(29, "/// Method1 comments")}
        /// </summary>
        public class TypeName
        {{   
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void InterfaceDeclaration_NoComments_Passes()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public interface ITypeName
        {   
            
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void InterfaceDeclaration_SummaryTagsComments_Passes()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        /// <summary>
        /// </summary>
        public interface ITypeName
        {  
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void InterfaceDeclaration_LessThan30LinesComments_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
        public interface ITypeName
        {{   
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void InterfaceDeclaration_30LinesComments_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        /// <summary>
{GenerateMultipleLinesOfText(28, "/// Method1 comments")}
        /// </summary>
        public interface ITypeName
        {{ 
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void InterfaceDeclaration_More30LinesComments_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: Documentation comments from ITypeName exceed the allowed maximum number of lines {_threshold}.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 36, 26)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        /// <summary>
{GenerateMultipleLinesOfText(29, "/// Method1 comments")}
        /// </summary>
        public interface ITypeName
        {{  
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclaration_NoComments_Passes()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
            public void Method1()
            {
                string testString =""TestString"";
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void VariableDeclaration_LessThan5LinesCommentsBeforeVariable_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public void Method1()
            {{
{GenerateMultipleLinesOfText(3, "// Method1 comments")}
                string testString =""TestString"";
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void VariableDeclaration_5LinesCommentsBeforeVariable_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public void Method1()
            {{
{GenerateMultipleLinesOfText(5, "// Method1 comments")}
                string testString =""TestString"";
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void VariableDeclaration_LessThan5LinesCommentsAfterVariable_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public void Method1()
            {{
                string testString =""TestString"";
{GenerateMultipleLinesOfText(3, "// Method1 comments")}
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void VariableDeclaration_5LinesCommentsAfterVariable_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public void Method1()
            {{
                string testString =""TestString"";
{GenerateMultipleLinesOfText(5, "// Method1 comments")}
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void VariableDeclaration_CommentsBeforeAndAfterVariable_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has both leading and trailing comments. Only one type is allowed.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 31, 24)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public void Method1()
            {{
{GenerateMultipleLinesOfText(3, "// Method1 comments")}
                string testString =""TestString""; // Method1 comments
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclaration_MultiLineAndSingleLineCommentsBeforeVariable_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has both multiline and single line comments. Please use only one type of comments.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 13, 24)
                    }
            };

            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
            public void Method1()
            {
                /*
                   Multiline comments
                */
                //Single line comment
                string testString =""TestString"";
            }
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclaration_MultipleMultiLineCommentsBeforeVariable_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has multiple MultiLines comments. Please use no more than 1 MultiLines of comments.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 15, 24)
                    }
            };

            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
            public void Method1()
            {
                /*
                   Multiline comments
                */
                /*
                   Multiline comments
                */               
                string testString =""TestString"";
            }
        }
    }";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclaration_MoreThan5LinesOfMultiLineCommentsBeforeVariable_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has more than {5} lines comments. Please use no more than {5} lines of comments.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 17, 24)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
            public void Method1()
            {{ 
                /*
                   {GenerateMultipleLinesOfText(6, "Sample comments")}
                */            
                string testString =""TestString"";
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclaration_MoreThan5SingleLineCommentsBeforeVariable_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has more than {5} lines comments. Please use no more than {5} lines of comments.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 15, 24)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
            public void Method1()
            {{
{GenerateMultipleLinesOfText(6, "// Single line comments")}             
                string testString =""TestString"";
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }


        [TestMethod]
        public void VariableDeclaration_6LinesOfMultiLineCommentsBeforeVariable_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has more than {5} lines comments. Please use no more than {5} lines of comments.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 16, 24)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
            public void Method1()
            {{ 
                /*
                   {GenerateMultipleLinesOfText(5, "Sample comments")}
                */            
                string testString =""TestString"";
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void VariableDeclaration_6SingleLineCommentsBeforeVariable_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: testString has more than {5} lines comments. Please use no more than {5} lines of comments.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 15, 24)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
            public void Method1()
            {{
{GenerateMultipleLinesOfText(6, "// Single line comments")}             
                string testString =""TestString"";
            }}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void ConstructorDeclaration_NoMethodComments_Passes()
        {
            var test = @"

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
        public void ConstructorDeclaration_WithSummaryMethodComments_SummaryTagsComments_Passes()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {   
        /// <summary>
        /// </summary>
            public TypeName()
{
}
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ConstructorDeclaration_WithSummaryMethodComments_LessThan30LinesComments_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(17, "/// Method1 comments")}
        /// </summary>
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ConstructorDeclaration_WithSummaryMethodComments_30LinesComments_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(28, "/// Method1 comments")}
        /// </summary>
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ConstructorDeclaration_WithSummaryMethodComments_More30LinesComments_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0006",
                Message = $"{nameof(CommentsAnalyzer)}: Documentation comments from TypeName exceed the allowed maximum number of lines {_threshold}.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 38, 20)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
        /// <summary>
{GenerateMultipleLinesOfText(29, "/// Method1 comments")}
        /// </summary>
            public TypeName()
{{
}}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CommentsAnalyzer();
        }

    }
}