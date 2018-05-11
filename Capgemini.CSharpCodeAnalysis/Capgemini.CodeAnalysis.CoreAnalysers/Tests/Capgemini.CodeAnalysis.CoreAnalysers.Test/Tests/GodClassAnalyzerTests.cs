using System.Text;
using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Tests
{
    [TestClass]
    public class GodClassAnalyzerTests : CodeFixVerifier
    {
        private const int ClassMaxNumberOfPublicMethods = 20;

        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Class_With_NoPublicMethod_Passes()
        {
            var test = @"

    namespace ConsoleApplication1
    {
        public class TypeName
        {              
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Class_With_Single_PublicMethod_And19ProtectedMethods_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{      
            {GeneratePublicMethodText(1)}
            {GenerateProtectedMethodText(19)}
            {GeneratePrivateMethodText(20)}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Class_With_Single_PublicMethod_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{      
            {GeneratePublicMethodText(1)} 
            {GeneratePrivateMethodText(20)}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Class_With_Single_ProtectedMethod_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{      
            {GenerateProtectedMethodText(1)} 
            {GeneratePrivateMethodText(20)}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Class_With_20_PublicMethods_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{     
            {GeneratePublicMethodText(20)}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }


        [TestMethod]
        public void Class_With_19_PublicMethods_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{     
            {GeneratePublicMethodText(19)}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Class_With_20PublicMethods_AndOtherOtherPrivateMethods_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{     
            {GeneratePublicMethodText(20)} 
            {GeneratePrivateMethodText(20)}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }


        [TestMethod]
        public void Class_With_20ProtectedMethods_AndOtherOtherPrivateMethods_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{     
            {GenerateProtectedMethodText(20)} 
            {GeneratePrivateMethodText(20)}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Class_With_20PublicMethods_And_ProtectedMethods_Methods_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0008",
                Message = $"{nameof(GodClassAnalyzer)} \'This class has 21 methods which is more than the recommended {ClassMaxNumberOfPublicMethods} methods. \nPlease consider applying the SOLID principles to the class design. \nIt is recommended to have small focused classes.\'",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs",  5, 22)
                    }
            };
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{     
            {GeneratePublicMethodText(20)}
            {GenerateProtectedMethodText(1)}
            {GeneratePrivateMethodText(20)}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }


        [TestMethod]
        public void Class_With_MoreThan20ProtectedMethods_Methods_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0008",
                Message = $"{nameof(GodClassAnalyzer)} \'This class has 21 methods which is more than the recommended {ClassMaxNumberOfPublicMethods} methods. \nPlease consider applying the SOLID principles to the class design. \nIt is recommended to have small focused classes.\'",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs",  5, 22)
                    }
            };
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{      
            {GenerateProtectedMethodText(21)}
            {GeneratePrivateMethodText(20)}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Class_With_20_PublicMethods_And_1_PublicContructor_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0008",
                Message = $"{nameof(GodClassAnalyzer)} \'This class has 21 methods which is more than the recommended {ClassMaxNumberOfPublicMethods} methods. \nPlease consider applying the SOLID principles to the class design. \nIt is recommended to have small focused classes.\'",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs",  5, 22)
                    }
            };
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{     
            public TypeName(){{}}
            {GeneratePublicMethodText(20)} 
            {GeneratePrivateMethodText(20)}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Class_With_20_PublicAndProtectedMethods_And_1_PublicContructor_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0008",
                Message = $"{nameof(GodClassAnalyzer)} \'This class has 21 methods which is more than the recommended {ClassMaxNumberOfPublicMethods} methods. \nPlease consider applying the SOLID principles to the class design. \nIt is recommended to have small focused classes.\'",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 5, 22)
                    }
            };
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{     
            public TypeName(){{}}
            {GeneratePublicMethodText(15)}
            {GenerateProtectedMethodText(5)}
            {GeneratePrivateMethodText(20)}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Class_With_20_Public_And_Protected_Methods_With_1_PrivateContructor_Passes()
        {
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
            private TypeName(){{}}
            {GeneratePublicMethodText(3)}
            {GenerateProtectedMethodText(17)}
            {GeneratePrivateMethodText(20)}
        }}
    }}";

            VerifyCSharpDiagnostic(test);
        }


        [TestMethod]
        public void Class_With_MoredThan20_PublicAndProtectedMethods_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0008",
                Message = $"{nameof(GodClassAnalyzer)} \'This class has 24 methods which is more than the recommended {ClassMaxNumberOfPublicMethods} methods. \nPlease consider applying the SOLID principles to the class design. \nIt is recommended to have small focused classes.\'",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 5, 22)
                    }
            };

            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
            private TypeName(){{}}
            {GeneratePublicMethodText(13)}
            {GenerateProtectedMethodText(11)}
            {GeneratePrivateMethodText(20)}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Class_WithMoreThan20PublicMethods_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0008",
                Message = $"{nameof(GodClassAnalyzer)} \'This class has 21 methods which is more than the recommended {ClassMaxNumberOfPublicMethods} methods. \nPlease consider applying the SOLID principles to the class design. \nIt is recommended to have small focused classes.\'",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs",  5, 22)
                    }
            };
            var test = $@"

    namespace ConsoleApplication1
    {{
        public class TypeName
        {{   
            {GeneratePublicMethodText(21)}
        }}
    }}";

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new GodClassAnalyzer();
        }

        private string GeneratePublicMethodText(int numberOfLines)
        {
            var stringBuilder = new StringBuilder();
            for (var counter = 0; counter < numberOfLines; counter++)
            {
                if (counter == numberOfLines - 1)
                {
                    stringBuilder.Append($" public void Method{counter}(){{}}");
                }
                else
                {
                    stringBuilder.AppendLine($" public  void Method{counter}(){{}}");
                }
            }

            return stringBuilder.ToString();
        }

        private string GeneratePrivateMethodText(int numberOfLines)
        {
            var stringBuilder = new StringBuilder();
            for (var counter = 0; counter < numberOfLines; counter++)
            {
                if (counter == numberOfLines - 1)
                {
                    stringBuilder.Append($" private void Method{counter}(){{}}");
                }
                else
                {
                    stringBuilder.AppendLine($" private  void Method{counter}(){{}}");
                }
            }

            return stringBuilder.ToString();
        }

        private string GenerateProtectedMethodText(int numberOfLines)
        {
            var stringBuilder = new StringBuilder();
            for (var counter = 0; counter < numberOfLines; counter++)
            {
                if (counter == numberOfLines - 1)
                {
                    stringBuilder.Append($" protected void Method{counter}(){{}}");
                }
                else
                {
                    stringBuilder.AppendLine($" protected  void Method{counter}(){{}}");
                }
            }

            return stringBuilder.ToString();
        }
    }
}