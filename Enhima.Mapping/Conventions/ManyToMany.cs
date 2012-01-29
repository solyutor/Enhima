using System.Reflection;
using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    public class ManyToMany : Convention
    {
        public ManyToMany(Mapper mapper) : base(mapper)
        {
        }

        public override void Attach()
        {
            Mapper.IsManyToMany(IsManyToMany);
            Mapper.BeforeMapManyToMany += ApplyConventions;
        }

        public override void Detach()
        {
            Mapper.BeforeMapManyToMany -= ApplyConventions;
        }

        private void ApplyConventions(IModelInspector modelinspector, PropertyPath member, IManyToManyMapper collectionrelationmanytomanycustomizer)
        {
            var itemType = member.LocalMember.GetPropertyOrFieldType().DetermineCollectionElementType();
            collectionrelationmanytomanycustomizer.Column(itemType.Name + "Id");
        }

        public bool IsManyToMany(MemberInfo memberInfo, bool declared)
        {
            return declared || (Mapper.ModelInspector.IsEntity(memberInfo.ReflectedType) && IsManyToMany(memberInfo));
        }

        public static bool IsManyToMany(MemberInfo memberInfo)
        {
            return
                memberInfo.GetPropertyOrFieldType().IsGenericCollection() &&
                BidirectionAssociation.AnalizeManyToMany(memberInfo).IsManyToMany;
        }
    }
}