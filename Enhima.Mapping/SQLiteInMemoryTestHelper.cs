using System;
using System.Data;
using System.Data.SQLite;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Enhima
{
    /// <summary>
    /// Facilitate in common NHibernate-SQLite related test issues: schema creation, session and SQLite-connection management. 
    /// </summary>
    public class SQLiteInMemoryTestHelper : IDisposable
    {
        private readonly Configuration _configuration;
        
        private readonly ISessionFactory _sessionFactory;

        private IDbConnection _currentConnection;
        
        private readonly SchemaExport _schemaExport;
        
        private ISession _currentSession;

        private IStatelessSession _currentStatelessSession;

        /// <summary>
        /// Returns session factory. 
        /// <remarks>You shouldn't use it directly, because new sessions will be opened with new SQLite in memory connection, and no data could be maintained for tests.</remarks>
        /// </summary>
        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }
        
        /// <summary>
        /// Returns current opened session
        /// </summary>
        public ISession CurrentSession
        {
            get
            {
                EnsureReady();
                return _currentSession;
            }
        }

        /// <summary>
        /// Return current opened stateless session
        /// </summary>
        public IStatelessSession CurrentStatelessSession
        {
            get
            {
                EnsureReady();
                return _currentStatelessSession;
            }
        }

        /// <summary>
        /// Creates new instance of <see cref="SQLiteInMemoryTestHelper"/> on top of supplied Configuration
        /// </summary>
        /// <param name="configuration"></param>
        public SQLiteInMemoryTestHelper(Configuration configuration)
        {
            _configuration = configuration;

            _sessionFactory = configuration.BuildSessionFactory();
            _schemaExport = new SchemaExport(_configuration);
        }

        /// <summary>
        /// Opens new SQLite connection and exports shema
        /// </summary>
        public void CreateSchema()
        {
            DropSchema();
            OpenConnection();
            ExportSchema();
            OpenSessions();
        }

        /// <summary>
        /// Opens new session with current open connection
        /// </summary>
        public ISession OpenSession()
        {
            EnsureReady();
            return _sessionFactory.OpenSession(_currentConnection);
        }

        /// <summary>
        /// Opens new stateless session with current open connection
        /// </summary>
        public IStatelessSession OpenStatelessSession()
        {
            EnsureReady();
            return _sessionFactory.OpenStatelessSession(_currentConnection);
        }

        /// <summary>
        /// Drops a sheme and closes SQLite connetion
        /// </summary>
        public void DropSchema()
        {
            CloseSessions();
            CloseConnection();
        }

        private void CloseConnection()
        {
            if (_currentConnection != null)
            {
                _currentConnection.Dispose();
                _currentConnection = null;
            }
        }

        private void CloseSessions()
        {
            if (_currentStatelessSession != null) _currentStatelessSession.Dispose();

            if (_currentSession != null) _currentSession.Dispose();
        }

        private void OpenConnection()
        {
            _currentConnection = new SQLiteConnection(Configurator.SqliteInMemoryConnnectionString);
            _currentConnection.Open();
        }

        private void EnsureReady()
        {
            if(_currentConnection == null)
            {
                ExportSchema();
            }
        }

        private void ExportSchema()
        {
            _schemaExport.Execute(false, true, false, _currentConnection, null);
        }

        private void OpenSessions()
        {
            _currentSession = OpenSession();
            _currentStatelessSession = OpenStatelessSession();
        }

        public void Dispose()
        {
            DropSchema();
            _sessionFactory.Dispose();
        }
    }
}