using System;
using NHibernate.Mapping.ByCode;

namespace Enhima
{
    public class EntityHighLowGeneratorDef : IGeneratorDef
    {
        private readonly string _entityName;

        public EntityHighLowGeneratorDef(Type entityType) :this(entityType.Name)
        {
            
        }

        public EntityHighLowGeneratorDef(string entityName)
        {
            _entityName = entityName;
        }

        public string Class
        {
            get { return typeof (EntityHighLowGenerator).AssemblyQualifiedName; }
        }

        public object Params
        {
            get
            {
                return new
                           {
                               table = EntityHighLowGenerator.DefaultTableName,
                               column = EntityHighLowGenerator.DefaultNextHighColumn,
                               max_lo = EntityHighLowGenerator.DefaultMaxLow,
                               where = String.Format("{0} = '{1}'", EntityHighLowGenerator.DefaultEntityColumn, _entityName)
                           };
            }
        }

        public Type DefaultReturnType
        {
            get { return typeof (long); }
        }

        public bool SupportedAsCollectionElementId
        {
            get { return true; }
        }
    }
}