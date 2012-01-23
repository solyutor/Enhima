using System;
using Enhima.Inflector;
using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    public class Class : Convention
    {
        public Class(Mapper mapper) : base(mapper)
        {
            Mapper.IsEntity(IsEntity);
            Mapper.IsRootEntity(IsRootEntity);
        }

        public bool IsEntity(Type type, bool declared)
        {
            return declared || type.IsEntity();
        }

        public bool IsRootEntity(Type type, bool declared)
        {
            return declared || type.IsRootEntity();
        }

        public override void Attach()
        {
            Mapper.BeforeMapClass += DicriminatorColumnNamedClass;
            Mapper.BeforeMapClass += DisciminatorValueAsClassName;
            Mapper.BeforeMapClass += PluralizeTableName;
            Mapper.BeforeMapClass += SetHiloId;
        }

        public override void Detach()
        {
            Mapper.BeforeMapClass -= DicriminatorColumnNamedClass;
            Mapper.BeforeMapClass -= DisciminatorValueAsClassName;
            Mapper.BeforeMapClass -= PluralizeTableName;
            Mapper.BeforeMapClass -= SetHiloId;
        }

        private void PluralizeTableName(IModelInspector modelInspector, Type type, IClassAttributesMapper classCustomizer)
        {
            classCustomizer.Table(type.Pluralize());
        }

        private void DicriminatorColumnNamedClass(IModelInspector modelInspector, Type type, IClassAttributesMapper classCustomizer)
        {
            if(DiscriminatorIsNotRequired(type)) return;

            classCustomizer.Discriminator(
                dm =>
                    {
                        dm.Column("Class");
                        dm.NotNullable(true);
                    });
        }

        private void DisciminatorValueAsClassName(IModelInspector modelInspector, Type type, IClassAttributesMapper classCustomizer)
        {
            if (DiscriminatorIsNotRequired(type)) return;

            classCustomizer.DiscriminatorValue(type.Name);
        }

        private void SetHiloId(IModelInspector modelInspector, Type type, IClassAttributesMapper classCustomizer)
        {
            classCustomizer.Id(x => x.Generator(Generators.HighLow, gm =>
                                                                    gm.Params(new
                                                                                  {
                                                                                      table = "HighLowGenerator",
                                                                                      column = "NextHigh",
                                                                                      max_lo = 49,
                                                                                      where = String.Format("EntityName = '{0}'", type.Name)
                                                                                  }
                                                                        )));
        }

        private bool DiscriminatorIsNotRequired(Type type)
        {
            return Mapper.ModelInspector.IsTablePerClassHierarchy(type) == false;
        }
    }
}