using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    public class ManyToOne : Convention
    {
        public ManyToOne(Mapper mapper) : base(mapper)
        {
        }

        public override void Attach()
        {
            Mapper.BeforeMapManyToOne += NameForeignKeyColumn;
        }

        public override void Detach()
        {
            Mapper.BeforeMapManyToOne -= NameForeignKeyColumn;
        }

        private void NameForeignKeyColumn(IModelInspector modelinspector, PropertyPath member, IManyToOneMapper propertycustomizer)
        {
            propertycustomizer.Column(member.ToColumnName() + "Id");
        }
    }
}