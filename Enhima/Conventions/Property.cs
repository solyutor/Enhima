using System.Linq;
using NHibernate.Mapping.ByCode;
using NHibernate.Type;

namespace Enhima.Conventions
{
    public class Property : Convention
    {
        public Property(Mapper mapper) : base(mapper)
        {

        }

        public override void Attach()
        {
            Mapper.BeforeMapProperty += SetNotNullable;
            Mapper.BeforeMapProperty += MapEnumAsString;
            Mapper.BeforeMapProperty += PrefixColumnWithComponentPropertyName;
        }

        public override void Detach()
        {
            Mapper.BeforeMapProperty -= SetNotNullable;
            Mapper.BeforeMapProperty -= MapEnumAsString;
            Mapper.BeforeMapProperty -= PrefixColumnWithComponentPropertyName;            
        }

        private static void SetNotNullable(IModelInspector modelInspector, PropertyPath member, IPropertyMapper propertyCustomizer)
        {
            propertyCustomizer.NotNullable(member.IsNotNullable());
        }

        private static void MapEnumAsString(IModelInspector modelInspector, PropertyPath member, IPropertyMapper propertyCustomizer)
        {
            var propertyType = member.LocalMember.GetPropertyOrFieldType();

            if (propertyType.IsEnumOrNullableEnum() == false) return;

            var enumType = propertyType.IsEnum ? propertyType : propertyType.GetGenericArguments().Single();

            var type = typeof(EnumStringType<>).MakeGenericType(enumType);

            propertyCustomizer.Type(type, null);
        }

        private static void PrefixColumnWithComponentPropertyName(IModelInspector modelInspector, PropertyPath member, IPropertyMapper propertyCustomizer)
        {
            if (member.PreviousPath == null) return;

            propertyCustomizer.Column(member.ToColumnName());
        }
    }
}