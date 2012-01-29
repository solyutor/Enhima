using System;

namespace Enhima.Inflector
{
    public static class InflectorExtensions
    {
        public static string Pluralize(this Type type)
        {
            return type.Name.Pluralize();
        }
    }
}