using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    public class Map : Convention
    {
        public Map(Mapper mapper) : base(mapper)
        {
        }

        public override void Attach()
        {
            Mapper.BeforeMapMap += MapKeyColumn;
            Mapper.BeforeMapMap += MapTableName;
        }

        public override void Detach()
        {
            Mapper.BeforeMapMap -= MapKeyColumn;
            Mapper.BeforeMapMap += MapTableName;
        }

        private void MapKeyColumn(IModelInspector modelInspector, PropertyPath member, IMapPropertiesMapper propertyCustomizer)
        {
            propertyCustomizer.Key(km => km.Column(member.GetRootMember().ReflectedType.Name + "Id"));
        }

        private void MapTableName(IModelInspector modelInspector, PropertyPath member, IMapPropertiesMapper propertyCustomizer)
        {
            var ownerName = member.GetRootMember().ReflectedType.Name;
            var propertyName = member.ToColumnName();
            propertyCustomizer.Table(ownerName + propertyName);
        }
    }
}