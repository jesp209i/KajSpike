using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KajSpike.Framework
{
    public abstract class AggregateRoot<TId>: IInternalEventHandler where TId : ValueObject<TId>
    {
        public TId Id { get; protected set; }
        private readonly List<object> _changes;
        protected abstract void When(object @event);
        protected AggregateRoot() => _changes = new List<object>();
        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _changes.Add(@event);
        }
        protected abstract void EnsureValidState();
        public IEnumerable<object> GetChanges() => _changes.AsEnumerable();
        public void ClearChanges() => _changes.Clear();
        protected void ApplyToEntity(IInternalEventHandler entity, object @event) => entity?.Handle(@event);
        void IInternalEventHandler.Handle(object @event) => When(@event);
    }
}
