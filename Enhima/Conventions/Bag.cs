using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    public class Bag : Convention
    {
        public Bag(Mapper mapper) : base(mapper)
        {
        }

        private void NameKeyColumn(IModelInspector modelinspector, PropertyPath member, IBagPropertiesMapper propertycustomizer)
        {
            propertycustomizer.Key(keyMapper => keyMapper.Column(member.GetRootMember().ReflectedType.Name + "Id"));
        }

        public override void Attach()
        {
            Mapper.BeforeMapBag += NameKeyColumn;
        }

        public override void Detach()
        {
            Mapper.BeforeMapBag -= NameKeyColumn;
        }
    }
}