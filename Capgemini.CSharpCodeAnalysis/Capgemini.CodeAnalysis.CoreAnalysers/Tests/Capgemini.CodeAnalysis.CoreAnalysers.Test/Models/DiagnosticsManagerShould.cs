using System;
using System.Collections.Generic;
using System.Text;
using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Models
{
    [TestClass]
    public class DiagnosticsManagerShould
    {
        private DiagnosticsManager systemUnderTest;
        private SyntaxNodeAnalysisContext s;
        private Location location;
        private DiagnosticDescriptor rule;

        [TestInitialize]
        public void Initialize()
        {
            s = new SyntaxNodeAnalysisContext();
            location = Substitute.For<Location>();
            rule = new DiagnosticDescriptor();
            systemUnderTest = new DiagnosticsManager();
        }

        [TestMethod]
        public void ReturnTheExpectedMessageFromCreateStaticClassDiagnostic()
        {
            systemUnderTest.CreateStaticClassDiagnostic(s, location, rule);
        }
    }
}
