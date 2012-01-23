using System;
using System.Linq;
using System.Reflection;
using NHibernate.Cfg;

namespace Enhima
{
    /// <summary>
    /// Added mappings to NHibernate configuration from specified assemlies 
    /// </summary>
    public static class From
    {
        /// <summary>
        /// Maps all entities from speciefied assemlies.
        /// </summary>
        /// <param name="self">NHibernate configuration to add mappings.</param>
        /// <param name="assemblies">Asseblies with mappings and/or entities.</param>
        public static void MapEntities(this Configuration self, Assembly[] assemblies)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if(assemblies == null || assemblies.Length == 0)
                throw new ArgumentException("Assemblies should be not null not empty array");
            
            var mapper = new Mapper(assemblies);

            self.AddDeserializedMapping(mapper.CompileMappings(), null);
        }

        /// <summary>
        /// Returns all supplied assemblies. It exists only to keep API fluent.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static Assembly[] Assemblies(Assembly[] assemblies)
        {
            return assemblies;
        }

        /// <summary>
        /// Returns an assembly the type belongs to. 
        /// </summary>
        /// <typeparam name="T">Type to lookup assembly.</typeparam>
        /// <returns></returns>
        public static Assembly[] AssemblyOf<T>()
        {
            return AssemblyOf(typeof(T));
        }
        
        /// <summary>
        /// Returns an assembly the type belongs to. 
        /// </summary>
        /// <param name="type">Type to lookup assembly.</param>
        /// <returns></returns>
        public static Assembly[] AssemblyOf(Type type)
        {
            return ToArray(type.Assembly);
        }
        
        /// <summary>
        /// Return all assemblies from current AppDomain whose names stars with first part of namespase of calling assembly.
        /// </summary>
        /// <returns></returns>
        public static Assembly[] ThisApplication()
        {
            var applicationPrefix = Assembly.GetCallingAssembly().GetName().Name.Split('.').First();

            return
                AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.GetName().Name.StartsWith(applicationPrefix))
                .ToArray();
        }

        private static Assembly[] ToArray(Assembly assembly)
        {
            return new[] {assembly};
        }
    }
}