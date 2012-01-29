using System;
using System.Data;
using System.Text;
using NHibernate.Id;
using NHibernate.SqlTypes;

namespace Enhima
{
    public class EntityHighLowGenerator : TableHiLoGenerator
    {
        public static string DefaultTableName = "HighLowGenerator";

        public static string DefaultEntityColumn = "Entity";

        public static string DefaultNextHighColumn = "NextHigh";

        public static long DefaultMaxLow = 49;

        public override string[] SqlCreateStrings(NHibernate.Dialect.Dialect dialect)
        {
            
            var createBuilder = new StringBuilder(200);

            createBuilder.Append(dialect.CreateTableString)
                .Append(" ")
                .Append(DefaultTableName)
                .Append(" (")
                .AppendFormat("{0} {1} not null, ", DefaultEntityColumn, dialect.GetTypeName(new StringSqlType(100)))
                .AppendFormat("{0} {1} not null ", DefaultNextHighColumn, dialect.GetTypeName(new SqlType(DbType.Int64)))
                .Append(" ) ");
            
            return new[] { createBuilder.ToString() };
        }

        public override string[] SqlDropString(NHibernate.Dialect.Dialect dialect)
        {
            return new[] {dialect.GetDropTableString(DefaultTableName)};
        }

        public static string GetInsertFor(Type type)
        {
            return GetInsertFor(type.Name);
        }

        public static string GetInsertFor(string entityName)
        {
            return String.Format("insert into {0} ({1}, {2}) values ('{3}', 1)", DefaultTableName, DefaultEntityColumn, DefaultNextHighColumn, entityName);
        }
    }
}