using System.Linq;
using Enhima.Tests.Domain;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace Enhima.Tests
{
    [TestFixture]
    public class IdBagFixture : MappingFixture
    {
        private HbmIdbag IdBagTagSide
        {
            get { return MappingOf<Tag>().Get<HbmIdbag>("Products"); }
        }

        private HbmIdbag IdBagProductSide
        {
            get { return MappingOf<Product>().Get<HbmIdbag>("Tags"); }
        }

        [Test]
        public void IdBag_should_have_hilo_generator()
        {
            IdBagTagSide.collectionid.Satisfy(id =>
                                              id.generator.@class == "hilo" && //use hilo
                                              id.generator.param.Any(
                                                  x =>
                                                  x.name == "where" && x.Text.First() == "EntityName = 'ProductsTags'") &&
                                              //where set to table name
                                              id.type == "Int64"); //using long as others class identifier
        }

        [Test]
        public void IdBag_id_column_should_be_named_id()
        {
            Assert.That(IdBagTagSide.collectionid.column1, Is.EqualTo("Id"));
        }

        [Test]
        public void IdBag_table_should_be_same()
        {
            IdBagTagSide.Satisfy(ib =>
                                 ib.table == "ProductsTags" &&
                                 ib.table == IdBagProductSide.table);
        }

        [Test]
        public void IdBag_key_columns_should_be_named_properly()
        {
            IdBagTagSide.Satisfy(ib =>
                                 ib.key.column1 == "TagId" &&
                                 ib.Item.As<HbmManyToMany>().column == "ProductId" &&
                                 IdBagProductSide.key.column1 == "ProductId" &&
                                 IdBagProductSide.Item.As<HbmManyToMany>().column == "TagId");
        }

        [Test]
        public void Collection_of_elements_mapped_as_elements()
        {
            var bag = MappingOf<Product>().Get<HbmIdbag>("Opinions");

            bag.Satisfy(b =>
                        b.table == "ProductOpinions" &&
                        b.key.column1 == "ProductId" &&
                        b.Item.As<HbmElement>() != null);
        }

        [Test]
        public void Collection_id_of_element_collection_should_have_where_with_table_name()
        {
            var bag = MappingOf<Product>().Get<HbmIdbag>("Opinions");

            var param = bag.collectionid.generator.param.Single(x => x.name == "where");

            Assert.That(param.Text[0], Is.EqualTo("EntityName = 'ProductOpinions'"));
                                               
        }

    }
}