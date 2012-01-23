using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Mapping.ByCode;

namespace Enhima
{
    public static class MappingExtensions
    {
        public static bool IsEntity(this Type self)
        {
            return self.IsSubclassOf<Entity>() && self != typeof(AggregateRoot);
        }

        public static bool IsRootEntity(this Type self)
        {
            return (self.IsFirstDescendantOf<AggregateRoot>() || self.IsFirstDescendantOf<Entity>()) && self != typeof(AggregateRoot);
        }

        public static bool IsAggregateRoot(this Type self)
        {
            return self.IsSubclassOf<AggregateRoot>();
        }

        public static bool IsSubclassOf<TType>(this Type self)
        {
            return typeof(TType).IsAssignableFrom(self) && self != typeof(TType);
        }

        public static bool IsFirstDescendantOf<TType>(this Type self)
        {
            return  typeof(TType) == self.BaseType;
        }

        public static bool HasAncestorsAmong(this Type self, IEnumerable<Type> others)
        {
            return others.Any(other => other.IsSubclassOf(self));
        }

        public static bool IsComponentCollection(this MemberInfo self)
        {
            var propertyType = self.GetPropertyOrFieldType();
            return propertyType.IsGenericCollection() && propertyType.CollectionElementType().IsEntity() == false;
        }

        public static Type CollectionElementType(this Type itemType)
        {
            return itemType.GetGenericArguments().First();
        }

        public static bool IsNotNullable(this PropertyPath self)
        {
            return self.IsNullable() == false;
        }
        public static bool IsNullable(this PropertyPath self)
        {
            return self.IsNullableValueTypeProperty() || self.IsNullableReferenceType();
        }

        public static bool IsNullableValueTypeProperty(this PropertyPath self)
        {
            var type = self.LocalMember.ReflectedType;
            return type.IsValueType && (type.IsGenericType && type.GetGenericTypeDefinition() == (typeof(Nullable<>)));
        }

        public static bool IsNullableReferenceType(this PropertyPath self)
        {
            var propertyInfo = (PropertyInfo)self.LocalMember;
            var instance = Activator.CreateInstance(propertyInfo.DeclaringType, true);

            if (propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(instance, null, null);
            }

            var readedValue = propertyInfo.GetValue(instance, null);

            return readedValue == null;
        }
    }
}