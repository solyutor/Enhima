using System;
using Enhima.Tests.Domain;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Enhima.Tests
{
    [TestFixture]
    public class DomainMapperFixture 
    {
        private Configuration _config;
        private SQLiteInMemoryTestHelper _testHelper;

        [TestFixtureSetUp]
        public void TestFxtureSetup()
        {
            _config =  new Configuration();
            _config
                .ConfigureSQLiteInMemory()
                .MapEntities(From.ThisApplication());

            _testHelper = new SQLiteInMemoryTestHelper(_config);
        }

        [SetUp]
        public void Setup()
        {
            _testHelper.CreateSchema();
        }

        [TearDown]
        public void TearDown()
        {
            _testHelper.DropSchema();
        }

        [Test]
        public void Show_the_mapping()
        {
            Console.WriteLine(new Mapper(From.ThisApplication()).CompileMappings().AsString());
        }

        [Test]
        public void Show_the_schema()
        {
            new SchemaExport(_config).Create(Console.WriteLine, false);
        }

        [Test]
        public void Try_test_shema_export()
        {
            using(var tx = _testHelper.CurrentSession.BeginTransaction())
            {
                var product = new Product();
                _testHelper.CurrentSession.Persist(product);
                tx.Commit();
            }
        }

        [Test]
        public void Entity_exists_for_another_session()
        {
            using (var  session = _testHelper.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var product = new Product {Name = "Enhima"};
                session.Persist(product);
                tx.Commit();
            }

            using (var session = _testHelper.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var product = session.CreateQuery("from Product p where p.Name = 'Enhima'").UniqueResult<Product>();
                Assert.That(product, Is.Not.Null);
                tx.Commit();
            }
        }
    }
}