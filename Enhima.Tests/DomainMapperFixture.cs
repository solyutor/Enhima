using System;
using Enhima.Tests.Domain;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Enhima.Tests
{
    [TestFixture]
    public class DomainMapperFixture 
    {
        private Configuration _config;
        private SQLiteInMemorySchemaMaker _schemaMaker;

        [TestFixtureSetUp]
        public void TestFxtureSetup()
        {
            _config =  new Configuration();
            _config
                .ConfigureSQLiteInMemory()
                .MapEntities(From.ThisApplication());

            _schemaMaker = new SQLiteInMemorySchemaMaker(_config);
        }

        [SetUp]
        public void Setup()
        {
            _schemaMaker.CreateSchema();
        }

        [TearDown]
        public void TearDown()
        {
            _schemaMaker.DropSchema();
        }

        [Test]
        public void Show_the_schema()
        {
            new SchemaExport(_config).Create(Console.WriteLine, false);
        }

        [Test]
        public void Try_test_shema_export()
        {
            using(var tx = _schemaMaker.CurrentSession.BeginTransaction())
            {
                var product = new Product();
                _schemaMaker.CurrentSession.Persist(product);
                tx.Commit();
            }
        }

        [Test]
        public void Entity_exists_for_another_session()
        {
            using (var  session = _schemaMaker.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var product = new Product {Name = "Enhima"};
                session.Persist(product);
                tx.Commit();
            }

            using (var session = _schemaMaker.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var product = session.CreateQuery("from Product p where p.Name = 'Enhima'").UniqueResult<Product>();
                Assert.That(product, Is.Not.Null);
                tx.Commit();
            }
        }
    }
}