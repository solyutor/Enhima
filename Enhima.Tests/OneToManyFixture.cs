using Enhima.Tests.Domain;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;

namespace Enhima.Tests
{
    [TestFixture]
    public class OneToManyFixture : MappingFixture
    {
        [Test]
        public void Collection_of_self_type_should_be_one_to_many()
        {
            var itemsMapping = MappingOf<Tag>().Get<HbmSet>("Children");
            
            Assert.That(itemsMapping.ElementRelationship, Is.InstanceOf<HbmOneToMany>());
        }
    }
}