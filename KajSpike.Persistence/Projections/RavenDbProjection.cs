using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KajSpike.Framework.Interfaces;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace KajSpike.Persistence.Projections
{
    public abstract class RavenDbProjection<T> : IProjection
    {
        protected RavenDbProjection(Func<IAsyncDocumentSession> getSession) => GetSession = getSession;

        public Func<IAsyncDocumentSession> GetSession { get; }
        public abstract Task Project(object @event);
        protected async Task UsingSession(Func<IAsyncDocumentSession, Task> operation)
        {
            using var session = GetSession();
            await operation(session);
            await session.SaveChangesAsync();
        }
        protected Task Create(Func<Task<T>> model)
            => UsingSession(async session => await session.StoreAsync(await model()));
        protected Task UpdateOne(Guid id, Action<T> update)
            => UsingSession(session => UpdateItem(session, id, update));
        private static async Task UpdateItem(IAsyncDocumentSession session, Guid id, Action<T> update)
        {
            var item = await session.LoadAsync<T>(id.ToString());
            if (item == null) return;
            update(item);
        }
        async Task UpdateMultipleItems(IAsyncDocumentSession session, Expression<Func<T, bool>> query, Action<T> update)
        {
            var items = await session
                .Query<T>()
                .Where(query)
                .ToListAsync();
            foreach (var item in items)
                update(item);
        }
        protected Task UpdateWhere(
            Expression<Func<T, bool>> where,
            Action<T> update
        ) => UsingSession(
            session =>
                UpdateMultipleItems(
                    session, where, update
                )
        );
        protected async Task Remove(Expression<Func<T,bool>> query)
        {
            UsingSession(async session => session.Delete(query));
        }
    }
}
