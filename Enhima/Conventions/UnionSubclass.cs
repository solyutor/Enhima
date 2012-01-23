using System;
using Enhima.Inflector;
using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    public class UnionSubclass : Convention
    {
        public UnionSubclass(Mapper mapper) : base(mapper)
        {
        }

        private void PluralizeTableName(IModelInspector modelInspector, Type type, IUnionSubclassAttributesMapper unionSubclassCustomizer)
        {
            unionSubclassCustomizer.Table(type.Pluralize());
        }

        public override void Attach()
        {
            Mapper.BeforeMapUnionSubclass += PluralizeTableName;
        }

        public override void Detach()
        {
            Mapper.BeforeMapUnionSubclass -= PluralizeTableName;
        }
    }
}