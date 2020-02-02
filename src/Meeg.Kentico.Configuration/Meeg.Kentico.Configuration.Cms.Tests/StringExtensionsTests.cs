using NUnit.Framework;

namespace Meeg.Kentico.Configuration.Cms.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestCase(null, "-")]
        [TestCase("", "-")]
        [TestCase("Source", null)]
        [TestCase("Source", "")]
        [TestCase("Source", "-")]
        public void SplitOnLastIndexOf_WithNoSourceOrValue_ReturnsSourceInSingleItemArray(string source, string value)
        {
            string[] actual = source.SplitOnLastIndexOf(value);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Length, Is.EqualTo(1));
                Assert.That(actual[0], Is.EqualTo(source));
            });
        }

        [TestCase("", "-")]
        [TestCase("Source", "-")]
        public void SplitOnLastIndexOf_WithNothingBeforeValue_ReturnsEmptyStringInLeftPart(string rightPart, string value)
        {
            string source = $"{value}{rightPart}";
            string[] actual = source.SplitOnLastIndexOf(value);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Length, Is.EqualTo(2));
                Assert.That(actual[0], Is.EqualTo(string.Empty));
                Assert.That(actual[1], Is.EqualTo(rightPart));
            });
        }

        [TestCase("Source", "-")]
        [TestCase("Source-", "-")]
        public void SplitOnLastIndexOf_WithNothingAfterValue_ReturnsEmptyStringInRightPart(string leftPart, string value)
        {
            string source = $"{leftPart}{value}";
            string[] actual = source.SplitOnLastIndexOf(value);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Length, Is.EqualTo(2));
                Assert.That(actual[0], Is.EqualTo(leftPart));
                Assert.That(actual[1], Is.EqualTo(string.Empty));
            });
        }

        [TestCase("Left", "Right", "-")]
        [TestCase("Left-", "Right", "-")]
        [TestCase("-Left", "Right", "-")]
        [TestCase("-Left-", "Right", "-")]
        [TestCase("Left-Left", "Right", "-")]
        [TestCase("Left-Left-", "Right", "-")]
        [TestCase("-Left-Left", "Right", "-")]
        [TestCase("-Left-Left-", "Right", "-")]
        [TestCase("--Left-Left--", "Right", "-")]
        public void SplitOnLastIndexOf_WithSourceContainingValue_SplitsOnLastIndexOfValue(string leftPart, string rightPart, string value)
        {
            string source = $"{leftPart}{value}{rightPart}";
            string[] actual = source.SplitOnLastIndexOf(value);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Length, Is.EqualTo(2));
                Assert.That(actual[0], Is.EqualTo(leftPart));
                Assert.That(actual[1], Is.EqualTo(rightPart));
            });
        }
    }
}
