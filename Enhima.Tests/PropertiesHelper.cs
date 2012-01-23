using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Cfg.MappingSchema;

namespace Enhima.Tests
{
    public static class PropertiesHelper
    {
        public static THbmProperty Get<THbmProperty>(this IPropertiesContainerMapping self, string propertyName)
            where THbmProperty : IEntityPropertyMapping
        {
            return (THbmProperty) self.Properties.Single(x => x.Name == propertyName);
        }

        public static THbmMapping As<THbmMapping>(this object self)
        {
            return (THbmMapping) self;
        }

        public static HbmClass MappingOf<TEntity>(this IEnumerable<HbmClass> self)
        {
            return MappingOf(self, typeof(TEntity));
        }

        public static HbmClass MappingOf(this IEnumerable<HbmClass> self, Type type)
        {
            return self.Single(map => map.Name == type.AssemblyQualifiedName);
        }

        public static HbmJoinedSubclass JoinedSubclass<TEntity>(this IEnumerable<HbmJoinedSubclass> self)
        {
            return self.Single(map => map.Name == typeof(TEntity).AssemblyQualifiedName);
        }
    }
}