using System;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace Enhima.Tests
{
    public class MappingFixture
    {
        private static readonly HbmMapping _mappings;

        static MappingFixture()
        {
            _mappings = new Mapper(From.ThisApplication()).CompileMappings();
        }

        protected HbmMapping CompiledMappings
        {
            get { return _mappings; }
        }

        protected HbmClass MappingOf(Type type)
        {
            return CompiledMappings.RootClasses.MappingOf(type);
        }

        protected HbmClass MappingOf<T>()
        {
            return MappingOf(typeof (T));
        }

        protected HbmSubclass SubclassMappingOf<T>()
        {
            return CompiledMappings.SubClasses.Single(map => map.Name == typeof(T).AssemblyQualifiedName);
        }

        protected HbmUnionSubclass UnionMappingOf<T>()
        {
            return CompiledMappings.UnionSubclasses.Single(map => map.Name == typeof (T).AssemblyQualifiedName);
        }

        protected HbmJoinedSubclass JoinedMappingOf<T>()
        {
            return CompiledMappings.JoinedSubclasses.JoinedSubclass<T>();
        }

        protected Configuration ConfigureNHibernate()
        {
            return new Configuration()
                .ConfigureSqlite()
                .MapEntities(From.ThisApplication());
        }
    }
}