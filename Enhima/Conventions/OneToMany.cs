using System.Linq;
using System.Reflection;
using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    public class OneToMany : Convention
    {
        public OneToMany(Mapper mapper) : base(mapper)
        {
        }

        private bool IsOneToMany(MemberInfo member, bool declared)
        {
            if (declared) return true;
            if (member.GetPropertyOrFieldType().IsGenericCollection() == false) return false;

            var collectionElementType = member.GetPropertyOrFieldType().GetGenericArguments().First();

            var isOneToMany = ManyToMany.IsManyToMany(member) == false && collectionElementType.IsEntity();
            
            return  isOneToMany;
        }

        public override void Attach()
        {
            Mapper.IsOneToMany(IsOneToMany);
        }

        public override void Detach()
        {
            
        }
    }
}