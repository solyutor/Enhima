using System.Linq;
using Enhima.Tests.Domain;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace Enhima.Tests
{
    [TestFixture]
    public class ComponentFixture : MappingFixture
    {
        [Test]
        public void Component_columns_should_have_property_name_prefix()
        {
            var component = MappingOf<Customer>().Get<HbmComponent>("DeliveryAddress");

            component.Properties.OfType<HbmProperty>().ToList().Satisfy(properties =>
                                                                        properties[0].column == "DeliveryAddressCity" &&
                                                                        properties[1].column == "DeliveryAddressStreet"
                );
        }
    }
}