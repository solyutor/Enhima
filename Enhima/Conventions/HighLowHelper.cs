using System;
using System.Text;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    public class HighLowHelper
    {
        private readonly string _entityName;

        private readonly string _insertToHighLowTable;
        private readonly string _whereTemplate;

        public HighLowHelper(Type entityType) :this(entityType.Name)
        {
            

        }

        public HighLowHelper(string entityName)
        {
            if(entityName == null)
                throw new ArgumentNullException("entityName");

            _entityName = entityName;
            _whereTemplate = EntityColumn + " = '{0}'";

            _insertToHighLowTable = String.Format("insert into {0} ({1}, {2}) values ({3}, 1)", TableName, EntityColumn, NextHighColumn, _entityName);
        }

        public string WhereTemplate
        {
            get { return _whereTemplate; }
        }

        public string TableName
        {
            get { return "HighLowGenerator"; }
        }

        public string NextHighColumn
        {
            get { return "NextHigh"; }
        }

        public string EntityColumn
        {
            get { return "Entity"; }
        }

        public long MaxLow
        {
            get { return 49; }
        }

        public string InsertToHighLowTable
        {
            get { return _insertToHighLowTable; }
        }

        public void MapGenerator(IGeneratorMapper generatorMapper)
        {
            generatorMapper.Params(new
                                       {
                                           table = TableName,
                                           column = NextHighColumn,
                                           max_lo = MaxLow,
                                           where = String.Format(WhereTemplate, _entityName)
                                       });

        }

        public string CreateHighLowTable
        {
            get
            {
                var builder = new StringBuilder(300)
                    .AppendLine(DropHighLowTable)
                    .AppendFormat("create table {0} ({1} varchar(128), {2} bigint)", TableName, EntityColumn, NextHighColumn)
                    .AppendLine();

                return builder.ToString();
            }
        }

        public string DropHighLowTable
        {
            get { return string.Format("drop table {0}", TableName); }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (HighLowHelper)) return false;
            return Equals((HighLowHelper) obj);
        }

        public bool Equals(HighLowHelper other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._entityName, _entityName);
        }

        public override int GetHashCode()
        {
            return _entityName.GetHashCode();
        }
    }
}