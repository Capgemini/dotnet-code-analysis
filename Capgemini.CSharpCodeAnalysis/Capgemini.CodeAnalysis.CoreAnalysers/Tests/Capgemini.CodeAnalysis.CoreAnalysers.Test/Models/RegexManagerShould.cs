using Capgemini.CodeAnalysis.CoreAnalysers.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Models
{
    [TestClass]
    public class RegexManagerShould
    {
        private RegexManager regexManager;

        [TestInitialize]
        public void Initialize()
        {
            regexManager = new RegexManager();
        }

        [DataTestMethod]
        [DataRow("", 0)]
        [DataRow("a\r\n", 1)]
        [DataRow("a\r\na", 2)]
        [DataRow("a\r\na\r\na", 3)]
        public void ReturnTheCorrectNumberOfLinesForTheInputString(string inputString, int expectedRowCount)
        {
            var lineCount = regexManager.NumberOfLines(inputString);

            lineCount.Should().Be(expectedRowCount);
        }
    }
}
