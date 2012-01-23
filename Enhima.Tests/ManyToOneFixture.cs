using Enhima.Tests.Domain;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace Enhima.Tests
{
    [TestFixture]
    public class ManyToOneFixture : MappingFixture
    {
        [Test]
        public void Many_to_one_column_should_be_named_properly()
        {
            var order = MappingOf<OrderItem>().Get<HbmManyToOne>("OwnedOrder");

            Assert.That(order.column, Is.EqualTo("OwnedOrderId"));
        }

        [Test]
        public void Many_to_one_columns_should_be_named_PropertyId()
        {
            var customerMapping = MappingOf<Order>().Get<HbmManyToOne>("Customer");

            customerMapping.Satisfy(map => map.column == "CustomerId");
        }
    }
}