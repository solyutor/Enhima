using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Enhima.Inflector;
using NHibernate;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Type;

namespace Enhima.Conventions
{
    public class Class : Convention
    {
        private readonly List<Type> _versionTypes;

        public Class(Mapper mapper) : base(mapper)
        {
            _versionTypes = new List<Type>(3)
                                {
                                    typeof (short), 
                                    typeof (int), 
                                    typeof (long)
                                };

            Mapper.IsEntity(IsEntity);
            Mapper.IsRootEntity(IsRootEntity);
            Mapper.IsVersion(IsVersion);
        }

        public bool IsEntity(Type type, bool declared)
        {
            return declared || type.IsEntity();
        }

        public bool IsRootEntity(Type type, bool declared)
        {
            return declared || type.IsRootEntity();
        }

        private bool IsVersion(MemberInfo member, bool declared)
        {
            return declared || member.Name.Equals("Version", StringComparison.OrdinalIgnoreCase) && _versionTypes.Contains(member.GetPropertyOrFieldType());
        }

        public override void Attach()
        {
            Mapper.BeforeMapClass += DicriminatorColumnNamedClass;
            Mapper.BeforeMapClass += DisciminatorValueAsClassName;
            Mapper.BeforeMapClass += PluralizeTableName;
            Mapper.BeforeMapClass += MapHiloId;
            Mapper.BeforeMapClass += MapVersion;
        }

        public override void Detach()
        {
            Mapper.BeforeMapClass -= DicriminatorColumnNamedClass;
            Mapper.BeforeMapClass -= DisciminatorValueAsClassName;
            Mapper.BeforeMapClass -= PluralizeTableName;
            Mapper.BeforeMapClass -= MapHiloId;
            Mapper.BeforeMapClass -= MapVersion;
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

        private bool DiscriminatorIsNotRequired(Type type)
        {
            return Mapper.ModelInspector.IsTablePerClassHierarchy(type) == false;
        }

        private void MapHiloId(IModelInspector modelInspector, Type type, IClassAttributesMapper classCustomizer)
        {
            Mapper.AddHiLoScript(EntityHighLowGenerator.GetInsertFor(type));

            classCustomizer.Id(x => x.Generator(new EntityHighLowGeneratorDef(type)));
        }

        private void MapVersion(IModelInspector modelinspector, Type type, IClassAttributesMapper classCustomizer)
        {
            foreach (var member in type.GetMembers().Where(member => Mapper.ModelInspector.IsVersion(member)))
            {
                classCustomizer.Version(member, vm => vm.Type((IVersionType) NHibernateUtil.GuessType(member.GetPropertyOrFieldType())));
            }
        }
    }
}