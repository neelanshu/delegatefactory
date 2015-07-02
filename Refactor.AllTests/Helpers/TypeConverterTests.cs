using NUnit.Framework;
using Refactor.Lib.Helpers;

namespace Refactor.AllTests
{
    [TestFixture]
    public class TypeConverterTests
    {
        private ITypeConverter _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new TypeConverter();
        }

        [TestCase("true", true)]
        [TestCase("True", true)]
        [TestCase("False", false)]
        [TestCase("false", false)]
        public void ShouldReturnCorrectBooleansWhenStringsAreValidBoolean(string strToConvert, bool expectedBool)
        {
            Assert.AreEqual(expectedBool, _sut.SafeConvertStringToBool(strToConvert));
        }

        [TestCase(null)]
        [TestCase("0")]
        [TestCase("10")]
        [TestCase("abc")]
        [TestCase("!*(*(*(")]
        [TestCase("")]
        [TestCase(" ")]
        public void ShouldReturnFalseWhenStringIsNullOrInvalidBoolean(string strToConvert)
        {
            Assert.IsFalse(_sut.SafeConvertStringToBool(strToConvert));
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("10", 10)]
        [TestCase("-1", -1)]
        public void ShouldReturnCorrectIntWhenStringsAreValidInt(string strToConvert, int expectedInt)
        {
            Assert.AreEqual(expectedInt, _sut.SafeConvertStringToInt(strToConvert));
        }

        [TestCase(null)]
        [TestCase("abc")]
        [TestCase("!*(*(*(")]
        [TestCase("")]
        [TestCase(" ")]
        public void ShouldReturnZeroWhenStringsAreInvalidInt(string strToConvert)
        {
            Assert.AreEqual(0, _sut.SafeConvertStringToInt(strToConvert));
        }
        
        [TestCase("1", -1)]
        [TestCase("-10", -10)]
        public void ShouldReturnNegativeIntWhenStringsAreValidInt(string strToConvert, int expectedInt)
        {
            Assert.AreEqual(expectedInt, _sut.SafeConvertStringToNegativeInt(strToConvert));
        }


        [TestCase(null)]
        [TestCase("abc")]
        [TestCase("!*(*(*(")]
        [TestCase("")]
        [TestCase(" ")]
        public void ShouldReturnMinusOneWhenStringsAreInvalidInt(string strToConvert)
        {
            Assert.AreEqual(-1, _sut.SafeConvertStringToNegativeInt(strToConvert));
        }
    }

}
