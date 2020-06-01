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
        public Task<Position> GetCheckpoint()
        {
            throw new NotImplementedException();
        }

        public Task StoreCheckpoint(Position checkpoint)
        {
            throw new NotImplementedException();
        }
    }
}
