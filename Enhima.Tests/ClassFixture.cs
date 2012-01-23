using System;
using Enhima.Tests.Domain;
using NUnit.Framework;
using SharpTestsEx;

namespace Enhima.Tests
{
    [TestFixture]
    public class ClassFixture : MappingFixture
    {
        [Test]
        public void Discriminator_value_should_be_class_name()
        {
            Assert.That(MappingOf<Product>().DiscriminatorValue, Is.EqualTo("Product"));
        }

        [Test]
        public void Discriminator_column_should_be_named_class()
        {
            Assert.That(MappingOf<Product>().discriminator.column, Is.EqualTo("Class"));
        }

        [Test]
        public void Table_names_should_be_pluralized()
        {
            CompiledMappings.RootClasses.Satisfy(
                roots =>
                    roots.Length == 5 &&
                    roots.MappingOf<Customer>().table == "Customers" &&
                    roots.MappingOf<Order>().table == "Orders" &&
                    roots.MappingOf<Tag>().table == "Tags" &&
                    roots.MappingOf<Product>().table == "Products" &&
                    roots.MappingOf<OrderItem>().table == "OrderItems");
        }

        [Test]
        public void Should_not_map_disctiminator_if_class_has_not_ancestors()
        {
            var tag = MappingOf<Tag>();

            tag.Satisfy(t =>
                        t.discriminator == null &&
                        t.discriminatorvalue == null);
        }

        [TestCase(typeof(Order))]
        [TestCase(typeof(Customer))]
        public void Should_not_have_discriminator_if_ancestors_are_joined_or_union(Type type)
        {
            Assert.That(MappingOf(type).discriminator, Is.Null);
        }
    }
}