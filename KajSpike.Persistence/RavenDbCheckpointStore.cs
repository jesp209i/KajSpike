using EventStore.ClientAPI;
using KajSpike.Framework.Interfaces;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KajSpike.Persistence
{
    public class RavenDbCheckpointStore : ICheckpointStore
    {
        private readonly Func<IAsyncDocumentSession> _getSession;
        private readonly string _checkpointName;
        public RavenDbCheckpointStore(
            Func<IAsyncDocumentSession> getSession,
            string checkpointName
            )
        {
            _getSession = getSession;
            _checkpointName = checkpointName;
        }
        public async Task<Position> GetCheckpoint()
        {
            using var session = _getSession();
            var checkpoint = await session.LoadAsync<Checkpoint>(_checkpointName);
            return checkpoint?.Position ?? Position.Start;
        }

        public async Task StoreCheckpoint(Position position)
        {
            using var session = _getSession();
            var checkpoint = await session.LoadAsync<Checkpoint>(_checkpointName);
            if (checkpoint == null)
            {
                checkpoint = new Checkpoint
                {
                    Id = _checkpointName
                };
                await session.StoreAsync(checkpoint);
            }
            checkpoint.Position = position;
            await session.SaveChangesAsync();
        }
    }
}
