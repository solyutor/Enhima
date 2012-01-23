using Enhima.Tests.Domain;
using NUnit.Framework;

namespace Enhima.Tests
{
    public class SubclassFixture : MappingFixture
    {
        [Test]
        public void Discriminator_value_should_be_class_name()
        {
            Assert.That(SubclassMappingOf<ProductSet>().DiscriminatorValue, Is.EqualTo("ProductSet"));
        }
    }
}