using Enhima.Tests.Domain;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace Enhima.Tests
{
    [TestFixture]
    public class SetFixture : MappingFixture
    {
        [Test]
        public void Set_key_column_should_be_named_()
        {
            var itemsMapping = MappingOf<Order>().Get<HbmSet>("Items");

            itemsMapping.Satisfy(map =>
                                 map.key.column1 == "OwnedOrderId" &&
                                 map.inverse == true &&
                                 map.Cascade == "all,delete-orphan");
        }
    }
}