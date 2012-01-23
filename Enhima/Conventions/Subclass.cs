using System;
using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    public class Subclass : Convention
    {
        public Subclass(Mapper mapper) : base(mapper)
        {
        }

        private static void DisciminatorValueAsClassName(IModelInspector modelInspector, Type type, ISubclassAttributesMapper subclassCustomizer)
        {
            subclassCustomizer.DiscriminatorValue(type.Name);
        }

        public override void Attach()
        {
            Mapper.BeforeMapSubclass += DisciminatorValueAsClassName;
        }

        public override void Detach()
        {
            Mapper.BeforeMapSubclass -= DisciminatorValueAsClassName;
        }
    }
}