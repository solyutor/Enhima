using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    public class Set : Convention
    {
        public Set(Mapper mapper) : base(mapper)
        {
        }

        public override void Attach()
        {
            Mapper.BeforeMapSet += KeyColumnNaming;
            Mapper.BeforeMapSet += InverseIfBidirectional;
            Mapper.BeforeMapSet += CascadeIfSetOfNonAggregateRoots;
        }

        public override void Detach()
        {
            Mapper.BeforeMapSet -= KeyColumnNaming;
            Mapper.BeforeMapSet -= InverseIfBidirectional;
            Mapper.BeforeMapSet -= CascadeIfSetOfNonAggregateRoots;
        }

        private static void KeyColumnNaming(IModelInspector modelinspector, PropertyPath member, ISetPropertiesMapper propertycustomizer)
        {
            var columnName = BidirectionAssociation.AnalyzeManyToOne(member).ColumnNameOnCollectionSide;
            propertycustomizer.Key(km => km.Column(columnName));
        }

        private static void InverseIfBidirectional(IModelInspector modelinspector, PropertyPath member, ICollectionPropertiesMapper propertycustomizer)
        {
            propertycustomizer.Inverse(BidirectionAssociation.AnalyzeManyToOne(member).Inverse);
        }

        private static void CascadeIfSetOfNonAggregateRoots(IModelInspector modelinspector, PropertyPath member, ICollectionPropertiesMapper propertycustomizer)
        {
            propertycustomizer.Cascade(BidirectionAssociation.AnalyzeManyToOne(member).Cascade);
        }
    }
}