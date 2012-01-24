using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Enhima.Conventions;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;

namespace Enhima
{
    public class Mapper : ConventionModelMapper
    {
        private readonly Assembly[] _assemblies;
        private readonly List<Convention> _conventions;
        private List<IAuxiliaryDatabaseObject> _auxiliaries;

        public Mapper(Assembly[] assemblies)
        {
            _assemblies = assemblies;
            _conventions = new List<Convention>(12);
            _auxiliaries = new List<IAuxiliaryDatabaseObject>(20);
            _auxiliaries.Add(new HighLowHelper(GetType()).CreateHighLowTable);
            AppendConventions();
        }

        private void AppendConventions()
        {
            var conventions = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsSubclassOf<Convention>());

            foreach (var instance in conventions.Select(convention => (Convention)Activator.CreateInstance(convention, this)))
            {
                AttachConvention(instance);
            }
        }

        public void AttachConvention(Convention convention)
        {
            convention.Attach();
            _conventions.Add(convention);
        }

        public void DetachConvention(Convention convention)
        {
            convention.Detach();
            _conventions.Remove(convention);
        }

        public IEnumerable<Type> TypesFromAddedAssemblies
        {
            get 
            {
                return _assemblies.SelectMany(assembly => assembly.GetExportedTypes());
            }
        }

        public IEnumerable<Convention> Conventions
        {
            get { return _conventions.AsReadOnly(); }
        }

        public IEnumerable<IAuxiliaryDatabaseObject> Auxiliaries
        {
            get { return _auxiliaries; }
        }

        public HbmMapping CompileMappings()
        {
            AddMappings(TypesFromAddedAssemblies);
            return CompileMappingFor(TypesFromAddedAssemblies);
        }

        public void AddAuxiliaryObject(IAuxiliaryDatabaseObject dbObject)
        {
            _auxiliaries.Add(dbObject);
        }
    }
}