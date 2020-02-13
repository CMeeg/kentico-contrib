using NUnit.Framework;

namespace Meeg.Kentico.Configuration.Cms.Tests
{
    [TestFixture]
    public class FindCmsQueryByNameQueryTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("QueryNameNotInCodeNameFormat")]
        public void Ctor_WithInvalidQueryName_ThrowsException(string queryName)
        {
            Assert.That(
                () => new FindCmsQueryByNameQuery(queryName),
                Throws.ArgumentException
            );
        }

        [TestCase("ClassName.QueryName", "ClassName", "QueryName")]
        [TestCase("Namespace.ClassName.QueryName", "Namespace.ClassName", "QueryName")]
        public void Ctor_WithValidQueryName_HasClassAndQueryName(string queryName, string expectedClassName, string expectedQueryName)
        {
            var query = new FindCmsQueryByNameQuery(queryName);

            Assert.Multiple(() =>
            {
                Assert.That(query.ClassName, Is.EqualTo(expectedClassName));
                Assert.That(query.QueryName, Is.EqualTo(expectedQueryName));
            });
        }
    }
}
