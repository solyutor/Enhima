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

        [Test]
        public void Bag_if_bidirection_key_column_should_named_as_many_to_one_side()
        {
            var bag = MappingOf<Customer>().Get<HbmBag>("SubCustomers");

            Assert.That(bag.key.column1, Is.EqualTo("ParentId"));
        }

        [Test]
        public void Bag_of_SubCustomers_should_be_one_to_many()
        {
            var bag = MappingOf<Customer>().Get<HbmBag>("SubCustomers");

            Assert.That(bag.Item, Is.TypeOf<HbmOneToMany>());
        }
    }
}