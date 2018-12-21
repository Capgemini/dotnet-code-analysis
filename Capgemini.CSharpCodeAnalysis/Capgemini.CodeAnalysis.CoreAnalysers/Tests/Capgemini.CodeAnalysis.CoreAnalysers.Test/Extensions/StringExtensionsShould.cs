using Capgemini.CodeAnalysis.CoreAnalysers.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capgemini.CodeAnalysis.CoreAnalysers.Test.Extensions
{
    [TestClass]
    public class StringExtensionsShould
    {
        [DataTestMethod]
        [DataRow(@"D:\Project\Product.Mock\Product.Mock\bin\Class.cs")]
        [DataRow(@"D:\Project\Product.Mock\Product.Mock\service\Class.cs")]
        [DataRow(@"D:\Project\Product.Mock\Product.Mock\obj\Class.cs")]
        [DataRow(@"D:\Project\Product.Mock\Product.Mock\TemporaryGeneratedFile_\Class.cs")]
        [DataRow(@"D:\Project\Product.Mock\Product.Mock\assemblyinfo.cs")]
        [DataRow(@"D:\Project\Product.Mock\Product.Mock\assemblyattributes.cs")]
        [DataRow(@"D:\Project\Product.Mock\Product.Mock\x.g.something.cs")]
        [DataRow(@"D:\Project\Product.Mock\Product.Mock\x.i.something.cs")]
        [DataRow(@"D:\Project\Product.Mock\Product.Mock\something.designer.cs")]
        [DataRow(@"D:\Project\Product.Mock\Product.Mock\x.generated.file.cs")]
        [DataRow(@"D:\Project\Product.Mock\Product.Mock\_Layout.g.cshtml.cs")]
        public void ReturnTrueForSuppliedStrings(string pathToTest)
        {
            var result = pathToTest.IsGeneratedFileName();

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(@"D:\Project\Product.Mock\Product.Mock\TestDelete\binned\Class.cs")]
        public void ReturnFalseForSuppliedStrings(string pathToTest)
        {
            var result = pathToTest.IsGeneratedFileName();

            Assert.IsFalse(result);
        }
    }
}