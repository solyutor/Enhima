using Enhima.Tests.Domain;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace Enhima.Tests
{
    [TestFixture]
    public class ElementCollection : MappingFixture
    {
        [Test]
        public void Element_should_have_proper_type()
        {
            var bag = MappingOf<Product>().Get<HbmIdbag>("Opinions");

            var element = (HbmElement)bag.Item;

            Assert.That(element.type1, Is.EqualTo("String"));
        }
    }
}