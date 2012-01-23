using NUnit.Framework;
using SharpTestsEx;
using Solyutor.Conformapper.Tests.Domain;

namespace Solyutor.Conformapper.Tests
{
    [TestFixture]
    public class ClassConventions : MappingFixture
    {
        [Test]
        public void Discriminator_value_should_be_class_name()
        {
            Assert.That(MappingOf<Order>().DiscriminatorValue, Is.EqualTo("Order"));
        }

        [Test]
        public void Discriminator_column_should_be_named_class()
        {
            Assert.That(MappingOf<Order>().discriminator.column, Is.EqualTo("Class"));
        }

        [Test]
        public void Table_names_should_be_pluralized()
        {
            CompiledMappings.RootClasses.Satisfy(
                roots =>
                roots[0].table == "Customers" &&
                roots[1].table == "Orders" &&
                roots[2].table == "Tags" &&
                roots[3].table == "Products" &&
                roots[4].table == "OrderItems");
        }
    }
}