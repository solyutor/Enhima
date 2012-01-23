using System;
using Enhima.Tests.Domain;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using SharpTestsEx;

namespace Enhima.Tests
{
    [TestFixture]
    public class MappingExtensionsFixture
    {
        [TestCase(typeof (Order), true, true, true)]
        [TestCase(typeof (OrderItem), true, false, true)]
        [TestCase(typeof (Address), false, false, false)]
        [TestCase(typeof (ProductSet), true, true, false)]
        [TestCase(typeof (AggregateRoot), false, false, false)]
        public void IsEntity_returns_true_for_entites(Type entityType, bool isEntity, bool isAggregateRoot,
                                                      bool isRootEntity)
        {
            entityType.Satisfy(type =>
                               type.IsEntity() == isEntity &&
                               type.IsAggregateRoot() == isAggregateRoot &&
                               type.IsRootEntity() == isRootEntity
                );
        }

        [TestCase("Name", false)]
        [TestCase("Tags", true)]
        public static void IsCollection_retuns_valid_values(string propertyName, bool ethalon)
        {
            var propertyInfo = typeof (Product).GetProperty(propertyName);

            Assert.That(propertyInfo.PropertyType.IsGenericCollection(), Is.EqualTo(ethalon));
        }
    }
}