using System;
using System.Reflection;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Type;

namespace Enhima.Conventions
{
    public class Idbag : Convention
    {
        public Idbag(Mapper mapper) : base(mapper)
        {
        }

        public override void Attach()
        {
            Mapper.IsIdBag(IsIdbag);

            Mapper.BeforeMapIdBag += IdBagHiloGenerator;
            Mapper.BeforeMapIdBag += KeyColumnNaming;
            Mapper.BeforeMapIdBag += IdBagTableName;
        }

        public override void Detach()
        {
            Mapper.BeforeMapIdBag -= IdBagHiloGenerator;
            Mapper.BeforeMapIdBag -= KeyColumnNaming;
            Mapper.BeforeMapIdBag -= IdBagTableName;
        }

        private bool IsIdbag(MemberInfo member, bool declared)
        {
            return declared || (Mapper.ModelInspector.IsManyToMany(member)) || member.IsComponentCollection();
        }

        private void IdBagHiloGenerator(IModelInspector modelInspector, PropertyPath member, IIdBagPropertiesMapper propertyCustomizer)
        {
            var hiloRowName = member.LocalMember.IsComponentCollection()
                                  ? member.GetRootMember().DeclaringType.Name + member.ToColumnName()
                                  : BidirectionAssociation.AnalizeManyToMany(member.LocalMember).ManyToManyTablename;


            propertyCustomizer.Id(idMap =>
            {
                idMap.Generator(new EntityHighLowGeneratorDef(hiloRowName));

                idMap.Column("Id");
                idMap.Type((IIdentifierType)NHibernateUtil.Int64);
            });

            Mapper.AddHiLoScript(EntityHighLowGenerator.GetInsertFor(hiloRowName));
        }

        private void KeyColumnNaming(IModelInspector modelInspector, PropertyPath member, IIdBagPropertiesMapper propertyCustomizer)
        {
            propertyCustomizer.Key(keyMapper => keyMapper.Column(member.GetRootMember().ReflectedType.Name + "Id"));
        }

        private void IdBagTableName(IModelInspector modelInspector, PropertyPath member, IIdBagPropertiesMapper propertyCustomizer)
        {
            if (member.LocalMember.IsComponentCollection())
            {
                propertyCustomizer.Table(member.GetRootMember().DeclaringType.Name + member.ToColumnName());
            }
            else
            {
                propertyCustomizer.Table(BidirectionAssociation.AnalizeManyToMany(member.LocalMember).ManyToManyTablename);
            }
        }
    }
}