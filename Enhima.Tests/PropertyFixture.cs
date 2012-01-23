using Enhima.Tests.Domain;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Type;
using NUnit.Framework;
using SharpTestsEx;

namespace Enhima.Tests
{
    [TestFixture]
    public class PropertyFixture : MappingFixture
    {
        [Test]
        public void Nullable_enum_is_mapped_as_string()
        {
            var approvedMapping = MappingOf<Order>().Get<HbmProperty>("Approved");

            approvedMapping.Satisfy(map =>
                                    map.type1 == typeof (EnumStringType<YesNo>).AssemblyQualifiedName &&
                                    map.notnull == false
                );
        }

        [Test]
        public void Name_property_mapped_as_not_nullable()
        {
            var propertyMap = MappingOf<Product>().Get<HbmProperty>("Name");

            Assert.That(propertyMap.notnull, Is.True);
        }

        [Test]
        public void Active_property_mapped_as_string_enum()
        {
            var property = MappingOf<Product>().Get<HbmProperty>("Active");

            Assert.That(property.type1, Is.EqualTo(typeof (EnumStringType<YesNo>).AssemblyQualifiedName));
        }
    }
}