using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace Enhima
{
    public static class Configurator
    {
        public static string SqliteInMemoryConnnectionString = "Data Source=:memory:;Version=3;New=True;";

        /// <summary>
        /// Configures NHibernate to use SQLite in memory. Use it for test purposes.
        /// </summary>
        /// <param name="self"></param>
        public static Configuration ConfigureSQLiteInMemory(this Configuration self)
        {
            return Configure(self, SqliteInMemoryConnnectionString);
        }

        /// <summary>
        /// Configures NHibernate to use SQLite in file. Use it for test purposes.
        /// </summary>
        /// <param name="self"></param>
        public static Configuration ConfigureSQLite(this Configuration self)
        {
            var appname = Assembly.GetCallingAssembly().GetName().FullName.Split('.')[0];
            var filename = appname + ".db";
            return ConfigureSQLite(self, filename);
        }

        /// <summary>
        /// Configures NHibernate to use SQLite in file. Use it for test purposes. 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="filename">Filename of the database.</param>
        public static Configuration ConfigureSQLite(this Configuration self, string filename)
        {
            var connectionString = string.Format("Data Source = {0}; Version = 3;", filename);
            return Configure(self, connectionString);
        }

        private static Configuration Configure(Configuration self, string connectionString)
        {
            self.DataBaseIntegration(db =>
                                         {
                                             db.ConnectionString = connectionString;
                                             db.Driver<SQLite20Driver>();
                                             db.Dialect<SQLiteDialect>();
                                             db.LogFormattedSql = true;
                                             db.LogSqlInConsole = true;
                                             db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                                             db.SchemaAction = SchemaAutoAction.Recreate;
                                         });
            return self;
        }
    }
}