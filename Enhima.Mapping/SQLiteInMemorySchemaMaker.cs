using System;
using System.Data;
using System.Data.SQLite;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Enhima.Tests
{
    public class SQLiteInMemorySchemaMaker : IDisposable
    {
        private readonly Configuration _configuration;
        
        private readonly ISessionFactory _sessionFactory;

        private IDbConnection _currentConnection;
        
        private readonly SchemaExport _schemaExport;
        
        private ISession _currentSession;

        private IStatelessSession _currentStatelessSession;

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        public ISession CurrentSession
        {
            get { return _currentSession; }
        }

        public IStatelessSession CurrentStatelessSession
        {
            get { return _currentStatelessSession; }
        }
        
        public SQLiteInMemorySchemaMaker(Configuration configuration)
        {
            _configuration = configuration;

            _sessionFactory = configuration.BuildSessionFactory();
            _schemaExport = new SchemaExport(_configuration);
        }

        public void CreateSchema()
        {
            DropSchema();

            OpenConnection();
            ExportSchema();
            OpenSessions();
        }

        public ISession OpenSession()
        {
            return _sessionFactory.OpenSession(_currentConnection);
        }

        public IStatelessSession OpenStatelessSession()
        {
            return _sessionFactory.OpenStatelessSession(_currentConnection);
        }

        public void DropSchema()
        {
            CloseSessions();
            CloseConnection();
        }

        private void CloseConnection()
        {
            if (_currentConnection != null) _currentConnection.Dispose();
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