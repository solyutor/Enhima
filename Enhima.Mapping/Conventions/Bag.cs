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
            var association = BidirectionAssociation.AnalyzeManyToOne(member);
            
            var columnName = association.IsBidirectional
                           ? association.ColumnNameOnCollectionSide
                           : member.GetRootMember().ReflectedType.Name + "Id";

            propertycustomizer.Key(keyMapper => keyMapper.Column(columnName));

            propertycustomizer.Inverse(association.IsBidirectional);
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