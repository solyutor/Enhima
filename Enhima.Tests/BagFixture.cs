using Enhima.Tests.Domain;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;

namespace Enhima.Tests
{
    public class BagFixture : MappingFixture
    {
        [Test]
        public void Bag_key_column_should_be_named_as_base_class()
        {
            var bag = SubclassMappingOf<ProductSet>().Get<HbmBag>("Components");

            Assert.That(bag.key.column1, Is.EqualTo("ProductSetId"));
        }

        [Test, Ignore]
        public void Bag_if_bidirection_key_column_should_named_as_many_to_one_side()
        {

        }
    }
}