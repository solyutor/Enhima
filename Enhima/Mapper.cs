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
        private readonly List<string> _hiloInserts;

        public Mapper(Assembly[] assemblies)
        {
            _assemblies = assemblies;
            _conventions = new List<Convention>(12);
            _hiloInserts = new List<string>(20);
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

        public IEnumerable<string> HiloInserts
        {
            get { return _hiloInserts; }
        }

        public HbmMapping CompileMappings()
        {
            AddMappings(TypesFromAddedAssemblies);
            return CompileMappingFor(TypesFromAddedAssemblies);
        }

        public void AddHiLoScript(string script)
        {
            if (_hiloInserts.Any(x => x.Equals(script))) return;

            _hiloInserts.Add(script);
        }

        public IEnumerable<IAuxiliaryDatabaseObject> HiloInsertObjects
        {
            get
            {
                foreach (var hiloInsert in HiloInserts)
                {
                    yield return new SimpleAuxiliaryDatabaseObject(hiloInsert, null);
                }
            }
        }
    }
}