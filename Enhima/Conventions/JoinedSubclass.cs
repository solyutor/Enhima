using System;
using Enhima.Inflector;
using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    public class JoinedSubclass : Convention
    {
        public JoinedSubclass(Mapper mapper) : base(mapper)
        {
        }


        private static void NameKeyColumnId(IModelInspector modelInspector, Type type, IJoinedSubclassAttributesMapper joinedSubclassCustomizer)
        {
            joinedSubclassCustomizer.Key(key => key.Column("Id"));
        }

        private static void PluralizeTableName(IModelInspector modelInspector, Type type, IJoinedSubclassAttributesMapper joinedSubclassCustomizer)
        {
            joinedSubclassCustomizer.Table(type.Pluralize());
        }

        public override void Attach()
        {
            Mapper.BeforeMapJoinedSubclass += NameKeyColumnId;
            Mapper.BeforeMapJoinedSubclass += PluralizeTableName;
        }

        public override void Detach()
        {
            Mapper.BeforeMapJoinedSubclass -= NameKeyColumnId;
            Mapper.BeforeMapJoinedSubclass -= PluralizeTableName;
        }
    }
}