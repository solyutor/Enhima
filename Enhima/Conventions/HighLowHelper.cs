using System;
using System.Text;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;

namespace Enhima.Conventions
{
    public class HighLowHelper
    {
        private readonly Type _typeToMapHilo;
        private IAuxiliaryDatabaseObject _insertToHighLowTable;
        private string _whereTemplate;

        public HighLowHelper(Type typeToMapHilo)
        {
            _typeToMapHilo = typeToMapHilo;
            _whereTemplate = EntityNameColumn + " = '{0}'";

            var insert = String.Format("insert into {0} ({1}, {2}) values ({3}, 1)", TableName, EntityNameColumn, NextHighColumn, _typeToMapHilo.Name);
            
            _insertToHighLowTable = new SimpleAuxiliaryDatabaseObject(insert, null);
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

        public string EntityNameColumn
        {
            get { return "EntityName"; }
        }

        public long MaxLow
        {
            get { return 49; }
        }

        public IAuxiliaryDatabaseObject InsertToHighLowTable
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
                                           where = String.Format(WhereTemplate, _typeToMapHilo.Name)
                                       });

        }

        public IAuxiliaryDatabaseObject CreateHighLowTable
        {
            get
            {
                var builder = new StringBuilder(4000)
                    .AppendFormat("delete from {0}", TableName).AppendLine()
                    .AppendFormat("alter table {0} add {1} varchar(128)", TableName, EntityNameColumn).AppendLine();

                return new SimpleAuxiliaryDatabaseObject(builder.ToString(), null);
            }
            
        }
    }
}