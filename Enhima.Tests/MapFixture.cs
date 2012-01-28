using Enhima.Tests.Domain;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace Enhima.Tests
{
    [TestFixture]
    public class MapFixture :MappingFixture
    {
        [Test]
        public void Map_key_column_name_should_be_named_entityId()
        {
            MappingOf<Tag>().Get<HbmMap>("Reviews")
                .Satisfy(map =>
                         map.key.column1 == "TagId" &&
                         map.table == "TagReviews"
                );


        }
    }
}