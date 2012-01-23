using Enhima.Tests.Domain;
using NUnit.Framework;

namespace Enhima.Tests
{
    [TestFixture]
    public class UnionSubclassFixture : MappingFixture
    {
        [Test]
        public void Key_column_should_be_named_id()
        {
            Assert.That(UnionMappingOf<PreOrder>().table, Is.EqualTo("PreOrders"));
        }
    }
}