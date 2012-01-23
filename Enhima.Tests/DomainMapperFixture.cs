﻿using System;
using NHibernate.Cfg;
using NHibernate.Dialect;
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
            var config = new Configuration();
            config.DataBaseIntegration(db =>
                                           { 
                                               db.Dialect<SQLiteDialect>();
                                               db.LogFormattedSql = true;
                                           })
                                       ;
            
            config.AddDeserializedMapping(CompiledMappings,"domain");


            new SchemaExport(config).Create(Console.WriteLine, false);
            
        }
    }
}