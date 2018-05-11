using System;
using Capgemini.CodeAnalysis.CoreAnalysers.Analyzers;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Tests
{
    [TestClass]
    public class MethodComplexityAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void AnalysisPassesForNoCode()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodWithNoLogicPasses()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace Test0
    {
        public class TypeName
        {   
            public void DoStuff()
            {
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }
        
        [TestMethod]
        public void MethodWithComplexityLessThan15_Passes()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace Test0
    {
        public class TypeName
        {   
            public void DoStuff(int input)
            {
                var variable1 = 20;
                var variable2  = ""Hello world"";
                if(input>variable1)
                {
                    variable2 = ""Maybe not hello world!"";
                }
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodWithReturnValueAndComplexityLessThan15_Passes()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace Test0
    {
        public class TypeName
        {              
            public bool IsALeapYear(int year)
            {
                var leapYear =false; 
                var date = new DateTime(year,3,1);
                var lastDayOfFebruaryForThisYear = date.AddDays(-1);

                if (lastDayOfFebruaryForThisYear.Day>28)
                {
                    leapYear=true;
                }
                return leapYear;
            }
        }
    }";
            if (DateTime.UtcNow.IsDaylightSavingTime() && DateTime.UtcNow.DayOfWeek == DayOfWeek.Friday)
            {
                var date = new DateTime(DateTime.UtcNow.Year, 3, 1);
                var lastDayOfFebruaryForThisYear = date.AddDays(-1);

                if (lastDayOfFebruaryForThisYear.Day > 28)
                {

                }
            }
            VerifyCSharpDiagnostic(test);
        }
        [TestMethod]
        public void MethodWithComplexityOf15_Passes()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace Test0
    {
        public class TypeName
        {              
            public void DoStuff(int year, string message,int input)
            {   
                if(!string.IsNullOrWhiteSpace(message))
                {
                    var variable1 = 20;
                   var processedMessage = new StringBuilder();
                    if(input > variable1)
                    {
                        processedMessage.AppendLine( ""Maybe not hello world!"");
                    }
                    else if(input < variable1)
                    {
                       processedMessage.AppendLine( message + ""Some other interesting message!"");
                    }
                    else
                    {
                         processedMessage.AppendLine( ""Hello world"" + message);
                    }

                    var leapYear =false; 
                    var date = new DateTime(year,3,1);
                    var lastDayOfFebruaryForThisYear = date.AddDays(-1);

                    if (lastDayOfFebruaryForThisYear.Day>28)
                    {
                        leapYear=true;
                    }

                    var curDate = DateTime.UtcNow;
                    if (curDate.Month == 2 && curDate.Day > 20)
                    {
                        processedMessage.AppendLine(""This is some month!"");
                    }
                    else if (leapYear && curDate.Month > 6 && && curDate.Mont < 8 && curDate.Day > 10 && curDate.Day < 31)
                    {
                        processedMessage.AppendLine(""This is getting exciting!"");
                    }
                    else if (curDate.Month == 12 && curDate.Day == 25)
                    {
                        processedMessage.AppendLine(""This is Christmas!"");
                    }
            }
        }

    }
}";
            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void MethodWithComplexityGreaterThan15_Fails()
        {
            var expected = new DiagnosticResult
            {
                Id = "CAP0014",
                Message = $"{nameof(MethodComplexityAnalyzer)}: The cyclometric complexity of this method method is 17 which is greater than the maximum value of 15. Please consider splitting this method into smaller methods.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 13, 25)
                    }
            };

            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace Test0
    {
        public class TypeName
        {              
            public void DoStuff(int year, string message,int input)
            {   
                if(!string.IsNullOrWhiteSpace(message))
                {
                    var variable1 = 20;
                   var processedMessage = new StringBuilder();
                    if(input > variable1)
                    {
                        processedMessage.AppendLine( ""Maybe not hello world!"");
                    }
                    else if(input < variable1)
                    {
                       processedMessage.AppendLine( message + ""Some other interesting message!"");
                    }
                    else
                    {
                         processedMessage.AppendLine( ""Hello world"" + message);
                    }

                    var leapYear =false; 
                    var date = new DateTime(year,3,1);
                    var lastDayOfFebruaryForThisYear = date.AddDays(-1);

                    if (lastDayOfFebruaryForThisYear.Day>28)
                    {
                        leapYear=true;
                    }

                    var curDate = DateTime.UtcNow;
                    if (curDate.Month == 2 && curDate.Day > 20)
                    {
                        processedMessage.AppendLine(""This is some month!"");
                    }
                    else if (leapYear && curDate.Month > 6 && && curDate.Mont < 8 && curDate.Day > 10 && curDate.Day < 31)
                    {
                        processedMessage.AppendLine(""This is getting exciting!"");
                    }
                    else if (curDate.Month == 12 && && curDate.Day == 25 && curDate.UtcNow.DayOfWeek== DayOfWeek.Monday)
                    {
                        processedMessage.AppendLine(""This is Christmas is on Monday!"");
                    }
            }
        }

    }
}";
            
            VerifyCSharpDiagnostic(test,expected);
        }
         
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new MethodComplexityAnalyzer();
        }
    }
}