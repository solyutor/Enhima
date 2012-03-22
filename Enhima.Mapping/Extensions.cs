using NHibernate;

namespace Enhima
{
    /// <summary>
    /// Useful extensions for testing purpose with enhima infrastructure
    /// </summary>
    public static class Extensions
    {
         /// <summary>
         /// Persist all supplied entities.
         /// </summary>
         public static SQLiteInMemoryTestHelper Persist(this SQLiteInMemoryTestHelper self, params object[] entities)
         {
             using (var session = self.OpenSession())
             {
                 Persist(session, entities);
             }
             return self;
         }

        /// <summary>
        /// Persists all supplied entities in the session. 
        /// </summary>
        public static ISession Persist(this ISession session, params object[] entities)
        {
            using (var tx = session.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    session.Persist(entity);
                }
                tx.Commit();
            }
            return session;
        }

        /// <summary>
        /// Load entity from database, initializing all lazy properties. 
        /// </summary>
        public static TEntity Load<TEntity>(this SQLiteInMemoryTestHelper self, object id)
        {
            using (var session =  self.OpenSession())
            {
                var entity = session.Get<TEntity>(id);
                NHibernateUtil.Initialize(entity);
                return entity;
            }
        }
    }
}