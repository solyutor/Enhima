using System;
using Enhima.Tests.Domain;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Enhima.Tests
{
    [TestFixture]
    public class DomainMapperFixture : MappingFixture
    {
        [Test]
        public void Do_you_want_to_see_the_mapping()
        {
            Console.WriteLine(CompiledMappings.AsString());

            Assert.That(CompiledMappings, Is.Not.Null);
        }

        [Test]
        public void Show_the_schema()
        {
            var config = ConfigureNHibernate();

            new SchemaExport(config).Create(Console.WriteLine, false);
        }

        [Test]
        public void Try_test_shema_export()
        {
            var config = ConfigureNHibernate();
            var factory = config.BuildSessionFactory();

            using(var session = factory.OpenSession())
            using(var tx = session.BeginTransaction())
            {
                var product = new Product();
                session.Persist(product);
                tx.Commit();
            }
        }
    }
}