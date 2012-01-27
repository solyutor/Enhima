using Enhima.Tests.Domain;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace Enhima.Tests
{
    public class BagFixture : MappingFixture
    {
        [Test]
        public void Bag_key_column_should_be_named_as_base_class()
        {
            SubclassMappingOf<ProductSet>().Get<HbmBag>("Components")

                .Satisfy(bag =>
                         bag.Item.GetType() == typeof (HbmOneToMany) &&
                         bag.key.column1 == "ProductSetId" &&
                         bag.inverse == false
                );
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
            MappingOf<Customer>().Get<HbmBag>("SubCustomers")

                .Satisfy(bag => 
                    bag.Item.GetType() == typeof(HbmOneToMany) &&
                    bag.inverse == true
                );
        }
    }
}