using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Enhima.Inflector;
using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    internal class BidirectionAssociation
    {
        public readonly PropertyInfo CollectionSide;
        public readonly PropertyInfo OtherSide;

        private BidirectionAssociation(PropertyInfo collectionSide, PropertyInfo otherSide)
        {
            CollectionSide = collectionSide;
            OtherSide = otherSide;
        }

        public bool IsBidirectional
        {
            get { return CollectionSide != null && OtherSide != null; }
        }

        public string ColumnNameOnCollectionSide
        {
            get
            {
                var basename = (OtherSide ?? CollectionSide).Name;
                string singular = basename.Singularize();
                var result = String.IsNullOrEmpty(singular) ? basename : singular;
                return result + "Id";
            }
        }

        public bool Inverse
        {
            get { return OtherSide != null; }
        }

        public Cascade Cascade
        {
            get
            {
                if (OtherSide == null)
                {
                    return Cascade.All.Include(Cascade.DeleteOrphans);
                }

                return OtherSide.DeclaringType.IsAggregateRoot()
                           ? Cascade.None
                           : Cascade.All.Include(Cascade.DeleteOrphans);
            }
        }

        public bool IsManyToMany
        {
            get { return OtherSide != null; }
        }

        public string ManyToManyTablename
        {
            get
            {
                string otherSide = string.Empty;
                
                if(OtherSide != null)
                {
                    otherSide = OtherSide.PropertyType.GetGenericArguments().Single().Name.Pluralize();
                }
                
                var classes = new List<string>
                                  {
                                      otherSide,
                                      CollectionSide.PropertyType.GetGenericArguments().Single().Name.Pluralize()
                                  };
                classes.Sort();
                return classes[0] + classes[1];
            }
        }

        public static implicit operator bool(BidirectionAssociation result)
        {
            return result.IsBidirectional;
        }

        public static BidirectionAssociation AnalizeManyToMany(MemberInfo memberInfo)
        {
            if (memberInfo.IsMap())
            {
                return new BidirectionAssociation((PropertyInfo)memberInfo, null);
            }

            var itemType = memberInfo.GetPropertyOrFieldType().GetGenericArguments().Single();

                var otherSideInterface = typeof (IEnumerable<>).MakeGenericType(memberInfo.ReflectedType);

                //has collection on the other end of association
                var otherSide =
                    itemType.GetProperties()
                        //Is a collection on the other side of association
                        .Where(property => property.PropertyType.GetInterfaces().Any(otherSideInterface.IsAssignableFrom)) 
                        //But no the same collection!
                        .Where(property => property.Equals(memberInfo) == false)
                        .SingleOrDefault();

                
                return new BidirectionAssociation((PropertyInfo)memberInfo, otherSide);
        }

        public static BidirectionAssociation AnalyzeManyToOne(PropertyPath member)
        {
            var ownerType = member.LocalMember.DeclaringType;

            var itemType = member.LocalMember.GetPropertyOrFieldType().GetGenericArguments().Single();

            var oppositeAssociation = itemType.GetProperties().FirstOrDefault(x => x.PropertyType == ownerType);

            return new BidirectionAssociation((PropertyInfo) member.LocalMember, oppositeAssociation);
        }
    }
}