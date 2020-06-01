using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace KajSpike.Persistence
{
    public class Checkpoint
    {
        public string Id { get; set; }
        public Position Position { get; set; }
    }
}
